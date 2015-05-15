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
using Website.Areas.Administrator.Models.Tour;
using Website.Helpers;

namespace Website.Areas.Administrator.Controllers
{
    public class TourController : Controller
    {
        public IProvinceService provinceService { get; set; }
        public INationalService nationalService { get; set; }
        public IDistrictService districtService { get; set; }
        public IAreaService areaService { get; set; }
        public ITourTypeService tourTypeService { get; set; }
        public ITourThemeService tourThemeService { get; set; }
        public IPaymentService paymentService { get; set; }
        public ITourProviderService tourProviderService { get; set; }
        public ITourService tourService { get; set; }
        public ITourPictureService tourPictureService { get; set; }
        public ITourCommentService tourCommentService { get; set; }
        public ITourActivityService tourActivityService { get; set; }


        public TourController(IProvinceService provinceService, INationalService nationalService, IDistrictService districtService, IAreaService areaService,
                                ITourTypeService tourTypeService, ITourThemeService tourThemeService, IPaymentService paymentService,
                                ITourService tourService, ITourPictureService tourPictureService, ITourProviderService tourProviderService,
                                ITourCommentService tourCommentService, ITourActivityService tourActivityService)
        {
            this.provinceService = provinceService;
            this.nationalService = nationalService;
            this.districtService = districtService;
            this.areaService = areaService;
            this.tourTypeService = tourTypeService;
            this.tourThemeService = tourThemeService;
            this.paymentService = paymentService;
            this.tourService = tourService;
            this.tourPictureService = tourPictureService;
            this.tourProviderService = tourProviderService;
            this.tourCommentService = tourCommentService;
            this.tourActivityService = tourActivityService;
        }
        #region HttpGet
        public ActionResult Index()
        {

            List<Tour> list = tourService.All.Where(t => !(bool)t.IsDeleted).ToList();
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
        public ActionResult IndexDelete(int[] checkedRecords, string key, string NationalList, string ProvinceList, string DistrictList)
        {
            checkedRecords = checkedRecords ?? new int[] { };
            if (checkedRecords != null && checkedRecords.Length > 0)
            {
                foreach (int item in checkedRecords)
                {
                    Tour tour = tourService.Find(item);
                    if (tour != null)
                    {
                        tour.IsDeleted = true;

                        tourService.Update(tour);
                        tourService.Save();
                    }
                }
            }
            RouteValueDictionary routeValues = this.GridRouteValues();
            return RedirectToAction("Index", routeValues);
        }

        [HttpPost]
        [ActionName("Index")]
        [AcceptButton(ButtonName = "Search")]
        public ActionResult IndexSearch(string key, string NationalList, string ProvinceList, string DistrictList)
        {
            var list = tourService.All.Where(t => !(bool)t.IsDeleted);
            int NationalID = Protector.Int(NationalList);
            int ProvinceID = Protector.Int(ProvinceList);
            int DistrictID = Protector.Int(DistrictList);
            if (!string.IsNullOrEmpty(key))
            {
                list = list.Where(r => r.Name.Contains(key));
            }
            if (NationalID > 0)
            {
                list = list.Where(r => r.DestinationNationalID == NationalID);

            }
            if (ProvinceID > 0)
            {
                list = list.Where(r => r.DestinationProvinceID == ProvinceID);

            }
            if (DistrictID > 0)
            {
                list = list.Where(r => r.DestinationDistrictID == DistrictID);

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

        public ActionResult Typical()
        {
            List<Tour> list = tourService.All.Where(p => !(bool)p.IsDeleted && (p.Type & 1) == 1).ToList();

            return View(list);
        }
        [HttpPost]
        [ActionName("Typical")]
        [AcceptButton(ButtonName = "Remove")]
        public ActionResult TypicalRemove(int[] checkedRecords)
        {
            checkedRecords = checkedRecords ?? new int[] { };
            if (checkedRecords != null && checkedRecords.Length > 0)
            {
                foreach (int item in checkedRecords)
                {
                    Tour apartment = tourService.Find(item);
                    if (apartment != null)
                    {
                        apartment.Type = apartment.Type - 1;
                        tourService.Save();
                    }
                }
            }
            RouteValueDictionary routeValues = this.GridRouteValues();
            return RedirectToAction("Typical", routeValues);
        }
        [HttpGet]
        public ActionResult Create()
        {
            var hotel = new TourModel();
            hotel.IsActive = true;

            //Destination
            hotel.DestNationals = nationalService.All.Where(n => n.IsActive).OrderByDescending(n => n.Order).ToSelectListItems(-1);
            List<Province> destprovinces = new List<Province>();
            hotel.DestProvinces = destprovinces.ToSelectListItems(-1);
            List<District> destdistricts = new List<District>();
            hotel.DestDistricts = destdistricts.ToSelectListItems(-1);
            List<Area> destareas = new List<Area>();
            hotel.DestAreas = destareas.ToSelectListItems(-1);

            //Departure  
            hotel.DepartNationals = nationalService.All.Where(n => n.IsActive).OrderByDescending(n => n.Order).ToSelectListItems(-1);
            List<Province> departprovinces = new List<Province>();
            hotel.DepartProvinces = departprovinces.ToSelectListItems(-1);
            List<District> departdistricts = new List<District>();
            hotel.DepartDistricts = departdistricts.ToSelectListItems(-1);
            List<Area> departareas = new List<Area>();
            hotel.DepartAreas = departareas.ToSelectListItems(-1);


            hotel.TourTypes = tourTypeService.All.Where(n => n.IsActive).OrderByDescending(n => n.Order).ToSelectListItems(-1);
            hotel.TourThemes = tourThemeService.All.Where(n => n.IsActive).OrderByDescending(n => n.Order).ToSelectListItems(-1);
            List<SelectListItem> listPayment = new List<SelectListItem>();
            var listP = paymentService.All.Where(p => p.IsActive).OrderByDescending(p => p.Order);
            foreach (Payment item in listP)
            {
                listPayment.Add(new System.Web.Mvc.SelectListItem() { Selected = true, Text = item.Name, Value = item.ID.ToString() });
            }
            ViewBag.ListPayment = listPayment;
            hotel.ListType = GlobalVariables.TourType;

            return View(hotel);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            int tourID = Protector.Int(id);
            var tour = new EditTourModel();
            Tour current = tourService.Get(p => p.ID == tourID);
            if (current != null)
            {
                ModelCopier.CopyModel(current, tour);
                tour.DestNationals = nationalService.All.Where(n => n.IsActive).ToSelectListItems(current.DestinationNationalID);
                tour.DestProvinces = provinceService.All.Where(n => n.IsActive && n.NationalID == current.DestinationNationalID).ToSelectListItems(current.DestinationProvinceID);
                tour.DestDistricts = districtService.All.Where(n => n.IsActive && n.ProvinceID == current.DestinationProvinceID).ToSelectListItems(Protector.Int(current.DestinationDistrictID, -1));
                if (current.DestinationDistrictID != null)
                {
                    tour.DestAreas = areaService.All.Where(n => n.IsActive && n.DistrictID == current.DestinationDistrictID).ToSelectListItems(Protector.Int(current.DestinationAreaID));
                }
                else
                {
                    List<Area> areas = new List<Area>();
                    tour.DestAreas = areas.ToSelectListItems(-1);

                }

                tour.DepartNationals = nationalService.All.Where(n => n.IsActive).ToSelectListItems(current.DepartureNationalID);
                tour.DepartProvinces = provinceService.All.Where(n => n.IsActive && n.NationalID == current.DepartureNationalID).ToSelectListItems(current.DepartureProvinceID);
                tour.DepartDistricts = districtService.All.Where(n => n.IsActive && n.ProvinceID == current.DepartureProvinceID).ToSelectListItems(Protector.Int(current.DepartureDistrictID, -1));
                if (current.DepartureDistrictID != null)
                {
                    tour.DepartAreas = areaService.All.Where(n => n.IsActive && n.DistrictID == current.DepartureDistrictID).ToSelectListItems(Protector.Int(current.DepartureAreaID));
                }
                else
                {
                    List<Area> areas = new List<Area>();
                    tour.DepartAreas = areas.ToSelectListItems(-1);
                }
                tour.TourTypes = tourTypeService.All.Where(n => n.IsActive).ToSelectListItems(Protector.Int(current.TourTypeID));
                tour.TourThemes = tourThemeService.All.Where(n => n.IsActive).ToSelectListItems(Protector.Int(current.TourThemeID));
                tour.TourProviders = tourProviderService.All.Where(n => (bool)n.IsActive && !(bool)n.IsDeleted).ToList().ToSelectListItems(-1);
                List<SelectListItem> listPayment = new List<SelectListItem>();
                var listP = paymentService.All.Where(l => l.IsActive);
                foreach (Payment item in listP)
                {
                    if (current.ListPayment.Contains(item.ID.ToString()))
                    {
                        listPayment.Add(new System.Web.Mvc.SelectListItem() { Selected = true, Text = item.Name, Value = item.ID.ToString() });
                    }
                    else { listPayment.Add(new System.Web.Mvc.SelectListItem() { Selected = false, Text = item.Name, Value = item.ID.ToString() }); }
                }
                List<SelectListItem> listType = new List<SelectListItem>();
                foreach (SelectListItem item in GlobalVariables.TourType)
                {
                    if ((Protector.Int(item.Value) & current.Type) == Protector.Int(item.Value))
                    {
                        listType.Add(new System.Web.Mvc.SelectListItem() { Selected = true, Text = item.Text, Value = item.Value.ToString() });
                    }
                    else { listType.Add(new System.Web.Mvc.SelectListItem() { Selected = false, Text = item.Text, Value = item.Value.ToString() }); }
                }
                ViewBag.ListPayment = listPayment;
                tour.ListType = listType;

                return View(tour);
            }
            return RedirectToAction("Index");
        }

        #endregion
        
        #region  HttpPost
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(TourModel tour, string[] listPaymentID, string[] type)
        {
            if (!ModelState.IsValid)
            {
                tour.IsActive = true;
                tour.DepartNationals = nationalService.All.Where(n => n.IsActive).ToSelectListItems(tour.DepartureNationalID);
                tour.DepartProvinces = provinceService.All.Where(n => n.IsActive && n.NationalID == tour.DepartureNationalID).ToSelectListItems(tour.DepartureProvinceID);
                tour.DepartDistricts = districtService.All.Where(n => n.IsActive && n.ProvinceID == tour.DepartureProvinceID).ToSelectListItems(Protector.Int(tour.DepartureDistrictID, -1));
                if (tour.DepartureDistrictID != null)
                {
                    tour.DepartAreas = areaService.All.Where(n => n.IsActive && n.DistrictID == tour.DepartureDistrictID).ToSelectListItems(Protector.Int(tour.DepartureAreaID));
                }
                else
                {
                    List<Area> areas = new List<Area>();
                    tour.DepartAreas = areas.ToSelectListItems(-1);
                }

                //Destination
                tour.DestNationals = nationalService.All.Where(n => n.IsActive).ToSelectListItems(tour.DestinationNationalID);
                tour.DestProvinces = provinceService.All.Where(n => n.IsActive && n.NationalID == tour.DestinationNationalID).ToSelectListItems(tour.DestinationProvinceID);
                tour.DestDistricts = districtService.All.Where(n => n.IsActive && n.ProvinceID == tour.DestinationProvinceID).ToSelectListItems(Protector.Int(tour.DestinationDistrictID, -1));
                if (tour.DestinationDistrictID != null)
                {
                    tour.DestAreas = areaService.All.Where(n => n.IsActive && n.DistrictID == tour.DestinationDistrictID).ToSelectListItems(Protector.Int(tour.DestinationAreaID));
                }
                else
                {
                    var areas = new List<Area>();
                    tour.DestAreas = areas.ToSelectListItems(-1);

                }

                tour.TourTypes = tourTypeService.All.Where(n => n.IsActive).OrderByDescending(n => n.Order).ToSelectListItems(-1);
                tour.TourThemes = tourThemeService.All.Where(n => n.IsActive).OrderByDescending(n => n.Order).ToSelectListItems(-1);
                List<SelectListItem> listPayment = new List<SelectListItem>();
                var listP = paymentService.All.Where(p => p.IsActive).OrderByDescending(p => p.Order);
                foreach (Payment item in listP)
                {
                    listPayment.Add(new System.Web.Mvc.SelectListItem() { Selected = true, Text = item.Name, Value = item.ID.ToString() });
                }
                ViewBag.ListPayment = listPayment;
                tour.ListType = GlobalVariables.TourType;
                return View(tour);
            }
            Tour _tour = new Tour();
            ModelCopier.CopyModel(tour, _tour);
            
            if (listPaymentID != null && listPaymentID.Length > 0)
            {
                string paymentID = string.Empty;
                foreach (string item in listPaymentID)
                {
                    paymentID += item + ";";
                }
                paymentID = paymentID.Replace(";", " ").Trim().Replace(" ", ";");
                _tour.ListPayment = paymentID;
            }
            else { _tour.ListPayment = string.Empty; }

            _tour.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
            _tour.CreatedDate = DateTime.Now;
            _tour.Type = TextHelper.UpdateType(type);
            _tour.Url = UrlHelp.NormalizeStringForUrl(_tour.Name);
            _tour.IsDeleted = false;
            var total = (_tour.Enddate.Value - _tour.Startdate.Value).TotalDays;
            _tour.Duration = (int)Math.Round(total);

            tourService.Insert(_tour);
            tourService.Save();

            HttpPostedFileBase file = Request.Files[0];
            if (file != null && file.ContentLength > 0)
            {
                TourPicture picture = new TourPicture();
                string fileName = DateTime.Now.ToString("HHmmss_ddMMyyyy_") + TextHelper.HandleTitleStringForImg(Path.GetFileName(file.FileName));
                picture.FileName = fileName;
                picture.ContentLength = file.ContentLength;
                picture.ContentType = file.ContentType;
                picture.Extension = Path.GetExtension(fileName);
                picture.TourID = _tour.ID;
                picture.CreatedDate = DateTime.Now;
                picture.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
                picture.IsAvartar = true;
                picture.IsActive = true;
                picture.Order = 0;
                picture.Alt = _tour.Name;
                tourPictureService.Insert(picture);
                tourPictureService.Save();

                FileHelper.SaveApartment_Picture(file, picture.FileName, StorageElement.Tour_PictureFolder, _tour.ID);
            }
            return RedirectToAction("Index");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(EditTourModel tour, string[] listPaymentID, string[] type, string id)
        {
            int tourID = Protector.Int(id);
            Tour current = tourService.Get(p => p.ID == tourID);
            if (current != null)
            {
                if (!ModelState.IsValid)
                {
                    tour.DestNationals = nationalService.All.Where(n => n.IsActive).ToSelectListItems(current.DestinationNationalID);
                    tour.DestProvinces = provinceService.All.Where(n => n.IsActive && n.NationalID == current.DestinationProvinceID).ToSelectListItems(current.DestinationProvinceID);
                    tour.DestDistricts = districtService.All.Where(n => n.IsActive && n.ProvinceID == current.DestinationDistrictID).ToSelectListItems(Protector.Int(current.DestinationDistrictID, -1));
                    if (current.DestinationDistrictID != null)
                    {
                        tour.DestAreas = areaService.All.Where(n => n.IsActive && n.DistrictID == current.DestinationDistrictID).ToSelectListItems(Protector.Int(current.DestinationAreaID));
                    }
                    else
                    {
                        List<Area> areas = new List<Area>();
                        tour.DestAreas = areas.ToSelectListItems(-1);

                    }

                    tour.DepartNationals = nationalService.All.Where(n => n.IsActive).ToSelectListItems(current.DepartureNationalID);
                    tour.DepartProvinces = provinceService.All.Where(n => n.IsActive && n.NationalID == current.DepartureNationalID).ToSelectListItems(current.DepartureProvinceID);
                    tour.DepartDistricts = districtService.All.Where(n => n.IsActive && n.ProvinceID == current.DepartureProvinceID).ToSelectListItems(Protector.Int(current.DepartureDistrictID, -1));
                    if (current.DepartureDistrictID != null)
                    {
                        tour.DepartAreas = areaService.All.Where(n => n.IsActive && n.DistrictID == current.DepartureDistrictID).ToSelectListItems(Protector.Int(current.DepartureAreaID));
                    }
                    else
                    {
                        List<Area> areas = new List<Area>();
                        tour.DepartAreas = areas.ToSelectListItems(-1);
                    }
                    tour.TourTypes = tourTypeService.All.Where(n => n.IsActive).ToSelectListItems(Protector.Int(current.TourTypeID));
                    tour.TourThemes = tourThemeService.All.Where(n => n.IsActive).ToSelectListItems(Protector.Int(current.TourThemeID));
                    List<SelectListItem> listPayment = new List<SelectListItem>();
                    var listP = paymentService.All.Where(l => l.IsActive);
                    foreach (Payment item in listP)
                    {
                        if (current.ListPayment.Contains(item.ID.ToString()))
                        {
                            listPayment.Add(new System.Web.Mvc.SelectListItem() { Selected = true, Text = item.Name, Value = item.ID.ToString() });
                        }
                        else { listPayment.Add(new System.Web.Mvc.SelectListItem() { Selected = false, Text = item.Name, Value = item.ID.ToString() }); }
                    }
                    List<SelectListItem> listType = new List<SelectListItem>();
                    foreach (SelectListItem item in GlobalVariables.TourType)
                    {
                        if ((Protector.Int(item.Value) & current.Type) == Protector.Int(item.Value))
                        {
                            listType.Add(new System.Web.Mvc.SelectListItem() { Selected = true, Text = item.Text, Value = item.Value.ToString() });
                        }
                        else { listType.Add(new System.Web.Mvc.SelectListItem() { Selected = false, Text = item.Text, Value = item.Value.ToString() }); }
                    }
                    ViewBag.ListPayment = listPayment;
                    tour.ListType = listType;
                    tour.ListType = listType;
                    return View(tour);
                }
                TryUpdateModel(current);

                if (listPaymentID != null && listPaymentID.Length > 0)
                {
                    string paymentID = string.Empty;
                    foreach (string item in listPaymentID)
                    {
                        paymentID += item + ";";
                    }
                    paymentID = paymentID.Replace(";", " ").Trim().Replace(" ", ";");
                    current.ListPayment = paymentID;
                }
                else { current.ListPayment = string.Empty; }

                current.Url = UrlHelp.NormalizeStringForUrl(current.Name);
                current.Type = TextHelper.UpdateType(type);
                current.IsDeleted = false;
                var total = (current.Enddate.Value - current.Startdate.Value).TotalDays;
                current.Duration = (int)Math.Round(total);

                tourService.Save();
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
