using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Models.Comfort
{
    public class ComfortModel
    {
        public int ProvinceID { get; set; }
        public string DistrictID { get; set; }
        public string AreaID { get; set; }
        public string HotelThemeID { get; set; }
        public string HotelTypeID { get; set; }
        public string HotelPromotion { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string NumberRoom { get; set; }
        public string NumberGuest { get; set; }
        public List<Domain.Entities.Comfort> Comforts { get; set; }
        public string[] ListComfortID { get; set; }

    }
}