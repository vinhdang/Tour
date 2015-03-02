using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Service.Attribute;
using Domain.Entities;
using System.Web.Routing;
using Telerik.Web.Mvc.Extensions;
using Service.Helpers;
using Website.Areas.Administrator.Models.Comfort;
using Microsoft.Web.Mvc;

namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class ComfortController : Controller
    {
        public IComfortService comfortService { get; set; }
        public ComfortController(IComfortService comfortService)
        {
            this.comfortService = comfortService;
        }
        #region HttpGet
        public ActionResult Index()
        {
            List<Comfort> list = comfortService.All.ToList();
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

            }
            RouteValueDictionary routeValues = this.GridRouteValues();
            return RedirectToAction("Index", routeValues);
        }

        [HttpPost]
        [ActionName("Index")]
        [AcceptButton(ButtonName = "Search")]
        public ActionResult IndexSearch(string key)
        {
            var list = comfortService.All;
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
            var comfort = new CreateComfortModel();
            comfort.IsActive = true;

            return View(comfort);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            int comfortID = Protector.Int(id);
            var comfort = new CreateComfortModel();
            Comfort info = comfortService.Find(comfortID);
            if (info != null)
            {
                ModelCopier.CopyModel(info, comfort);
                return View(comfort);
            }
            return RedirectToAction("Index");
        }

        #endregion
        #region HttpPost

        [HttpPost]
        public ActionResult Create(CreateComfortModel comfort)
        {
            if (!ModelState.IsValid)
            {
                return View(comfort);
            }
            Comfort info = comfortService.Get(r => r.Name == comfort.Name);
            if (info != null)
            {
                ModelState.AddModelError("", string.Format("Tên tiện nghi [{0}] đã hiện hữu vui lòng chọn tên khác", comfort.Name));
                return View(comfort);
            }
            info = new Comfort();
            ModelCopier.CopyModel(comfort, info);
            info.CreatedDate = DateTime.Now;
            info.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
            comfortService.Insert(info);
            comfortService.Save();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Edit(CreateComfortModel current, string id)
        {
            int comfortID = Protector.Int(id);
            Comfort info = comfortService.Get(r => r.ComfortID == comfortID);
            if (info != null)
            {
                if (!ModelState.IsValid)
                {
                    return View(current);
                }
                TryUpdateModel(info);
                comfortService.Save();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        #endregion
    }
}
