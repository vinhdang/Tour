using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Website.Areas.Administrator.Models.User;
using Domain.Entities;
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
        public UserController(IRoleService roleService, IUserService userService)
        {
            this.roleService = roleService;
            this.userService = userService;
        }
        #region HttpGet
        /// <summary>
        /// get all users
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var list = userService.All;
            IEnumerable<Role> roles = roleService.All.Where(r => r.IsActive).OrderByDescending(r => r.Order);
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
                    User user = userService.Find(id);
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
        public ActionResult IndexSearch(string key, string RoleList)
        {
            var list = userService.All;
            int RoleId = Protector.Int(RoleList);
            if (!string.IsNullOrEmpty(key))
            {
                list = list.Where(r => r.Email.Contains(key) || r.FullName.Contains(key) || r.Phone.Contains(key));
            }
            if (RoleId > 0)
            {
                list = list.Where(r => r.RoleID == RoleId);

            }
            ViewBag.Key = key;
            IEnumerable<Role> roles = roleService.All.Where(r => r.IsActive).OrderByDescending(r => r.Order);
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
            IEnumerable<Role> roles = roleService.All.Where(r => r.IsActive).OrderByDescending(r => r.Order);
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
            User user = userService.Find(Protector.Int(id, -1));
            ModelCopier.CopyModel(user, edit);
            var roles = roleService.All.Where(r => r.IsActive);
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

        public ActionResult Menu()
        {
            var info = userService.Find(Protector.Int(HttpContext.User.Identity.Name));
            if (info != null)
            {
                var role = roleService.Get(r => r.RoleID == info.RoleID);
                return PartialView("Menu", role);
            }
            return PartialView("Menu");
        }
        public ActionResult Logout()
        {
            var info = userService.Find(Protector.Int(HttpContext.User.Identity.Name));
            return PartialView("Logout", info);
        }
        [HttpPost]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
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
                var roles = roleService.All.Where(r => r.IsActive).OrderByDescending(r => r.Order);
                current.Role = roles.ToSelectListItems(-1);
                return View(current);
            }
            var emailExists = userService.Get(u => u.Email == current.Email);
            if (emailExists != null)
            {
                var roles = roleService.All.Where(r => r.IsActive).OrderByDescending(r => r.Order);
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
                var roles = roleService.All.Where(r => r.IsActive);
                current.Role = roles.ToSelectListItems(current.RoleID);
                return View(current);
            }
            int userID = Protector.Int(id);
            var emailExists = userService.Get(u => u.Email == current.Email && u.UserID != userID);
            if (emailExists != null)
            {
                var roles = roleService.All.Where(r => r.IsActive);
                current.Role = roles.ToSelectListItems(current.RoleID);
                ModelState.AddModelError("", string.Format("Email [{0}] đã hiện hữu vui lòng chọn email khác", current.Email));
                return View(current);
            }
            var userEdit = userService.Find(userID);
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
