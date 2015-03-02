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
using Website.Areas.Administrator.Models.HotelTheme;

namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class HotelThemeController : Controller
    {
        public IProvinceService provinceService { get; set; }
        public INationalService nationalService { get; set; }
        public IHotelThemeService hotelThemeService { get; set; }
        public HotelThemeController(IProvinceService provinceService, INationalService nationalService, IHotelThemeService hotelThemeService)
        {
            this.provinceService = provinceService;
            this.nationalService = nationalService;
            this.hotelThemeService = hotelThemeService;
        }
        #region HttpGet
        public ActionResult Index()
        {
            List<HotelTheme> list = hotelThemeService.All.ToList();
            IEnumerable<National> nationals = nationalService.All.Where(r => r.IsActive).OrderByDescending(r => r.Order);
            ViewBag.NationalList = nationals.ToSelectListItems(-1);
            List<Province> provinces = new List<Province>();
            ViewBag.ProvinceList = provinces.ToSelectListItems(-1);


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
        public ActionResult IndexSearch(string key, string NationalList, string ProvinceList)
        {
            var list = hotelThemeService.All;
            int NationalID = Protector.Int(NationalList);
            int ProvinceID = Protector.Int(ProvinceList);
            if (!string.IsNullOrEmpty(key))
            {
                list = list.Where(r => r.Name.Contains(key));
            }
            if (NationalID > 0)
            {
                list = list.Where(r => r.NationalID == NationalID);

            }
            if (ProvinceID > 0)
            {
                list = list.Where(r => r.ProvinceID == ProvinceID);

            }

            ViewBag.Key = key;
            IEnumerable<National> nationals = nationalService.All.Where(r => r.IsActive).OrderByDescending(r => r.Order);
            ViewBag.NationalList = nationals.ToSelectListItems(NationalID);
            IEnumerable<Province> provinces = provinceService.All.Where(r => r.IsActive && r.NationalID == NationalID).OrderByDescending(r => r.Order);
            ViewBag.ProvinceList = provinces.ToSelectListItems(ProvinceID);
            return View(list.ToList());
        }


        [HttpGet]
        public ActionResult Create()
        {
            var hotelTheme = new CreateHotelThemeModel();
            hotelTheme.IsActive = true;
            hotelTheme.Nationals = nationalService.All.Where(n => n.IsActive).OrderByDescending(n => n.Order).ToSelectListItems(-1);
            List<Province> provinces = new List<Province>();
            hotelTheme.Provinces = provinces.ToSelectListItems(-1);
            return View(hotelTheme);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            int hotelThemeID = Protector.Int(id);
            var hotelTheme = new CreateHotelThemeModel();
            HotelTheme info = hotelThemeService.Find(hotelThemeID);
            if (info != null)
            {
                ModelCopier.CopyModel(info, hotelTheme);
                hotelTheme.Nationals = nationalService.All.Where(n => n.IsActive).ToSelectListItems(info.NationalID);
                hotelTheme.Provinces = provinceService.All.Where(p => p.NationalID == info.NationalID).ToSelectListItems(info.ProvinceID);
                return View(hotelTheme);
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region HttpPost
        [HttpPost]
        public ActionResult Create(CreateHotelThemeModel hotelTheme)
        {
            if (!ModelState.IsValid)
            {
                hotelTheme.Nationals = nationalService.All.Where(n => n.IsActive).OrderByDescending(n => n.Order).ToSelectListItems(-1);
                List<Province> provinces = new List<Province>();
                hotelTheme.Provinces = provinces.ToSelectListItems(-1);
                return View(hotelTheme);
            }
            HotelTheme info = hotelThemeService.Get(r => r.Name == hotelTheme.Name && r.ProvinceID == hotelTheme.ProvinceID);
            if (info != null)
            {
                ModelState.AddModelError("", string.Format("Tên chuỗi khách sạn [{0}] đã hiện hữu vui lòng chọn tên khác", hotelTheme.Name));
                hotelTheme.Nationals = nationalService.All.Where(n => n.IsActive).OrderByDescending(n => n.Order).ToSelectListItems(-1);
                List<Province> provinces = new List<Province>();
                hotelTheme.Provinces = provinces.ToSelectListItems(-1);
                return View(hotelTheme);
            }
            info = new HotelTheme();
            ModelCopier.CopyModel(hotelTheme, info);
            info.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
            info.CreatedDate = DateTime.Now;
            hotelThemeService.Insert(info);
            hotelThemeService.Save();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(CreateHotelThemeModel current, string id)
        {
            int hotelThemeID = Protector.Int(id);
            HotelTheme info = hotelThemeService.Get(d => d.HotelThemeID == hotelThemeID);
            if (info != null)
            {
                if (!ModelState.IsValid)
                {
                    current.Nationals = nationalService.All.Where(n => n.IsActive).ToSelectListItems(info.NationalID);
                    current.Provinces = provinceService.All.Where(p => p.NationalID == info.NationalID).ToSelectListItems(info.ProvinceID);
                    return View(current);
                }
                TryUpdateModel(info);
                hotelThemeService.Save();
            }
            return RedirectToAction("Index");
        }
        #endregion
        #region Json Action
        public ActionResult SelectProvince(string nationalID)
        {
            List<Province> list = new List<Province>();
            int nID = Protector.Int(nationalID);
            list = provinceService.All.Where(p => p.NationalID == nID).OrderByDescending(p => p.Order).ToList();
            return Json(list.ToSelectListItems(-1), JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
