using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Models.Search
{
    public class SearchByPriceByStarModel
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
        public string priceID { get; set; }
        public List<System.Web.Mvc.SelectListItem> PriceList { get; set; }
        public List<System.Web.Mvc.SelectListItem> StarList { get; set; }
        public string[] ListPriceID { get; set; }
        public string[] ListStarID { get; set; }
    }
}