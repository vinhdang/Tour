using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.Log;
using Service;
using Website.Areas.Administrator.Models;
using Domain;
using Microsoft.Web.Mvc;
using Service.Helpers;
using Service.IServices;
using Website.Areas.Administrator.Models.Contact;
using Service.Attribute;
using Telerik.Web.Mvc.Extensions;
using System.Web.Routing;

namespace Website.Areas.Administrator.Controllers
{

    [Authorize]
    public class ContactController : Controller
    {
        private readonly IContactService contactService;
        public ContactController(IContactService contactService)
        {
            this.contactService = contactService;
        }
        [HttpGet]
        public ActionResult Index()
        {
            List<Contact> list = contactService.All.ToList();
            return View(list);
        }
        [HttpPost]
        [ActionName("Index")]
        [AcceptButton(ButtonName = "Delete")]
        public ActionResult IndexDelete(int[] checkedRecords)
        {
            if (checkedRecords != null && checkedRecords.Length > 0)
            {
                foreach (int item in checkedRecords)
                {
                    Contact _contact = contactService.Find(item);
                    if (_contact != null)
                    {
                        contactService.Delete(_contact);
                        contactService.Save();
                    }
                }
            }
            RouteValueDictionary routeValues = this.GridRouteValues();
            return RedirectToAction("Index", routeValues);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var edit = new EditContactModel();

            Contact info = contactService.Find(Protector.Int(id, 0));
            if (info != null)
            {
                ModelCopier.CopyModel(info, edit);
                return View(edit);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Edit(int id, string Isprocessing)
        {
            Contact info = contactService.Find(Protector.Int(id, 0));
            if (!ModelState.IsValid)
            {
                return View(info);
            }
            info.Isprocessing = Protector.Bool(Isprocessing);
            contactService.Save();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Contact info = contactService.Find(id);
            if (info != null)
            {
                contactService.Delete(info);

            }
            return PartialView("ContactList", contactService.All);
        }
    }


}
