using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Website.Areas.Administrator.Models.Config
{
    public class CreateConfigEmailModel
    {

        [Display(Name = "Email liên hệ")]
        public string Email { get; set; }

        [Display(Name = "EmailCC")]
        public string EmailCC { get; set; }

     

        [Display(Name = "Hostname")]
        public string HostName { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Port")]
        public int Port { get; set; }


        [Display(Name = "Timeout")]
        public int Timeout { get; set; }

       }
}