using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Website.Areas.Administrator.Models.Category
{
    public class EditCategoryModel
    {

        public Nullable<int> ParentID { get; set; }
        public string ListParentID { get; set; }

        [Required]
        [Display(Name = "Tên")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Tiêu đề trang")]
        public string PageTitle { get; set; }


        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Từ khóa")]
        public string Keyword { get; set; }

        [AllowHtml]
        [Display(Name = "Nội dung")]
        public string Content { get; set; }

        [Display(Name = "Đường dẫn")]
        public string Url { get; set; }



        [Required]
        [Display(Name = "Thứ tự")]
        public int Order { get; set; }

        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; }


        [Display(Name = "Vị trí")]
        public int Position { get; set; }



        [Display(Name = "Trang Admin")]
        public bool IsAdmin { get; set; }

        [Display(Name = "Link Admin")]
        public string AdminUrl { get; set; }


    }
}