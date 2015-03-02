using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Service.Attribute;
using Domain.Entities;
using System.Web.Routing;
using Telerik.Web.Mvc.Extensions;

namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class EmailGetPriceController : Controller
    {
        public IEmailGetPriceService emailGetPriceService { get; set; }
        public EmailGetPriceController(IEmailGetPriceService emailGetPriceService)
        {
            this.emailGetPriceService = emailGetPriceService;
        }

        #region HttpGet
        public ActionResult Index()
        {
            List<EmailGetPrice> list = emailGetPriceService.All.ToList();

            return View(list);
        }
        [HttpPost]
        [ActionName("Index")]
        [AcceptButton(ButtonName = "Delete")]
        public ActionResult IndexDelete(int[] checkedRecords)
        {
            checkedRecords = checkedRecords ?? new int[] { };
            if (checkedRecords != null && checkedRecords.Length > 0)
            {
                foreach (int item in checkedRecords)
                {
                    EmailGetPrice email = emailGetPriceService.Find(item);
                    if (email != null)
                    {

                        emailGetPriceService.Delete(email);
                        emailGetPriceService.Save();
                    }
                }
            }
            RouteValueDictionary routeValues = this.GridRouteValues();
            return RedirectToAction("Index", routeValues);
        }

        [HttpPost]
        [ActionName("Index")]
        [AcceptButton(ButtonName = "Search")]
        public ActionResult IndexSearch(string key)
        {
            var list = emailGetPriceService.All;
            if (!string.IsNullOrEmpty(key))
            {
                list = list.Where(r => r.Email.Contains(key) || r.Phone.Contains(key));
            }
            ViewBag.Key = key;
            return View(list.ToList());
        }


        #endregion

    }
}
