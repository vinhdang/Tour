using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;

namespace Website.Controllers
{
    public class SiteMapController : Controller
    {
        public ICategoryService categoryService { get; set; }
        public INewService newService { get; set; }
        public IProvinceService provinceService { get; set; }
        public ITourService tourService { get; set; }
        public IConfigService configService { get; set; }
        public SiteMapController(ICategoryService categoryService, IProvinceService provinceService, ITourService tourService)
        {
            this.categoryService = categoryService;
            this.provinceService = provinceService;
            this.tourService = tourService;
        }

        public ActionResult Index()
        {

            ViewBag.Categories = categoryService.All.Where(c => c.IsActive && c.IsAdmin==false).ToList();
            ViewBag.Provinces = provinceService.All.Where(c => c.IsActive).ToList();
            ViewBag.Tours = tourService.All.Where(c => c.IsActive).ToList();
            return PartialView("Index");
        }

    }
}
