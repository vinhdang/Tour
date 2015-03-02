using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Website.Models.Room
{
    public class RoomListModel
    {
        [Required]
        public string FromDate { get; set; }
        [Required]
        public string ToDate { get; set; }
        public string NumberRoom { get; set; }
        public string NumberGuest { get; set; }
        public Hotel Hotel { get; set; }
        public List<Domain.Entities.Room> RoomList { get; set; }
    }
}