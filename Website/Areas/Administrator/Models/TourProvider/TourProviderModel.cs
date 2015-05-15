using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Domain;

namespace Website.Areas.Administrator.Models.TourProvider
{
    public class TourProviderModel
    {
        public int ID { get; set; }

        [Required]
        [Display(Name="Tên nhà cung cấp")]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        
        
        public int NationalID { get; set; }


        public Nullable<int> ProvinceID { get; set; }
        
        
        public Nullable<int> DistrictID { get; set; }
        
        
        public Nullable<int> AreaID { get; set; }

        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public bool IsActive { get; set; }

        public HttpPostedFileBase File { get; set; }
    }
}