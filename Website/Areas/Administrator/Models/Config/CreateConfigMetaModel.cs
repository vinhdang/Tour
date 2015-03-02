using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Website.Areas.Administrator.Models.Config
{
    public class CreateConfigMetaModel
    {
        [Display(Name = "Tiêu đề")]
        public string IndexTitle { get; set; }

        [Display(Name = "Mô tả")]
        public string IndexDescription { get; set; }

        [Display(Name = "Từ khóa")]
        public string IndexKeyWords { get; set; }


    }
}