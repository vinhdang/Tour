using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Website.Areas.Administrator.Models.Apartment
{
    public class EditApartmentModel
    {
        [Required]
        [Display(Name = "Quốc gia")]
        public int NationalID { get; set; }

        [Required]
        [Display(Name = "Tỉnh thành")]
        public int ProvinceID { get; set; }

        [Display(Name = "Quận huyện")]
        public Nullable<int> DistrictID { get; set; }

        [Display(Name = "Khu vực")]
        public Nullable<int> AreaID { get; set; }

        [Required]
        [Display(Name = "Loại căn hộ")]
        public Nullable<int> ApartmentTypeID { get; set; }

        [Display(Name = "Chuỗi căn hộ")]
        public Nullable<int> ApartmentThemeID { get; set; }

        [Required]
        [Display(Name = "Tên căn hộ")]
        public string Name { get; set; }

       
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Tiêu đề trang(seo)")]
        public string PageTitle { get; set; }

        [Required]
        [Display(Name = "Mô tả ngắn(seo)")]
        public string Description { get; set; }

        [Display(Name = "Từ khóa(seo)")]
        public string KeyWord { get; set; }

        [Display(Name = "Mô tả dài")]
        public string Content { get; set; }
        [Display(Name = "Quy định")]
        public string Policy { get; set; }
   
      

        [Display(Name = "Thông tin khuyến mãi")]
        public string Promotion { get; set; }

        [Display(Name = "Duyệt TT khuyến mãi")]
        public Boolean IsPromotion { get; set; }

        [Display(Name = "Từ Ngày")]
        public DateTime? Startdate { get; set; }


        [Display(Name = "Đến Ngày")]
        public DateTime? Enddate { get; set; }

        [Display(Name = "Bản đồ Lat")]
        public string Lat { get; set; }
        [Display(Name = "Bản đồ Lng")]
        public string Lng { get; set; }

        [Required]
        [Display(Name = "Duyệt?")]
        public bool IsActive { get; set; }

     
        [Required]
        [Display(Name = "Thứ tự")]
        public int Order { get; set; }

 

        public IEnumerable<System.Web.Mvc.SelectListItem> Nationals { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Provinces { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Districts { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Areas { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> ApartmentTypes { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> ApartmentThemes { get; set; }
        public List<System.Web.Mvc.SelectListItem> ListType { get; set; }
    }
}