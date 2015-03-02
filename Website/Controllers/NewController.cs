using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;

namespace Website.Controllers
{
    public class NewController : Controller
    {
        public INewService newService { get; set; }

        public NewController(INewService newService)
        {
            this.newService = newService;
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
