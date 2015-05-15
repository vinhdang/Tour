using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Service.IServices;
using Service;
using Domain;
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
        public ICategoryService categoryService { get; set; }
        public IRolePermissionService rolePermissionService { get; set; }
        public RoleController(IRoleService roleService, IUserService userService, ICategoryService categoryService, IRolePermissionService rolePermissionService)
        {
            this.roleService = roleService;
            this.userService = userService;
            this.categoryService = categoryService;
            this.rolePermissionService = rolePermissionService;
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
                        List<User> list = userService.GetMany(u => u.RoleID == role.ID).ToList();
                        if (list != null && list.Count > 0)
                        {
                            foreach (User info in list)
                            {
                                userService.Delete(info);
                                userService.Save();
                            }
                        }
                        List<RolePermission> rolelist = rolePermissionService.GetMany(u => u.RoleID == role.ID).ToList();
                        if (rolelist != null && rolelist.Count > 0)
                        {
                            foreach (RolePermission info in rolelist)
                            {
                                rolePermissionService.Delete(info);
                                rolePermissionService.Save();
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
        [HttpGet]
        public ActionResult RolePermission(string id)
        {
            int roleID = Protector.Int(id);
            var role = roleService.Find(roleID);
            if (role != null)
            {
                var list = categoryService.GetMany(c => c.ParentID == -1 && c.ID != 183 && c.ID != 184).ToList();
                return View(list);
            }
            return RedirectToAction("Error", "Administrator");
        }
        public ActionResult SubList(string categoryID)
        {
            int _categoryID = Protector.Int(categoryID);
            List<Category> list = new List<Category>();
            if (_categoryID == 80)
            {
                list = categoryService.GetMany(c => c.IsActive && c.ParentID == _categoryID && c.IsAdmin).OrderByDescending(c => c.Order).ToList();
            }
            else
            {
                list = categoryService.GetMany(c => c.IsActive && c.ParentID == _categoryID).OrderByDescending(c => c.Order).ToList();
            }

            // list = categoryService.GetMany(m => m.IsActive && m.ParentID == _categoryID);
            return PartialView("SubList", list);
        }
        public ActionResult CheckBox(string categoryID, string id)
        {
            int _categoryID = Protector.Int(categoryID);
            int RoleID = Protector.Int(id);
            Category cat = categoryService.Get(m => m.ID == _categoryID);
            if (cat != null && RoleID > 0)
            {
                RolePermission role = rolePermissionService.Get(r => r.RoleID == RoleID && r.CategoryID == _categoryID);
                if (role != null)
                {
                    cat.IsActive = true;

                }
                else { cat.IsActive = false; }
                return PartialView("CheckBox", cat);
            }
            return RedirectToAction("Error", "Administrator");

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
            Role role = roleService.Get(r => r.Name == name && r.ID != roleID);
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
        [HttpPost]
        public ActionResult RolePermission(string id, string[] permission)
        {
            int roleID = Protector.Int(id);
            if (roleID > 0)
            {
                List<RolePermission> list = rolePermissionService.GetMany(r => r.RoleID == roleID).ToList();
                foreach (RolePermission item in list)
                {
                    rolePermissionService.Delete(item);
                    rolePermissionService.Save();
                }
                if (permission != null && permission.Length > 0)
                {
                    foreach (string item in permission)
                    {
                        int CategoryId = Protector.Int(item);
                        rolePermissionService.Insert(new RolePermission { CategoryID = CategoryId, RoleID = roleID });
                        rolePermissionService.Save();
                    }
                }
                else
                {

                }
            }
            return Redirect(Request.RawUrl);
        }



        #endregion
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

    }
}
