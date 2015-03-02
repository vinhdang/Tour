using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Models.Search
{
    public class HotelPromotionModel
    {
        public int ProvinceID { get; set; }
        public string DistrictID { get; set; }
        public string AreaID { get; set; }
        public string ComfortID { get; set; }
        public string HotelThemeID { get; set; }
        public string HotelTypeID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string NumberRoom { get; set; }
        public string NumberGuest { get; set; }
        public List<System.Web.Mvc.SelectListItem> HotelPromotionList { get; set; }
        public string[] ListHotelPromotionID { get; set; }
    }
}