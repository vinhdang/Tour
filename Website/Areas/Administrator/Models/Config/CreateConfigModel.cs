using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Website.Areas.Administrator.Models.Config
{
    public class CreateConfigModel
    {
        public int ConfigID
        { get; set; }
        [Required]
        [Display(Name = "Tên cấu hình")]
        [StringLength(256)]
        public string Name { get; set; }

        [AllowHtml]
        [Required]
        [Display(Name = "Giá trị")]
        public string Value { get; set; }

       

        [Required]
        [Display(Name = "Loại dữ liệu")]
        public int Type { get; set; }
        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; }

        public List<SelectListItem> ListType { get; set; }
    }
}