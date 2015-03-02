using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Service.Helpers;
using Domain.Entities;
using MvcPaging;

namespace Website.Controllers
{
    public class CategoryController : Controller
    {
        public ICategoryService categoryService { get; set; }
        public INewService newService { get; set; }
        public CategoryController(ICategoryService categoryService, INewService newService)
        {
            this.categoryService = categoryService;
            this.newService = newService;
        }
        [OutputCache(Duration = 100000)]
        public ActionResult Index(string name, string id)
        {

            int CatID = Protector.Int(id);
            Category info = categoryService.Find(CatID);
            if (info != null)
            {
                ViewBag.Title = info.PageTitle;
                ViewBag.KeyWords = info.Keyword;
                ViewBag.Description = info.Description;
                return View(info);
            }
            return View();
        }
        [OutputCache(Duration = 100000)]
        public ActionResult RenderMainMenu()
        {
            List<Category> list = categoryService.All.Where(c => c.IsActive && (c.Position & 1) == 1).OrderByDescending(c => c.Order).ToList();
            return PartialView("RenderMainMenu", list);
        }
        [OutputCache(Duration = 100000)]
        public ActionResult RenderFooterMenu()
        {
            List<Category> list = categoryService.All.Where(c => c.IsActive && (c.Position & 2) == 2).OrderByDescending(c => c.Order).ToList();
            return PartialView("RenderFooterMenu", list);
        }
        [OutputCache(Duration = 100000)]
        public ActionResult SiteMap()
        {
            List<Category> list = categoryService.All.Where(c => c.IsActive && (c.Position & 2) == 2).OrderByDescending(c => c.Order).ToList();
            return PartialView("SiteMap", list);
        }
        [OutputCache(Duration = 100000)]
        public ActionResult Introduction()
        {
            List<Category> list = categoryService.All.Where(c => c.IsActive && (c.Position & 4) == 4).OrderByDescending(c => c.Order).ToList();
            return PartialView("Introduction", list);
        }
        public ActionResult BreadCrumb(string id, string newID)
        {
            string str = "";
            if (!string.IsNullOrEmpty(id))
            {
                int CatID = Protector.Int(id);
                Category info = categoryService.Find(CatID);
                if (info != null)
                {
                    if (!string.IsNullOrEmpty(info.ListParentID))
                    {
                        str += string.Format("<a  href='{0}' >{1}</a>", "/", "Trang chủ");
                        string[] listparentID = info.ListParentID.Split(',');
                        foreach (string item in listparentID)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                Category Iteminfo = categoryService.Find(Protector.Int(item));
                                if (Iteminfo != null)
                                {
                                    str += string.Format("<span class='breadcrumb-break'>»</span><a  href='{0}' >{1}</a>", UrlHelp.getCategoryUrl(Iteminfo.Url, Iteminfo.Name, Iteminfo.CategoryID), Iteminfo.Name);
                                }
                            }
                        }
                        str += string.Format("<span class='breadcrumb-break'>»</span><span class='breadcrumb-active'>{0}</span> ", info.Name);


                    }
                    else
                    {
                        str += string.Format("<a  href='{0}' >{1}</a><span class='breadcrumb-break'>»</span><span class='breadcrumb-active'>{2}</span> ", "/", "Trang chủ", info.Name);
                    }
                }
            }
            return PartialView("BreadCrumb", str);
        }
        public ActionResult BreadCrumbProduct(string id, string newID)
        {
            string str = "";
            if (!string.IsNullOrEmpty(id))
            {
                int CatID = Protector.Int(id);
                Category info = categoryService.Find(CatID);
                if (info != null)
                {
                    if (!string.IsNullOrEmpty(info.ListParentID))
                    {
                        str += string.Format("<a  href='{0}' >{1}</a>", "/", "Trang chủ");
                        string[] listparentID = info.ListParentID.Split(',');
                        foreach (string item in listparentID)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                Category Iteminfo = categoryService.Find(Protector.Int(item));
                                if (Iteminfo != null)
                                {
                                    str += string.Format("<span class='breadcrumb-break'>»</span><a  href='{0}' >{1}</a>", UrlHelp.getCategoryUrl(Iteminfo.Url, Iteminfo.Name, Iteminfo.CategoryID, "pl"), Iteminfo.Name);
                                }
                            }
                        }
                        str += string.Format("<span class='breadcrumb-break'>»</span><span class='breadcrumb-active'>{0}</span> ", info.Name);


                    }
                    else
                    {
                        str += string.Format("<a  href='{0}' >{1}</a><span class='breadcrumb-break'>»</span><span class='breadcrumb-active'>{2}</span> ", "/", "Trang chủ", info.Name);
                    }
                }
            }
            return PartialView("BreadCrumb", str);
        }


        public string GetActiveMainMenu(string CurrentCatID, string id, string newID)
        {
            if (!string.IsNullOrEmpty(id))
            {
                if (CurrentCatID == id)
                {
                    return "active";
                }
                else
                {
                    Category info = categoryService.Find(Protector.Int(id));
                    if (info != null)
                    {
                        if (info.ParentID.ToString() == CurrentCatID)
                        { return "active"; }
                        else
                        {
                            string[] listParentID = info.ListParentID.Split(',');
                            if (listParentID.Contains(CurrentCatID))
                            { return "active"; }
                        }
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(newID))
                {


                }
                else if (CurrentCatID == "1") { return "active"; }
            }
            return "";
        }
    }
}
