using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Domain.Entities;
using Service.Helpers;

namespace Website.Controllers
{
    public class BannerController : Controller
    {
        public IBannerService bannerService { get; set; }
        public BannerController(IBannerService bannerService)
        {
            this.bannerService = bannerService;
        }
        [OutputCache(Duration = 100)]
        public ActionResult BannerHome()
        {
            List<Banner> list = bannerService.All.Where(p => p.IsActive && (p.Position & 1) == 1).OrderByDescending(p => p.Order).ToList();
            return PartialView("BannerHome", list);
        }
        [OutputCache(Duration = 100)]
        public ActionResult BannerPopup()
        {
            Banner info = bannerService.Get(p => p.IsActive && (p.Position & 4) == 4);
            return PartialView("BannerPopup", info);
        }

        public ActionResult ProvinceBanner(string p)
        {
            int provinceID = Protector.Int(p);
            List<Banner> list = new List<Banner>();
            if (provinceID > 0)
            {
                list = bannerService.All.Where(b => b.IsActive && (b.Position & 8) == 8 && b.ProvinceID == provinceID).ToList();
            }
            return PartialView("ProvinceBanner", list);
        }
        [OutputCache(Duration = 100)]
        public ActionResult LeftBanner(string name, string id)
        {
            int provinceID = Protector.Int(id);
            List<Banner> list = new List<Banner>();
            if (provinceID > 0)
            {
                list = bannerService.All.Where(p => p.IsActive && (p.ProvinceID == provinceID)).OrderByDescending(p => p.Order).ToList();
            }
            else
            {

                list = bannerService.All.Where(p => p.IsActive).OrderByDescending(p => p.Order).ToList();
            }
            return PartialView("LeftBanner", list);
        }
    }
}
