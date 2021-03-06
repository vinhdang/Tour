﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Service.Helpers;
using Domain.Entities;
using Microsoft.Web.Mvc;
using Website.Model.Contact;

namespace Website.Controllers
{
    public class ContactController : Controller
    {
        public ICategoryService categoryService { get; set; }
        public IContactService contactService { get; set; }
        public IConfigService configService { get; set; }
        public ContactController(IContactService contactService, ICategoryService categoryService, IConfigService configService)
        {
            this.contactService = contactService;
            this.categoryService = categoryService;
            this.configService = configService;
        }
        #region HttpGet
        public ActionResult Index(string id)
        {
            int CatID = Protector.Int(id);
            Category info = categoryService.Find(CatID);
            if (info != null)
            {
                ViewBag.Title = info.PageTitle;
                ViewBag.KeyWords = info.Keyword;
                ViewBag.Description = info.Description;
                ContactModel contact = new ContactModel();
                contact.Category = info;
                return View(contact);
            }
            return View();
        }
        public ActionResult ThankYou()
        {
            return View();
        }
        #endregion
        #region HttpPost
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(ContactModel contact, string id)
        {
            if (!ModelState.IsValid)
            {
                int CatID = Protector.Int(id);
                Category category = categoryService.Find(CatID);
                if (category != null)
                {
                    contact.Category = category;
                    return View(contact);
                }
            }
            Contact info = new Contact();
            ModelCopier.CopyModel(contact, info);
            info.CreatedDate = DateTime.Now;
            info.Isprocessing = false;
            contactService.Insert(info);
            contactService.Save();
            return RedirectToAction("ThankYou");
        }
        #endregion

    }
}
