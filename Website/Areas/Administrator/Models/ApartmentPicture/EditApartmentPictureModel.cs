﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Service.Validations;

namespace Website.Areas.Administrator.Models.ApartmentPicture
{
    public class EditApartmentPictureModel
    {
       
        [Required]
        [Display(Name = "Chú thích")]
        public string Alt { get; set; }

     

        [Required]
        [Display(Name = "Thứ tự")]
        public int Order { get; set; }

        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; }

        [Required]
        [Display(Name = "Ảnh đại diện")]
        public Boolean IsAvartar { get; set; }


        [Display(Name = "Tệp tin mới")]
        [File(AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".swf" }, MaxContentLength = 1024 * 1024 * 8, ErrorMessage = "Invalid File")]
        public HttpPostedFileBase File { get; set; }

      
    }
}