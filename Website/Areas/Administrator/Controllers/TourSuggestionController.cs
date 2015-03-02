using System;
using System.Collections.Generic;
using System.IO;
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
using Website.Areas.Administrator.Models.TourSuggestion;
using Website.Helpers;

namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class TourSuggestionController : Controller
    {
        private ITourActivityService tourActivityService { get; set; }
        private ITourService tourService { get; set; }
        private IProvinceService provinceService { get; set; }
        private INationalService nationalService { get; set; }
        private IDistrictService districtService { get; set; }
        private IAreaService areaService { get; set; }

        public TourSuggestionController(ITourActivityService tourActivityService, ITourService tourService, IProvinceService provinceService,
            INationalService nationalService, IDistrictService districtService, IAreaService areaService)
        {
            this.tourActivityService = tourActivityService;
            this.tourService = tourService;
            this.nationalService = nationalService;
            this.districtService = districtService;
            this.areaService = areaService;
            this.provinceService = provinceService;
        } 
        
        //
        // GET: /Administrator/TourComment/

        #region HttpGet
        public ActionResult Index()
        {
            var type = EnumTourActivity.TourSuggestion.ToString();
            var list = tourActivityService.All.Where(a => !a.IsDeleted && a.Type == type).OrderByDescending(t => t.CreatedDate).ToList();

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
                foreach (var record in checkedRecords)
                {
                    var item = tourActivityService.Get(a => a.ID == record);
                    item.IsDeleted = true;
                    tourActivityService.Update(item);
                }

                tourActivityService.Save();
            }
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
            if(areaId > 0)
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
        public ActionResult Create()
        {
            int tourId = Protector.Int(Request.QueryString["tourid"]);

            ViewBag.TourID = tourId;
            var suggestion = new SuggestionModel();

            suggestion.IsDeleted = false;
            suggestion.Nationals = nationalService.All.Where(n => n.IsActive).OrderByDescending(n => n.Order).ToSelectListItems(-1);
            List<Province> destprovinces = new List<Province>();
            suggestion.Provinces = destprovinces.ToSelectListItems(-1);
            List<District> destdistricts = new List<District>();
            suggestion.Districts = destdistricts.ToSelectListItems(-1);
            List<Area> destareas = new List<Area>();
            suggestion.Areas = destareas.ToSelectListItems(-1);

            return View(suggestion);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            int suggestID = Protector.Int(id);
            var suggest = tourActivityService.Get(a => a.ID == suggestID && !a.IsDeleted);
            var model = new SuggestionModel();
            ModelCopier.CopyModel(suggest, model);
            model.Nationals = nationalService.All.Where(n => n.IsActive).ToSelectListItems(suggest.NationalID);
            model.Provinces = provinceService.All.Where(n => n.IsActive && n.NationalID == suggest.NationalID).ToSelectListItems(suggest.ProvinceID);
            model.Districts = districtService.All.Where(n => n.IsActive && n.ProvinceID == suggest.ProvinceID).ToSelectListItems(Protector.Int(suggest.DistrictID, -1));
            if (suggest.DistrictID != null)
            {
                model.Areas = areaService.All.Where(n => n.IsActive && n.DistrictID == suggest.DistrictID).ToSelectListItems(Protector.Int(suggest.AreaID));
            }
            else
            {
                List<Area> areas = new List<Area>();
                model.Areas = areas.ToSelectListItems(-1);

            }

            ViewBag.TourID = Request.QueryString["tourid"];
            return View(model);
        }
        #endregion

        #region HttpPost
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(SuggestionModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Nationals = nationalService.All.Where(n => n.IsActive).ToSelectListItems(model.NationalID);
                model.Provinces = provinceService.All.Where(n => n.IsActive && n.NationalID == model.NationalID).ToSelectListItems(model.ProvinceID);
                model.Districts = districtService.All.Where(n => n.IsActive && n.ProvinceID == model.ProvinceID).ToSelectListItems(Protector.Int(model.DistrictID, -1));
                if (model.DistrictID != null)
                {
                    model.Areas = areaService.All.Where(n => n.IsActive && n.DistrictID == model.DistrictID).ToSelectListItems(Protector.Int(model.AreaID));
                }
                else
                {
                    List<Area> areas = new List<Area>();
                    model.Areas = areas.ToSelectListItems(-1);

                }
                return View(model);
            }
            var suggest = new TourActivity();

            ModelCopier.CopyModel(model, suggest);

            suggest.CreatedDate = DateTime.Now;
            suggest.IsDeleted = false;
            suggest.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
            suggest.Type = EnumTourActivity.TourSuggestion.ToString();
            tourActivityService.Insert(suggest);
            tourActivityService.Save();

            HttpPostedFileBase file = Request.Files[0];
            if (file != null && file.ContentLength > 0)
            {
                string fileName = DateTime.Now.ToString("HHmmss_ddMMyyyy_") + TextHelper.HandleTitleStringForImg(Path.GetFileName(file.FileName));
                fileName = FileHelper.SaveApartment_Picture(file, fileName, StorageElement.SupportService_PictureFolder, suggest.ID);
                suggest.ImageUrl = FileHelper.GetTourService_Picture(fileName, suggest.ID);
            }

            tourActivityService.Update(suggest);
            tourActivityService.Save();

            return RedirectToAction("Index", "TourSuggestion");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(string id, SuggestionModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Nationals = nationalService.All.Where(n => n.IsActive).ToSelectListItems(model.NationalID);
                model.Provinces = provinceService.All.Where(n => n.IsActive && n.NationalID == model.NationalID).ToSelectListItems(model.ProvinceID);
                model.Districts = districtService.All.Where(n => n.IsActive && n.ProvinceID == model.ProvinceID).ToSelectListItems(Protector.Int(model.DistrictID, -1));
                if (model.DistrictID != null)
                {
                    model.Areas = areaService.All.Where(n => n.IsActive && n.DistrictID == model.DistrictID).ToSelectListItems(Protector.Int(model.AreaID));
                }
                else
                {
                    List<Area> areas = new List<Area>();
                    model.Areas = areas.ToSelectListItems(-1);

                }
                return View(model);
            }
            int suggestID = Protector.Int(model.ID);
            var suggest = tourActivityService.Get(r => r.ID == suggestID && !r.IsDeleted);
            if (suggest != null)
            {
                HttpPostedFileBase file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = DateTime.Now.ToString("HHmmss_ddMMyyyy_") + TextHelper.HandleTitleStringForImg(Path.GetFileName(file.FileName));
                    fileName = FileHelper.SaveApartment_Picture(file, fileName, StorageElement.SupportService_PictureFolder, suggest.ID);
                    suggest.ImageUrl = FileHelper.GetTourService_Picture(fileName, suggest.ID);
                }
                
                if (TryUpdateModel(suggest))
                {
                    tourActivityService.Update(suggest);
                    tourActivityService.Save();

                    return RedirectToAction("Index", "TourSuggestion");
                }
            }

            return View(model);
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
        public ActionResult SelectArea(string districtID)
        {
            List<Area> list = new List<Area>();
            int dt = Protector.Int(districtID);
            list = areaService.All.Where(p => p.DistrictID == dt).OrderByDescending(p => p.Order).ToList();
            return Json(list.ToSelectListItems(-1), JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}
