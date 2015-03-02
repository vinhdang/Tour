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
    public class ProvinceController : Controller
    {
        public IProvinceService provinceService { get; set; }
        public ProvinceController(IProvinceService provinceService)
        {
            this.provinceService = provinceService;

        }
        #region Partial
         [OutputCache(Duration = 10000)]
        public ActionResult Home_Province_Left()
        {
            List<Province> list = provinceService.All.Where(p => p.IsActive && (p.Position & 4) == 4).OrderByDescending(p => p.Order).ToList();
            return PartialView("Home_Province_Left", list);
        }
          [OutputCache(Duration = 10000)]
        public ActionResult ProvinceFooter()
        {
            return PartialView("ProvinceFooter");
        }
  
        public ActionResult ProvinceFooterItem(string type)
        {
            int _type = Protector.Int(type);
            List<Province> list = provinceService.All.Where(p => p.IsActive && p.Type == _type && (p.Position & 1) == 1).ToList();
            return PartialView("ProvinceFooterItem", list);
        }
          [OutputCache(Duration = 10000)]
        public ActionResult Province_Attractive()
        {
            List<Province> list = provinceService.All.Where(p => p.IsActive && (p.Position & 2) == 2).ToList();
            return PartialView("Province_Attractive", list);
        }
        #endregion
    }
}
