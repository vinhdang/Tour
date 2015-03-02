using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Model
{
    public class MonthFilter
    {
        public int Id { get; set; }
        public string Display { get; set; }
        public int Value { get; set; }
        public bool Disable { get; set; }
        public DateTime OriginalValue { get; set; }
        public int TotalTour { get; set; }
    }
}