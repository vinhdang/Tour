using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Service.Helpers;
using Website.Helpers;
using Domain;
using Microsoft.Web.Mvc;
using Website.Areas.Administrator.Models.Area;
using System.Web.Routing;
using Service.Attribute;
using Telerik.Web.Mvc.Extensions;

namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class AreaController : Controller
    {
        public IProvinceService provinceService { get; set; }
        public INationalService nationalService { get; set; }
        public IDistrictService districtService { get; set; }
        public IAreaService areaService { get; set; }
        public AreaController(IProvinceService provinceService, INationalService nationalService, IDistrictService districtService, IAreaService areaService)
        {
            this.provinceService = provinceService;
            this.nationalService = nationalService;
            this.districtService = districtService;
            this.areaService = areaService;
        }
        #region HttpGet
        public ActionResult Index()
        {
            List<Area> list = areaService.All.ToList();
            IEnumerable<National> nationals = nationalService.All.Where(r => r.IsActive).OrderByDescending(r => r.Order);
            ViewBag.NationalList = nationals.ToSelectListItems(-1);
            List<Province> provinces = new List<Province>();
            ViewBag.ProvinceList = provinces.ToSelectListItems(-1);

            List<District> districts = new List<District>();
            ViewBag.DistrictList = districts.ToSelectListItems(-1);
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
        public ActionResult IndexSearch(string key, string NationalList, string ProvinceList, string DistrictList)
        {
            var list = areaService.All;
            int NationalID = Protector.Int(NationalList);
            int ProvinceID = Protector.Int(ProvinceList);
            int DistrictID = Protector.Int(DistrictList);
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
            if (DistrictID > 0)
            {
                list = list.Where(r => r.DistrictID == DistrictID);

            }
            ViewBag.Key = key;
            IEnumerable<National> nationals = nationalService.All.Where(r => r.IsActive).OrderByDescending(r => r.Order);
            ViewBag.NationalList = nationals.ToSelectListItems(NationalID);
            IEnumerable<Province> provinces = provinceService.All.Where(r => r.IsActive && r.NationalID == NationalID).OrderByDescending(r => r.Order);
            ViewBag.ProvinceList = provinces.ToSelectListItems(ProvinceID);

            IEnumerable<District> districts = districtService.All.Where(r => r.IsActive && r.ProvinceID == ProvinceID).OrderByDescending(r => r.Order);
            ViewBag.DistrictList = districts.ToSelectListItems(DistrictID);
            return View(list.ToList());
        }
        [HttpGet]
        public ActionResult Create()
        {
            var area = new CreateAreaModel();
            area.IsActive = true;
            area.Nationals = nationalService.All.Where(n => n.IsActive).OrderByDescending(n => n.Order).ToSelectListItems(-1);
            List<Province> provinces = new List<Province>();
            area.Provinces = provinces.ToSelectListItems(-1);
            List<District> districts = new List<District>();
            area.Districts = districts.ToSelectListItems(-1);
            return View(area);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            int areaID = Protector.Int(id);
            var area = new CreateAreaModel();
            Area info = areaService.Find(areaID);
            if (info != null)
            {
                ModelCopier.CopyModel(info, area);
                area.Nationals = nationalService.All.Where(n => n.IsActive).ToSelectListItems(info.NationalID);
                area.Provinces = provinceService.All.Where(p => p.NationalID == info.NationalID).ToSelectListItems(info.ProvinceID);
                area.Districts = districtService.All.Where(p => p.ProvinceID == info.ProvinceID).ToSelectListItems((int)info.DistrictID);
                return View(area);
            }
            return RedirectToAction("Index");
        }


        #endregion
        #region  HttpPost
        [HttpPost]
        public ActionResult Create(CreateAreaModel area)
        {
            if (!ModelState.IsValid)
            {
                area.Nationals = nationalService.All.Where(n => n.IsActive).OrderByDescending(n => n.Order).ToSelectListItems(-1);
                List<Province> provinces = new List<Province>();
                area.Provinces = provinces.ToSelectListItems(-1);
                List<District> districts = new List<District>();
                area.Districts = districts.ToSelectListItems(-1);
                return View(area);
            }
            Area info = areaService.Get(r => r.Name == area.Name && r.DistrictID == area.DistrictID);
            if (info != null)
            {
                ModelState.AddModelError("", string.Format("Tên khu vực [{0}] đã hiện hữu vui lòng chọn tên khác", area.Name));
                area.Nationals = nationalService.All.Where(n => n.IsActive).OrderByDescending(n => n.Order).ToSelectListItems(-1);
                List<Province> provinces = new List<Province>();
                area.Provinces = provinces.ToSelectListItems(-1);
                List<District> districts = new List<District>();
                area.Districts = districts.ToSelectListItems(-1);
                return View(area);
            }
            info = new Area();
            ModelCopier.CopyModel(area, info);
            info.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
            info.CreatedDate = DateTime.Now;
            areaService.Insert(info);
            areaService.Save();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult Edit(CreateAreaModel current, string id)
        {
            int areaID = Protector.Int(id);
            Area info = areaService.Get(d => d.ID == areaID);
            if (info != null)
            {
                if (!ModelState.IsValid)
                {
                    current.Nationals = nationalService.All.Where(n => n.IsActive).ToSelectListItems(info.NationalID);
                    current.Provinces = provinceService.All.Where(p => p.NationalID == info.NationalID).ToSelectListItems(info.ProvinceID);
                    current.Districts = districtService.All.Where(p => p.ProvinceID == info.ProvinceID).ToSelectListItems((int) info.DistrictID);
                    return View(current);
                }
                TryUpdateModel(info);
                areaService.Save();
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
        public ActionResult SelectDistrict(string provinceID)
        {
            List<District> list = new List<District>();
            int pr = Protector.Int(provinceID);
            list = districtService.All.Where(p => p.ProvinceID == pr).OrderByDescending(p => p.Order).ToList();
            return Json(list.ToSelectListItems(-1), JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}
