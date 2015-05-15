using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Website.Areas.Administrator.Models.TourPrice
{
    public class TourPriceModel
    {
        public long ID { get; set; }
        public int TourID { get; set; }

        [Required]
        [Display(Name = "Nhà cung cấp")]
        public int ProviderID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }

        [Display(Name = "Link Booking")]
        public string LinkBooking { get; set; }
        
        [Required]
        [Display(Name = "Giá")]
        public decimal? Price { get; set; }

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

        [Display(Name = "Thứ tự hiển thị")]
        public int Sequence { get; set; }

        public List<System.Web.Mvc.SelectListItem> TourProviders { get; set; }
    }
}