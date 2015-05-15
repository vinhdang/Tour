using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Website.Areas.Administrator.Models.ApartmentPrice
{
    public class CreateApartmentPriceModel
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
        [Display(Name = "Số khách")]
        public int? NumberGuest { get; set; }

        [Required]
        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; }

        [Required]
        [Display(Name = "Liên hệ?")]
        public bool IsContact { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        public List<System.Web.Mvc.SelectListItem> DateList { get; set; }
    }
}