using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Model.Tour
{
    public class JsonAgenda
    {
        public int ID { get; set; }
        public int TourID { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Description { get; set; }
        public int NationalID { get; set; }
        public int ProvinceID { get; set; }
        public Nullable<int> DistrictID { get; set; }
        public Nullable<int> AreaID { get; set; }
        public System.DateTime FromDate { get; set; }
        public System.DateTime ToDate { get; set; }
        public Nullable<int> Sequence { get; set; }
        public bool IsActivate { get; set; }
        public string Name { get; set; }
    }
}