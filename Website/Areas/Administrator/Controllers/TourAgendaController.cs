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
using Website.Areas.Administrator.Models.TourAgenda;
using Website.Helpers;

namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class TourAgendaController : Controller
    {
        public ITourAgendaService tourAgendaService { get; set; }
        public ITourService tourService { get; set; }
        public IProvinceService provinceService { get; set; }
        public INationalService nationalService { get; set; }
        public IDistrictService districtService { get; set; }
        public IAreaService areaService { get; set; }
        public TourAgendaController(ITourService tourService, ITourAgendaService tourAgendaService, IProvinceService provinceService,
            INationalService nationalService, IDistrictService districtService, IAreaService areaService)
        {
            this.tourAgendaService = tourAgendaService;
            this.tourService = tourService;
            this.nationalService = nationalService;
            this.provinceService = provinceService;
            this.districtService = districtService;
            this.areaService = areaService;
        }

        //
        // GET: /Administrator/TourTheme/

        #region HttpGet
        public ActionResult Index(string id)
        {
            var tourID = Protector.Int(id ?? Request["tourid"]);
            var list = tourAgendaService.All.Where(a => a.TourID == tourID).OrderByDescending(t => t.Sequence).ToList();
            var tour = tourService.Get(t => t.ID == tourID);

            var result = new List<AgendaDisplay>();
            foreach (var agenda in list)
            {
                var model = new AgendaDisplay();
                ModelCopier.CopyModel(agenda, model);

                model.National = nationalService.Get(n => n.ID == model.NationalID);
                model.Province = provinceService.Get(p => p.ID == model.ProvinceID);
                model.District = districtService.Get(d => d.ID == model.DistrictID);
                model.Area = areaService.Get(a => a.ID == model.AreaID);

                result.Add(model);
            }


            ViewBag.TourName = tour == null ? string.Empty : tour.Name;
            ViewBag.TourID = id;
            return View(result);
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
                    tourAgendaService.Delete(record);
                }

                tourAgendaService.Save();
            }
            RouteValueDictionary routeValues = this.GridRouteValues();
            return RedirectToAction("Index", routeValues);
        }

        [HttpPost]
        [ActionName("Index")]
        [AcceptButton(ButtonName = "Search")]
        public ActionResult IndexSearch(string key, FormCollection data)
        {
            var list = tourAgendaService.All;
            if (!string.IsNullOrEmpty(key))
            {
                list = list.Where(r => r.Description.Contains(key));
            }
            ViewBag.Key = key;
            ViewBag.TourID = data["TourID"];
            return View(list.ToList());
        }


        [HttpGet]
        public ActionResult Create()
        {
            int tourId = Protector.Int(Request.QueryString["tourid"]);

            var tour = tourService.Get(t => t.ID == tourId && !(bool)t.IsDeleted);

            ViewBag.TourID = tourId;
            var tourAgenda = new AgendaModel();
            if(tour != null)
            {
                tourAgenda.FromDate = tour.Startdate;
                tourAgenda.ToDate = tour.Enddate;
            }
            tourAgenda.IsActivate = true;


            tourAgenda.TourID = Protector.Int(Request.QueryString["tourid"]);
            tourAgenda.Nationals = nationalService.All.Where(n => n.IsActive).OrderByDescending(n => n.Order).ToSelectListItems(-1);
            List<Province> destprovinces = new List<Province>();
            tourAgenda.Provinces = destprovinces.ToSelectListItems(-1);
            List<District> destdistricts = new List<District>();
            tourAgenda.Districts = destdistricts.ToSelectListItems(-1);
            List<Area> destareas = new List<Area>();
            tourAgenda.Areas = destareas.ToSelectListItems(-1); 



            return View(tourAgenda);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            int tourAgendaID = Protector.Int(id);
            var tourAgenda = tourAgendaService.Find(tourAgendaID);
            var model = new AgendaModel();
            ModelCopier.CopyModel(tourAgenda, model);

            model.Nationals = nationalService.All.Where(n => n.IsActive).ToSelectListItems(tourAgenda.NationalID);
            model.Provinces = provinceService.All.Where(n => n.IsActive && n.NationalID == tourAgenda.NationalID).ToSelectListItems(tourAgenda.ProvinceID);
            model.Districts = districtService.All.Where(n => n.IsActive && n.ProvinceID == tourAgenda.ProvinceID).ToSelectListItems(Protector.Int(tourAgenda.DistrictID, -1));
            if (tourAgenda.DistrictID != null || tourAgenda.DistrictID > -1)
            {
                model.Areas = areaService.All.Where(n => n.IsActive && n.DistrictID == tourAgenda.DistrictID).ToSelectListItems(Protector.Int(tourAgenda.AreaID));
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
        public ActionResult Create(AgendaModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Nationals = nationalService.All.Where(n => n.IsActive).OrderByDescending(n => n.Order).ToSelectListItems(-1);
                List<Province> destprovinces = new List<Province>();
                model.Provinces = destprovinces.ToSelectListItems(-1);
                List<District> destdistricts = new List<District>();
                model.Districts = destdistricts.ToSelectListItems(-1);
                List<Area> destareas = new List<Area>();
                model.Areas = destareas.ToSelectListItems(-1); 
                
                return View(model);
            }
            var info = tourAgendaService.Get(r => r.Description == model.Description);
            if (info != null)
            {
                model.Nationals = nationalService.All.Where(n => n.IsActive).OrderByDescending(n => n.Order).ToSelectListItems(-1);
                List<Province> destprovinces = new List<Province>();
                model.Provinces = destprovinces.ToSelectListItems(-1);
                List<District> destdistricts = new List<District>();
                model.Districts = destdistricts.ToSelectListItems(-1);
                List<Area> destareas = new List<Area>();
                model.Areas = destareas.ToSelectListItems(-1); 
                ModelState.AddModelError("", string.Format("Lịch trình [{0}] đã hiện hữu vui lòng chọn tên khác", model.Description));
                return View(model);
            }
            var tourAgendaData = new TourAgenda();

            ModelCopier.CopyModel(model, tourAgendaData);

            tourAgendaService.Insert(tourAgendaData);
            tourAgendaService.Save();
            return RedirectToAction("Index", "TourAgenda", new {id=tourAgendaData.TourID});
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(string id, AgendaModel current)
        {
            if (!ModelState.IsValid)
            {
                current.Nationals = nationalService.All.Where(n => n.IsActive).ToSelectListItems(current.NationalID);
                current.Provinces = provinceService.All.Where(n => n.IsActive && n.NationalID == current.NationalID).ToSelectListItems(current.ProvinceID);
                current.Districts = districtService.All.Where(n => n.IsActive && n.ProvinceID == current.ProvinceID).ToSelectListItems(Protector.Int(current.DistrictID, -1));
                if (current.DistrictID != null || current.DistrictID > -1)
                {
                    current.Areas = areaService.All.Where(n => n.IsActive && n.DistrictID == current.DistrictID).ToSelectListItems(Protector.Int(current.AreaID));
                }
                else
                {
                    List<Area> areas = new List<Area>();
                    current.Areas = areas.ToSelectListItems(-1);

                }

                ViewBag.TourID = Request.QueryString["tourid"];
                return View(current);
            }
            int tourAgendaID = Protector.Int(current.ID);
            var tourAgenda= tourAgendaService.Get(r => r.ID == tourAgendaID);
            if (tourAgenda != null)
            {
                tourAgenda = tourAgendaService.Find(tourAgendaID);
                if (TryUpdateModel(tourAgenda))
                {
                    tourAgendaService.Save();
                    return RedirectToAction("Index", "TourAgenda", new { id = tourAgenda.TourID });
                }
            }
            
            return View(current);
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
