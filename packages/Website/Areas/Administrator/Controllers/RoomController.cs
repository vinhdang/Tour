using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Service.Helpers;
using Domain.Entities;
using Microsoft.Web.Mvc;
using Website.Areas.Administrator.Models.Room;

namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class RoomController : Controller
    {
        public IHotelService hotelService { get; set; }
        public IRoomTypeService roomTypeService { get; set; }
        public IRoomService roomService { get; set; }
        public RoomController(IRoomService roomService, IHotelService hotelService, IRoomTypeService roomTypeService)
        {
            this.roomService = roomService;
            this.hotelService = hotelService;
            this.roomTypeService = roomTypeService;
        }
        #region HttpGet
        [HttpGet]
        public ActionResult Create(string id)
        {
            int hotelID = Protector.Int(id);
            var info = hotelService.Get(h => h.HotelID == hotelID);
            if (info != null)
            {
                CreateRoomModel room = new CreateRoomModel();
                room.Hotel = info;
                room.IsActive = true;
                room.Rooms = roomService.All.Where(p => p.HotelID == hotelID);
                room.NumberGuest = 2;
                return View(room);
            }
            return RedirectToAction("Create", new { id = hotelID });
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            int roomID = Protector.Int(id);
            var room = new CreateRoomModel();
            Room current = roomService.Get(p => p.RoomID == roomID);
            if (current != null)
            {
                ModelCopier.CopyModel(current, room);
                var info = hotelService.Get(h => h.HotelID == current.HotelID);
                if (info != null)
                {
                    room.Hotel = info;
                    room.Rooms = roomService.All.Where(p => p.HotelID == info.HotelID);
                }
                return View(room);
            }
            return RedirectToAction("Index", "Hotel");
        }
        #endregion

        #region HttpPost
        [HttpPost]
        public ActionResult Create(CreateRoomModel room, string id)
        {
            int hotelID = Protector.Int(id);
            var info = hotelService.Get(h => h.HotelID == hotelID);
            if (info != null)
            {
                if (!ModelState.IsValid)
                {
                    room.Hotel = info;
                    room.IsActive = true;
                    room.Rooms = roomService.All.Where(p => p.HotelID == hotelID);
                    return View(room);
                }
                Room _room = roomService.Get(r => r.Name == room.Name && r.HotelID == info.HotelID);
                if (_room != null)
                {
                    ModelState.AddModelError("", string.Format("Tên loại phòng [{0}] đã hiện hữu vui lòng chọn tên khác", room.Name));
                    room.Hotel = info;
                    room.IsActive = true;
                    room.Rooms = roomService.All.Where(p => p.HotelID == hotelID);
                    return View(room);
                }
                _room = new Room();
                ModelCopier.CopyModel(room, _room);
                _room.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
                _room.CreatedDate = DateTime.Now;
                _room.IsPromotion = false;
                _room.HotelID = info.HotelID;
                roomService.Insert(_room);
                roomService.Save();
                return RedirectToAction("Create", new { id = hotelID });
            }
            return RedirectToAction("Index", "Hotel");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(CreateRoomModel current, string id)
        {
            int roomID = Protector.Int(id);
            Room info = roomService.Get(p => p.RoomID == roomID);
            if (info != null)
            {
                if (!ModelState.IsValid)
                {
                    var hotel = hotelService.Get(h => h.HotelID == current.HotelID);
                    if (hotel != null)
                    {
                        current.Hotel = hotel;
                        current.Rooms = roomService.All.Where(p => p.HotelID == hotel.HotelID);
                    }
                    return View(current);
                }
                TryUpdateModel(info);
                roomService.Save();
                return RedirectToAction("Create", "Room", new { id = info.HotelID });

            }
            return RedirectToAction("Index", "Hotel");
        }
        [HttpPost]
        public ActionResult Delete(string id, string HotelID)
        {
            int hotelID = Protector.Int(HotelID);
            int roomID = Protector.Int(id);
            Room room = roomService.Find(roomID);
            if (room != null)
            {
                roomService.Delete(room);
                roomService.Save();
            }
            var list = roomService.All.Where(c => c.HotelID == hotelID);
            return PartialView("RoomList", list);
        }
        #endregion

    }
}
