using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Domain;
using Website.Helpers;
using Service.Attribute;
using System.Web.Routing;
using Telerik.Web.Mvc.Extensions;
using Service.Helpers;
using Microsoft.Web.Mvc;
using Website.Areas.Administrator.Models.Apartment;
using System.IO;

namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class ApartmentController : Controller
    {
        public IProvinceService provinceService { get; set; }
        public INationalService nationalService { get; set; }
        public IDistrictService districtService { get; set; }
        public IAreaService areaService { get; set; }
        public IApartmentTypeService apartmentTypeService { get; set; }
        public IApartmentThemeService apartmentThemeService { get; set; }
        public IPaymentService paymentService { get; set; }
        public IComfortService comfortService { get; set; }
        public IApartmentService apartmentService { get; set; }
        public IApartmentPictureService apartmentPictureService { get; set; }
        public ApartmentController(IProvinceService provinceService, INationalService nationalService, IDistrictService districtService, IAreaService areaService,
                                IApartmentTypeService apartmentTypeService, IApartmentThemeService apartmentThemeService, IPaymentService paymentService,
                                 IComfortService comfortService, IApartmentService apartmentService, IApartmentPictureService apartmentPictureService)
        {
            this.provinceService = provinceService;
            this.nationalService = nationalService;
            this.districtService = districtService;
            this.areaService = areaService;
            this.apartmentTypeService = apartmentTypeService;
            this.apartmentThemeService = apartmentThemeService;
            this.paymentService = paymentService;
            this.comfortService = comfortService;
            this.apartmentService = apartmentService;
            this.apartmentPictureService = apartmentPictureService;
        }
        #region HttpGet
        public ActionResult Index()
        {

            List<Apartment> list = apartmentService.All.ToList();
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
                    Apartment apartment = apartmentService.Find(item);
                    if (apartment != null)
                    {

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
            var list = apartmentService.All;
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

        public ActionResult Typical()
        {

            List<Apartment> list = apartmentService.All.Where(p => (p.Type & 1) == 1).ToList();

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
                    Apartment apartment = apartmentService.Find(item);
                    if (apartment != null)
                    {
                        apartment.Type = apartment.Type - 1;
                        apartmentService.Save();
                    }
                }
            }
            RouteValueDictionary routeValues = this.GridRouteValues();
            return RedirectToAction("Typical", routeValues);
        }
        [HttpGet]
        public ActionResult Create()
        {
            var hotel = new CreateApartmentModel();
            hotel.IsActive = true;
            hotel.Nationals = nationalService.All.Where(n => n.IsActive).OrderByDescending(n => n.Order).ToSelectListItems(-1);
            List<Province> provinces = new List<Province>();
            hotel.Provinces = provinces.ToSelectListItems(-1);
            List<District> districts = new List<District>();
            hotel.Districts = districts.ToSelectListItems(-1);
            List<Area> areas = new List<Area>();
            hotel.Areas = areas.ToSelectListItems(-1);
            hotel.ApartmentTypes = apartmentTypeService.All.Where(n => n.IsActive).OrderByDescending(n => n.Order).ToSelectListItems(-1);
            hotel.ApartmentThemes = apartmentThemeService.All.Where(n => n.IsActive).OrderByDescending(n => n.Order).ToSelectListItems(-1);
            List<SelectListItem> listPayment = new List<SelectListItem>();
            var listP = paymentService.All.Where(p => p.IsActive).OrderByDescending(p => p.Order);
            foreach (Payment item in listP)
            {
                listPayment.Add(new System.Web.Mvc.SelectListItem() { Selected = true, Text = item.Name, Value = item.PaymentID.ToString() });
            }
            List<SelectListItem> listComfort = new List<SelectListItem>();
            var listC = comfortService.All.Where(p => p.IsActive).OrderByDescending(p => p.Order);
            foreach (Comfort item in listC)
            {
                listComfort.Add(new System.Web.Mvc.SelectListItem() { Selected = false, Text = item.Name, Value = item.ComfortID.ToString() });
            }
            ViewBag.ListPayment = listPayment;
            ViewBag.ListComfort = listComfort;
            hotel.ListType = GlobalVariables.TourType;

            return View(hotel);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            int hotelID = Protector.Int(id);
            var hotel = new EditApartmentModel();
            Apartment current = apartmentService.Get(p => p.ApartmentID == hotelID);
            if (current != null)
            {
                ModelCopier.CopyModel(current, hotel);
                hotel.Nationals = nationalService.All.Where(n => n.IsActive).ToSelectListItems(current.NationalID);
                hotel.Provinces = provinceService.All.Where(n => n.IsActive && n.NationalID == current.NationalID).ToSelectListItems(current.ProvinceID);
                hotel.Districts = districtService.All.Where(n => n.IsActive && n.ProvinceID == current.ProvinceID).ToSelectListItems(Protector.Int(current.DistrictID, -1));
                if (current.DistrictID != null)
                {
                    hotel.Areas = areaService.All.Where(n => n.IsActive && n.DistrictID == current.DistrictID).ToSelectListItems(Protector.Int(current.AreaID));
                }
                else
                {
                    List<Area> areas = new List<Area>();
                    hotel.Areas = areas.ToSelectListItems(-1);
                }
                hotel.ApartmentTypes = apartmentTypeService.All.Where(n => n.IsActive).ToSelectListItems(Protector.Int(current.ApartmentTypeID));
                hotel.ApartmentThemes = apartmentThemeService.All.Where(n => n.IsActive).ToSelectListItems(Protector.Int(current.ApartmentThemeID));
                List<SelectListItem> listPayment = new List<SelectListItem>();
                var listP = paymentService.All.Where(l => l.IsActive);
                foreach (Payment item in listP)
                {
                    if (current.ListPayment.Contains(item.PaymentID.ToString()))
                    {
                        listPayment.Add(new System.Web.Mvc.SelectListItem() { Selected = true, Text = item.Name, Value = item.PaymentID.ToString() });
                    }
                    else { listPayment.Add(new System.Web.Mvc.SelectListItem() { Selected = false, Text = item.Name, Value = item.PaymentID.ToString() }); }
                }
                List<SelectListItem> listComfort = new List<SelectListItem>();
                var listA = comfortService.All.Where(l => l.IsActive);
                foreach (Comfort item in listA)
                {
                    if (current.ListComfort.Contains(item.ComfortID.ToString()))
                    {
                        listComfort.Add(new System.Web.Mvc.SelectListItem() { Selected = true, Text = item.Name, Value = item.ComfortID.ToString() });
                    }
                    else { listComfort.Add(new System.Web.Mvc.SelectListItem() { Selected = false, Text = item.Name, Value = item.ComfortID.ToString() }); }
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
                ViewBag.ListComfort = listComfort;
                hotel.ListType = listType;
                hotel.ListType = listType;

                return View(hotel);
            }
            return RedirectToAction("Index");
        }


        #endregion
        #region  HttpPost
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(CreateApartmentModel hotel, string[] listPaymentID, string[] listComfortID, string[] type)
        {
            if (!ModelState.IsValid)
            {
                hotel.IsActive = true;
                hotel.Nationals = nationalService.All.Where(n => n.IsActive).OrderByDescending(n => n.Order).ToSelectListItems(-1);
                List<Province> provinces = new List<Province>();
                hotel.Provinces = provinces.ToSelectListItems(-1);
                List<District> districts = new List<District>();
                hotel.Districts = districts.ToSelectListItems(-1);
                List<Area> areas = new List<Area>();
                hotel.Areas = areas.ToSelectListItems(-1);
                hotel.ApartmentTypes = apartmentTypeService.All.Where(n => n.IsActive).OrderByDescending(n => n.Order).ToSelectListItems(-1);
                hotel.ApartmentThemes = apartmentThemeService.All.Where(n => n.IsActive).OrderByDescending(n => n.Order).ToSelectListItems(-1);
                List<SelectListItem> listPayment = new List<SelectListItem>();
                var listP = paymentService.All.Where(p => p.IsActive).OrderByDescending(p => p.Order);
                foreach (Payment item in listP)
                {
                    listPayment.Add(new System.Web.Mvc.SelectListItem() { Selected = true, Text = item.Name, Value = item.PaymentID.ToString() });
                }
                List<SelectListItem> listComfort = new List<SelectListItem>();
                var listC = comfortService.All.Where(p => p.IsActive).OrderByDescending(p => p.Order);
                foreach (Comfort item in listC)
                {
                    listComfort.Add(new System.Web.Mvc.SelectListItem() { Selected = false, Text = item.Name, Value = item.ComfortID.ToString() });
                }
                ViewBag.ListPayment = listPayment;
                ViewBag.ListComfort = listComfort;
                hotel.ListType = GlobalVariables.TourType;
                return View(hotel);
            }
            Apartment _hotel = new Apartment();
            ModelCopier.CopyModel(hotel, _hotel);
            _hotel.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
            _hotel.CreatedDate = DateTime.Now;
            if (listPaymentID != null && listPaymentID.Length > 0)
            {
                string paymentID = string.Empty;
                foreach (string item in listPaymentID)
                {
                    paymentID += item + ";";
                }
                paymentID = paymentID.Replace(";", " ").Trim().Replace(" ", ";");
                _hotel.ListPayment = paymentID;
            }
            else { _hotel.ListPayment = string.Empty; }
            if (listComfortID != null && listComfortID.Length > 0)
            {
                string ComfortID = string.Empty;
                foreach (string item in listComfortID)
                {
                    ComfortID += item + ";";
                }
                ComfortID = ComfortID.Replace(";", " ").Trim().Replace(" ", ";");
                _hotel.ListComfort = ComfortID;
            }
            else { _hotel.ListComfort = string.Empty; }
            _hotel.Type = TextHelper.UpdateType(type);
            _hotel.Url = UrlHelp.NormalizeStringForUrl(_hotel.Name);
            apartmentService.Insert(_hotel);
            apartmentService.Save();
            HttpPostedFileBase file = Request.Files[0];
            if (file != null && file.ContentLength > 0)
            {
                ApartmentPicture picture = new ApartmentPicture();
                string fileName = DateTime.Now.ToString("HHmmss_ddMMyyyy_") + TextHelper.HandleTitleStringForImg(Path.GetFileName(file.FileName));
                picture.FileName = fileName;
                picture.ContentLength = file.ContentLength;
                picture.ContentType = file.ContentType;
                picture.Extension = Path.GetExtension(fileName);
                picture.ApartmentID = _hotel.ApartmentID;
                picture.CreatedDate = DateTime.Now;
                picture.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
                picture.IsAvartar = true;
                picture.IsActive = true;
                picture.Order = 0;
                picture.Alt = _hotel.Name;
                apartmentPictureService.Insert(picture);
                apartmentPictureService.Save();
                FileHelper.SaveApartment_Picture(file, picture.FileName, StorageElement.Apartment_PictureFolder, _hotel.ApartmentID);
            }
            return RedirectToAction("Index");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(EditApartmentModel apartment, string[] listPaymentID, string[] listComfortID, string[] type, string id)
        {
            int apartmentID = Protector.Int(id);
            Apartment current = apartmentService.Get(p => p.ApartmentID == apartmentID);
            if (current != null)
            {
                if (!ModelState.IsValid)
                {
                    apartment.Nationals = nationalService.All.Where(n => n.IsActive).ToSelectListItems(current.NationalID);
                    apartment.Provinces = provinceService.All.Where(n => n.IsActive && n.NationalID == current.NationalID).ToSelectListItems(current.ProvinceID);
                    apartment.Districts = districtService.All.Where(n => n.IsActive && n.ProvinceID == current.ProvinceID).ToSelectListItems(Protector.Int(current.DistrictID, -1));
                    if (current.DistrictID != null)
                    {
                        apartment.Areas = areaService.All.Where(n => n.IsActive && n.DistrictID == current.DistrictID).ToSelectListItems(Protector.Int(current.AreaID));
                    }
                    else
                    {
                        List<Area> areas = new List<Area>();
                        apartment.Areas = areas.ToSelectListItems(-1);
                    }
                    apartment.ApartmentTypes = apartmentTypeService.All.Where(n => n.IsActive).ToSelectListItems(Protector.Int(current.ApartmentTypeID));
                    apartment.ApartmentThemes = apartmentThemeService.All.Where(n => n.IsActive).ToSelectListItems(Protector.Int(current.ApartmentThemeID));
                    List<SelectListItem> listPayment = new List<SelectListItem>();
                    var listP = paymentService.All.Where(l => l.IsActive);
                    foreach (Payment item in listP)
                    {
                        if (current.ListPayment.Contains(item.PaymentID.ToString()))
                        {
                            listPayment.Add(new System.Web.Mvc.SelectListItem() { Selected = true, Text = item.Name, Value = item.PaymentID.ToString() });
                        }
                        else { listPayment.Add(new System.Web.Mvc.SelectListItem() { Selected = false, Text = item.Name, Value = item.PaymentID.ToString() }); }
                    }
                    List<SelectListItem> listComfort = new List<SelectListItem>();
                    var listA = comfortService.All.Where(l => l.IsActive);
                    foreach (Comfort item in listA)
                    {
                        if (current.ListComfort.Contains(item.ComfortID.ToString()))
                        {
                            listComfort.Add(new System.Web.Mvc.SelectListItem() { Selected = true, Text = item.Name, Value = item.ComfortID.ToString() });
                        }
                        else { listComfort.Add(new System.Web.Mvc.SelectListItem() { Selected = false, Text = item.Name, Value = item.ComfortID.ToString() }); }
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
                    ViewBag.ListComfort = listComfort;
                    apartment.ListType = listType;
                    apartment.ListType = listType;
                    return View(current);
                }
                TryUpdateModel(current);

                current.Url = UrlHelp.NormalizeStringForUrl(current.Name);

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
                if (listComfortID != null && listComfortID.Length > 0)
                {
                    string ComfortID = string.Empty;
                    foreach (string item in listComfortID)
                    {
                        ComfortID += item + ";";
                    }
                    ComfortID = ComfortID.Replace(";", " ").Trim().Replace(" ", ";");
                    current.ListComfort = ComfortID;
                }
                else { current.ListComfort = string.Empty; }
                current.Type = TextHelper.UpdateType(type);
                apartmentService.Save();
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

