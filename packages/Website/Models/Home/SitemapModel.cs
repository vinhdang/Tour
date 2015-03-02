using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Entities;

namespace Website.Models.Home
{
    public class SitemapModel
    {
        public List<Category> Categories { get; set; }
        public List<Province> Provinces { get; set; }
        public List<Hotel> Hotels { get; set; }
    }
}