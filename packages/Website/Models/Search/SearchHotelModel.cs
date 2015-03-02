using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Website.Models.Search
{
    public class SearchHotelModel
    {

        [Required]
        [Display(Name = "Nơi đến")]
        public string ProvinceName { get; set; }

        public int _ProvinceID { get; set; }

        [Display(Name = "Tên khách sạn")]
        public string HotelName { get; set; }

        [Display(Name = "Ngày đến")]
        public string FDate { get; set; }

        [Display(Name = "Ngày về")]
        public string TDate { get; set; }

        [Display(Name = "Số phòng")]
        public int NumberRoom { get; set; }

        [Display(Name = "Số khách")]
        public int NumberGuest { get; set; }
    }
}