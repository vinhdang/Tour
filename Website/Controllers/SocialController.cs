using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Domain.Entities;

namespace Website.Controllers
{
    public class SocialController : Controller
    {
        public ISocialService socialService { get; set; }

        public SocialController(ISocialService socialService)
        {
            this.socialService = socialService;

        }
        public ActionResult Home()
        {
            List<Social> list = socialService.All.Where(s => s.IsActive).OrderByDescending(s => s.Order).ThenByDescending(s => s.CreatedDate).ToList();
            return PartialView("Home", list);
        }

    }
}
