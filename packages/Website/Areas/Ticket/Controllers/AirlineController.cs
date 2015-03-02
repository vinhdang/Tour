using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Domain.Entities;
using Website.Areas.Ticket.Models.Airline;
using Service.Helpers;
using Microsoft.Web.Mvc;
using System.IO;
using Service.Attribute;
using System.Web.Routing;
using Telerik.Web.Mvc.Extensions;

namespace Website.Areas.Ticket.Controllers
{
    [Authorize]
    public class AirlineController : Controller
    {
        public IAirlineService airlineService { get; set; }
        public AirlineController(IAirlineService airlineService)
        {
            this.airlineService = airlineService;
        }

        #region HttpGet
        public ActionResult Index()
        {
            List<Airline> list = airlineService.All.ToList();
            return View(list);
        }
        [HttpPost]
        [ActionName("Index")]
        [AcceptButton(ButtonName = "Delete")]
        public ActionResult IndexDelete(int[] checkedRecords)
        {
            checkedRecords = checkedRecords ?? new int[] { };
            if (checkedRecords != null && checkedRecords.Length > 0)
            {
                foreach (int item in checkedRecords)
                {
                    Airline airline = airlineService.Find(item);
                    if (airline != null)
                    {
                        if (!string.IsNullOrEmpty(airline.Logo))
                        {
                            FileHelper.DeleteDirectory("Airline", airline.AirlineID);

                        }
                        airlineService.Delete(airline);
                        airlineService.Save();
                    }
                }
            }
            RouteValueDictionary routeValues = this.GridRouteValues();
            return RedirectToAction("Index", routeValues);
        }

        [HttpPost]
        [ActionName("Index")]
        [AcceptButton(ButtonName = "Search")]
        public ActionResult IndexSearch(string key)
        {
            var list = airlineService.All;
            if (!string.IsNullOrEmpty(key))
            {
                list = list.Where(r => r.Name.Contains(key));
            }
            ViewBag.Key = key;
            return View(list.ToList());
        }
        [HttpGet]
        public ActionResult Create()
        {
            var airline = new CreateAirlineModel();
            airline.IsActive = true;
            airline.Types = GlobalVariables.AirlineType;
            airline.Positions = GlobalVariables.AirlinePosition;
            return View(airline);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            int airlineID = Protector.Int(id);
            var airline = new CreateAirlineModel();
            Airline info = airlineService.Find(airlineID);
            if (info != null)
            {
                ModelCopier.CopyModel(info, airline);

                airline.Types = GlobalVariables.AirlineType;
                airline.Positions = GlobalVariables.AirlinePosition;
                return View(airline);
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region HttpPost
        [HttpPost]
        public ActionResult Create(CreateAirlineModel airline, string[] p)
        {
            if (!ModelState.IsValid)
            {
                airline.Types = GlobalVariables.AirlineType;
                airline.Positions = GlobalVariables.AirlinePosition;
                return View(airline);
            }
            Airline info = airlineService.Get(r => r.Name == airline.Name);
            if (info != null)
            {
                airline.Types = GlobalVariables.AirlineType;
                airline.Positions = GlobalVariables.AirlinePosition;
                ModelState.AddModelError("", string.Format("Tên hãng bay [{0}] đã hiện hữu vui lòng chọn tên khác", airline.Name));
                return View(airline);
            }
            Airline _airline = new Airline();
            ModelCopier.CopyModel(airline, _airline);
            _airline.CreatedDate = DateTime.Now;
            _airline.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
            _airline.Position = TextHelper.UpdateType(p);
            airlineService.Insert(_airline);
            airlineService.Save();
            HttpPostedFileBase file = Request.Files[0];
            if (file != null && file.ContentLength > 0)
            {
                string fileName = DateTime.Now.ToString("HHmmss_ddMMyyyy_") + TextHelper.HandleTitleStringForImg(Path.GetFileName(file.FileName));
                _airline.Logo = fileName;
                airlineService.Save();
                FileHelper.SaveHotel_Picture(file, _airline.Logo, StorageElement.Airline_PictureFolder, _airline.AirlineID);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Edit(CreateAirlineModel current, string id, string[] p)
        {
            int airlineID = Protector.Int(id);
            Airline info = airlineService.Get(r => r.AirlineID == airlineID);
            if (info != null)
            {
                if (!ModelState.IsValid)
                {
                    current.Types = GlobalVariables.ProvinceType;
                    current.Positions = GlobalVariables.ProvincePosition;
                    return View(current);
                }
                TryUpdateModel(info);
                info.Position = TextHelper.UpdateType(p);
                HttpPostedFileBase file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    if (!string.IsNullOrEmpty(info.Logo))
                    {
                        var fullFilePath = FileHelper.getFullFilePath("Airlines", info.AirlineID, info.Logo);
                        if (FileHelper.imageFileAvailable(fullFilePath))
                        {
                            System.IO.File.Delete(fullFilePath);
                        }
                    }
                    string fileName = DateTime.Now.ToString("HHmmss_ddMMyyyy_") + TextHelper.HandleTitleStringForImg(Path.GetFileName(file.FileName));
                    info.Logo = fileName;
                    FileHelper.SaveHotel_Picture(file, info.Logo, StorageElement.Airline_PictureFolder, info.AirlineID);

                }
                airlineService.Save();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        #endregion

    }
}
