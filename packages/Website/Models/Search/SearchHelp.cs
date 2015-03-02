using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Website.Models.Search
{
    public class SearchHelp
    {
        public string Name { get; set; }

        [Required]
        public string KeyWord { get; set; }
        public List<Province> Provinces { get; set; }
        public List<Hotel> Hotels { get; set; }
        public List<Banner> Banners { get; set; }
    }
}