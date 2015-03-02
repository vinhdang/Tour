using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Models.Search
{
    public class FilterByModel
    {
        public int ProvinceID { get; set; }
        public string DistrictID { get; set; }
        public string AreaID { get; set; }
        public string ComfortID { get; set; }
        public string HotelThemeID { get; set; }
        public string HotelTypeID { get; set; }
        public string HotelPromotion { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string NumberRoom { get; set; }
        public string NumberGuest { get; set; }
        public string Price { get; set; }
        public string Star { get; set; }
        public string Filter { get; set; }
        public string FType { get; set; }

    }
}