using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Model.Tour
{
    public class TourPictureDisplay
    {
        public long ID { get; set; }
        public int TourID { get; set; }
        public bool IsActive { get; set; }
        public int Order { get; set; }
        public string Alt { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string Extension { get; set; }
        public long ContentLength { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public bool IsAvartar { get; set; }
        public string BackLink { get; set; }

        public string PictureUrl { get; set; }
    }
}