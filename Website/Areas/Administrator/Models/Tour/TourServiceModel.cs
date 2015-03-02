using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Website.Areas.Administrator.Models.Tour
{
    public class TourServiceModel
    {
        public int ID { get; set; }

        public int TourID { get; set; }
       
        public string ActivityID { get; set; }

        [Display(Name = "Thứ tự")]
        public int Order { get; set; }

        [Required]
        [Display(Name = "Duyệt?")]
        public bool IsActive { get; set; }
        
        public DateTime CreatedDate { get; set; }
        
        public int CreatedBy { get; set; }
    }
}