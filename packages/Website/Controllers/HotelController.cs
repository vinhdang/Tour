using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Domain.Entities;
using Service.Helpers;

namespace Website.Controllers
{
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
        public ICategoryService categoryService { get; set; }
        public HotelController(IProvinceService provinceService, INationalService nationalService, IDistrictService districtService, IAreaService areaService,
                                IHotelTypeService hotelTypeService, IHotelThemeService hotelThemeService, IPaymentService paymentService,
                                 IComfortService comfortService, IHotelService hotelService, IHotelPictureService hotelPictureService,
                                  ICategoryService categoryService)
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
            this.categoryService = categoryService;
        }
         [OutputCache(Duration = 1)]
        public ActionResult Index(string id)
        {
            int catID = Protector.Int(id);
            Category info = categoryService.Find(catID);
            if (info != null)
            {
                ViewBag.Title = info.PageTitle;
                ViewBag.KeyWords = info.Keyword;
                ViewBag.Description = info.Description;
            }
            return View();
        }
        public ActionResult Detail(string h)
        {
            int _hotelID = Protector.Int(h);
            Hotel info = hotelService.Get(p => p.HotelID == _hotelID && p.IsActive);
            if (info != null)
            {
                ViewBag.Title = info.PageTitle;
                ViewBag.KeyWords = info.KeyWord;
                ViewBag.Description = info.Description;
            }
            return View(info);
        }
        public ActionResult MapDetail(string h, string p)
        {
            int HotelID = Protector.Int(h);
            MyHotel info = hotelService.getByID(HotelID);
            return PartialView("MapDetail", info);
        }
        #region Partial
        public string HotelUrl(string provinceName, string hotelName, int hotelID, int provinceID, string fd, string td, string nr, string ng)
        {

            string url = UrlHelp.getHotelUrl(provinceName, hotelName, hotelID, provinceID);
            if (!string.IsNullOrEmpty(fd))
            {
                url += "?fd=" + HttpUtility.UrlEncode(fd.Trim().Replace("/", "_"));
                if (!string.IsNullOrEmpty(td))
                {
                    url += "&td=" + HttpUtility.UrlEncode(td.Trim().Replace("/", "_"));
                }
                if (!string.IsNullOrEmpty(nr))
                {
                    url += "&nr=" + HttpUtility.UrlEncode(nr);
                }
                if (!string.IsNullOrEmpty(ng))
                {
                    url += "&ng=" + HttpUtility.UrlEncode(ng);
                }
            }

            else
            {
                if (!string.IsNullOrEmpty(td))
                {
                    url += "?td=" + HttpUtility.UrlEncode(td.Trim().Replace("/", "_"));
                    if (!string.IsNullOrEmpty(nr))
                    {
                        url += "&nr=" + HttpUtility.UrlEncode(nr);
                    }
                    if (!string.IsNullOrEmpty(ng))
                    {
                        url += "&ng=" + HttpUtility.UrlEncode(ng);
                    }
                }
            }
            return url;
        }
        [OutputCache(Duration = 10000)]
        public ActionResult Home_Hotel_Left_GoodPrice()
        {

            DateTime startdate = DateTime.Today;
            DateTime enddate = startdate.AddHours(23.99);
            List<MyHotel> list = hotelService.getHotelsByType(startdate, enddate, 2);
            return PartialView("Home_Hotel_Left_GoodPrice", list);
        }
         [OutputCache(Duration = 10000)]
        public ActionResult Hotel_Left_Promotion()
        {

            DateTime startdate = DateTime.Today;
            DateTime enddate = startdate.AddHours(23.99);
            List<MyHotel> list = hotelService.getHotelsByType(startdate, enddate, 1);
            return PartialView("Hotel_Left_Promotion", list);
        }
       public ActionResult getTopHotelsByProvinceID(int provinceID)
        {

            DateTime startdate = DateTime.Today;
            DateTime enddate = startdate.AddHours(23.99);
            List<MyHotel> list = hotelService.getTopHotelsByProvinceID(startdate, enddate, provinceID, 3);
            return PartialView("getTopHotelsByProvinceID", list);
        }
         [OutputCache(Duration = 10000)]
        public ActionResult Hotel_Highlight()
        {
            DateTime startdate = DateTime.Today;
            DateTime enddate = startdate.AddHours(23.99);
            List<MyHotel> list = hotelService.getHotelsByType(startdate, enddate, 4);
            return PartialView("Hotel_Highlight", list);
        }
        [OutputCache(Duration = 10000)]
        public ActionResult Content(string content)
        {
            return PartialView("Content", content);
        }
        [OutputCache(Duration = 10000)]
        public ActionResult Policy(string policy)
        {
            return PartialView("Policy", policy);
        }
         
        public ActionResult Star(string star)
        {
            return PartialView("Star", star);
        }
      
        public ActionResult StaticImage(Hotel hotel)
        {
            return PartialView("StaticImage", hotel);
        }
        public ActionResult MapList(string id)
        {
            int _hotelID = Protector.Int(id);
            Hotel _hotel = hotelService.Find(_hotelID);
            return PartialView("MapList", _hotel);
        }
        [HttpGet]
        public ActionResult Xml(string hotelID)
        {
            int _hotelID = Protector.Int(hotelID);
            Hotel _hotel = hotelService.Find(_hotelID);
            List<MyHotel> list = new List<MyHotel>();
            if (_hotel != null)
            {
                list = hotelService.getTopHotelsByProvinceID(DateTime.Today, DateTime.Today.AddDays(1), _hotel.ProvinceID, 100);
            }
            return PartialView("Xml", list);
        }
        [HttpGet]
        public ActionResult Xml1(string hotelID)
        {
            int _hotelID = Protector.Int(hotelID);
            Hotel _hotel = hotelService.Find(_hotelID);
            List<MyHotel> list = new List<MyHotel>();
            if (_hotel != null)
            {
                list = hotelService.getTopHotelsByProvinceID(DateTime.Today, DateTime.Today.AddDays(1), _hotel.ProvinceID, 100).Where(h => h.HotelID == _hotel.HotelID).ToList();
            }
            return PartialView("Xml", list);
        }
        #endregion
    }
}
