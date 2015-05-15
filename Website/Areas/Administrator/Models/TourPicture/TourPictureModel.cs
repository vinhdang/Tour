using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Service.Validations;

namespace Website.Areas.Administrator.Models.TourPicture
{
    public class TourPictureModel
    {
        [Required]
        [Display(Name = "Chú thích")]
        public string Alt { get; set; }

        [Display(Name = "Backlink SEO")]
        public string BackLink { get; set; }


        [Required]
        [Display(Name = "Thứ tự")]
        public int Order { get; set; }

        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; }

        [Required]
        [Display(Name = "Ảnh đại diện")]
        public Boolean IsAvartar { get; set; }


        [Required()]
        [Display(Name = "Tệp tin")]
        [File(AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".swf" }, MaxContentLength = 1024 * 1024 * 8, ErrorMessage = "Invalid File")]
        public HttpPostedFileBase File { get; set; }
    }
}