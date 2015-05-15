using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Website.Areas.Administrator.Models.ApartmentPrice
{
    public class EditApartmentPriceModel
    {
        [Display(Name = "Ngày")]
        public DateTime? Date { get; set; }
        [Required]
        [Display(Name = "Giá")]
        public long Price { get; set; }

        

        [Required]
        [Display(Name = "Số khách")]
        public int? NumberGuest { get; set; }

        [Required]
        [Display(Name = "Liên hệ?")]
        public bool IsContact { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; }

      
    }
}