using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Service.Validations;

namespace Website.Areas.Administrator.Models.Tour
{
    public class TourModel
    {
        [Required]
        [Display(Name = "Quốc gia khởi hành")]
        public int DepartureNationalID { get; set; }

        [Required]
        [Display(Name = "Tỉnh thành khởi hành")]
        public int DepartureProvinceID { get; set; }

        [Display(Name = "Quận huyện khởi hành")]
        public Nullable<int> DepartureDistrictID { get; set; }

        [Display(Name = "Khu vực khởi hành")]
        public Nullable<int> DepartureAreaID { get; set; }
        
        [Required]
        [Display(Name = "Quốc gia nơi đến")]
        public int DestinationNationalID { get; set; }

        [Required]
        [Display(Name = "Tỉnh thành nơi đến")]
        public int DestinationProvinceID { get; set; }

        [Display(Name = "Quận huyện nơi đến")]
        public Nullable<int> DestinationDistrictID { get; set; }

        [Display(Name = "Khu vực nơi đến")]
        public Nullable<int> DestinationAreaID { get; set; }

        [Required]
        [Display(Name = "Loại Tour")]
        public int TourTypeID { get; set; }

        [Display(Name = "Chuỗi Tour")]
        public Nullable<int> TourThemeID { get; set; }
        
        [Display(Name = "Nhà cung cấp Tour")]
        public int TourProviderID { get; set; }

        [Required]
        [Display(Name = "Tên tour")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Số lượng khách")]
        public int? NumberGuest { get; set; }

        [Required]
        [Display(Name = "Tiêu đề trang(seo)")]
        public string PageTitle { get; set; }

        [Required]
        [Display(Name = "Mô tả ngắn(seo)")]
        [MinLength(144, ErrorMessage = "Chuỗi phải có độ dài 144 kí tự")]
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

        [Required]
        [Display(Name = "Từ Ngày")]
        public DateTime? Startdate { get; set; }

        [Required]
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



        [Required]
        [Display(Name = "Ảnh đại diện")]
        [File(AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".swf" }, MaxContentLength = 1024 * 1024 * 8, ErrorMessage = "Invalid File")]
        public HttpPostedFileBase FAvartar { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> DepartNationals { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> DepartProvinces { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> DepartDistricts { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> DepartAreas { get; set; }
        
        public IEnumerable<System.Web.Mvc.SelectListItem> DestNationals { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> DestProvinces { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> DestDistricts { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> DestAreas { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> TourTypes { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> TourThemes { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> TourProviders { get; set; }
        public List<System.Web.Mvc.SelectListItem> ListType { get; set; }
    }

    public class EditTourModel
    {
        [Required]
        [Display(Name = "Quốc gia khởi hành")]
        public int DepartureNationalID { get; set; }

        [Required]
        [Display(Name = "Tỉnh thành khởi hành")]
        public int DepartureProvinceID { get; set; }

        [Display(Name = "Quận huyện khởi hành")]
        public Nullable<int> DepartureDistrictID { get; set; }

        [Display(Name = "Khu vực khởi hành")]
        public Nullable<int> DepartureAreaID { get; set; }

        [Required]
        [Display(Name = "Quốc gia nơi đến")]
        public int DestinationNationalID { get; set; }

        [Required]
        [Display(Name = "Tỉnh thành nơi đến")]
        public int DestinationProvinceID { get; set; }

        [Display(Name = "Quận huyện nơi đến")]
        public Nullable<int> DestinationDistrictID { get; set; }

        [Display(Name = "Khu vực nơi đến")]
        public Nullable<int> DestinationAreaID { get; set; }

        [Required]
        [Display(Name = "Loại Tour")]
        public int TourTypeID { get; set; }

        [Display(Name = "Chuỗi Tour")]
        public Nullable<int> TourThemeID { get; set; }

        [Display(Name = "Nhà cung cấp Tour")]
        public int TourProviderID { get; set; }

        [Required]
        [Display(Name = "Tên tour")]
        public string Name { get; set; }

        [Display(Name = "Số lượng khách")]
        public int NumberGuest { get; set; }

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

        public IEnumerable<System.Web.Mvc.SelectListItem> DepartNationals { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> DepartProvinces { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> DepartDistricts { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> DepartAreas { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> DestNationals { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> DestProvinces { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> DestDistricts { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> DestAreas { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> TourTypes { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> TourThemes { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> TourProviders { get; set; }
        public List<System.Web.Mvc.SelectListItem> ListType { get; set; }
    }
}