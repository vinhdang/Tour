using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Service.Attribute;
using Domain.Entities;
using Service.Helpers;
using System.Web.Routing;
using Telerik.Web.Mvc.Extensions;
using Microsoft.Web.Mvc;

namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class GroupTemplateController : Controller
    {
        public IGroupTemplateService groupTemplateService { get; set; }
        public GroupTemplateController(IGroupTemplateService groupTemplateService)
        {
            this.groupTemplateService = groupTemplateService;
        }
        #region HttpGet
        public ActionResult Index()
        {
            var list = groupTemplateService.All;

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
                foreach (int item in checkedRecords)
                {
                    GroupTemplate email = groupTemplateService.Find(item);
                    if (email != null)
                    {

                        groupTemplateService.Delete(email);
                        groupTemplateService.Save();
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
            var list = groupTemplateService.All;
            if (!string.IsNullOrEmpty(key))
            {
                list = list.Where(r => r.Name.Contains(key));
            }
            ViewBag.Key = key;
            return View(list.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            var email = new GroupTemplate();
            email.IsActive = true;
            return View(email);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            int emailID = Protector.Int(id);

            GroupTemplate info = groupTemplateService.Find(emailID);
            if (info != null)
            {
                return View(info);
            }
            return RedirectToAction("Index");
        }
        #endregion
        #region HttpPost

        [HttpPost]
        public ActionResult Create(GroupTemplate email)
        {
            if (!ModelState.IsValid)
            {

                return View(email);
            }
            GroupTemplate info = groupTemplateService.Get(r => r.Name == email.Name);
            if (info != null)
            {

                ModelState.AddModelError("", string.Format("Tên email [{0}] đã hiện hữu vui lòng chọn tên khác", email.Name));
                return View(email);
            }
            GroupTemplate _email = new GroupTemplate();
            ModelCopier.CopyModel(email, _email);
            _email.CreatedDate = DateTime.Now;
            _email.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
            groupTemplateService.Insert(_email);
            groupTemplateService.Save();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(GroupTemplate current, string id)
        {
            int emailSettingID = Protector.Int(id);
            GroupTemplate info = groupTemplateService.Find(emailSettingID);
            if (info != null)
            {
                if (!ModelState.IsValid)
                {
                    return View(current);
                }
                TryUpdateModel(info);

                groupTemplateService.Save();
            }
            return RedirectToAction("Index");
        }
        #endregion

    }
}
