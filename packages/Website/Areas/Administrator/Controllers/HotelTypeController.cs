using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Service.Helpers;
using Website.Helpers;
using Domain.Entities;
using Microsoft.Web.Mvc;
using Service.Attribute;
using System.Web.Routing;
using Telerik.Web.Mvc.Extensions;

namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class HotelTypeController : Controller
    {
        public IHotelTypeService hotelTypeService { get; set; }
        public HotelTypeController(IHotelTypeService hotelTypeService)
        {
            this.hotelTypeService = hotelTypeService;
        }
        #region HttpGet
        public ActionResult Index()
        {
            List<HotelType> list = hotelTypeService.All.ToList();
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
            var list = hotelTypeService.All;
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
            var hotelType = new HotelType();
            hotelType.IsActive = true;
            return View(hotelType);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            int hotelTypeID = Protector.Int(id);
            var hotelType = hotelTypeService.Find(hotelTypeID);
            return View(hotelType);
        }
        #endregion

        #region HttpPost
        [HttpPost]
        public ActionResult Create(HotelType hotelType)
        {
            if (!ModelState.IsValid)
            {
                return View(hotelType);
            }
            HotelType info = hotelTypeService.Get(r => r.Name == hotelType.Name);
            if (info != null)
            {
                ModelState.AddModelError("", string.Format("Tên loại khách sạn [{0}] đã hiện hữu vui lòng chọn tên khác", hotelType.Name));
                return View(hotelType);
            }
            hotelType.CreatedDate = DateTime.Now;
            hotelType.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
            hotelTypeService.Insert(hotelType);
            hotelTypeService.Save();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(string id, HotelType current)
        {
            if (!ModelState.IsValid)
            {
                return View(current);
            }
            int hotelTypeID = Protector.Int(id);
            HotelType hotelType = hotelTypeService.Get(r => r.Name == current.Name && r.HotelTypeID != hotelTypeID);
            if (hotelType != null)
            {
                ModelState.AddModelError("", string.Format("Tên quốc gia [{0}] đã hiện hữu vui lòng chọn tên khác", current.Name));
                return View(current);
            }
            hotelType = hotelTypeService.Find(hotelTypeID);
            if (TryUpdateModel(hotelType))
            {
                hotelTypeService.Save();
                return RedirectToAction("Index");
            }
            else return View(hotelType);

        }
        #endregion

    }
}
