using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Service.Validations;

namespace Website.Areas.Administrator.Models.Config
{
    public class CreateRecruitmentModel
    {
        public int ConfigID { get; set; }
        public string FilePath { get; set; }
        public string Value { get; set; }

        //[Required()]
        //[Display(Name = "Form tuyển dụng mới")]
        //[File(AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".swf", ".doc", ".docx", ".ppt" }, MaxContentLength = 1024 * 1024 * 8, ErrorMessage = "Invalid File")]
        //public HttpPostedFileBase File { get; set; }
    }
}