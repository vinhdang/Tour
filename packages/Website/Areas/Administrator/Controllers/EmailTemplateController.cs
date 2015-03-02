using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Service.Attribute;
using Domain.Entities;
using System.Web.Routing;
using Telerik.Web.Mvc.Extensions;
using Service.Helpers;
using Website.Areas.Administrator.Models.EmailTemplate;
using Website.Helpers;
using Microsoft.Web.Mvc;
using Website.Helpers;

namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class EmailTemplateController : Controller
    {
        public IEmailTemplateService emailTemplateService { get; set; }
        public IHotelService hotelService { get; set; }
        public INationalService nationalService { get; set; }
        public IProvinceService provinceService { get; set; }
        public IGroupTemplateService groupTemplateService { get; set; }
        public IEmailSettingService emailSettingService { get; set; }
        public EmailTemplateController(IEmailSettingService emailSettingService, IGroupTemplateService groupTemplateService, IEmailTemplateService emailTemplateService, INationalService nationalService, IProvinceService provinceService, IHotelService hotelService)
        {
            this.emailTemplateService = emailTemplateService;
            this.hotelService = hotelService;
            this.nationalService = nationalService;
            this.provinceService = provinceService;
            this.groupTemplateService = groupTemplateService;
            this.emailSettingService = emailSettingService;
        }
        #region HttpGet
        public ActionResult Index()
        {
            var list = emailTemplateService.All.ToList();
            List<GroupTemplate> group = groupTemplateService.All.Where(r => r.IsActive).ToList();
            ViewBag.GroupTemplate = group.ToSelectListItems(-1);
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
                    EmailTemplate email = emailTemplateService.Find(item);
                    if (email != null)
                    {

                        emailTemplateService.Delete(email);
                        emailTemplateService.Save();
                    }
                }
            }
            RouteValueDictionary routeValues = this.GridRouteValues();
            return RedirectToAction("Index", routeValues);
        }
        [HttpPost]
        [ActionName("Index")]
        [AcceptButton(ButtonName = "Search")]
        public ActionResult IndexSearch(string key, string GroupTemplate)
        {
            int groupTemplateID = Protector.Int(GroupTemplate, 1);
            var list = emailTemplateService.All;
            if (groupTemplateID > 0)
            {
                list = list.Where(r => r.GroupTemplateID == groupTemplateID);
            }
            List<GroupTemplate> group = groupTemplateService.All.Where(r => r.IsActive).ToList();
            ViewBag.GroupTemplate = group.ToSelectListItems(groupTemplateID);
            return View(list.ToList());
        }

        [HttpPost]
        [ActionName("Index")]
        [AcceptButton(ButtonName = "Export")]
        public ActionResult Export(int[] checkedRecords, string GroupTemplate)
        {
            int groupTemplateID = Protector.Int(GroupTemplate, 1);
            return RedirectToAction("Export", new { id = groupTemplateID });
        }
        public ActionResult Export(string id)
        {
            int groupTemplateID = Protector.Int(id, 1);
            var list = emailTemplateService.All.Where(p => p.IsActive && p.GroupTemplateID == groupTemplateID).OrderByDescending(e => e.Order).ToList();
            return View(list);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Export(string id, string textbox)
        {
            int groupTemplateID = Protector.Int(id, 1);
            if (groupTemplateID > 0)
            {
                List<EmailSetting> list = emailSettingService.All.Where(p => p.GroupTemplateID == groupTemplateID).ToList();
                if (list != null && list.Count > 0)
                {
                    foreach (EmailSetting item in list)
                    {
                        item.Body = textbox;
                        emailSettingService.Save();
                    }
                }
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Create()
        {
            var email = new CreateEmailTemplateModel();
            email.IsActive = true;
            email.Nationals = nationalService.All.Where(n => n.IsActive).ToSelectListItems(-1);
            List<Province> provinces = new List<Province>();
            email.Provinces = provinces.ToSelectListItems(-1);
            List<Hotel> hotels = new List<Hotel>();
            email.Hotels = hotels.ToSelectListItems(-1);
            email.Groups = groupTemplateService.All.Where(g => g.IsActive).ToSelectListItems(-1);
            return View(email);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            int emailID = Protector.Int(id);
            var email = new CreateEmailTemplateModel();
            EmailTemplate info = emailTemplateService.Find(emailID);
            if (info != null)
            {
                ModelCopier.CopyModel(info, email);
                email.Nationals = nationalService.All.Where(n => n.IsActive).ToSelectListItems(info.Hotel.NationalID);
                email.NationalID = info.Hotel.NationalID;
                email.Provinces = provinceService.All.Where(p => p.NationalID == info.Hotel.NationalID).ToSelectListItems(info.Hotel.ProvinceID);
                email.ProvinceID = info.Hotel.ProvinceID;
                email.Hotels = hotelService.All.Where(p => p.ProvinceID == info.Hotel.ProvinceID).ToSelectListItems(info.Hotel.HotelID);
                email.Groups = groupTemplateService.All.Where(g => g.IsActive).ToSelectListItems(info.GroupTemplateID);
                return View(email);
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region HttpPost
        [HttpPost]
        public ActionResult Create(CreateEmailTemplateModel emailTemplate)
        {
            if (!ModelState.IsValid)
            {
                emailTemplate.Nationals = nationalService.All.Where(n => n.IsActive).ToSelectListItems(-1);
                List<Province> provinces = provinceService.All.Where(p => p.NationalID == emailTemplate.NationalID).ToList();
                emailTemplate.Provinces = provinces.ToSelectListItems(-1);
                return View(emailTemplate);
            }
            EmailTemplate info = emailTemplateService.Get(r => r.HotelID == emailTemplate.HotelID);
            if (info != null)
            {
                ModelState.AddModelError("", string.Format("Tên khách sạn đã hiện hữu vui lòng chọn tên khác", ""));
                emailTemplate.Nationals = nationalService.All.Where(n => n.IsActive).ToSelectListItems(-1);
                List<Province> provinces = provinceService.All.Where(p => p.NationalID == emailTemplate.NationalID).ToList();
                emailTemplate.Provinces = provinces.ToSelectListItems(-1);
                emailTemplate.Hotels = hotelService.All.Where(p => p.ProvinceID == info.Hotel.ProvinceID).ToSelectListItems(info.Hotel.HotelID);
                return View(emailTemplate);
            }
            info = new EmailTemplate();
            ModelCopier.CopyModel(emailTemplate, info);

            info.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
            info.CreatedDate = DateTime.Now;
            emailTemplateService.Insert(info);
            emailTemplateService.Save();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Edit(CreateEmailTemplateModel current, string id)
        {
            int emailTemplateID = Protector.Int(id);
            EmailTemplate info = emailTemplateService.Get(d => d.EmailTemplateID == emailTemplateID);
            if (info != null)
            {

                TryUpdateModel(info);
                emailTemplateService.Save();
            }
            return RedirectToAction("Index");
        }
        #endregion
        #region Json Action
        public ActionResult SelectProvince(string nationalID)
        {
            List<Province> list = new List<Province>();
            int nID = Protector.Int(nationalID);
            list = provinceService.All.Where(p => p.NationalID == nID).ToList();
            return Json(list.ToSelectListItems(-1), JsonRequestBehavior.AllowGet);
        }
        public ActionResult SelectHotel(string provinceID)
        {
            List<Hotel> list = new List<Hotel>();
            int _provinceID = Protector.Int(provinceID);
            list = hotelService.All.Where(p => p.ProvinceID == _provinceID).ToList();
            return Json(list.ToSelectListItems(-1), JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
