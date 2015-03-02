using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Website.Areas.Administrator.Models.Order
{
    public class CreateOrderModel
    {
        public CreateOrderModel()
        {
            RingSizeMen = "0";
            RingSizeWomen = "0";
        }
        public int OrderID { get; set; }
        [Required]
        [Display(Name = "Sản phẩm ID")]
        public int ProductID { get; set; }
        [Required]
        public int StatusID { get; set; }
        [Required]
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }
        [Required]
        [Display(Name = "Họ tên")]
        public string FullName { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Điện thoại")]
        public string Phone { get; set; }
        [Required]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "Ghi chú")]
        public string Note { get; set; }
        [Required]
        [Display(Name = "Tổng số tiền")]
        public int TotalPrice { get; set; }
        [Required]
        [Display(Name = "Giá sản phẩm")]
        public int ProductPrice { get; set; }
        [Display(Name = "Size nam")]
        public string RingSizeMen { get; set; }
        [Display(Name = "Size nữ")]
        public string RingSizeWomen { get; set; }
        public string Reason { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> StatusList { get; set; }
        public List<System.Web.Mvc.SelectListItem> QuantityList { get; set; }

        public List<System.Web.Mvc.SelectListItem> RingSizeMenList { get; set; }
        public List<System.Web.Mvc.SelectListItem> RingSizeWomenList { get; set; }
    }
}