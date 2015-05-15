using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain;
using Service.Helpers;

namespace Website.Model.Tour
{
    public class TourDisplay
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string PageTitle { get; set; }
        public string Description { get; set; }
        public Nullable<int> Duration { get; set; }
        public string KeyWord { get; set; }
        public string Content { get; set; }
        public string Policy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public int Order { get; set; }
        public string ListPayment { get; set; }
        public int TourTypeID { get; set; }
        public Nullable<int> TourThemeID { get; set; }
        public string ListComfort { get; set; }
        public int DepartureNationalID { get; set; }
        public int DepartureProvinceID { get; set; }
        public Nullable<int> DepartureDistrictID { get; set; }
        public Nullable<int> DepartureAreaID { get; set; }
        public int DestinationNationalID { get; set; }
        public int DestinationProvinceID { get; set; }
        public Nullable<int> DestinationDistrictID { get; set; }
        public Nullable<int> DestinationAreaID { get; set; }
        public int Type { get; set; }
        public Nullable<System.DateTime> Startdate { get; set; }
        public Nullable<System.DateTime> Enddate { get; set; }
        public string Url { get; set; }
        public string Promotion { get; set; }
        public bool IsPromotion { get; set; }
        public Nullable<int> TourProviderID { get; set; }
        public string LinkToSite { get; set; }
        public Nullable<int> NumberGuest { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        
        public string AvatarUrl { get; set; }
        public string LinkToDetail { get; set; }
        public TourPicture AvatarPic { get; set; }

        public decimal AVGComment { get; set; }
        public string Rating_Url { get; set; }
        public int CommentCount { get; set; }

        public virtual TourTheme TourTheme { get; set; }
        public virtual TourType TourType { get; set; }
        public virtual ICollection<TourActivity> TourActivities { get; set; }
        public virtual ICollection<TourAgenda> TourAgendas { get; set; }
        public virtual ICollection<TourPicture> TourPictures { get; set; }
        public virtual ICollection<TourPrice> TourPrices { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }

    public class TourResult : BasePagingResult
    {
        public ICollection<TourDisplay> Tours { get; set; } 
    }
}