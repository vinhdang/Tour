using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Models.Order
{
    public class OrderResultModel
    {
        public string PageTitle { get; set; }
        public string Description { get; set; }
        public string Keyword { get; set; }
        public string Html { get; set; }
        public long OrderID { get; set; }
        public int HotelID { get; set; }
    }
}