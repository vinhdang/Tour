using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Domain.Entities;
using Service.Attribute;
using System.Web.Routing;
using Website.Areas.Administrator.Models.Hotel;
using Website.Helpers;
using Service.Helpers;
using Microsoft.Web.Mvc;
using System.IO;
using Telerik.Web.Mvc.Extensions;
namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class HotelController : Controller
    {
        public IProvinceService provinceService { get; set; }
        public INationalService nationalService { get; set; }
        public IDistrictService districtService { get; set; }
        public IAreaService areaService { get; set; }
        public IHotelTypeService hotelTypeService { get; set; }
        public IHotelThemeService hotelThemeService { get; set; }
        public IPaymentService paymentService { get; set; }
        public IComfortService comfortService { get; set; }
        public IHotelService hotelService { get; set; }
        public IHotelPictureService hotelPictureService { get; set; }
        public HotelController(IProvinceService provinceService, INationalService nationalService, IDistrictService districtService, IAreaService areaService,
                                IHotelTypeService hotelTypeService, IHotelThemeService hotelThemeService, IPaymentService paymentService,
                                 IComfortService comfortService, IHotelService hotelService, IHotelPictureService hotelPictureService)
        {
            this.provinceService = provinceService;
            this.nationalService = nationalService;
            this.districtService = districtService;
            this.areaService = areaService;
            this.hotelTypeService = hotelTypeService;
            this.hotelThemeService = hotelThemeService;
            this.paymentService = paymentService;
            this.comfortService = comfortService;
            this.hotelService = hotelService;
            this.hotelPictureService = hotelPictureService;
        }
        #region HttpGet
        public ActionResult Index()
        {
           
            List<Hotel> list = hotelService.All.ToList();
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
                    Hotel hotel = hotelService.Find(item);
                    if (hotel != null)
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
            var list = hotelService.All;
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
            var hotel = new CreateHotelModel();
            hotel.IsActive = true;
            hotel.Nationals = nationalService.All.Where(n => n.IsActive).OrderByDescending(n => n.Order).ToSelectListItems(-1);
            List<Province> provinces = new List<Province>();
            hotel.Provinces = provinces.ToSelectListItems(-1);
            List<District> districts = new List<District>();
            hotel.Districts = districts.ToSelectListItems(-1);
            List<Area> areas = new List<Area>();
            hotel.Areas = areas.ToSelectListItems(-1);
            hotel.HotelTypes = hotelTypeService.All.Where(n => n.IsActive).OrderByDescending(n => n.Order).ToSelectListItems(-1);
            List<HotelTheme> hotelThemes = new List<HotelTheme>();
            hotel.HotelThemes = hotelThemes.ToSelectListItems(-1);
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
            hotel.ListType = GlobalVariables.HotelType;
            hotel.ListStars = GlobalVariables.Star;

            return View(hotel);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            int hotelID = Protector.Int(id);
            var hotel = new EditHotelModel();
            Hotel current = hotelService.Get(p => p.HotelID == hotelID);
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
                hotel.HotelTypes = hotelTypeService.All.Where(n => n.IsActive).ToSelectListItems(Protector.Int(current.HotelTypeID));
                hotel.HotelThemes = hotelThemeService.All.Where(n => n.IsActive && n.ProvinceID == current.ProvinceID).ToSelectListItems(Protector.Int(current.HotelThemeID));
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
                foreach (SelectListItem item in GlobalVariables.HotelType)
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
                hotel.ListStars = GlobalVariables.Star;
                hotel.ListType = listType;

                return View(hotel);
            }
            return RedirectToAction("Index");
        }


        #endregion

        #region  HttpPost
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(CreateHotelModel hotel, string[] listPaymentID, string[] listComfortID, string[] type)
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
                hotel.HotelTypes = hotelTypeService.All.Where(n => n.IsActive).OrderByDescending(n => n.Order).ToSelectListItems(-1);
                List<HotelTheme> hotelThemes = new List<HotelTheme>();
                hotel.HotelThemes = hotelThemes.ToSelectListItems(-1);
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
                hotel.ListType = GlobalVariables.HotelType;
                hotel.ListStars = GlobalVariables.Star;
                return View(hotel);
            }
            Hotel _hotel = new Hotel();
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
            hotelService.Insert(_hotel);
            hotelService.Save();
            HttpPostedFileBase file = Request.Files[0];
            if (file != null && file.ContentLength > 0)
            {
                HotelPicture picture = new HotelPicture();
                string fileName = DateTime.Now.ToString("HHmmss_ddMMyyyy_") + TextHelper.HandleTitleStringForImg(Path.GetFileName(file.FileName));
                picture.FileName = fileName;
                picture.ContentLength = file.ContentLength;
                picture.ContentType = file.ContentType;
                picture.Extension = Path.GetExtension(fileName);
                picture.HotelID = _hotel.HotelID;
                picture.CreatedDate = DateTime.Now;
                picture.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
                picture.IsAvartar = true;
                picture.IsActive = true;
                picture.Order = 0;
                picture.Alt = _hotel.Name;
                hotelPictureService.Insert(picture);
                hotelPictureService.Save();
                FileHelper.SaveHotel_Picture(file, picture.FileName, StorageElement.Hotel_PictureFolder, _hotel.HotelID);
            }
            return RedirectToAction("Index");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(EditHotelModel hotel, string[] listPaymentID, string[] listComfortID, string[] type, string id)
        {
            int hotelID = Protector.Int(id);
            Hotel current = hotelService.Get(p => p.HotelID == hotelID);
            if (current != null)
            {
                if (!ModelState.IsValid)
                {
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
                    hotel.HotelTypes = hotelTypeService.All.Where(n => n.IsActive).ToSelectListItems(Protector.Int(current.HotelTypeID));
                    hotel.HotelThemes = hotelThemeService.All.Where(n => n.IsActive && n.ProvinceID == current.ProvinceID).ToSelectListItems(Protector.Int(current.HotelThemeID));
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
                    foreach (SelectListItem item in GlobalVariables.HotelType)
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
                    hotel.ListStars = GlobalVariables.Star;
                    hotel.ListType = listType;
                    return View(current);
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
                hotelService.Save();
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
        public ActionResult HotelTheme(string provinceID)
        {
            List<HotelTheme> list = new List<HotelTheme>();
            int pr = Protector.Int(provinceID);
            list = hotelThemeService.All.Where(p => p.ProvinceID == pr).OrderByDescending(p => p.Order).ToList();
            return Json(list.ToSelectListItems(-1), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
