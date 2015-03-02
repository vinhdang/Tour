using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Service.Validations;

namespace Website.Areas.Administrator.Models.Social
{
    public class CreateSocialModel
    {
        [Required]
        [Display(Name = "Tên")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Đường dẫn")]
        public string Link { get; set; }

     
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        
        [Required]
        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; }

        [Required]
        [Display(Name = "Thứ tự")]
        public int Order { get; set; }

        [Required()]
        [Display(Name = "ICon")]
        [File(AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".swf" }, MaxContentLength = 1024 * 1024 * 8, ErrorMessage = "Invalid File")]
        public HttpPostedFileBase File { get; set; }


    }
}