using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Service.Validations;

namespace Website.Areas.Administrator.Models.Email
{
    public class ImportMailModel
    {
        [Required()]
        [Display(Name = "Tệp tin")]
        [File(AllowedFileExtensions = new string[] { ".xls", ".xlsx" }, MaxContentLength = 1024 * 1024 * 1024, ErrorMessage = "Invalid File")]
        public HttpPostedFileBase File { get; set; }
    }
}