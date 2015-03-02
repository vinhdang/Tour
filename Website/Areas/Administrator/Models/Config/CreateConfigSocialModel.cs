using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Website.Areas.Administrator.Models.Config
{
    public class CreateConfigSocialModel
    {
        [Display(Name = "Facebook")]
        public string Facebook { get; set; }

        [Display(Name = "Twitter")]
        public string Twitter { get; set; }

        [Display(Name = "Google plus")]
        public string GooglePlus { get; set; }

        [Display(Name = "Youtube")]
        public string Youtube { get; set; }



    }
}