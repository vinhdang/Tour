using System;
using System.Web.Mvc;
using Website.Models;
using Service;
using Service.Helpers;
using Website.Helpers;
using Website.Log;
using Service.IServices;
using Website.Models.User;
using LogOnModel = Website.Models.User.LogOnModel;

namespace Website.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/
        private readonly IUserService userService;
        private readonly IRoleService roleService;

        public UserController(IUserService userService, IRoleService roleService)
        {
            this.userService = userService;
            this.roleService = roleService;
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult LogOn()
        {
            var user = new LogOnModel();
            return View(user);
        }
        [HttpPost]
        public ActionResult LogOn(string returnUrl, LogOnModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            //returnUrl = "/Administrator";
            returnUrl = string.IsNullOrEmpty(returnUrl) ? "/Home/" : returnUrl;
            string pass = TextHelper.Hash(user.Password, HashMethod.MD5);
            var info = userService.Get(u => u.Email == user.Email && u.Password == pass);
            if (info != null)
            {
                FormsAuthenticationHelper.SetAuthCookie(info.UserID, info.Role.Name, false, TimeSpan.FromMinutes(1000));
                return Redirect(returnUrl);
            }
            else
            {
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }
            return View(user);
        }
        [ChildActionOnly]
        [OutputCache(Duration = 13600)]
        public ActionResult TopLogin()
        {
            return PartialView("TopLogin");
        }

    }
}
