using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Website.Areas.Administrator.Models.Comment
{
    public class CommentModel
    {
        public long ID { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Nội dung")]
        public string Content { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }

        [Display(Name = "Mức độ hài lòng")]
        public int Level { get; set; }

        public Nullable<int> TourID { get; set; }

        [Display(Name = "Duyệt?")]
        public bool IsEnable { get; set; }

        public Nullable<bool> IsDeleted { get; set; }
    }
}