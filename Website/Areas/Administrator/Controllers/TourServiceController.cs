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
using Website.Areas.Administrator.Models.Tour;
using Website.Helpers;

namespace Website.Areas.Administrator.Controllers
{
    public class TourServiceController : Controller
    {
        private ITourActivityService tourActivityService { get; set; }
        private ITourUtilitiesService tourUtilService { get; set; }
        private ITourService tourService { get; set; }
        private IProvinceService provinceService { get; set; }
        private INationalService nationalService { get; set; }
        private IDistrictService districtService { get; set; }
        private IAreaService areaService { get; set; }

        public TourServiceController(ITourActivityService tourActivityService, ITourService tourService, IProvinceService provinceService,
            INationalService nationalService, IDistrictService districtService, IAreaService areaService,
            ITourUtilitiesService tourUtilService)
        {
            this.tourActivityService = tourActivityService;
            this.tourService = tourService;
            this.nationalService = nationalService;
            this.districtService = districtService;
            this.areaService = areaService;
            this.provinceService = provinceService;
            this.tourUtilService = tourUtilService;
        }

        #region HttpGet
        public ActionResult Index(string id)
        {
            int tourID = Protector.Int(id);
            var info = tourService.Find(tourID);
            if (info != null)
            {
                
                var list = tourUtilService.All.Where(p => p.TourID == tourID).OrderByDescending(p => p.Order).ToList();
                ViewBag.TourID = tourID;
                ViewBag.TourName = info.Name;
                return View(list);
            }
            return RedirectToAction("Index", "Tour");
        }


        [HttpPost]
        [ActionName("Index")]
        [AcceptButton(ButtonName = "Delete")]
        public ActionResult IndexDelete(int[] checkedRecords, string id)
        {
            checkedRecords = checkedRecords ?? new int[] { };
            if (checkedRecords != null && checkedRecords.Length > 0)
            {
                foreach (int item in checkedRecords)
                {
                    var price = tourUtilService.Find(item);
                    if (price != null)
                    {

                        tourUtilService.Delete(price);
                        tourUtilService.Save();
                    }
                }
            }
            int tourID = Protector.Int(id);
            ViewBag.TourID = tourID;
            RouteValueDictionary routeValues = this.GridRouteValues();
            return RedirectToAction("Index", routeValues);
        }

        [HttpPost]
        [ActionName("Index")]
        [AcceptButton(ButtonName = "Search")]
        public ActionResult IndexSearch(string key, FormCollection data)
        {
            var nationId = Protector.Int(data["NationalList"]);
            var provinceId = Protector.Int(data["ProvinceList"]);
            var districtId = Protector.Int(data["DistrictList"]);
            var areaId = Protector.Int(data["area"]);
            var list = tourActivityService.All.Where(t => !t.IsDeleted);

            if (!string.IsNullOrEmpty(key))
            {
                list = list.Where(r => r.Name.Contains(key));
            }
            if (nationId > 0)
            {
                list = list.Where(r => r.NationalID == nationId);

            }
            if (provinceId > 0)
            {
                list = list.Where(r => r.ProvinceID == provinceId);

            }
            if (districtId > 0)
            {
                list = list.Where(r => r.DistrictID == districtId);

            }
            if (areaId > 0)
            {
                list = list.Where(r => r.AreaID == areaId);
            }

            ViewBag.Key = key;

            IEnumerable<National> nationals = nationalService.All.Where(r => r.IsActive).OrderByDescending(r => r.Order);
            ViewBag.NationalList = nationals.ToSelectListItems(nationId);

            IEnumerable<Province> provinces = provinceService.All.Where(r => r.IsActive && r.NationalID == nationId).OrderByDescending(r => r.Order);
            ViewBag.ProvinceList = provinces.ToSelectListItems(provinceId);

            IEnumerable<District> districts = districtService.All.Where(r => r.IsActive && r.ProvinceID == provinceId).OrderByDescending(r => r.Order);
            ViewBag.DistrictList = districts.ToSelectListItems(districtId);

            IEnumerable<Area> areas = areaService.All.Where(r => r.IsActive && r.DistrictID == districtId).OrderByDescending(r => r.Order);
            ViewBag.DistrictList = areas.ToSelectListItems(areaId);

            return View(list.ToList());
        }

        [HttpGet]
        public ActionResult Create(string id)
        {
            int tourID = Protector.Int(id);
            var type = EnumTourActivity.TourSuggestion.ToString();
            var tour = tourService.Find(tourID);
            if (tour != null)
            {
                var price = new TourServiceModel();
                ViewBag.TourName = tour.Name;
                price.TourID = tourID;
                price.IsActive = true;

                var listInserted = tourUtilService.All.Where(t => t.TourID == tourID).Select(t => t.ActivityID).ToList();

                ViewData["Suggestion"] = tourActivityService.All.Where(a => !a.IsDeleted && a.Type == type && !listInserted.Contains(a.ID)).OrderByDescending(t => t.CreatedDate).ToList();
                return View(price);
            }
            return RedirectToAction("Index", "Tour", null);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            long priceID = Protector.Int(id);
            var service = new TourServiceModel();
            var info = tourUtilService.Find(priceID);
            if (info != null)
            {
                ModelCopier.CopyModel(info, service);
                var tour = tourService.All.SingleOrDefault(h => h.ID == info.TourID);
                if (tour != null)
                {
                    ViewBag.TourName = tour.Name;
                    ViewBag.TourID = tour.ID;
                    service.ActivityID = info.ActivityID.ToString();
                }
                return View(service);
            }
            return RedirectToAction("Index", "Tour");
        }
        #endregion

        #region Post

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(TourServiceModel model, FormCollection data)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Create");
            }

            var serviceIds = data["checkedRecords"];
            if (!string.IsNullOrEmpty(serviceIds))
            {
                var arrIds = serviceIds.Split(new[] {','}, StringSplitOptions.None);
                foreach (var id in arrIds)
                {
                    var suggest = new TourSuggestion();

                    ModelCopier.CopyModel(model, suggest);

                    suggest.CreatedDate = DateTime.Now;
                    suggest.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);

                    suggest.Order = 0;
                    suggest.ActivityID = Protector.Int(id);

                    tourUtilService.Insert(suggest);
                }

                tourUtilService.Save();
            }

            return RedirectToAction("Index", "TourService", new {id = model.TourID});
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(TourServiceModel model)
        {
            if (!ModelState.IsValid)
            {
               
                return View(model);
            }
            int suggestID = Protector.Int(model.ID);
            var suggest = tourUtilService.Get(r => r.ID == suggestID);
            if (suggest != null)
            {
                suggest.Order = model.Order;
                suggest.IsActive = model.IsActive;
                if (TryUpdateModel(suggest))
                {
                    tourUtilService.Update(suggest);
                    tourUtilService.Save();

                    return RedirectToAction("Index", "TourService", new {id = suggest.TourID});
                }
            }

            return View(model);
        }

        #endregion
    }
}
