using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Website.Areas.Administrator.Models.Area
{
    public class CreateAreaModel
    {
        [Required]
        [Display(Name = "Quốc gia")]
        public int NationalID { get; set; }

        [Required]
        [Display(Name = "Tỉnh thành")]
        public int ProvinceID { get; set; }

        [Required]
        [Display(Name = "Quận huyện")]
        public int DistrictID { get; set; }

        [Required]
        [Display(Name = "Tên khu vực")]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Từ khóa")]
        public string KeyWord { get; set; }

        [Required]
        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; }
        [Required]
        [Display(Name = "Thứ tự")]
        public int Order { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> Nationals { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Provinces { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Districts { get; set; }
    }
}