using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Domain;
using Service.Helpers;
using Website.Model;
using Website.Model.Tour;

namespace Website.Helpers
{
    public static class WebHelpers
    {
        public static string DETAIL_TOUR_ID = "SESSION::DETAIL_TOUR_ID";
        public static string DETAIL_TOUR = "SESSION::DETAIL_TOUR";
        public static string SEARCH_CRITERIA_MONTH = "SESSION::SEARCH_CRITERIA_MONTH";
        public static string SEARCH_TOUR_RESULT = "SESSION::SEARCH_TOUR_RESULT";
        public static string SEARCH_FOREIGN_TOUR_RESULT = "SESSION::SEARCH_FOREIGN_TOUR_RESULT";
        public static string FOREIGN_NATIONAL_SELECTED = "SESSION::FOREIGN_NATIONAL_SELECTED";

        public static int SessionTourID
        {
            get
            {
                if (HttpContext.Current.Session[DETAIL_TOUR_ID] != null)
                {
                    return Protector.Int(HttpContext.Current.Session[DETAIL_TOUR_ID]);
                }else
                {
                    return -1;
                }  
            } 
  
            set { HttpContext.Current.Session[DETAIL_TOUR_ID] = value; }
        }

        public static Tour SessionTour
        {
            get
            {
                if (HttpContext.Current.Session[DETAIL_TOUR] != null)
                {
                    return (Tour)(HttpContext.Current.Session[DETAIL_TOUR]);
                }
                else
                {
                    return null;
                }
            }

            set { HttpContext.Current.Session[DETAIL_TOUR] = value; }
        }

        public static List<MonthFilter> SessionMonthCriteria
        {
            get
            {
                if (HttpContext.Current.Session[SEARCH_CRITERIA_MONTH] != null)
                {
                    return (List<MonthFilter>)(HttpContext.Current.Session[SEARCH_CRITERIA_MONTH]);
                }
                else
                {
                    return new List<MonthFilter>();
                }
            }

            set { HttpContext.Current.Session[SEARCH_CRITERIA_MONTH] = value; }
        }
        
        public static TourResult SessionTourResult
        {
            get
            {
                if (HttpContext.Current.Session[SEARCH_TOUR_RESULT] != null)
                {
                    return (TourResult)(HttpContext.Current.Session[SEARCH_TOUR_RESULT]);
                }
                else
                {
                    return null;
                }
            }

            set { HttpContext.Current.Session[SEARCH_TOUR_RESULT] = value; }
        }

        public static TourResult SessionForeignTourResult
        {
            get
            {
                if (HttpContext.Current.Session[SEARCH_FOREIGN_TOUR_RESULT] != null)
                {
                    return (TourResult)(HttpContext.Current.Session[SEARCH_FOREIGN_TOUR_RESULT]);
                }
                else
                {
                    return null;
                }
            }

            set { HttpContext.Current.Session[SEARCH_FOREIGN_TOUR_RESULT] = value; }
        }

        public static National SessionForeignNational
        {
            get
            {
                if (HttpContext.Current.Session[FOREIGN_NATIONAL_SELECTED] != null)
                {
                    return (National)(HttpContext.Current.Session[FOREIGN_NATIONAL_SELECTED]);
                }
                else
                {
                    return new National();
                }
            }

            set { HttpContext.Current.Session[FOREIGN_NATIONAL_SELECTED] = value; }
        }

        public static List<MonthFilter> GetAvailableMonth()
        {
            var result = new List<MonthFilter>();
            var limit = 12;
            var index = 1;

            while (index <= limit)
            {
                var month = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[index];
            }
            
            return result;
        }

        public static List<MonthFilter> GetMonths(DateTime StartDate)
        {
            List<string> MonthList = new List<string>();
            var result = new List<MonthFilter>();
            DateTime ThisMonth = DateTime.Now.Date;

            var i = 1;
            while (ThisMonth.Date < StartDate.Date)
            {
                var model = new MonthFilter();
                model.Display = ThisMonth.ToString("MMMM") + " " + ThisMonth.Year.ToString();
                model.OriginalValue = ThisMonth;
                model.Disable = false;
                model.Value = i;

                result.Add(model);
                ThisMonth = ThisMonth.AddMonths(1);
                i++;
            }

            return result;
        }
    }
}