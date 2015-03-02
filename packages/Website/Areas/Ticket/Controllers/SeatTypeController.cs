using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Domain.Entities;
using Service.Attribute;
using System.Web.Routing;
using Telerik.Web.Mvc.Extensions;
using Service.Helpers;
using Microsoft.Web.Mvc;

namespace Website.Areas.Ticket.Controllers
{
    [Authorize]
    public class SeatTypeController : Controller
    {
        public ISeatTypeService seatTypeService { get; set; }
        public SeatTypeController(ISeatTypeService seatTypeService)
        {
            this.seatTypeService = seatTypeService;
        }
        #region HttpGet
        public ActionResult Index()
        {
            List<SeatType> list = seatTypeService.All.ToList();
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
                    SeatType seatType = seatTypeService.Find(item);
                    if (seatType != null)
                    {

                        seatTypeService.Delete(seatType);
                        seatTypeService.Save();
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
            var list = seatTypeService.All;
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
            var seatType = new SeatType();
            seatType.IsActive = true;

            return View(seatType);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            int seatTypeID = Protector.Int(id);
            SeatType info = seatTypeService.Find(seatTypeID);
            if (info != null)
            {

                return View(info);
            }
            return RedirectToAction("Index");
        }
        #endregion
        #region HttpPost

        [HttpPost]
        public ActionResult Create(SeatType seatType)
        {
            if (!ModelState.IsValid)
            {
                return View(seatType);
            }
            SeatType info = seatTypeService.Get(r => r.Name == seatType.Name);
            if (info != null)
            {
                ModelState.AddModelError("", string.Format("Tên hạng vé [{0}] đã hiện hữu vui lòng chọn tên khác", seatType.Name));
                return View(seatType);
            }
            seatType.CreatedDate = DateTime.Now;
            seatType.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
            seatTypeService.Insert(seatType);
            seatTypeService.Save();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(SeatType current,string id)
        {
            int seatTypeID = Protector.Int(id);
            SeatType seatType = seatTypeService.Get(r => r.Name == current.Name && r.SeatTypeID != seatTypeID);
            if (seatType != null)
            {
                ModelState.AddModelError("", string.Format("Tên hạng ghế [{0}] đã hiện hữu vui lòng chọn tên khác", current.Name));
                return View(current);
            }
            else
            {
                seatType = seatTypeService.Find(seatTypeID);
                if (TryUpdateModel(seatType))
                {
                    seatTypeService.Save();
                    return RedirectToAction("Index");
                }
                else return View(current);
            }
        }




        #endregion

    }
}
