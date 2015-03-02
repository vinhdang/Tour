using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Domain.Entities;

namespace Website.Controllers
{
    public class CompareController : Controller
    {
        public IHotelService hotelService { get; set; }
        public ICompareService compareService { get; set; }
        public CompareController(ICompareService compareService, IHotelService hotelService)
        {

            this.compareService = compareService;
            this.hotelService = hotelService;

        }
        public ActionResult CompareList(IEnumerable<Compare> list)
        {
            return PartialView("CompareList", list);
        }
    }
}
