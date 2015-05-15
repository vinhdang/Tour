using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Website.Areas.Administrator.Models.TourType
{
    public class TourTypeModel
    {
        public int ID { get; set; }
        
        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; }
        
        [Display(Name = "Thứ tự")]
        public int Order { get; set; }

        [Required(ErrorMessage = "Tên loại tour là bắt buộc...")]
        [Display(Name="Tên Loại Tour")]
        public string Name { get; set; }
        
        [Display(Name = "Mô tả")]
        
        public string Description { get; set; }
        
        public System.DateTime CreatedDate { get; set; }
        
        public int CreatedBy { get; set; }
    }
}