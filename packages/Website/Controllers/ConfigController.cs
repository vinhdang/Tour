using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Domain.Entities;

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
            Config config = configService.Get(p => p.Name == "Logo");
            if (config != null)
            {
                return PartialView("Logo", config);
            }
            return PartialView("Logo");
        }
        [OutputCache(Duration = 100000)]
        public ActionResult Contact()
        {
            Config config = configService.Get(p => p.Name == "Contact");
            if (config != null)
            {
                return PartialView("Contact", config);
            }
            return PartialView("Contact");
        }
        [OutputCache(Duration = 100000)]
        public ActionResult HotLine()
        {
            Config config = configService.Get(p => p.Name == "HotLine");
            if (config != null)
            {
                return PartialView("HotLine", config);
            }
            return PartialView("HotLine");
        }
        [OutputCache(Duration = 100000)]
        public ActionResult LikeFaceBook()
        {
            Config config = configService.Get(p => p.Name == "LikeFaceBook");
            if (config != null)
            {
                return PartialView("LikeFaceBook", config);
            }
            return PartialView("LikeFaceBook");
        }
        [OutputCache(Duration = 100000)]
        public ActionResult GooglePlus()
        {
            Config config = configService.Get(p => p.Name == "GooglePlus");
            if (config != null)
            {
                return PartialView("GooglePlus", config);
            }
            return PartialView("GooglePlus");
        }
        [OutputCache(Duration = 100000)]
        public ActionResult HotLine1()
        {
            Config config = configService.Get(p => p.Name == "HotLine");
            if (config != null)
            {
                return PartialView("HotLine1", config);
            }
            return PartialView("HotLine1");
        }
        [OutputCache(Duration = 100000)]
        public ActionResult Copyright()
        {
            Config config = configService.Get(p => p.Name == "Copyright");
            if (config != null)
            {
                return PartialView("Copyright", config);
            }
            return PartialView("HotLine");
        }
        [OutputCache(Duration = 100000)]
        public ActionResult CompanyInfo()
        {
            Config config = configService.Get(p => p.Name == "CompanyInfo");
            if (config != null)
            {
                return PartialView("CompanyInfo", config);
            }
            return PartialView("CompanyInfo");
        }
        [OutputCache(Duration = 100000)]
        public ActionResult GoogleAnalytics()
        {
            Config config = configService.Get(p => p.Name == "GoogleAnalytics");
            if (config != null)
            {
                return PartialView("GoogleAnalytics", config);
            }
            return PartialView("GoogleAnalytics");
        }
    }
}
