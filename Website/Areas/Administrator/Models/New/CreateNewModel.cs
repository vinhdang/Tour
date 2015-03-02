using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Domain;
using Service.Validations;
namespace Website.Areas.Administrator.Models.New
{
    public class CreateNewModel
    {

        [Required]
        [Display(Name = "Chuyên mục cha")]
        public int CategoryID { get; set; }

        [Required]
        [Display(Name = "Tiêu đề")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Tiêu đề trang")]
        public string PageTitle { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Từ khóa")]
        public string Keyword { get; set; }

        [Display(Name = "Nội dung")]
        public string Content { get; set; }

        [Required]
        [Display(Name = "Thứ tự")]
        public int Order { get; set; }

        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; }

        [Display(Name = "Loại tin")]
        public int Type { get; set; }

        [Display(Name = "Ảnh đại diện")]
        [File(AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".swf" }, MaxContentLength = 1024 * 1024 * 8, ErrorMessage = "Invalid File")]
        public HttpPostedFileBase FAvartar { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> Categories { get; set; }
        public List<System.Web.Mvc.SelectListItem> ListType { get; set; }
    }
}