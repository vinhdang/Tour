using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Domain.Entities;
using Service.Helpers;
using Service.Attribute;
using System.Web.Routing;
using Telerik.Web.Mvc.Extensions;

namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class NationalController : Controller
    {

        public INationalService nationalService { get; set; }
        public NationalController(INationalService nationalService)
        {

            this.nationalService = nationalService;

        }
        #region HttpGet
        public ActionResult Index()
        {
            List<National> list = nationalService.All.ToList();
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
            var list = nationalService.All;
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
            var national = new National();
            national.IsActive = true;
            return View(national);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            int nationalID = Protector.Int(id);
            var national = nationalService.Find(nationalID);
            return View(national);
        }
        #endregion

        #region HttpPost
        [HttpPost]
        public ActionResult Create(National national)
        {
            if (!ModelState.IsValid)
            {
                return View(national);
            }
            National info = nationalService.Get(r => r.Name == national.Name);
            if (info != null)
            {
                ModelState.AddModelError("", string.Format("Tên quốc gia [{0}] đã hiện hữu vui lòng chọn tên khác", national.Name));
                return View(national);
            }
            national.CreatedDate = DateTime.Now;
            national.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
            nationalService.Insert(national);
            nationalService.Save();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(string id, National current)
        {
            if (!ModelState.IsValid)
            {
                return View(current);
            }
            int nationalID = Protector.Int(id);
            National national = nationalService.Get(r => r.Name == current.Name && r.NationalID != nationalID);
            if (national != null)
            {
                ModelState.AddModelError("", string.Format("Tên quốc gia [{0}] đã hiện hữu vui lòng chọn tên khác", current.Name));
                return View(national);
            }
            national = nationalService.Find(nationalID);
            if (TryUpdateModel(national))
            {
                nationalService.Save();
                return RedirectToAction("Index");
            }
            else return View(national);

        }
        #endregion

    }
}
