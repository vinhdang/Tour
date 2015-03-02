using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Service.Helpers;
using Domain.Entities;
using Website.Models.Order;
using System.Web.Routing;

namespace Website.Controllers
{
    public class OrderController : Controller
    {
        public IRoomService roomService { get; set; }
        public IHotelService hotelService { get; set; }
        public IPaymentService paymentService { get; set; }
        public IOrderService orderService { get; set; }
        public IConfigService configService { get; set; }
        public IOrderInfoService orderInfoService { get; set; }
        public IRoomPriceService roomPriceService { get; set; }
        public OrderController(IRoomService roomService, IHotelService hotelService, IOrderService orderService, IPaymentService paymentService,
           IConfigService configService, IOrderInfoService orderInfoService, IRoomPriceService roomPriceService)
        {
            this.roomService = roomService;
            this.hotelService = hotelService;
            this.orderService = orderService;
            this.paymentService = paymentService;
            this.configService = configService;
            this.orderInfoService = orderInfoService;
            this.roomPriceService = roomPriceService;
        }

        public ActionResult Index(string p, string h)
        {
            int _hotelID = Protector.Int(h);
            int _provinceID = Protector.Int(p);
            List<MyOrder> list = new List<MyOrder>();
            try
            {
                list = Session["MyOrder"] as List<MyOrder>;

            }
            catch { }
            if (_hotelID > 0 && _provinceID > 0 && list != null && list.Count > 0)
            {

                MyHotel hotel = hotelService.getByID(_hotelID);
                CreateOrderModel info = new CreateOrderModel();
                info.Hotel = hotel;
                info.Orders = list;
                info.ProvinceID = _provinceID;
                info.HotelID = _hotelID;
                info.Payments = paymentService.All.Where(pay => pay.IsActive).OrderByDescending(pay => pay.Order).ToList();
                return View(info);
            }
            return Redirect("/");
        }
        [HttpPost]
        public ActionResult Index(CreateOrderModel current, string p, string h, string PaymentType, string RoomType)
        {
            int _hotelID = Protector.Int(h);
            int _provinceID = Protector.Int(p);
            List<MyOrder> list = new List<MyOrder>();
            try
            {
                list = Session["MyOrder"] as List<MyOrder>;

            }
            catch { }
            if (_hotelID > 0 && _provinceID > 0 && list != null && list.Count > 0)
            {
                Hotel info = hotelService.Find(_hotelID);
                if (info != null)
                {
                    Order order = new Order();
                    order.CreatedDate = DateTime.Now;
                    order.StatusID = 1;
                    order.HotelID = info.HotelID;
                    order.NationalID = info.NationalID;
                    order.ProvinceID = info.ProvinceID;
                    order.DistrictID = info.DistrictID;
                    order.AreaID = info.AreaID;
                    order.IP = TextHelper.GetIPAddress();
                    order.FullName = Protector.String(current.FullName);
                    order.Email = Protector.String(current.Email);
                    order.Phone = Protector.String(current.Phone);
                    order.Address = Protector.String(current.Address);
                    order.PaymentType = Protector.String(PaymentType);
                    order.RoomType = Protector.Int(RoomType);
                    order.Note = current.Content;
                    order.SiteID = 1;
                    order.OrderCode = "1";
                    order.CompanyName = Protector.String(current.CompanyName);
                    order.CompanyAddress = Protector.String(current.CompanyAddress);
                    order.MST = Protector.String(current.MST);
                    orderService.Insert(order);
                    orderService.Save();
                    if (DateTime.Today.Day > 9)
                    {
                        order.OrderCode = DateTime.Today.Day.ToString();
                    }
                    else { order.OrderCode = "0" + DateTime.Today.Day.ToString(); }
                    if (DateTime.Today.Month > 9)
                    {
                        order.OrderCode += DateTime.Today.Month.ToString();
                    }
                    else { order.OrderCode += "0" + DateTime.Today.Month.ToString(); }
                    order.OrderCode += DateTime.Now.Year.ToString() + order.OrderID.ToString();
                    orderService.Save();
                    decimal TotalPrice = 0;
                    string tr = "";
                    foreach (MyOrder item in list)
                    {
                        OrderInfo _orderInfo = new OrderInfo();
                        _orderInfo.OrderID = order.OrderID;

                        _orderInfo.RoomID = item.RoomID;
                        _orderInfo.RoomPrice = item.RoomPrice;
                        _orderInfo.Quantity = item.Quantity;
                        decimal VATPrice = _orderInfo.RoomPrice / 10 + ((_orderInfo.RoomPrice / 100) * (Decimal)5.5);
                        TimeSpan span = item.ToDate.Subtract(item.FromDate);
                        int NumberNight = span.Days;
                        _orderInfo.TotalPrice = NumberNight * _orderInfo.Quantity * (_orderInfo.RoomPrice + VATPrice);
                        TotalPrice = TotalPrice + _orderInfo.TotalPrice;
                        _orderInfo.FromDate = item.FromDate;
                        _orderInfo.ToDate = item.ToDate;
                        orderInfoService.Insert(_orderInfo);
                        orderInfoService.Save();
                        Room _room = roomService.Find(item.RoomID);

                        tr += "<tr>"
      + "<td style='border: solid 1px #c2c2c2; color: #000000; margin: 0; padding: 5px; text-align: center;border-top: 0px; border-right: 0px;'><a href='http://vietrip.vn" + UrlHelp.getHotelUrl(info.Province.Name, info.Name, info.HotelID, info.ProvinceID) + "'>" + info.Name + "</a></td>"
       + "<td style='border: solid 1px #c2c2c2; color: #000000; margin: 0; padding: 5px; text-align: center;border-top: 0px; border-right: 0px;'>" + _room.Name + "</td>"
       + "<td style='border: solid 1px #c2c2c2; color: #000000; margin: 0; padding: 5px; text-align: center;border-top: 0px; border-right: 0px;'>" + _orderInfo.Quantity + "</td>"
       + "<td style='border: solid 1px #c2c2c2; color: #000000; margin: 0; padding: 5px; text-align: center;border-top: 0px; border-right: 0px;'>" + string.Format("{0:dd/MM/yyyy}", _orderInfo.FromDate) + "</td>"
      + "<td style='border: solid 1px #c2c2c2; color: #000000; margin: 0; padding: 5px; text-align: center;border-top: 0px; border-right: 0px;'>" + string.Format("{0:dd/MM/yyyy}", _orderInfo.ToDate) + "</td>"

       + "<td style='border: solid 1px #c2c2c2; color: #000000; margin: 0; padding: 5px; text-align: center;border-top: 0px; border-right: 0px;'>" + NumberNight + "</td>"
       + "<td style='border: solid 1px #c2c2c2; color: #000000; margin: 0; padding: 5px; text-align: center;border-top: 0px; border-right: 0px;'>" + String.Format("{0:#,###}", _orderInfo.RoomPrice) + "</td>"
      + "<td style='border: solid 1px #c2c2c2; color: #000000; margin: 0; padding: 5px; text-align: center;border-top: 0px; border-right: 0px;'>" + String.Format("{0:#,###}", VATPrice) + "</td>"
       + "<td style='border: solid 1px #c2c2c2; color: #000000; margin: 0; padding: 5px; text-align: center;border-top: 0px;'>" + String.Format("{0:#,###}", _orderInfo.TotalPrice) + " </td>"
   + "</tr>";
                    }
                    tr = tr + "<tr style='background-color: #EDEEEE;'>"
                   + "<td colspan='8' style='border: solid 1px #c2c2c2; border-top: 0px; border-right: 0px; padding-right: 10px;text-align: right; font-weight: bold;'> Số tiền phải trả  </td>"
                   + "<td style='border: solid 1px #c2c2c2; font-weight: bold; color: Red; font-size: 15px;border-top: 0px;'>" + String.Format("{0:#,###}", TotalPrice) + "</td></tr>";

                    Config config = configService.All.Where(c => c.IsActive && c.Name == "EmailTemplateOrder").FirstOrDefault();
                    if (config != null)
                    {
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
                        emailTo.Add(order.Email);
                        string subject = "www.Vietrip.vn thong bao booking " + order.OrderCode;
                        string roomtype = "";
                        switch (order.RoomType)
                        {
                            case 1:
                                roomtype = "Phòng không hút thuốc";
                                break;
                            case 2:
                                roomtype = "Phòng hút thuốc";
                                break;
                        }
                        int payID = Protector.Int(PaymentType);
                        Payment pay = paymentService.Get(_pay => _pay.IsActive && _pay.PaymentID == payID);
                        string payName = "";
                        if (pay != null)
                        {
                            payName = pay.Name;
                        }
                        string body = string.Format(config.Value, order.FullName, order.OrderCode, String.Format("{0:HH:mm:ss}", order.CreatedDate), String.Format("{0:dd/MM/yyyy}", order.CreatedDate), tr, order.OrderCode, Protector.String(order.FullName), Protector.String(order.Phone), Protector.String(order.Email), Protector.String(order.Address), roomtype, Protector.String(order.Note), Protector.String(payName), Protector.String(order.CompanyName), Protector.String(order.CompanyAddress), Protector.String(order.MST));
                        int success = EmailHelper.SMTPSendEmail(emailTo, string.Empty, subject, string.Empty, body);
                        RouteValueDictionary obj = new RouteValueDictionary();
                        obj.Add("h", info.HotelID);
                        obj.Add("or", order.OrderID);

                        return RedirectToAction("Result", "Order", obj);
                    }
                }

            }
            return Redirect("/");
        }
        public ActionResult Result(string or, string h)
        {
            long _orderID = Protector.Long(or);
            int HotelID = Protector.Int(h);
            Order order = orderService.Find(_orderID);
            if (order != null)
            {
                MyHotel info = hotelService.getByID(HotelID);

                if (info != null)
                {

                    OrderResultModel result = new OrderResultModel();
                    result.OrderID = order.OrderID;
                    result.PageTitle = info.Name;
                    result.Description = info.Description;
                    result.Keyword = info.KeyWord;
                    result.HotelID = info.HotelID;
                    Config config = configService.All.Where(c => c.IsActive && c.Name == "CompleteBooking").FirstOrDefault();
                    if (config != null)
                    {
                        result.Html = string.Format(config.Value, order.OrderCode);
                    }
                    return View(result);
                }
            }
            return Redirect("/");

        }
        public ActionResult ViewOrder(string orderID)
        {
            long _orderID = Protector.Long(orderID);
            Order _order = orderService.Find(_orderID);
            if (_order != null)
            {
                MyHotel info = hotelService.getByID(_order.HotelID);
                ViewOrderModel order = new ViewOrderModel();
                order.Hotel = info;
                order.PageTitle = info.Name;
                order.Description = info.Description;
                order.Keyword = info.KeyWord;
                order.Hotel = info;
                order.Payments = paymentService.All.Where(p => p.IsActive).OrderByDescending(p => p.Order).ToList();
                List<OrderInfo> _listOrder = orderInfoService.All.Where(o => o.OrderID == _order.OrderID && o.Order.HotelID == info.HotelID).ToList();
                List<MyOrder> list = new List<MyOrder>();
                if (_listOrder != null && _listOrder.Count > 0)
                {
                    foreach (OrderInfo item in _listOrder)
                    {
                        MyOrder _morder = new MyOrder();
                        Room _room = roomService.Get(r => r.RoomID == item.RoomID);
                        if (_room != null)
                        {
                            _morder.Name = _room.Name;
                            _morder.RoomID = _room.RoomID;
                            _morder.RoomPrice = item.RoomPrice;
                            _morder.Quantity = item.Quantity;
                            _morder.ToDate = item.ToDate;
                            _morder.FromDate = item.FromDate;
                            TimeSpan span = _morder.ToDate.Subtract(_morder.FromDate);
                            _morder.NumberNight = span.Days;
                            _morder.VATPrice = item.RoomPrice / 10 + ((item.RoomPrice / 100) * (Decimal)5.5);
                            _morder.TotalPrice = item.TotalPrice;
                            list.Add(_morder);
                        }
                    }
                }

                order.Orders = list;
                order.FullName = _order.FullName;
                order.Email = _order.Email;
                order.Phone = _order.Phone;
                order.Address = _order.Address;
                order.Content = _order.Note;
                order.CompanyName = _order.CompanyName;
                order.CompanyAddress = _order.CompanyAddress;
                order.MST = _order.MST;
                order.RoomType = _order.RoomType;
                order.PaymentID = Protector.Int(_order.PaymentType);
                return View(order);
            }
            return Redirect("/");
        }
    }
}