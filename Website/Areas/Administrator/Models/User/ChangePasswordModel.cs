using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Website.Areas.Administrator.Models.User
{
    public class ChangePasswordModel
    {

        [DataType(DataType.Password)]
        [Required()]
        [StringLength(128, MinimumLength = 3)]
        [Display(Name = "Mật khẫu cũ")]
        [RegularExpression(@"(\S)+")]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Required()]
        [StringLength(128, MinimumLength = 3)]
        [RegularExpression(@"(\S)+")]
        [Display(Name = "Mật khẫu mới")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        [Display(Name = "Xác nhận mật khẫu")]
        public string ConfirmNewPassword { get; set; }
    }
}