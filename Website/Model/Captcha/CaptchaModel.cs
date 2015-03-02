using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Website.Models.Captcha
{
    public class Captcha
    {
        [Display(Name = "Nhập mã hiển thị", Order = 20)]
        [Remote("ValidateCaptcha", "Captcha", "", ErrorMessage = "Mã không đúng")]
        [Required(ErrorMessage = "Mã không đúng")]
        public virtual string CaptchaValue { get; set; }
        public Captcha()
        {

        }
    }

    public class InvisibleCaptcha
    {
        [Display(Name = "InvisibleCaptcha", Order = 20)]
        [Remote("ValidateInvisibleCaptcha", "Captcha", "", ErrorMessage = "Mã không đúng")]
        public virtual string InvisibleCaptchaValue { get; set; }
        public InvisibleCaptcha()
        {

        }
    }
}