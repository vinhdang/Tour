using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain;
using Service.IServices;

namespace Website.Controllers
{
    public class ConfigController : Controller
    {
        public IConfigService configService { get; set; }
        public ConfigController(IConfigService configService)
        {
            this.configService = configService;
        }
         [OutputCache(Duration = 100000)]
        public ActionResult Logo()
        {
            Config info = configService.All.Where(c => c.Name == "Logo").FirstOrDefault();
            return PartialView("Logo", info);
        }

        [OutputCache(Duration = 100000)]
        public ActionResult Analytics()
        {
            Config info = configService.All.Where(c => c.Name == "GOOGLEANALYTICS").FirstOrDefault();
            return PartialView("Analytics", info);
        }
          [OutputCache(Duration = 100000)]
        public ActionResult Company()
        {
            Config info = configService.All.Where(c => c.Name == "Company").FirstOrDefault();
            return PartialView("Company", info);
        }
        
    }
}
