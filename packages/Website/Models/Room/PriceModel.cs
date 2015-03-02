using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Entities;

using System.ComponentModel.DataAnnotations;

namespace Website.Models.Room
{
    public class PriceModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string NumberRoom { get; set; }
        public string NumberGuest { get; set; }
        public Decimal Price { get; set; }
        public Domain.Entities.Room Room { get; set; }
    }
}