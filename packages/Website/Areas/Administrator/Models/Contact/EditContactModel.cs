using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Website.Areas.Administrator.Models.Contact
{
    public class EditContactModel
    {

        public int ContactID
        { get; set; }

        [Required]
        [StringLength(256)]
        [Display(Name = "Họ tên")]
        public string FullName { get; set; }

        [Required]
        [StringLength(256)]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Required]
        [StringLength(256)]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Điện thoại")]
        [StringLength(128)]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Nội dung")]
        public string Content { get; set; }

        [Required]
        [Display(Name = "Ngày tạo")]
        public DateTime CreatedDate { get; set; }

        [Required]
        [Display(Name = "Xữ lý?")]
        public Boolean Isprocessing { get; set; }
    }
}