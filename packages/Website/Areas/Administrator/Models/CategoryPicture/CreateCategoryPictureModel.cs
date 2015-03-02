using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Service.Validations;

namespace Website.Areas.Administrator.Models.CategoryPicture
{
    public class CreateCategoryPictureModel
    {
        public int PictureID { get; set; }
        [Required]
        [Display(Name = "Tên chuyên mục")]
        public int ProductID { get; set; }

        [Required]
        [Display(Name = "Chú thích")]
        public string Alt { get; set; }

        [Required]
        [Display(Name = "Thứ tự")]
        public int Order { get; set; }

        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; }


        [Required()]
        [Display(Name = "Tệp tin")]
        [File(AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".swf" }, MaxContentLength = 1024 * 1024 * 8, ErrorMessage = "Invalid File")]
        public HttpPostedFileBase File { get; set; }

        // Navigation properties
        public virtual Domain.Entities.Category Category { get; set; }
        public List<Domain.Entities.CategoryPicture> ListPictures { get; set; }
    }
}