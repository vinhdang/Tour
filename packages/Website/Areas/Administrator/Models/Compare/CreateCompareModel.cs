using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Service.Validations;
using Domain.Entities;
namespace Website.Areas.Administrator.Models.Province
{
    public class CreateCompareModel
    {
        public long CompareID { get; set; }


        [Required]
        [Display(Name = "Tên")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Đường dẫn")]
        public string Link { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }



        [Required]
        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; }

        [Required]
        [Display(Name = "Đối tượng cần so sánh")]
        public int CompareTypeID { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> Types { get; set; }
        public Domain.Entities.Hotel Hotel { get; set; }
        public List<Domain.Entities.Compare> Compares { get; set; }
    }
}