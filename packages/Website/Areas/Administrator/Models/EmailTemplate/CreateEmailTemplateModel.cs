using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Website.Areas.Administrator.Models.EmailTemplate
{
    public class CreateEmailTemplateModel
    {
        [Required]
        [Display(Name = "Quốc gia")]
        public int NationalID { get; set; }

         [Required]
        [Display(Name = "Tỉnh thành")]
        public int ProvinceID { get; set; }

         [Required]
        [Display(Name = "Khách sạn")]
        public int HotelID { get; set; }
         [Required]
         [Display(Name = " Group")]
        public int GroupTemplateID { get; set; }


        [Required]
        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; }
        [Required]
        [Display(Name = "Thứ tự")]
        public int Order { get; set; }

        [Required]
        [Display(Name = "Giá")]
        public decimal? Price { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> Nationals { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Provinces { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Hotels { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Groups { get; set; }
    }
}