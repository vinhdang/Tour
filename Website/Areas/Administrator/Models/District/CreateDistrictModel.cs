using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Website.Areas.Administrator.Models.District
{
    public class CreateDistrictModel
    {
        [Required]
        [Display(Name = "Quốc gia")]
        public int NationalID { get; set; }

        [Required]
        [Display(Name = "Tỉnh thành")]
        public int ProvinceID { get; set; }


        [Required]
        [Display(Name = "Tên quận huyện")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Tiêu đề trang")]
        public string PageTitle { get; set; }

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
    }
}