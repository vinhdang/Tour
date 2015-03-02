using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Website.Models.Room
{
    public class RoomListResultModel
    {
        [Required]
        public string FromDate { get; set; }
        [Required]
        public string ToDate { get; set; }
        public string NumberRoom { get; set; }
        public string NumberGuest { get; set; }
        public string HotelID { get; set; }
        public string ProvinceID { get; set; } 
        public List<Domain.Entities.Room> RoomList { get; set; }
    }
}