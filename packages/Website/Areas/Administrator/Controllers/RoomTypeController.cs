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
using Telerik.Web.Mvc.UI;
using Telerik.Web.Mvc.Extensions;

namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class RoomTypeController : Controller
    {
        public IRoomTypeService roomTypeService { get; set; }

        public RoomTypeController(IRoomTypeService roomTypeService)
        {
            this.roomTypeService = roomTypeService;
        }
        #region HttpGet
        [HttpGet]
        public ActionResult Index()
        {
            List<RoomType> list = roomTypeService.All.ToList();
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
                foreach (int id in checkedRecords)
                {
                    RoomType roomType = roomTypeService.Find(id);
                    if (roomType != null)
                    {

                        roomTypeService.Delete(roomType);
                        roomTypeService.Save();
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
            var list = roomTypeService.All;
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
            var roomType = new RoomType();
            roomType.IsActive = true;
            return View(roomType);
        }

        #endregion

        #region  HttpPost
        [HttpPost]
        public ActionResult Create(RoomType roomType)
        {
            if (!ModelState.IsValid)
            {
                return View(roomType);
            }
            RoomType info = roomTypeService.Get(r => r.Name == roomType.Name);
            if (info != null)
            {
                ModelState.AddModelError("", string.Format("Tên loại phòng [{0}] đã hiện hữu vui lòng chọn tên khác", roomType.Name));
                return View(roomType);
            }
            roomType.CreatedDate = DateTime.Now;
            roomTypeService.Insert(roomType);
            roomTypeService.Save();
            return RedirectToAction("Index");
        }
        #endregion

    }
}
