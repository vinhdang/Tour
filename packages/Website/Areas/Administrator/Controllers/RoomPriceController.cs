using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Domain.Entities;
using Service.Helpers;
using Service.Attribute;
using Website.Areas.Administrator.Models.RoomPrice;
using Website.Helpers;
using Microsoft.Web.Mvc;
using System.Web.Routing;
using Telerik.Web.Mvc.Extensions;

namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class RoomPriceController : Controller
    {
        public IHotelService hotelService { get; set; }
        public IRoomService roomService { get; set; }
        public IRoomPriceService roomPriceService { get; set; }
        public RoomPriceController(IRoomService roomService, IHotelService hotelService, IRoomPriceService roomPriceService)
        {
            this.roomService = roomService;
            this.hotelService = hotelService;
            this.roomPriceService = roomPriceService;
        }
        #region HttpGet
        public ActionResult Index(string id)
        {
            List<RoomPrice> list1 = roomPriceService.All.Where(r => r.Date < DateTime.Today).ToList();
            if (list1 != null && list1.Count > 0)
            {
                foreach (RoomPrice item in list1)
                {
                    roomPriceService.Delete(item);
                    roomPriceService.Save();
                }
            }
            int hotelID = Protector.Int(id);
            List<RoomPrice> list = roomPriceService.All.Where(p => p.HotelID == hotelID).ToList();
            List<Room> rooms = roomService.All.Where(r => r.HotelID == hotelID).ToList();
            ViewBag.RoomList = rooms.ToSelectListItems(-1);
            return View(list);
        }
        [HttpPost]
        [ActionName("Index")]
        [AcceptButton(ButtonName = "Delete")]
        public ActionResult IndexDelete(int[] checkedRecords, string id, string From, string To, string RoomList)
        {
            checkedRecords = checkedRecords ?? new int[] { };
            if (checkedRecords != null && checkedRecords.Length > 0)
            {
                foreach (int item in checkedRecords)
                {
                    RoomPrice roomPrice = roomPriceService.Find(item);
                    if (roomPrice != null)
                    {

                        roomPriceService.Delete(roomPrice);
                        roomPriceService.Save();
                    }
                }
            }
            RouteValueDictionary routeValues = this.GridRouteValues();
            return RedirectToAction("Index", routeValues);
        }

        [HttpPost]
        [ActionName("Index")]
        [AcceptButton(ButtonName = "Search")]
        public ActionResult IndexSearch(string id, string From, string To, string RoomList)
        {
            int hotelID = Protector.Int(id);
            IQueryable<RoomPrice> list = roomPriceService.All.Where(p => p.HotelID == hotelID);
            int roomID = Protector.Int(RoomList, -1);
            if (roomID > 0)
            {
                list = list.Where(r => r.RoomID == roomID);
            }
            List<Room> rooms = roomService.All.Where(r => r.HotelID == hotelID).ToList();
            ViewBag.RoomList = rooms.ToSelectListItems(roomID);
            if (!string.IsNullOrEmpty(From))
            {
                DateTime FromDate = Protector.DateTime(From);
                list = list.Where(r => r.Date >= FromDate);
                ViewBag.From = From;
            }
            if (!string.IsNullOrEmpty(To))
            {
                DateTime ToDate = Protector.DateTime(To);
                list = list.Where(r => r.Date <= ToDate);
                ViewBag.To = To;
            }

            return View(list.ToList());
        }
        [HttpPost]
        [ActionName("Index")]
        [AcceptButton(ButtonName = "Add")]
        public ActionResult IndexDelete(string id)
        {

            return RedirectToAction("Create", new { id = id });
        }
        [HttpGet]
        public ActionResult Create(string id)
        {
            int hotelID = Protector.Int(id);
            Hotel hotel = hotelService.Find(hotelID);
            if (hotel != null)
            {
                var price = new CreateRoomPriceModel();
                price.Hotel = hotel;
                price.IsActive = true;
                price.Rooms = roomService.All.Where(r => r.HotelID == hotel.HotelID).ToSelectListItems(-1);
                price.DateList = GlobalVariables.DateList;
                return View(price);
            }
            return RedirectToAction("Index", "Hotel", null);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            long roomPriceID = Protector.Int(id);
            var roomPrice = new EditRoomPriceModel();
            RoomPrice info = roomPriceService.Find(roomPriceID);
            if (info != null)
            {
                ModelCopier.CopyModel(info, roomPrice);
                var hotel = hotelService.All.Where(h => h.HotelID == info.HotelID).FirstOrDefault();
                if (hotel != null)
                {
                    roomPrice.Hotel = hotel;
                    roomPrice.Room = info.Room;
                    roomPrice.Price = Convert.ToInt64(info.Price);
                }
                return View(roomPrice);
            }
            return RedirectToAction("Index", "Hotel");
        }
        #endregion
        #region HttpPost
        [HttpPost]
        public ActionResult Create(CreateRoomPriceModel roomPrice, string id, string[] dl)
        {
            int hotelID = Protector.Int(id);
            var hotel = hotelService.Find(hotelID);
            if (hotel != null)
            {
                if (!ModelState.IsValid)
                {
                    roomPrice.Hotel = hotel;
                    roomPrice.IsActive = true;
                    roomPrice.Rooms = roomService.All.Where(r => r.HotelID == hotel.HotelID).ToSelectListItems(-1);
                    return View(roomPrice);
                }
                foreach (DateTime day in EachDay(Protector.DateTime(roomPrice.FromDate), Protector.DateTime(roomPrice.ToDate)))
                {
                    DateTime startdate = day.Date;
                    DateTime enddate = startdate.AddHours(23.99);
                    RoomPrice checkPrice = roomPriceService.All.Where(r => r.HotelID == hotel.HotelID && r.RoomID == roomPrice.RoomID && r.Date >= startdate && r.Date <= enddate).FirstOrDefault();
                    if (checkPrice != null)
                    {
                        checkPrice.Quantity = Protector.Int(roomPrice.Quantity);
                        checkPrice.IsActive = roomPrice.IsActive;
                        checkPrice.CreatedDate = DateTime.Now;
                        checkPrice.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
                        switch (day.DayOfWeek.ToString())
                        {
                            case "Friday":
                                if (dl.Contains("Friday"))
                                {
                                    checkPrice.Price = Protector.Decimal(roomPrice.Price1);
                                }
                                else
                                {
                                    checkPrice.Price = Protector.Decimal(roomPrice.Price);
                                }
                                break;
                            case "Saturday":
                                if (dl.Contains("Saturday"))
                                {
                                    checkPrice.Price = Protector.Decimal(roomPrice.Price1);
                                }
                                else
                                {
                                    checkPrice.Price = Protector.Decimal(roomPrice.Price);
                                }
                                break;
                            case "Sunday":
                                if (dl.Contains("Sunday"))
                                {
                                    checkPrice.Price = Protector.Decimal(roomPrice.Price1);
                                }
                                else
                                {
                                    checkPrice.Price = Protector.Decimal(roomPrice.Price);
                                }
                                break;
                            default:
                                checkPrice.Price = Protector.Decimal(roomPrice.Price);
                                break;
                        }

                        roomPriceService.Save();
                    }
                    else
                    {
                        checkPrice = new RoomPrice();
                        checkPrice.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
                        checkPrice.CreatedDate = DateTime.Now;
                        checkPrice.Date = startdate;
                        switch (day.DayOfWeek.ToString())
                        {
                            case "Friday":
                                if (dl.Contains("Friday"))
                                {
                                    checkPrice.Price = Protector.Decimal(roomPrice.Price1);
                                }
                                else
                                {
                                    checkPrice.Price = Protector.Decimal(roomPrice.Price);
                                }
                                break;
                            case "Saturday":
                                if (dl.Contains("Saturday"))
                                {
                                    checkPrice.Price = Protector.Decimal(roomPrice.Price1);
                                }
                                else
                                {
                                    checkPrice.Price = Protector.Decimal(roomPrice.Price);
                                }
                                break;
                            case "Sunday":
                                if (dl.Contains("Sunday"))
                                {
                                    checkPrice.Price = Protector.Decimal(roomPrice.Price1);
                                }
                                else
                                {
                                    checkPrice.Price = Protector.Decimal(roomPrice.Price);
                                }
                                break;
                            default:
                                checkPrice.Price = Protector.Decimal(roomPrice.Price);
                                break;
                        }


                        checkPrice.HotelID = hotel.HotelID;
                        checkPrice.RoomID = roomPrice.RoomID;
                        checkPrice.IsActive = roomPrice.IsActive;
                        checkPrice.Quantity = Protector.Int(roomPrice.Quantity);
                        roomPriceService.Insert(checkPrice);
                        roomPriceService.Save();
                    }
                }
                return RedirectToAction("Index", new { id = hotelID });
            }
            return RedirectToAction("Index", "Hotel");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(EditRoomPriceModel current, string id)
        {
            int roomPriceID = Protector.Int(id);
            RoomPrice info = roomPriceService.All.Where(p => p.RoomPriceID == roomPriceID).FirstOrDefault();
            if (info != null)
            {
                if (!ModelState.IsValid)
                {
                    var hotel = hotelService.Find(info.HotelID);
                    if (hotel != null)
                    {
                        current.Hotel = hotel;
                        current.Room = info.Room;
                        current.Price = Convert.ToInt64(info.Price);
                    }
                    return View(current);
                }
                TryUpdateModel(info);
                roomService.Save();
                return RedirectToAction("Index", "RoomPrice", new { id = info.HotelID });

            }
            return RedirectToAction("Index", "Hotel");
        }
        #endregion
        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}
