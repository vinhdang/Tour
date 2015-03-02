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
    public class NewController : Controller
    {
        public ICategoryService categoryService { get; set; }
        public INewService newService { get; set; }
        public NewController(ICategoryService categoryService, INewService newService)
        {
            this.categoryService = categoryService;
            this.newService = newService;
        }
          [OutputCache(Duration = 10000)]
        public ActionResult Index(string newID, string id)
        {
            int nID = Protector.Int(id);

            Category info = categoryService.Find(nID);
            if (info != null)
            {
                ViewBag.Title = info.PageTitle;
                ViewBag.KeyWords = info.Keyword;
                ViewBag.Description = info.Description;
                return View(info);
            }
            return RedirectToAction("Index", "Home");

        }
         [OutputCache(Duration = 10000)]
        public ActionResult Detail(string newID, string id)
        {
            int nID = Protector.Int(newID);

            New info = newService.Find(nID);
            if (info != null)
            {
                ViewBag.Title = info.PageTitle;
                ViewBag.KeyWords = info.Keyword;
                ViewBag.Description = info.Description;
                return View(info);
            }
            return RedirectToAction("Index", "Home");

        }
         [OutputCache(Duration = 10000)]
        public ActionResult OtherNews(string currentNewID, string catID)
        {
            int currentID = Protector.Int(currentNewID);
            int CatID = Protector.Int(catID);
            List<New> list = newService.All.Where(n => n.NewID != currentID && n.CategoryID == CatID).ToList();
            return PartialView("OtherNews", list);
        }
        [ChildActionOnly]
        [OutputCache(Duration = 10000)]
        public ActionResult NewHome()
        {
            List<New> list = newService.All.Where(c => c.IsActive && (c.Type & 1) == 1).OrderByDescending(c => c.Order).OrderByDescending(c => c.CreatedDate).Take(6).ToList();
            return PartialView("NewHome", list);
        }
    }
}
