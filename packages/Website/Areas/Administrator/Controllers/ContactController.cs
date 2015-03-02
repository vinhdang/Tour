using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.Log;
using Service;
using Website.Areas.Administrator.Models;
using Domain.Entities;
using Microsoft.Web.Mvc;
using Service.Helpers;
using Service.IServices;
using Website.Areas.Administrator.Models.Contact;

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
            var list = contactService.All;
            return View(list);
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
