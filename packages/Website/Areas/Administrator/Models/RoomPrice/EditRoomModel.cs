using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Website.Areas.Administrator.Models.RoomPrice
{
    public class EditRoomPriceModel
    {
        [Display(Name = "Ngày")]
        public DateTime? Date { get; set; }
        [Required]
        [Display(Name = "Giá")]
        public long Price { get; set; }

        [Required]
        [Display(Name = "Số phòng trống")]
        public int? Quantity { get; set; }

        [Required]
        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; }

        public virtual Domain.Entities.Hotel Hotel { get; set; }
        public virtual Domain.Entities.Room Room { get; set; }
    }
}