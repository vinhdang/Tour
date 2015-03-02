using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.Log;
using Service;
using Domain.Entities;
using Service.Helpers;
using Service.IServices;

namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class SupportController : Controller
    {
        public ISupportService supportService { get; set; }
        public SupportController(ISupportService supportService)
        {
            this.supportService = supportService;
        }

        #region HttpGet

        [HttpGet]
        public ActionResult Index()
        {
            var list = supportService.All;
            return View(list);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var support = new Support();
            support.IsActive = true;
            ViewBag.SupportType = GlobalVariables.SupportType;
            return View(support);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var support = supportService.Find(Protector.Int(id));
            ViewBag.SupportType = GlobalVariables.SupportType;
            return View(support);
        }

        #endregion

        #region HttpPost

        [HttpPost]
        public ActionResult Create(Support support)
        {
            if (!ModelState.IsValid)
            {
                return View(support);
            }
            support.CreatedDate = DateTime.Now;
            support.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
            supportService.Insert(support);
            supportService.Save();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(string id, string name)
        {
            int supportID = Protector.Int(id);

            Support support = supportService.Find(supportID);
            if (TryUpdateModel(support))
            {
                supportService.Save();
                return RedirectToAction("Index");
            }
            else return View(support);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            int supportID = Protector.Int(id);

            Support support = supportService.Find(supportID);
            supportService.Delete(support);
            supportService.Save();
            var supports = supportService.All;
            return PartialView("SupportList", supports);
        }

        #endregion

    }
}
