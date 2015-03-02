using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Website.Models.Comment
{
    public class CreateCommentModel
    {

        public int HotelID { get; set; }

        [Required(ErrorMessage = "Tên không hợp lệ")]
        [Display(Name = "Tên")]
        public string Name { get; set; }

        [Display(Name = "Email")]

        [Required(ErrorMessage = "Email không hợp lệ")]
        [StringLength(256, ErrorMessage = "Email không hợp lệ")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Tiêu đề không hợp lệ")]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Nội dung không hợp lệ")]
        [Display(Name = "Nội dung")]
        public string Content { get; set; }

    }
}