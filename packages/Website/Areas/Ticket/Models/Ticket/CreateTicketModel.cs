using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Website.Areas.Ticket.Models.Ticket
{
    public class CreateTicketModel
    {

        public int TicketID { get; set; }

        [Display(Name = "Nơi xuất phát")]
        public int OriginID { get; set; }

        [Display(Name = "Điểm đến")]
        public int DestinationID { get; set; }

        [Display(Name = "Hãng bay")]
        public int AirlineID { get; set; }

        [Display(Name = "Loại vé")]
        public int TicketTypeID { get; set; }

        [Display(Name = "Hạng ghế")]
        public string SeatTypeID { get; set; }

        [Display(Name = "Mã chuyến bay")]
        public string TicketCode { get; set; }

        [Display(Name = "Loại máy bay")]
        public string AircraftType { get; set; }

        [Display(Name = "Giờ bay")]
        public int Hour { get; set; }

        [Display(Name = "Phút bay")]
        public int Minute { get; set; }

        [Display(Name = "Tên sân bay đi")]
        public string OriginAirport { get; set; }

        [Display(Name = "Tên sân bay đến")]
        public string DestinationAirport { get; set; }

        [Display(Name = "Điều kiện hành lý")]
        public string LuggageCondition { get; set; }

        [Display(Name = "Điều kiện về vé")]
        public string TicketCondition { get; set; }

        [Required]
        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; }

        [Required]
        [Display(Name = "Thứ tự")]
        public int Order { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> Origins { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Destinations { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Airlines { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> TicketTypes { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> SeatTypes { get; set; }



    }
}