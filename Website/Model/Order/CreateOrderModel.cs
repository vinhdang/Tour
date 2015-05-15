using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Domain;
using Website.Models.Hotel;
namespace Website.Models.Order
{
    public class CreateOrderModel
    {


        public int HotelID { get; set; }
        public int ProvinceID { get; set; }
        [Required(ErrorMessage = "Họ tên không hợp lệ")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Di động Không hợp lệ (chỉ chấp nhận 09xxxxxxxx, 01xxxxxxxxx)")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email không hợp lệ")]
        [StringLength(256, ErrorMessage = "Email không hợp lệ")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }
        public string Address { get; set; }
        public string Content { get; set; }

        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string MST { get; set; }


        public List<MyOrder> Orders { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> RoomType { get; set; }
        public List<Payment> Payments { get; set; }

    }
}