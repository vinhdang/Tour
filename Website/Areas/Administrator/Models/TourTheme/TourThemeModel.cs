using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Website.Areas.Administrator.Models.TourTheme
{
    public class TourThemeModel
    {
        public int ID { get; set; }
        
        [Required(ErrorMessage = "Tên chuỗi tour là bắt buộc...")]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }


        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }

        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; }

        [Display(Name = "Thứ tự")]
        public int Order { get; set; }
    }
}