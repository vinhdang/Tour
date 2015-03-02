using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Domain.Entities;
using Website.Helpers;
using Service.Helpers;
using Microsoft.Web.Mvc;
using Service.Attribute;
using Telerik.Web.Mvc.Extensions;
using System.Web.Routing;

namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class AdminMenuController : Controller
    {
        public IAdminMenuService adminMenuService { get; set; }

        public AdminMenuController(IAdminMenuService adminMenuService)
        {
            this.adminMenuService = adminMenuService;
        }
        #region HttpGet
        public ActionResult Index()
        {
            var list = adminMenuService.All.OrderByDescending(m => m.Order).ToList();
            List<AdminMenu> menus = new List<AdminMenu>();
            foreach (AdminMenu item in list)
            {
                if (item.ParentID == null)
                {
                    menus.Add(item);
                    List<AdminMenu> listchild = list.Where(m => m.ParentID == item.MenuID).ToList();
                    if (listchild != null && listchild.Count > 0)
                    {
                        foreach (AdminMenu child in listchild)
                        {
                            child.Name = "  - - - " + child.Name;
                            menus.Add(child);
                        }
                    }
                }
            }
            return View(menus);
        }

        [HttpPost]
        [ActionName("Index")]
        [AcceptButton(ButtonName = "Delete")]
        public ActionResult IndexDelete(int[] checkedRecords)
        {
            checkedRecords = checkedRecords ?? new int[] { };
            if (checkedRecords != null && checkedRecords.Length > 0)
            {
                foreach (int id in checkedRecords)
                {
                    AdminMenu menu = adminMenuService.Find(id);
                    if (menu != null)
                    {
                        if (menu.ParentID == null)
                        {
                            List<AdminMenu> list = adminMenuService.All.Where(u => u.ParentID == menu.MenuID).ToList();
                            if (list != null && list.Count > 0)
                            {
                                foreach (AdminMenu info in list)
                                {
                                    adminMenuService.Delete(info);
                                    adminMenuService.Save();
                                }
                            }
                        }
                        adminMenuService.Delete(menu);
                        adminMenuService.Save();
                    }
                }
            }
            RouteValueDictionary routeValues = this.GridRouteValues();
            return RedirectToAction("Index", routeValues);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var menu = new AdminMenu();
            ViewBag.ListParent = adminMenuService.All.Where(p => p.ParentID == null).ToSelectListItems(-1);
            menu.IsActive = true;
            menu.IsShow = true;
            return View(menu);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            int menuID = Protector.Int(id);
            var menu = new AdminMenu();
            menu = adminMenuService.Find(menuID);
            ViewBag.ListParent = adminMenuService.All.Where(p => p.ParentID == null).ToSelectListItems(Protector.Int(menu.ParentID));
            return View(menu);
        }
        #endregion
        #region  HttpPost

        [HttpPost]
        public ActionResult Create(AdminMenu menu, string ListParent)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ListParent = adminMenuService.All.Where(p => p.ParentID == null).ToSelectListItems(-1);
                return View(menu);
            }
            AdminMenu info = adminMenuService.Get(r => r.Name == menu.Name);
            if (info != null)
            {
                ViewBag.ListParent = adminMenuService.All.Where(p => p.ParentID == null).ToSelectListItems(-1);

                ModelState.AddModelError("", string.Format("Tên menu [{0}] đã hiện hữu vui lòng chọn tên khác", menu.Name));
            }
            info = new AdminMenu();
            ModelCopier.CopyModel(menu, info);
            info.Permission = 0;
            if (!string.IsNullOrEmpty(ListParent))
            {
                info.ParentID = Protector.Int(ListParent);
            }
            else { info.ParentID = null; }
            adminMenuService.Insert(info);
            adminMenuService.Save();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Edit(AdminMenu current, string ListParent, string id)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ListParent = adminMenuService.All.Where(p => p.ParentID == null).ToSelectListItems(Protector.Int(current.ParentID));

                return View(current);
            }
            int menuID = Protector.Int(id);
            AdminMenu menu = adminMenuService.Find(menuID);
            if (menu != null)
            {
                TryUpdateModel(menu);
                menu.Permission = 0;
                if (!string.IsNullOrEmpty(ListParent))
                {
                    menu.ParentID = Protector.Int(ListParent);
                }
                else { menu.ParentID = null; }
                adminMenuService.Save();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        #endregion
    }
}
