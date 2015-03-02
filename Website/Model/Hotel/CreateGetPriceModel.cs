using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Website.Models.Hotel
{
    public class CreateGetPriceModel
    {
        [Required(ErrorMessage = "Email không hợp lệ")]
        [StringLength(256, ErrorMessage = "Email không hợp lệ")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }
        [Display(Name = "Điện thoại")]
        [Required(ErrorMessage = "Điện thoại không hợp lệ")]
        public int? Phone { get; set; }
        public int roomID { get; set; }
        public int hotelID { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
    }
}