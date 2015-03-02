using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Models.Hotel
{
    public class MyOrder
    {
        public int HotelID { get; set; }
        public int ProvinceID { get; set; }
        public long RoomID { get; set; }
        public int Quantity { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public Decimal RoomPrice { get; set; }

        public Decimal TotalPrice { get; set; }
        public Decimal VATPrice { get; set; }
        public int NumberNight { get; set; }
        public string Name { get; set; }

    }
}