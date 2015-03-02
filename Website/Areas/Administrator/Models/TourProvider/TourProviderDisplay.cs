using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain;

namespace Website.Areas.Administrator.Models.TourProvider
{
    public class TourProviderDisplay
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NationalID { get; set; }
        public Nullable<int> ProvinceID { get; set; }
        public Nullable<int> DistrictID { get; set; }
        public Nullable<int> AreaID { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string Avatar { get; set; }

        public virtual ICollection<Domain.Tour> Tours { get; set; }
        public virtual National National { get; set; }
        public virtual Domain.Province Province { get; set; }
    }
}