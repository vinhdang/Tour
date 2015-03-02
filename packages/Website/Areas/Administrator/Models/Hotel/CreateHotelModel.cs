using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Domain.Entities;
using Service.Validations;

namespace Website.Areas.Administrator.Models.Hotel
{
    public class CreateHotelModel
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
        [Display(Name = "Loại khách sạn")]
        public int HotelTypeID { get; set; }

        [Display(Name = "Chuỗi khách sạn")]
        public Nullable<int> HotelThemeID { get; set; }

        [Required]
        [Display(Name = "Tên khách sạn")]
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

        [Display(Name = "Chính sách khách sạn")]
        public string Policy { get; set; }


        [Display(Name = "Thông tin khuyến mãi")]
        public string Promotion { get; set; }

        [Display(Name = "Duyệt TT khuyến mãi")]
        public Boolean IsPromotion { get; set; }

        [Display(Name = "Bản đồ Lat")]
        public string Lat { get; set; }
        [Display(Name = "Bản đồ Lng")]
        public string Lng { get; set; }

        [Required]
        [Display(Name = "Duyệt?")]
        public bool IsActive { get; set; }

        [Display(Name = "Hiển thị liên hệ")]
        public bool IsContact { get; set; }

        [Required]
        [Display(Name = "Thứ tự")]
        public int Order { get; set; }

        [Required]
        [Display(Name = "Tiêu chuẩn sao")]
        public int Star { get; set; }

        [Required]
        [Display(Name = "Hoa hồng")]
        public int Commission { get; set; }

        [Required]
        [Display(Name = "Ảnh đại diện")]
        [File(AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".swf" }, MaxContentLength = 1024 * 1024 * 8, ErrorMessage = "Invalid File")]
        public HttpPostedFileBase FAvartar { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> Nationals { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Provinces { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Districts { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Areas { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> HotelTypes { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> HotelThemes { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> ListStars { get; set; }
        public List<System.Web.Mvc.SelectListItem> ListType { get; set; }

    }
}