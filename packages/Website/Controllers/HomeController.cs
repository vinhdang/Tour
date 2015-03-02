using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Domain.Entities;
using System.Web.Routing;
using Service.Helpers;
using Website.Models.Home;

namespace Website.Controllers
{
    public class HomeController : Controller
    {
        public ICategoryService categoryService { get; set; }
        public INewService newService { get; set; }
        public IProvinceService provinceService { get; set; }
        public IHotelService hotelService { get; set; }
        public IRoomService roomService { get; set; }
        public IRoomPriceService roomPriceService { get; set; }
        public IConfigService configService { get; set; }
        public HomeController(IConfigService configService, ICategoryService categoryService, INewService newService, IProvinceService provinceService, IHotelService hotelService, IRoomService roomService, IRoomPriceService roomPriceService)
        {
            this.categoryService = categoryService;
            this.newService = newService;
            this.provinceService = provinceService;
            this.hotelService = hotelService;
            this.roomService = roomService;
            this.roomPriceService = roomPriceService;
            this.configService = configService;
        }
        [OutputCache(Duration = 10000)]
        public ActionResult Index()
        {

            Category info = categoryService.Find(1);
            if (info != null)
            {
                ViewBag.Title = info.PageTitle;
                ViewBag.KeyWords = info.Keyword;
                ViewBag.Description = info.Description;
            }

            return View();
        }
        #region Partial
        public ActionResult Home_Search()
        {
            string f = Protector.String(Request.QueryString["s"]);
            string partial = "Home_Search_Hotel";
            switch (f)
            {
                case "ticket":
                    partial = "Home_Search_Ticket";
                    break;
                case "tour":
                    partial = "Home_Search_Tour";
                    break;
                case "car":
                    partial = "Home_Search_Car";
                    break;
                case "restaurant":
                    partial = "Home_Search_Restaurant";
                    break;
                case "apartment":
                    partial = "Home_Search_Apartment";
                    break;
                case "mail":
                    partial = "Home_Search_SentMail";
                    break;
                case "r":
                    partial = "Home_Search_SentMailResult";
                    Config config1 = configService.All.Where(c => c.IsActive && c.Name == "EmailTemplateKetQuaNhanGiaTot").FirstOrDefault();
                    return PartialView("Home_Search_SentMailResult", config1);
                    break;
                default:
                    break;
            }
            return PartialView(partial);
        }
        public ActionResult Hotel_Search()
        {
            string f = Protector.String(Request.QueryString["s"]);
            string partial = "Hotel_Search";
            switch (f)
            {

                case "mail":
                    partial = "Hotel_Search_SentMail";
                    break;
                case "r":
                    partial = "Hotel_Search_SentMailResult";
                    Config config1 = configService.All.Where(c => c.IsActive && c.Name == "EmailTemplateKetQuaNhanGiaTot").FirstOrDefault();
                    return PartialView("Hotel_Search_SentMailResult", config1);
                    break;
            }
            return PartialView(partial);
        }
        public ActionResult Hotel_Search_SentMail(string province, string provinceID, string hotel, string hotelID, string FromDate, string ToDate, string Email)
        {
            int _provinceID = Protector.Int(provinceID);
            int _hotelID = Protector.Int(hotelID);
            Province _province = provinceService.Find(_provinceID);
            if (_province != null && !string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate) && !string.IsNullOrEmpty(Email) && EmailHelper.IsValidEmail(Email))
            {
                DateTime _fromDate = DateTime.Now;
                DateTime _toDate = _fromDate.AddDays(1);
                try
                {
                    string[] day = FromDate.Split('/');
                    string[] day1 = ToDate.Split('/');
                    _fromDate = new DateTime(Protector.Int(day[2]), Protector.Int(day[1]), Protector.Int(day[0]));
                    _toDate = new DateTime(Protector.Int(day1[2]), Protector.Int(day1[1]), Protector.Int(day1[0]));

                }
                catch { }
                Config _config = configService.Get(c => c.Name == "EmailTemplateNhanGiaTot" && c.IsActive);

                string hotelRoom = "<td colspan='' style='border-bottom: 1px solid #BDC8CD; padding: 8px 5px 8px 10px;'>{0}</td>";
                if (_hotelID > 0)
                {
                    var list = hotelService.All.Where(h => h.IsActive && h.ProvinceID == _provinceID && h.HotelID == _hotelID);
                    string tr = "";
                    int i = 0;
                    foreach (Hotel item in list)
                    {
                        string hotelName = "";
                        string room = "";
                        i++;
                        var roomList = roomService.All.Where(r => r.IsActive && r.HotelID == item.HotelID).ToList();
                        hotelName = " <td style='border-bottom: 1px solid #BDC8CD; border-right: 1px solid #BDC8CD;'><span style='padding: 5px; float: left;'><a title='" + item.Name + "' href='http://vietrip.vn" + UrlHelp.getHotelUrl(item.Province.Name, item.Name, item.HotelID, item.ProvinceID) + "'>" + item.Name + "</a></span><br/><span style='padding: 5px; float: left; color: #999999;'>" + item.Address + "</span></td>";
                        foreach (Room _room in roomList)
                        {
                            List<RoomPrice> listprice = roomPriceService.All.Where(rp => rp.IsActive && rp.Quantity > 0 && rp.RoomID == _room.RoomID && rp.HotelID == item.HotelID && rp.Date >= _fromDate && rp.Date <= _fromDate).ToList();
                            if (listprice.Count > 0)
                            {
                                Decimal price = listprice.Average(rp => rp.Price);
                                if (price > 0)
                                {
                                    room += "<tr><td style='border-bottom: 1px solid #000; border-right: 1px solid #000; width: 302px;'><span style='padding: 5px; float: left;width: 250px;'>" + _room.Name + "</span> <br/><span style='width: 250px;padding: 5px; float: left; color: #999999;'>" + _room.Description + "</span>" + "</td><td style='border-bottom: 1px solid #000;'><span style='padding: 5px; float: left;'>" + String.Format("{0:#,###} VND", price) + "</span></td></tr>";
                                }
                            }
                        }
                        if (i % 2 == 0)
                        {
                            tr += "<tr>" + hotelName + "<td colspan='2' style='border-bottom: 1px solid #BDC8CD;'> <table border='0' width='100%' cellpadding='0' cellspacing='0'>" + room + "  </table></td><tr>";
                        }
                        else
                        {
                            tr += "<tr style='background-color: #E5F0FF;'>" + hotelName + "<td colspan='2' style='border-bottom: 1px solid #BDC8CD;'> <table border='0' width='100%' cellpadding='0' cellspacing='0'>" + room + "  </table></td><tr>";
                        }
                    }
                    Config config = configService.All.Where(c => c.IsActive && c.Name == "EmailTemplateNhanGiaTot").FirstOrDefault();
                    if (config != null)
                    {
                        string subject = "Email báo giá từ www.Vietrip.vn";
                        string body = string.Format(config.Value, _province.Name, FromDate, ToDate, tr);
                        List<string> emailTo = new List<string>();

                        emailTo.Add(Email);
                        int success = EmailHelper.SMTPSendEmail(emailTo, string.Empty, subject, string.Empty, body);
                        return Redirect("/khach-san?s=r");
                    }
                }
                else
                {
                    var list = hotelService.All.Where(h => h.IsActive && h.ProvinceID == _provinceID);
                    string tr = "";
                    int i = 0;
                    foreach (Hotel item in list)
                    {
                        string hotelName = "";
                        string room = "";
                        i++;
                        var roomList = roomService.All.Where(r => r.IsActive && r.HotelID == item.HotelID);
                        hotelName = " <td style='border-bottom: 1px solid #BDC8CD; border-right: 1px solid #BDC8CD;'><span style='padding: 5px; float: left;'><a title='" + item.Name + "' href='http://vietrip.vn" + UrlHelp.getHotelUrl(item.Province.Name, item.Name, item.HotelID, item.ProvinceID) + "'>" + item.Name + "</a></span><br/><span style='padding: 5px; float: left; color: #999999;'>" + item.Address + "</span></td>";
                        foreach (Room _room in roomList)
                        {
                            List<RoomPrice> listprice = roomPriceService.All.Where(rp => rp.IsActive && rp.Quantity > 0 && rp.RoomID == _room.RoomID && rp.HotelID == item.HotelID && rp.Date >= _fromDate && rp.Date <= _fromDate).ToList();
                            if (listprice.Count > 0)
                            {
                                Decimal price = listprice.Average(rp => rp.Price);
                                if (price > 0)
                                {
                                    room += "<tr><td style='border-bottom: 1px solid #000; border-right: 1px solid #000; width: 302px;'><span style='padding: 5px; float: left;width: 250px;'>" + _room.Name + "</span> <br/><span style='width: 250px;padding: 5px; float: left; color: #999999;'>" + _room.Description + "</span>" + "</td><td style='border-bottom: 1px solid #000;'><span style='padding: 5px; float: left;'>" + String.Format("{0:#,###} VND", price) + "</span></td></tr>";
                                }
                            }
                        }
                        if (i % 2 == 0)
                        {
                            tr += "<tr>" + hotelName + "<td colspan='2' style='border-bottom: 1px solid #BDC8CD;'> <table border='0' width='100%' cellpadding='0' cellspacing='0'>" + room + "  </table></td><tr>";
                        }
                        else
                        {
                            tr += "<tr style='background-color: #E5F0FF;'>" + hotelName + "<td colspan='2' style='border-bottom: 1px solid #BDC8CD;'> <table border='0' width='100%' cellpadding='0' cellspacing='0'>" + room + "  </table></td><tr>";
                        }
                    }
                    Config config = configService.All.Where(c => c.IsActive && c.Name == "EmailTemplateNhanGiaTot").FirstOrDefault();
                    if (config != null)
                    {
                        string subject = "Email báo giá từ www.Vietrip.vn";
                        string body = string.Format(config.Value, _province.Name, FromDate, ToDate, tr);
                        List<string> emailTo = new List<string>();

                        emailTo.Add(Email);
                        int success = EmailHelper.SMTPSendEmail(emailTo, string.Empty, subject, string.Empty, body);
                        return Redirect("/khach-san?s=r");
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(province))
                {
                    RouteValueDictionary obj = new RouteValueDictionary();
                    obj.Add("key", HttpUtility.HtmlEncode(province));
                    return RedirectToAction("Find", "Search", obj);
                }
            }
            return Redirect("/?s=mail");
        }
        public ActionResult getProvince(string term)
        {
            var list = provinceService.All.Where(p => p.IsActive).OrderByDescending(p => p.Order).ToList();
            List<JsonResult> result = new List<JsonResult>();
            foreach (Province item in list)
            {
                string str = UrlHelp.NormalizeStringForUrl(item.Name).Replace("-", " ");
                if (str.Contains(UrlHelp.NormalizeStringForUrl(term).Replace("-", " ")))
                {
                    result.Add(new JsonResult { value = item.ProvinceID, label = item.Name, desc = 1 });
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getHotel(string term, string provinceID)
        {
            int _provinceID = Protector.Int(provinceID);
            var list = hotelService.All.Where(h => h.ProvinceID == _provinceID && h.IsActive).OrderByDescending(h => h.Order);
            List<JsonResult> result = new List<JsonResult>();
            foreach (Hotel item in list)
            {
                string str = UrlHelp.NormalizeStringForUrl(item.Name).Replace("-", " ");
                if (str.Contains(UrlHelp.NormalizeStringForUrl(term).Replace("-", " ")))
                {
                    result.Add(new JsonResult { value = item.HotelID, label = item.Name, desc = item.Star });
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Sitemap()
        {
            SitemapModel info = new SitemapModel();
            info.Categories = categoryService.All.Where(c => c.IsActive).ToList();
            info.Provinces = provinceService.All.Where(c => c.IsActive).ToList();
            info.Hotels = hotelService.All.Where(c => c.IsActive).ToList();
            return PartialView("Sitemap", info);
        }
        #endregion

        #region Post Partial
        [HttpPost]
        public ActionResult Home_Search_Hotel(string province, string provinceID, string hotel, string hotelID, string FromDate, string ToDate)
        {
            int _provinceID = Protector.Int(provinceID);
            int _hotelID = Protector.Int(hotelID);
            if (_provinceID > 0)
            {
                RouteValueDictionary obj = new RouteValueDictionary();
                obj.Add("p", _provinceID);
                if (!string.IsNullOrEmpty(FromDate))
                {
                    obj.Add("fd", HttpUtility.UrlEncode(FromDate.Trim().Replace("/", "_")));
                }
                if (!string.IsNullOrEmpty(ToDate))
                {
                    obj.Add("td", HttpUtility.UrlEncode(ToDate.Trim().Replace("/", "_")));
                }
                if (_hotelID > 0)
                {
                    string url = UrlHelp.getHotelUrl(Protector.String(province), Protector.String(hotel), _hotelID, _provinceID);
                    if (!string.IsNullOrEmpty(FromDate))
                    {
                        url += "?fd=" + HttpUtility.UrlEncode(FromDate.Trim().Replace("/", "_"));
                        if (!string.IsNullOrEmpty(ToDate))
                        {
                            url += "&td=" + HttpUtility.UrlEncode(ToDate.Trim().Replace("/", "_"));
                        }
                    }

                    else
                    {
                        if (!string.IsNullOrEmpty(ToDate))
                        {
                            url += "?td=" + HttpUtility.UrlEncode(ToDate.Trim().Replace("/", "_"));
                        }
                    }
                    return Redirect(url);
                }

                return RedirectToAction("Index", "Search", obj);
            }
            else
            {
                if (!string.IsNullOrEmpty(province))
                {
                    RouteValueDictionary obj = new RouteValueDictionary();
                    obj.Add("key", HttpUtility.HtmlEncode(province));
                    return RedirectToAction("Find", "Search", obj);
                }
            }
            return View();
        }


        public ActionResult Home_Search_SentMail(string province, string provinceID, string hotel, string hotelID, string FromDate, string ToDate, string Email)
        {
            int _provinceID = Protector.Int(provinceID);
            int _hotelID = Protector.Int(hotelID);
            Province _province = provinceService.Find(_provinceID);
            if (_province != null && !string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate) && !string.IsNullOrEmpty(Email) && EmailHelper.IsValidEmail(Email))
            {
                DateTime _fromDate = DateTime.Now;
                DateTime _toDate = _fromDate.AddDays(1);
                try
                {
                    string[] day = FromDate.Split('/');
                    string[] day1 = ToDate.Split('/');
                    _fromDate = new DateTime(Protector.Int(day[2]), Protector.Int(day[1]), Protector.Int(day[0]));
                    _toDate = new DateTime(Protector.Int(day1[2]), Protector.Int(day1[1]), Protector.Int(day1[0]));

                }
                catch { }
                Config _config = configService.Get(c => c.Name == "EmailTemplateNhanGiaTot" && c.IsActive);

                string hotelRoom = "<td colspan='' style='border-bottom: 1px solid #BDC8CD; padding: 8px 5px 8px 10px;'>{0}</td>";
                if (_hotelID > 0)
                {
                    List<Hotel> list = hotelService.All.Where(h => h.IsActive && h.ProvinceID == _provinceID && h.HotelID == _hotelID).ToList();
                    string tr = "";
                    int i = 0;
                    foreach (Hotel item in list)
                    {
                        string hotelName = "";
                        string room = "";
                        i++;
                        List<Room> roomList = roomService.All.Where(r => r.IsActive && r.HotelID == item.HotelID).ToList();
                        hotelName = " <td style='border-bottom: 1px solid #BDC8CD; border-right: 1px solid #BDC8CD;'><span style='padding: 5px; float: left;'>" + item.Name + "</span><br/><span style='padding: 5px; float: left; color: #999999;'>" + item.Address + "</span></td>";
                        foreach (Room _room in roomList)
                        {
                            List<RoomPrice> listprice = roomPriceService.All.Where(rp => rp.IsActive && rp.Quantity > 0 && rp.RoomID == _room.RoomID && rp.HotelID == item.HotelID && rp.Date >= _fromDate && rp.Date <= _fromDate).ToList();
                            if (listprice.Count > 0)
                            {
                                Decimal price = listprice.Average(rp => rp.Price);
                                if (price > 0)
                                {
                                    room += "<tr><td style='border-bottom: 1px solid #000; border-right: 1px solid #000; width: 302px;'><span style='padding: 5px; float: left;width: 250px;'>" + _room.Name + "</span> <br/><span style='width: 250px;padding: 5px; float: left; color: #999999;'>" + _room.Description + "</span>" + "</td><td style='border-bottom: 1px solid #000;'><span style='padding: 5px; float: left;'>" + String.Format("{0:#,###} VND", price) + "</span></td></tr>";
                                }
                            }
                        }
                        if (i % 2 == 0)
                        {
                            tr += "<tr>" + hotelName + "<td colspan='2' style='border-bottom: 1px solid #BDC8CD;'> <table border='0' width='100%' cellpadding='0' cellspacing='0'>" + room + "  </table></td><tr>";
                        }
                        else
                        {
                            tr += "<tr style='background-color: #E5F0FF;'>" + hotelName + "<td colspan='2' style='border-bottom: 1px solid #BDC8CD;'> <table border='0' width='100%' cellpadding='0' cellspacing='0'>" + room + "  </table></td><tr>";
                        }
                    }
                    Config config = configService.All.Where(c => c.IsActive && c.Name == "EmailTemplateNhanGiaTot").FirstOrDefault();
                    if (config != null)
                    {
                        string subject = "Email báo giá từ www.Vietrip.vn";
                        string body = string.Format(config.Value, _province.Name, FromDate, ToDate, tr);
                        List<string> emailTo = new List<string>();

                        emailTo.Add(Email);
                        int success = EmailHelper.SMTPSendEmail(emailTo, string.Empty, subject, string.Empty, body);
                        return Redirect("/?s=r");
                    }
                }
                else
                {
                    var list = hotelService.All.Where(h => h.IsActive && h.ProvinceID == _provinceID);
                    string tr = "";
                    int i = 0;
                    foreach (Hotel item in list)
                    {
                        string hotelName = "";
                        string room = "";
                        i++;
                        var roomList = roomService.All.Where(r => r.IsActive && r.HotelID == item.HotelID);
                        hotelName = " <td style='border-bottom: 1px solid #BDC8CD; border-right: 1px solid #BDC8CD;'><span style='padding: 5px; float: left;'>" + item.Name + "</span><br/><span style='padding: 5px; float: left; color: #999999;'>" + item.Address + "</span></td>";
                        foreach (Room _room in roomList)
                        {
                            List<RoomPrice> listprice = roomPriceService.All.Where(rp => rp.IsActive && rp.Quantity > 0 && rp.RoomID == _room.RoomID && rp.HotelID == item.HotelID && rp.Date >= _fromDate && rp.Date <= _fromDate).ToList();
                            if (listprice.Count > 0)
                            {
                                Decimal price = listprice.Average(rp => rp.Price);
                                if (price > 0)
                                {
                                    room += "<tr><td style='border-bottom: 1px solid #000; border-right: 1px solid #000; width: 302px;'><span style='padding: 5px; float: left;width: 250px;'>" + _room.Name + "</span> <br/><span style='width: 250px;padding: 5px; float: left; color: #999999;'>" + _room.Description + "</span>" + "</td><td style='border-bottom: 1px solid #000;'><span style='padding: 5px; float: left;'>" + String.Format("{0:#,###} VND", price) + "</span></td></tr>";
                                }
                            }
                        }
                        if (i % 2 == 0)
                        {
                            tr += "<tr>" + hotelName + "<td colspan='2' style='border-bottom: 1px solid #BDC8CD;'> <table border='0' width='100%' cellpadding='0' cellspacing='0'>" + room + "  </table></td><tr>";
                        }
                        else
                        {
                            tr += "<tr style='background-color: #E5F0FF;'>" + hotelName + "<td colspan='2' style='border-bottom: 1px solid #BDC8CD;'> <table border='0' width='100%' cellpadding='0' cellspacing='0'>" + room + "  </table></td><tr>";
                        }
                    }
                    Config config = configService.All.Where(c => c.IsActive && c.Name == "EmailTemplateNhanGiaTot").FirstOrDefault();
                    if (config != null)
                    {
                        string subject = "Email báo giá từ www.Vietrip.vn";
                        string body = string.Format(config.Value, _province.Name, FromDate, ToDate, tr);
                        List<string> emailTo = new List<string>();

                        emailTo.Add(Email);
                        int success = EmailHelper.SMTPSendEmail(emailTo, string.Empty, subject, string.Empty, body);
                        return Redirect("/?s=r");
                    }
                }
            }
            else
            {
            }
            return Redirect("/?s=mail");
        }
        #endregion
    }
    public class JsonResult
    {
        public int value { get; set; }
        public string label { get; set; }
        public int desc { get; set; }
    }
}
