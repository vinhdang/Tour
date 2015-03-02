using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Service.Validations;

namespace Website.Areas.Administrator.Models.CategoryPicture
{
    public class EditCategoryPictureModel
    {
        public int PictureID { get; set; }
        [Required]
        [Display(Name = "Tên chuyên mục")]
        public int CategoryID { get; set; }

        [Required]
        [Display(Name = "Chú thích")]
        public string Alt { get; set; }

        [Required]
        [Display(Name = "Thứ tự")]
        public int Order { get; set; }

        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; }

          [Display(Name = "Ảnh")]
        public string FilePath { get; set; }
        [Display(Name = "Tệp tin mới")]
        [File(AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".swf" }, MaxContentLength = 1024 * 1024 * 8, ErrorMessage = "Invalid File")]
        public HttpPostedFileBase File { get; set; }

        // Navigation properties
        public virtual Domain.Entities.Category Category { get; set; }
        public IEnumerable<Domain.Entities.CategoryPicture> ListPictures { get; set; }
    }
}