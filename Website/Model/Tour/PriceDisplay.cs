using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Website.Areas.Administrator.Models.TourProvider;

namespace Website.Model.Tour
{
    public class PriceDisplay
    {
        public long ID { get; set; }
        public int TourID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public bool IsActive { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<int> NumberGuest { get; set; }
        public bool IsContact { get; set; }
        public string Description { get; set; }
        public string LinkBooking { get; set; }
        public Nullable<int> ProviderID { get; set; }
        public Nullable<int> Sequence { get; set; }

        public virtual Domain.Tour Tour { get; set; }
        public virtual TourProviderDisplay TourProvider { get; set; }
    }
}