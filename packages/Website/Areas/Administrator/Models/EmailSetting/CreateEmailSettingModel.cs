using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Website.Areas.Administrator.Models.EmailSetting
{
    public class CreateEmailSettingModel
    {
        public int EmailSettingID { get; set; }

        [Display(Name = " Group")]
        public Nullable<int> GroupTemplateID { get; set; }

        [Required]
        [Display(Name = "Tên")]
        public string Name { get; set; }

        [Display(Name = "Ngày giờ gởi")]
        public string Date { get; set; }

        [Display(Name = "Tiêu đề")]
        public string Subject { get; set; }



        [Required]
        [Display(Name = "Email gởi")]
        public string EmailSent { get; set; }


        [Required]
        [Display(Name = "Số lượng email")]
        public int NumberEmail { get; set; }

        


        [Required]
        [Display(Name = "Trạng thái")]
        public Boolean IsActive { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> Groups { get; set; }


    }
}