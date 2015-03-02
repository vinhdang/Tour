using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Website.Areas.Administrator.Models.User
{
    public class EditUserModel
    {
        public int UserID
        { get; set; }

        [Required]
        [Display(Name = "Vai trò")]
        public int RoleID { get; set; }


        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }
        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; }

        [Required]
        [StringLength(256)]
        [Display(Name = "Họ tên")]
        public string FullName { get; set; }

        [StringLength(100)]
        [Display(Name = "Điện thoại")]
        public string Phone { get; set; }


        public IEnumerable<System.Web.Mvc.SelectListItem> Role { get; set; }
    }
}