using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Domain;
using Service.Helpers;

namespace Website.Controllers
{
    public class CategoryController : Controller
    {
        public ICategoryService categoryService { get; set; }
        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }
        public ActionResult Detail(string id)
        {
            long categoryID = Protector.Long(id);
            if (categoryID > 0)
            {
                Category info = categoryService.All.FirstOrDefault(h => h.IsActive && h.ID == categoryID);
                if (info != null)
                {

                    ViewBag.Title = info.PageTitle;
                    ViewBag.Description = info.Description;
                    ViewBag.KeyWords = info.Keyword;
                    return View(info);
                }
            }
            return View();
        }
        public ActionResult Navigation(string active)
        {
            List<Category> list = categoryService.All.Where(c => c.IsActive && !c.IsAdmin && c.ParentID == -1 && (c.Position & 1) == 1).OrderByDescending(c => c.Order).ThenByDescending(c => c.CreatedDate).ToList();
            ViewBag.Active = active;
            return PartialView("Navigation", list);
        }
        public ActionResult SiteMap()
        {
            List<Category> list = categoryService.All.Where(c => c.IsActive && !c.IsAdmin && c.ParentID == -1 && (c.Position & 2) == 2).OrderByDescending(c => c.Order).ThenByDescending(c => c.CreatedDate).ToList();
            return PartialView("SiteMap", list);
        }
        public ActionResult Introduction()
        {
            List<Category> list = categoryService.All.Where(c => c.IsActive && !c.IsAdmin && c.ParentID == -1 && (c.Position & 4) == 4).OrderByDescending(c => c.Order).ThenByDescending(c => c.CreatedDate).ToList();
            return PartialView("Introduction", list);
        }
    }
}
