using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Models.Room
{
    public class MyRoom
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int NumberNight { get; set; }
        public Decimal VATPrice { get; set; }
        public Decimal Price { get; set; }
        public Decimal TotalPrice { get; set; }
    }
}