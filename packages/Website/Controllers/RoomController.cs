using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Domain.Entities;
using Website.Models.Room;
using Service.Helpers;
using System.Web.Routing;
using Website.Models.Order;

namespace Website.Controllers
{
    public class RoomController : Controller
    {
        public IHotelService hotelService { get; set; }
        public IRoomTypeService roomTypeService { get; set; }
        public IRoomService roomService { get; set; }
        public IRoomPriceService roomPriceService { get; set; }
        public IConfigService configService { get; set; }
        public IEmailGetPriceService mailGetPriceService { get; set; }
        public RoomController(IEmailGetPriceService mailGetPriceService, IConfigService configService, IRoomService roomService, IRoomPriceService roomPriceService, IHotelService hotelService, IRoomTypeService roomTypeService)
        {
            this.roomService = roomService;
            this.hotelService = hotelService;
            this.roomTypeService = roomTypeService;
            this.roomPriceService = roomPriceService;
            this.configService = configService;
            this.mailGetPriceService = mailGetPriceService;

        }
        #region Partial
        public ActionResult RoomList(Hotel hotel, string fd, string td, string nr, string ng)
        {
            if (hotel != null)
            {
                var list = new RoomListModel();
                list.Hotel = hotel;
                list.NumberRoom = Protector.String(nr);
                list.NumberGuest = Protector.String(ng);
                if (!string.IsNullOrEmpty(fd) && !string.IsNullOrEmpty(td))
                {
                    try
                    {
                        list.FromDate = Protector.String(fd.Replace("_", "/"));

                        list.ToDate = Protector.String(td.Replace("_", "/"));
                        list.RoomList = roomService.All.Where(r => r.IsActive && r.HotelID == hotel.HotelID).OrderByDescending(r => r.Order).ToList();
                    }
                    catch
                    {

                    }
                }
                else
                {
                    list.RoomList = roomService.All.Where(r => r.IsActive && r.HotelID == hotel.HotelID).OrderByDescending(r => r.Order).ToList();
                    list.FromDate = DateTime.Today.Day + "/" + DateTime.Today.Month + "/" + DateTime.Today.Year;
                    list.ToDate = DateTime.Today.Day + 1 + "/" + DateTime.Today.Month + "/" + DateTime.Today.Year;
                }
                return PartialView("RoomList", list);
            }
            return PartialView("RoomList");
        }
        [HttpPost]
        public ActionResult RoomList(string ProvinceName, string provinceID, string HotelName, string hotelID, string FromDate, string ToDate, string nr, string ng)
        {
            int _provinceID = Protector.Int(provinceID);
            int _hotelID = Protector.Int(hotelID);
            if (_provinceID > 0 && _hotelID > 0)
            {
                RouteValueDictionary obj = new RouteValueDictionary();
                string url = UrlHelp.getHotelUrl(Protector.String(ProvinceName), Protector.String(HotelName), _hotelID, _provinceID);
                if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
                {
                    url += "?fd=" + HttpUtility.UrlEncode(FromDate.Trim().Replace("/", "_"));
                    if (!string.IsNullOrEmpty(ToDate))
                    {
                        url += "&td=" + HttpUtility.UrlEncode(ToDate.Trim().Replace("/", "_"));
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
                return Redirect(url);
            }
            return View();
        }
        [HttpGet]
        public ActionResult RoomListResult(List<Room> rooms, string h, string p, string fd, string td, string nr, string ng)
        {
            var list = new RoomListResultModel();
            list.NumberRoom = Protector.String(nr);
            list.NumberGuest = Protector.String(ng);
            if (!string.IsNullOrEmpty(fd) && !string.IsNullOrEmpty(td))
            {
                list.FromDate = Protector.String(fd);
                list.ToDate = Protector.String(td);
            }
            else
            {
                list.FromDate = DateTime.Today.Day + "_" + DateTime.Today.Month + "_" + DateTime.Today.Year;
                list.ToDate = DateTime.Today.Day + 1 + "_" + DateTime.Today.Month + "_" + DateTime.Today.Year;
            }
            list.HotelID = Protector.String(h);
            list.ProvinceID = Protector.String(p);
            list.RoomList = rooms;
            return PartialView("RoomListResult", list);
        }
        [HttpPost]
        public ActionResult RoomListResult(string h, string p, string fd, string td, string nr, string ng, string[] Quantity)
        {
            int _provinceID = Protector.Int(p);
            int _hotelID = Protector.Int(h);

            if (_provinceID > 0 && _hotelID > 0)
            {
                if (Quantity != null && Quantity.Length > 0)
                {
                    List<MyOrder> list = new List<MyOrder>();
                    foreach (string item in Quantity)
                    {
                        string[] str = item.Split('_');
                        if (str != null && str.Length > 0)
                        {

                            try
                            {
                                long RoomID = Protector.Int(str[0]);
                                int _quantity = Protector.Int(str[4]);
                                Decimal _roomPrice = Protector.Decimal(str[2]);
                                if (_quantity > 0 && RoomID > 0)
                                {
                                    MyOrder info = new MyOrder();
                                    info.HotelID = _hotelID;
                                    info.Quantity = _quantity;
                                    info.RoomID = RoomID;
                                    info.RoomPrice = _roomPrice;
                                    info.ProvinceID = _provinceID;
                                    try
                                    {
                                        string[] day = fd.Split('_');
                                        info.FromDate = new DateTime(Protector.Int(day[2]), Protector.Int(day[1]), Protector.Int(day[0]));
                                        string[] day1 = td.Split('_');
                                        info.ToDate = new DateTime(Protector.Int(day1[2]), Protector.Int(day1[1]), Protector.Int(day1[0]));
                                    }
                                    catch { }
                                    list.Add(info);
                                }

                            }
                            catch { }
                        }
                    }
                    Session["MyOrder"] = list;
                    Hotel _hotel = hotelService.Get(_h => _h.HotelID == _hotelID && _h.ProvinceID == _provinceID);
                    if (_hotel != null)
                    {
                        return Redirect(UrlHelp.getOrderUrl(_hotel.Province.Name, _hotel.Name, _hotel.HotelID, _hotel.ProvinceID));
                    }
                }
            }
            return Redirect("/");
        }
        public ActionResult Price(Room r, string fd, string td, string nr, string ng)
        {
            if (r != null)
            {
                DateTime _fromDate = DateTime.Today;
                DateTime _toDate = DateTime.Today;
                if (!string.IsNullOrEmpty(fd))
                {
                    try
                    {
                        string[] day = fd.Split('_');
                        _fromDate = new DateTime(Protector.Int(day[2]), Protector.Int(day[1]), Protector.Int(day[0]));
                    }
                    catch { }
                }
                else
                {
                    _fromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                }
                if (!string.IsNullOrEmpty(td))
                {
                    try
                    {
                        string[] day = td.Split('_');
                        _toDate = new DateTime(Protector.Int(day[2]), Protector.Int(day[1]), Protector.Int(day[0]));
                    }
                    catch { }
                }
                else
                {
                    _toDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day + 1);
                }
                PriceModel price = new PriceModel();
                price.Room = r;
                price.NumberRoom = Protector.String(nr);
                price.NumberGuest = Protector.String(ng);
                price.FromDate = Protector.String(_fromDate.Day + "_" + _fromDate.Month + "_" + _fromDate.Year);
                price.ToDate = Protector.String(_toDate.Day + "_" + _toDate.Month + "_" + _toDate.Year);
                List<RoomPrice> list = roomPriceService.All.Where(p => p.HotelID == r.HotelID && p.RoomID == r.RoomID && p.Date >= _fromDate && p.Date < _toDate && p.IsActive && p.Quantity > 0).ToList();
                if (list != null && list.Count > 0)
                {
                    price.Price = list.Average(rp => rp.Price);
                }
                else { price.Price = 0; }
                return PartialView("Price", price);
            }
            return PartialView("Price");
        }
        public ActionResult PriceList(int h, long r, string fd, string td)
        {
            if (r != null && h != null && r > 0 && !string.IsNullOrEmpty(fd) && !string.IsNullOrEmpty(td))
            {
                DateTime _fromDate = DateTime.Today;
                DateTime _toDate = DateTime.Today;
                try
                {
                    string[] day = fd.Split('_');
                    _fromDate = new DateTime(Protector.Int(day[2]), Protector.Int(day[1]), Protector.Int(day[0]));
                    string[] day1 = td.Split('_');
                    _toDate = new DateTime(Protector.Int(day1[2]), Protector.Int(day1[1]), Protector.Int(day1[0]));
                }
                catch { }
                List<RoomPrice> list = roomPriceService.All.Where(p => p.IsActive && p.Quantity > 0 && p.HotelID == h && p.RoomID == r && p.Date >= _fromDate && p.Date < _toDate).ToList();
                return PartialView("PriceList", list);
            }
            return PartialView("PriceList");
        }
        [HttpGet]
        public ActionResult GetPrice(int h, long r, string fd, string td)
        {
            int _roomID = Protector.Int(r);
            int _hotelID = Protector.Int(h);
            if (_roomID > 0 && _hotelID > 0)
            {
                var info = new CreateGetPriceModel();
                info.hotelID = _hotelID;
                info.roomID = _roomID;
                info.fromdate = fd;
                info.todate = td;
                return PartialView("GetPrice", info);
            }
            return PartialView("GetPrice");
        }
        [HttpPost]
        public ActionResult GetPrice(CreateGetPriceModel current, string CaptchaValue, string InvisibleCaptchaValue, string h, string r, string fd, string td)
        {
            if (current != null)
            {
                bool cv = CaptchaController.IsValidCaptchaValue(CaptchaValue.ToUpper());
                bool icv = InvisibleCaptchaValue == "";

                if (!cv || !icv)
                {
                    return Redirect("/");
                }
                if (!ModelState.IsValid)
                {
                    return View(current);
                }
                int _roomID = Protector.Int(r);
                int _hotelID = Protector.Int(h);
                if (_roomID > 0 && _hotelID > 0)
                {
                    DateTime _fromDate = DateTime.Today;
                    DateTime _toDate = DateTime.Today;
                    string[] day = fd.Split('_');
                    _fromDate = new DateTime(Protector.Int(day[2]), Protector.Int(day[1]), Protector.Int(day[0]));
                    string[] day1 = td.Split('_');
                    _toDate = new DateTime(Protector.Int(day1[2]), Protector.Int(day1[1]), Protector.Int(day1[0]));
                    List<RoomPrice> list = roomPriceService.All.Where(pr => pr.HotelID == _hotelID && pr.RoomID == _roomID && pr.Date >= _fromDate && pr.Date < _toDate && pr.Quantity > 0 && pr.IsActive).ToList();
                    if (list != null && list.Count > 0)
                    {
                        Config config = configService.All.Where(c => c.IsActive && c.Name == "EmailTemplateNhanGiaPhong").FirstOrDefault();
                        if (config != null)
                        {
                            Room room = roomService.Find(_roomID);
                            if (room != null)
                            {
                                string subject = "Email báo giá phòng từ www.Vietrip.vn";
                                string table = " <table border='0' cellpadding='0' cellspacing='0' width='100%' style='border: solid 1px #0984C0;border-bottom: solid 0px #0984C0;'>";
                                string headerTr = "<tr style='background-color: #E5F0FF;'>"
                                                        + "<td style='color: #000000; padding: 5px; border-right: solid 1px #0984C0; border-bottom: solid 1px #0984C0;'>"
                                                        + "Tên khách sạn"
                                                      + "</td>"
                                                      + "<td style='color: #000000; padding: 3px; border-right: solid 1px #0984C0; border-bottom: solid 1px #0984C0;'>"
                                                         + "Tên Phòng"
                                                      + "</td>"
                                                      + "<td style='color: #000000; padding: 3px; border-right: solid 1px #0984C0; border-bottom: solid 1px #0984C0;'>"
                                                       + "Ngày lưu trú"
                                                      + "</td>"
                                                      + "<td style='color: #000000; padding: 3px; border-bottom: solid 1px #0984C0;'>"
                                                      + "Giá phòng / 1 ngày"
                                                     + "</td>"
                                                   + "</tr>";
                                table = table + headerTr;
                                string headerBody = "<tr >"
                                                        + "<td style='color: #000000; padding: 5px; border-right: solid 1px #0984C0; border-bottom: solid 1px #0984C0;'>"
                                                            + "{0}"
                                                        + "</td>"
                                                        + "<td style='color: #000000; padding: 3px; border-right: solid 1px #0984C0; border-bottom: solid 1px #0984C0;'>"
                                                            + "{1}"
                                                        + "</td>"
                                                        + "<td style='color: #000000; padding: 3px; border-right: solid 1px #0984C0; border-bottom: solid 1px #0984C0;'>"
                                                            + "{2}"
                                                        + "</td>"
                                                        + "<td style='color: #000000; padding: 3px; border-bottom: solid 1px #0984C0;'>"
                                                            + "{3}"
                                                        + "</td>"
                                                    + "</tr>";
                                string headerlist = "";
                                foreach (RoomPrice item in list)
                                {
                                    headerlist = headerlist + string.Format(headerBody, item.Hotel.Name, item.Room.Name, string.Format("{0:dd/MM/yyyy}", item.Date), String.Format("{0:#,###}", item.Price));
                                }
                                table = table + headerlist + " </table>";
                                string body = string.Format(config.Value, room.Name, room.Hotel.Name, string.Format("{0:dd/MM/yyyy}", _fromDate), string.Format("{0:dd/MM/yyyy}", _toDate), table);
                                List<string> emailTo = new List<string>();
                                emailTo.Add(current.Email);
                                EmailGetPrice getPrice = new EmailGetPrice();
                                getPrice.Phone = current.Phone.ToString();
                                getPrice.Email = current.Email;
                                getPrice.CreatedDate = DateTime.Now;
                                getPrice.FromDate = _fromDate;
                                getPrice.ToDate = _toDate;
                                getPrice.RoomID = room.RoomID;
                                getPrice.HotelID = room.HotelID;
                                getPrice.ProvinceID = room.Hotel.ProvinceID;
                                getPrice.IP = TextHelper.GetIPAddress();
                                mailGetPriceService.Insert(getPrice);
                                mailGetPriceService.Save();
                                int success = EmailHelper.SMTPSendEmail(emailTo, string.Empty, subject, string.Empty, body);

                                Config configEmail = configService.All.Where(c => c.IsActive && c.Name == "EmailTo").FirstOrDefault();
                                List<string> emailTo1 = new List<string>();
                                if (configEmail != null && !string.IsNullOrEmpty(configEmail.Value))
                                {
                                    string[] emails = configEmail.Value.Split(';');
                                    foreach (string e in emails)
                                    {
                                        emailTo1.Add(e);
                                    }
                                }
                                string subject1 = "www.Vietrip.vn thong bao thong tin lay gia";
                                string body1 = "";
                                body1 += "Email:" + current.Email + "<br/>";
                                body1 += "Điện thoại :" + current.Phone + "<br/>";
                                body1 += "Từ ngày:" + _fromDate + "<br/>";
                                body1 += "Đến ngày :" + _toDate + "<br/>";
                                body1 += "Vị trí :" + room.Hotel.Province.Name + "<br/>";
                                body1 += "Khách sạn :" + room.Hotel.Name + "<br/>";
                                body1 += "Tên phòng :" + room.Name + "<br/>";
                                body1 += "Gởi yêu cầu lúc :" + getPrice.CreatedDate + "<br/>";
                                int success1 = EmailHelper.SMTPSendEmail(emailTo1, string.Empty, subject1, string.Empty, body1);

                                return PartialView("EmailGetPriceResult");
                            }


                        }
                    }
                    else
                    {
                        Room room = roomService.Find(_roomID);
                        if (room != null)
                        {
                            EmailGetPrice getPrice = new EmailGetPrice();
                            getPrice.Phone = current.Phone.ToString();
                            getPrice.Email = current.Email;
                            getPrice.CreatedDate = DateTime.Now;
                            getPrice.FromDate = _fromDate;
                            getPrice.ToDate = _toDate;
                            getPrice.RoomID = room.RoomID;
                            getPrice.HotelID = room.HotelID;
                            getPrice.ProvinceID = room.Hotel.ProvinceID;
                            getPrice.IP = TextHelper.GetIPAddress();
                            mailGetPriceService.Insert(getPrice);
                            mailGetPriceService.Save();
                            Config configEmail = configService.All.Where(c => c.IsActive && c.Name == "EmailTo").FirstOrDefault();
                            List<string> emailTo = new List<string>();
                            if (configEmail != null && !string.IsNullOrEmpty(configEmail.Value))
                            {
                                string[] emails = configEmail.Value.Split(';');
                                foreach (string e in emails)
                                {
                                    emailTo.Add(e);
                                }
                            }
                            string subject = "www.Vietrip.vn thong bao thong tin lay gia";
                            string body = "";
                            body += "Email:" + current.Email + "<br/>";
                            body += "Điện thoại :" + current.Phone + "<br/>";
                            body += "Từ ngày:" + _fromDate + "<br/>";
                            body += "Đến ngày :" + _toDate + "<br/>";
                            body += "Vị trí :" + room.Hotel.Province.Name + "<br/>";
                            body += "Khách sạn :" + room.Hotel.Name + "<br/>";
                            body += "Tên phòng :" + room.Name + "<br/>";
                            body += "Gởi yêu cầu lúc :" + getPrice.CreatedDate + "<br/>";
                            int success = EmailHelper.SMTPSendEmail(emailTo, string.Empty, subject, string.Empty, body);
                            return PartialView("EmailGetPriceResult");
                        }

                    }
                }

            }
            return PartialView("GetPrice");
        }
        public ActionResult getTotal(string quantity, string price, string fd, string td)
        {
            DateTime _fromDate = DateTime.Today;
            DateTime _toDate = DateTime.Today;
            Decimal _price;
            Decimal TotalPrice = 0;
            int _quantity;
            try
            {
                string[] day = fd.Split('_');
                _fromDate = new DateTime(Protector.Int(day[2]), Protector.Int(day[1]), Protector.Int(day[0]));
                string[] day1 = td.Split('_');
                _toDate = new DateTime(Protector.Int(day1[2]), Protector.Int(day1[1]), Protector.Int(day1[0]));
                _quantity = Protector.Int(quantity);
                _price = Protector.Decimal(price);
                TimeSpan span = _toDate.Subtract(_fromDate);
                Decimal VATPrice = _price / 10 + ((_price / 100) * (Decimal)5.5);
                TotalPrice = span.Days * _quantity * (_price + VATPrice);
            }
            catch { }
            return Json(new { total = TotalPrice }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult RoomInfo(Website.Models.Order.MyOrder order)
        {
            if (order != null)
            {
                MyRoom info = new MyRoom();
                Decimal price = 0;
                Room _room = roomService.Find(order.RoomID);
                if (_room != null)
                {
                    List<RoomPrice> list = roomPriceService.All.Where(p => p.IsActive && p.Quantity > 0 && p.HotelID == order.HotelID && p.RoomID == order.RoomID && p.Date >= order.FromDate && p.Date < order.ToDate).ToList();
                    if (list != null && list.Count > 0)
                    {
                        price = list.Average(rp => rp.Price);
                    }
                    info.Price = price;
                    info.Name = _room.Name;
                    TimeSpan span = order.ToDate.Subtract(order.FromDate);
                    info.NumberNight = span.Days;
                    info.Quantity = order.Quantity;
                    info.FromDate = order.FromDate;
                    info.ToDate = order.ToDate;
                    info.VATPrice = price / 10 + ((price / 100) * (Decimal)5.5);
                    info.TotalPrice = info.NumberNight * order.Quantity * (price + info.VATPrice);
                    return PartialView("RoomInfo", info);
                }
            }
            return PartialView("RoomInfo");

        }

        [HttpGet]
        public ActionResult TotalPrice(List<Website.Models.Order.MyOrder> list)
        {
            if (list != null)
            {
                MyRoom info = new MyRoom();
                Decimal _price = 0;
                foreach (Website.Models.Order.MyOrder item in list)
                {
                    List<RoomPrice> listPrice = roomPriceService.All.Where(p => p.IsActive && p.Quantity > 0 && p.HotelID == item.HotelID && p.RoomID == item.RoomID && p.Date >= item.FromDate && p.Date < item.ToDate).ToList();
                    if (list != null && list.Count > 0)
                    {
                        _price = listPrice.Average(rp => rp.Price);
                    }
                    info.Price = _price;
                    TimeSpan span = item.ToDate.Subtract(item.FromDate);
                    info.NumberNight = span.Days;
                    info.VATPrice = _price / 10 + ((_price / 100) * (Decimal)5.5);
                    info.TotalPrice += info.NumberNight * item.Quantity * (_price + info.VATPrice);
                }
                return PartialView("TotalPrice", info);
            }
            return PartialView("TotalPrice");

        }

        #endregion

    }
}
