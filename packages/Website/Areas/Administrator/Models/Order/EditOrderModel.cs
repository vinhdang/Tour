using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Website.Areas.Administrator.Models.Order
{
    public class EditOrderModel
    {
        [Display(Name = "Tên khách sạn")]
        public int HotelID { get; set; }

        [Display(Name = "Tên quốc gia")]
        public int NationalID { get; set; }

        [Display(Name = "Tên tỉnh thành")]
        public int ProvinceID { get; set; }

        [Display(Name = "Tên quận huyện")]
        public Nullable<int> DistrictID { get; set; }

        [Display(Name = "Tên khu vực")]
        public Nullable<int> AreaID { get; set; }

        [Display(Name = "Mã booking")]
        public string OrderCode { get; set; }



        [Required]
        [Display(Name = "Ho tên")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Điện thoại")]
        public string Phone { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Loại thanh toán")]
        public string PaymentType { get; set; }

        [Display(Name = "Loại phòng")]
        public int RoomType { get; set; }

        [Display(Name = "Ghi chú")]
        public string Note { get; set; }

        [Display(Name = "Tên công ty")]
        public string CompanyName { get; set; }

        [Display(Name = "Địa chỉ công ty")]
        public string CompanyAddress { get; set; }

        [Display(Name = "Mã số thuế")]
        public string MST { get; set; }

        [Display(Name = "Ngày booking")]
        public System.DateTime CreatedDate { get; set; }

        [Required]
        [Display(Name = "Trạng thái")]
        public int StatusID { get; set; }

        [Display(Name = "IP")]
        public string IP { get; set; }
        public List<OrderInfo> OrderInfoList { get; set; }
        public List<Payment> Payments { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> StatusList { get; set; }
        public Domain.Entities.Hotel Hotel { get; set; }
    }
}