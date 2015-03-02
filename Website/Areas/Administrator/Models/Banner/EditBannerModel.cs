using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Service.Validations;

namespace Website.Areas.Administrator.Models.Banner
{
    public class EditBannerModel
    {
        public int BannerID { get; set; }
        public Nullable<int> ProvinceID { get; set; }
        [Required()]
        [Display(Name = "Tên banner")]
        public string Name { get; set; }

        [Display(Name = "Đường dẫn")]
        public string Link { get; set; }

        [Display(Name = "Thứ tự")]
        public int Order { get; set; }

        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; }

      
        [Display(Name = "Tệp tin")]
        public string FilePath { get; set; }


        [Display(Name = "Tệp tin mới")]
        [File(AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".swf" }, MaxContentLength = 1024 * 1024 * 8, ErrorMessage = "Invalid File")]
        public HttpPostedFileBase File { get; set; }

        public List<SelectListItem> ListPosition { get; set; }

        public IEnumerable<SelectListItem> ListProvince { get; set; }
    }
}