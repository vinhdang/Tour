using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Service.Validations;

namespace Website.Areas.Administrator.Models.TourSuggestion
{
    public class SuggestionModel
    {
        public int ID { get; set; }

        [Required]
        [Display(Name="Tiêu đề")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Từ khóa")]
        public string KeyWord { get; set; }


        public bool IsDeleted { get; set; }

        public System.DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }
        
        public string Type { get; set; }
        
        public string ImageUrl { get; set; }
        
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        
        public Nullable<int> ModifiedBy { get; set; }

        [Required]
        [Display(Name = "Quốc gia")]
        public int NationalID { get; set; }

        [Required]
        [Display(Name = "Tỉnh/TP")]
        public int ProvinceID { get; set; }

        [Display(Name = "Quận/Huyện")]
        public Nullable<int> DistrictID { get; set; }

        [Display(Name = "Khu vực")]
        public Nullable<int> AreaID { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> Nationals { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Provinces { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Districts { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Areas { get; set; }

        [Required]
        [Display(Name = "Ảnh đại diện")]
        [File(AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".swf" }, MaxContentLength = 1024 * 1024 * 8, ErrorMessage = "Invalid File")]
        public HttpPostedFileBase FAvartar { get; set; }
    }
}