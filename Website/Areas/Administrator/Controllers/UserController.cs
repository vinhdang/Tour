using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Website.Areas.Administrator.Models.User;
using Domain;
using Website.Helpers;
using Service.Helpers;
using Microsoft.Web.Mvc;
using Website.Log;
using System.Web.Security;
using Service.Attribute;
using System.Web.Routing;
using Telerik.Web.Mvc.Extensions;

namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        public IRoleService roleService { get; set; }
        public IUserService userService { get; set; }
        public ICategoryService categoryService { get; set; }
        public IRolePermissionService rolePermissionService { get; set; }
        public UserController(IRoleService roleService, IUserService userService, ICategoryService categoryService, IRolePermissionService rolePermissionService)
        {
            this.roleService = roleService;
            this.userService = userService;
            this.categoryService = categoryService;
            this.rolePermissionService = rolePermissionService;
        }
        #region HttpGet
        /// <summary>
        /// get all users
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var list = userService.All;
            IEnumerable<Role> roles = roleService.GetMany(r => r.IsActive).OrderByDescending(r => r.Order);
            ViewBag.RoleList = roles.ToSelectListItems(-1);
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
                    User user = userService.GetByID(id);
                    if (user != null)
                    {
                        userService.Delete(user);
                        userService.Save();
                    }
                }
            }
            RouteValueDictionary routeValues = this.GridRouteValues();
            return RedirectToAction("Index", routeValues);
        }

        [HttpPost]
        [ActionName("Index")]
        [AcceptButton(ButtonName = "Search")]
        public ActionResult IndexSearch(string RoleList)
        {
            var list = userService.All;
            int RoleId = Protector.Int(RoleList);

            if (RoleId > 0)
            {
                list = list.Where(r => r.RoleID == RoleId);

            }
            IEnumerable<Role> roles = roleService.GetMany(r => r.IsActive).OrderByDescending(r => r.Order);
            ViewBag.RoleList = roles.ToSelectListItems(RoleId);
            return View(list);
        }


        /// <summary>
        /// create user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet]
        public ActionResult Create()
        {
            var user = new CreateUserModel();
            IEnumerable<Role> roles = roleService.GetMany(r => r.IsActive).OrderByDescending(r => r.Order);
            user.Role = roles.ToSelectListItems(-1);
            user.IsActive = true;
            return View(user);
        }

        /// <summary>
        /// edit user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var edit = new EditUserModel();
            User user = userService.GetByID(Protector.Int(id, -1));
            ModelCopier.CopyModel(user, edit);
            var roles = roleService.GetMany(r => r.IsActive);
            edit.Role = roles.ToSelectListItems(edit.RoleID);
            edit.Email = user.Email;
            return View(edit);
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            var password = new ChangePasswordModel();
            return View(password);
        }

        public ActionResult Menu(string controller)
        {
            var info = userService.GetByID(Protector.Int(HttpContext.User.Identity.Name));
            List<Category> result = new List<Category>();
            if (info != null)
            {
                List<Category> list = categoryService.GetMany(c => c.IsActive && c.ParentID == -1);
                foreach (Category item in list)
                {
                    //List<RolePermission> roles = rolePermissionService.All.Where(r => r.Category.ParentID == item.ID).ToList();
                    //if (roles != null && roles.Count > 0)
                    //{
                    //    result.Add(item);
                    //}
                    List<RolePermission> roles = rolePermissionService.All.Where(r => r.RoleID == info.RoleID && r.Category.ParentID == item.ID).ToList();
                    if (roles != null && roles.Count > 0)
                    {
                        result.Add(item);
                    }
                }

            }
            ViewBag.Controller = controller;
            return PartialView("Menu", result);
        }
        public ActionResult MenuSub(int catID)
        {
            List<Category> result = new List<Category>();
            var info = userService.GetByID(Protector.Int(HttpContext.User.Identity.Name));
            if (info != null)
            {
                List<Category> list = new List<Category>();
                if (catID == 80)
                {
                    list = categoryService.GetMany(c => c.IsActive && c.ParentID == catID && c.IsAdmin).OrderByDescending(c => c.Order).ToList();
                }
                else
                {
                    list = categoryService.GetMany(c => c.IsActive && c.ParentID == catID).OrderByDescending(c => c.Order).ToList();
                }

                foreach (Category item in list)
                {
                    List<RolePermission> roles = rolePermissionService.All.Where(r => r.RoleID == info.RoleID && r.CategoryID == item.ID).ToList();
                    if (roles != null && roles.Count > 0)
                    {
                        result.Add(item);
                    }
                }
            }
            return PartialView("MenuSub", result);
        }
        public ActionResult Logout()
        {
            var info = userService.GetByID(Protector.Int(HttpContext.User.Identity.Name));
            return PartialView("Logout", info);
        }
        public ActionResult UserInfo()
        {
            var info = userService.GetByID(Protector.Int(HttpContext.User.Identity.Name));

            return PartialView("UserInfo", info);
        }
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return Redirect("/administrator/");
        }
        #endregion
        #region  HttpPost
        /// <summary>
        ///  post created user
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(CreateUserModel current)
        {
            if (!ModelState.IsValid)
            {
                var roles = roleService.GetMany(r => r.IsActive).OrderByDescending(r => r.Order);
                current.Role = roles.ToSelectListItems(-1);
                return View(current);
            }
            var emailExists = userService.Get(u => u.Email == current.Email);
            if (emailExists != null)
            {
                var roles = roleService.GetMany(r => r.IsActive).OrderByDescending(r => r.Order);
                current.Role = roles.ToSelectListItems(Protector.Int(current.RoleID, -1));
                ModelState.AddModelError("", string.Format("Email [{0}] đã hiện hữu vui lòng chọn email khác", current.Email));
                return View(current);
            }
            User info = new User();
            current.Password = TextHelper.Hash(current.Password, HashMethod.MD5);
            ModelCopier.CopyModel(current, info);
            info.Email = current.Email;
            info.CreatedDate = DateTime.Now;
            userService.Insert(info);
            roleService.Save();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(EditUserModel current, string id)
        {
            if (!ModelState.IsValid)
            {
                var roles = roleService.GetMany(r => r.IsActive);
                current.Role = roles.ToSelectListItems(current.RoleID);
                return View(current);
            }
            int userID = Protector.Int(id);
            var emailExists = userService.Get(u => u.Email == current.Email && u.UserID != userID);
            if (emailExists != null)
            {
                var roles = roleService.GetMany(r => r.IsActive);
                current.Role = roles.ToSelectListItems(current.RoleID);
                ModelState.AddModelError("", string.Format("Email [{0}] đã hiện hữu vui lòng chọn email khác", current.Email));
                return View(current);
            }
            var userEdit = userService.GetByID(userID);
            if (TryUpdateModel(userEdit))
            {
                userEdit.Email = current.Email;
                userService.Save();
                return RedirectToAction("Index");
            }
            return View(userID);
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel cpass)
        {
            if (!ModelState.IsValid)
            {
                return View(cpass);
            }
            int userID = Protector.Int(HttpContext.User.Identity.Name);
            string pass = TextHelper.Hash(cpass.CurrentPassword, HashMethod.MD5);
            var info = userService.Get(u => u.UserID == userID && u.Password == pass);
            if (info != null)
            {
                info.Password = TextHelper.Hash(cpass.NewPassword, HashMethod.MD5);
                userService.Save();
                return View("ChangePasswordSuccess");
            }
            ModelState.AddModelError("", "Change Password False");
            return View(cpass);
        }
        #endregion
    }
}
