using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Domain;
using Microsoft.Web.Mvc;
using Service.Attribute;
using Service.Helpers;
using Service.IServices;
using Telerik.Web.Mvc.Extensions;
using Website.Areas.Administrator.Models.TourTheme;

namespace Website.Areas.Administrator.Controllers
{
    public class TourThemeController : Controller
    {
        public ITourThemeService tourThemeService { get; set; }
        public TourThemeController(ITourThemeService tourThemeService)
        {
            this.tourThemeService = tourThemeService;
        }

        //
        // GET: /Administrator/TourTheme/

        #region HttpGet
        public ActionResult Index()
        {
            List<TourTheme> list = tourThemeService.All.OrderByDescending(t => t.Order).ToList();
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
                foreach (var record in checkedRecords)
                {
                    tourThemeService.Delete(record);
                }

                tourThemeService.Save();
            }
            RouteValueDictionary routeValues = this.GridRouteValues();
            return RedirectToAction("Index", routeValues);
        }

        [HttpPost]
        [ActionName("Index")]
        [AcceptButton(ButtonName = "Search")]
        public ActionResult IndexSearch(string key)
        {
            var list = tourThemeService.All;
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
            var tourTheme = new TourThemeModel();
            tourTheme.IsActive = true;
            return View(tourTheme);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            int tourThemeID = Protector.Int(id);
            var tourTheme = tourThemeService.Find(tourThemeID);
            var model = new TourThemeModel();
            ModelCopier.CopyModel(tourTheme, model);
            return View(model);
        }
        #endregion

        #region HttpPost
        [HttpPost]
        public ActionResult Create(TourThemeModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            TourTheme info = tourThemeService.Get(r => r.Name == model.Name);
            if (info != null)
            {
                ModelState.AddModelError("", string.Format("Tên loại khách sạn [{0}] đã hiện hữu vui lòng chọn tên khác", model.Name));
                return View(model);
            }
            var tourThemeData = new TourTheme();
            
            ModelCopier.CopyModel(model, tourThemeData);

            tourThemeData.CreatedDate = DateTime.Now;
            tourThemeData.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
            tourThemeService.Insert(tourThemeData);
            tourThemeService.Save();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(string id, TourThemeModel current)
        {
            if (!ModelState.IsValid)
            {
                return View(current);
            }
            int tourThemeID = Protector.Int(id);
            TourTheme tourTheme = tourThemeService.Get(r => r.Name == current.Name && r.ID != tourThemeID);
            if (tourTheme != null)
            {
                ModelState.AddModelError("", string.Format("Tên chuỗi tour [{0}] đã hiện hữu vui lòng chọn tên khác", current.Name));
                return View(current);
            }
            tourTheme = tourThemeService.Find(tourThemeID);
            if (TryUpdateModel(tourTheme))
            {
                tourThemeService.Save();
                return RedirectToAction("Index");
            }
            else return View(tourTheme);

        }
        #endregion

    }
}
