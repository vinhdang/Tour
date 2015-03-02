using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Domain.Entities;
using Website.Helpers;
using Service.Attribute;
using System.Web.Routing;
using Telerik.Web.Mvc.Extensions;
using Service.Helpers;
using Website.Areas.Administrator.Models.District;
using Microsoft.Web.Mvc;

namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class DistrictController : Controller
    {
        public INationalService nationalService { get; set; }
        public IProvinceService provinceService { get; set; }
        public IDistrictService districtService { get; set; }
        public DistrictController(INationalService nationalService, IProvinceService provinceService, IDistrictService districtService)
        {
            this.nationalService = nationalService;
            this.provinceService = provinceService;
            this.districtService = districtService;
        }

        #region HttpGet
        public ActionResult Index()
        {
            List<District> list = districtService.All.ToList();
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
            var list = districtService.All;
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
            var district = new CreateDistrictModel();
            district.IsActive = true;
            district.Nationals = nationalService.All.Where(n => n.IsActive).ToSelectListItems(-1);
            List<Province> provinces = new List<Province>();
            district.Provinces = provinces.ToSelectListItems(-1);
            return View(district);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            int districtID = Protector.Int(id);
            var district = new CreateDistrictModel();
            District info = districtService.Find(districtID);
            if (info != null)
            {
                ModelCopier.CopyModel(info, district);
                district.Nationals = nationalService.All.Where(n => n.IsActive).ToSelectListItems(info.NationalID);
                district.Provinces = provinceService.All.Where(p => p.NationalID == info.NationalID).ToSelectListItems(info.ProvinceID);
                return View(district);
            }
            return RedirectToAction("Index");
        }

        #endregion
        #region HttpPost
        [HttpPost]
        public ActionResult Create(CreateDistrictModel district)
        {
            if (!ModelState.IsValid)
            {
                district.Nationals = nationalService.All.Where(n => n.IsActive).ToSelectListItems(-1);
                List<Province> provinces = provinceService.All.Where(p => p.NationalID == district.NationalID).ToList();
                district.Provinces = provinces.ToSelectListItems(-1);
                return View(district);
            }
            District info = districtService.Get(r => r.Name == district.Name && r.ProvinceID == district.ProvinceID);
            if (info != null)
            {
                ModelState.AddModelError("", string.Format("Tên quận huyện [{0}] đã hiện hữu vui lòng chọn tên khác", district.Name));
                district.Nationals = nationalService.All.Where(n => n.IsActive).ToSelectListItems(-1);
                List<Province> provinces = provinceService.All.Where(p => p.NationalID == district.NationalID).ToList();
                district.Provinces = provinces.ToSelectListItems(-1);
                return View(district);
            }
            info = new District();
            ModelCopier.CopyModel(district, info);

            info.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
            info.CreatedDate = DateTime.Now;
            districtService.Insert(info);
            districtService.Save();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Edit(CreateDistrictModel current, string id)
        {
            int districtID = Protector.Int(id);
            District info = districtService.Get(d => d.DistrictID == districtID);
            if (info != null)
            {
                if (!ModelState.IsValid)
                {
                    current.Nationals = nationalService.All.Where(n => n.IsActive).ToSelectListItems(info.NationalID);
                    current.Provinces = provinceService.All.Where(p => p.NationalID == info.NationalID).ToSelectListItems(info.ProvinceID);
                    return View(current);
                }
                TryUpdateModel(info);
                districtService.Save();
            }
            return RedirectToAction("Index");
        }
        #endregion
        #region Json Action
        public ActionResult SelectProvince(string nationalID)
        {
            List<Province> list = new List<Province>();
            int nID = Protector.Int(nationalID);
            list = provinceService.All.Where(p => p.NationalID == nID).ToList();
            return Json(list.ToSelectListItems(-1), JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}
