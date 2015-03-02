using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Website.Areas.Administrator.Models.RoomPrice
{
    public class CreateRoomPriceModel
    {
        [Required]
        [Display(Name = "Loại phòng")]
        public long RoomID { get; set; }

        [Required]
        [Display(Name = "Từ Ngày")]
        public DateTime? FromDate { get; set; }


        [Required]
        [Display(Name = "Đến Ngày")]
        public DateTime? ToDate { get; set; }

        [Required]
        [Display(Name = "Giá")]
        public decimal? Price { get; set; }

        [Display(Name = "Giá ngày cuối tuần")]
        public decimal? Price1 { get; set; }

        [Required]
        [Display(Name = "Số phòng trống")]
        public int? Quantity { get; set; }

        [Required]
        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; }

        public virtual Domain.Entities.Hotel Hotel { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Rooms { get; set; }
        public List<System.Web.Mvc.SelectListItem> DateList { get; set; }
    }
}