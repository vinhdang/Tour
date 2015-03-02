using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Service.IServices;
using Service;
using Domain.Entities;
using Service.Helpers;
using Website.Log;
using Telerik.Web.Mvc.UI;
using Telerik.Web.Mvc.Extensions;
using Service.Attribute;



namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        public IRoleService roleService { get; set; }
        public IUserService userService { get; set; }
        public RoleController(IRoleService roleService, IUserService userService)
        {
            this.roleService = roleService;
            this.userService = userService;
        }

        #region HttpGet

        [HttpGet]
        public ActionResult Index()
        {
            var list = roleService.All;
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
                foreach (int id in checkedRecords)
                {
                    Role role = roleService.Find(id);
                    if (role != null)
                    {
                        List<User> list = userService.All.Where(u => u.RoleID == role.RoleID).ToList();
                        if (list != null && list.Count > 0)
                        {
                            foreach (User info in list)
                            {
                                userService.Delete(info);
                                userService.Save();
                            }
                        }
                        roleService.Delete(role);
                        roleService.Save();
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
            var list = roleService.All;
            if (!string.IsNullOrEmpty(key))
            {
                list = list.Where(r => r.Name.Contains(key));
            }
            ViewBag.Key = key;
            return View(list);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var role = new Role();
            role.IsActive = true;
            return View(role);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            int roleID = Protector.Int(id);
            var role = roleService.Find(roleID);
            return View(role);
        }

        #endregion

        #region HttpPost

        [HttpPost]
        public ActionResult Create(Role role)
        {
            if (!ModelState.IsValid)
            {
                return View(role);
            }
            Role info = roleService.Get(r => r.Name == role.Name);
            if (info != null)
            {
                ModelState.AddModelError("", string.Format("Tên vai trò [{0}] đã hiện hữu vui lòng chọn tên khác", role.Name));
                return View(role);
            }
            role.CreatedDate = DateTime.Now;
            roleService.Insert(role);
            roleService.Save();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(string id, string name)
        {
            int roleID = Protector.Int(id);
            Role role = roleService.Get(r => r.Name == name && r.RoleID != roleID);
            if (role != null)
            {
                ModelState.AddModelError("", string.Format("Tên vai trò [{0}] đã hiện hữu vui lòng chọn tên khác", role.Name));
                return View(role);
            }
            else
            {
                role = roleService.Find(roleID);
                if (TryUpdateModel(role))
                {
                    roleService.Save();
                    return RedirectToAction("Index");
                }
                else return View(role);
            }
        }




        #endregion
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

    }
}
