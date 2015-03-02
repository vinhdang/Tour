using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Domain.Entities;

namespace Website.Controllers
{
    public class HotelPictureController : Controller
    {
        public IHotelPictureService hotelPictureService { get; set; }
        public HotelPictureController(IHotelPictureService hotelPictureService)
        {
            this.hotelPictureService = hotelPictureService;
        }

        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult PictureGallery(IEnumerable<HotelPicture> list)
        {

            return PartialView("PictureGallery", list);
        }
        
        public ActionResult PictureList(IEnumerable<HotelPicture> list)
        {

            return PartialView("PictureList", list);
        }
    }
}
