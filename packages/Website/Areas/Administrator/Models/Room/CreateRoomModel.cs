using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Website.Areas.Administrator.Models.Room
{
    public class CreateRoomModel
    {
        [Required]
        public long RoomID { get; set; }

        [Required]
        [Display(Name = "Khách sạn")]
        public int HotelID { get; set; }

        [Required]
        [Display(Name = "Duyệt?")]
        public bool IsActive { get; set; }

        [Required]
        [Display(Name = "Thứ tự")]
        public int Order { get; set; }

        [Required]
        [Display(Name = "Số người")]
        public int NumberGuest { get; set; }

        [Required]
        [Display(Name = "Tên loại phòng")]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Hiển thị lấy giá")]
        public bool IsContact { get; set; }
        public virtual Domain.Entities.Hotel Hotel { get; set; }
        public IEnumerable<Domain.Entities.Room> Rooms { get; set; }

    }
}