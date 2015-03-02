using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.Log;
using Service;
using Domain.Entities;
using Website.Helpers;
using Service.IServices;

namespace Website.Controllers
{
    public class SupportController : Controller
    {

        public ISupportService supportService { get; set; }
        public SupportController(ISupportService supportService)
        {
            this.supportService = supportService;
        }
         [OutputCache(Duration = 10000)]
        public ActionResult GetOnlineSupport()
        {
            List<Support> list = supportService.All.Where(s => s.IsActive).OrderByDescending(s => s.Order).ToList();
            return PartialView("GetOnlineSupport", list);
        }
         [OutputCache(Duration = 10000)]
        public ActionResult TopSupport()
        {
            Support item = supportService.Get(s => s.IsActive && s.Name == "TopSupport");
            return PartialView("TopSupport", item);
        }
        [OutputCache(Duration = 10000)]
        public ActionResult Yahoo()
        {
            List<Support> item = supportService.All.Where(s => s.IsActive && s.Type == 1).ToList();
            return PartialView("Yahoo", item);
        }
        [OutputCache(Duration = 10000)]
        public ActionResult Skype()
        {
            List<Support> item = supportService.All.Where(s => s.IsActive && s.Type == 2).ToList();
            return PartialView("Skype", item);
        }
         [OutputCache(Duration = 10000)]
        public ActionResult Yahoo1()
        {
            List<Support> item = supportService.All.Where(s => s.IsActive && s.Type == 1).ToList();
            return PartialView("Yahoo1", item);
        }
        [OutputCache(Duration = 10000)]
        public ActionResult Skype1()
        {
            List<Support> item = supportService.All.Where(s => s.IsActive && s.Type == 2).ToList();
            return PartialView("Skype1", item);
        }
        [OutputCache(Duration = 10000)]
        public ActionResult Phone()
        {
            Support item = supportService.Get(s => s.IsActive && s.Name == "TopSupport");
            return PartialView("Phone", item);
        }
     [OutputCache(Duration = 10000)]
        public ActionResult Email()
        {
            Support item = supportService.Get(s => s.IsActive && s.Name == "Email");
            return PartialView("Email", item);
        }
      [OutputCache(Duration = 10000)]
        public ActionResult Troubleshooting()
        {
            Support item = supportService.Get(s => s.IsActive && s.Name == "GiaiQuyetSuCo");
            return PartialView("Troubleshooting", item);
        }
         [OutputCache(Duration = 10000)]
        public ActionResult PhoneListHotel()
        {
            Support item = supportService.Get(s => s.IsActive && s.Name == "PhoneListHotel");
            return PartialView("PhoneListHotel", item);
        }
 
        public ActionResult HomeSupport()
        {
            return PartialView("HomeSupport");
        }
      
        public ActionResult HotelSupport()
        {
            return PartialView("HotelSupport");
        }
    }
}
