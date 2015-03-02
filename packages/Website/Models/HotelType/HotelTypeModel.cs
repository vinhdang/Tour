using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Models.HotelType
{
    public class HotelTypeModel
    {
        public int ProvinceID { get; set; }
        public string AreaID { get; set; }
        public string ComfortID { get; set; }
        public string DistrictID { get; set; }
        public string HotelThemeID { get; set; }
        public string HotelPromotion { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string NumberRoom { get; set; }
        public string NumberGuest { get; set; }
        public List<Domain.Entities.HotelType> HotelTypeList { get; set; }
        public string[] ListHotelTypeID { get; set; }

    }
}