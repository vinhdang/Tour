using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Models.Area
{
    public class AreasModel
    {
        public int ProvinceID { get; set; }
        public string DistrictID { get; set; }
        public string ComfortID { get; set; }
        public string HotelThemeID { get; set; }
        public string HotelTypeID { get; set; }
        public string HotelPromotion { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string NumberRoom { get; set; }
        public string NumberGuest { get; set; }
        public List<Domain.Entities.District> DistrictList { get; set; }
        public string[] ListAreaID { get; set; }

    }
}