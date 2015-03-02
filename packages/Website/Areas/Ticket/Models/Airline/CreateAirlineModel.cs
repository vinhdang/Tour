using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Service.Validations;

namespace Website.Areas.Ticket.Models.Airline
{
    public class CreateAirlineModel
    {
        public int AirlineID { get; set; }

        [Required]
        [Display(Name = "Tên hãng bay")]
        public string Name { get; set; }



        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Từ khóa")]
        public string KeyWord { get; set; }

        [Required]
        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; }
        [Required]
        [Display(Name = "Thứ tự")]
        public int Order { get; set; }
        [Required]
        [Display(Name = "Khu vực / miền")]
        public int Type { get; set; }

        public int Position { get; set; }

        [Display(Name = "Ảnh đại diện")]
        public string Logo { get; set; }

        [Display(Name = "Ảnh đại diện")]
        [File(AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".swf" }, MaxContentLength = 1024 * 1024 * 8, ErrorMessage = "Invalid File")]
        public HttpPostedFileBase FAvartar { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> Types { get; set; }
        public List<System.Web.Mvc.SelectListItem> Positions { get; set; }
    }
}