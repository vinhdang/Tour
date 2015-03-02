using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Service.Attribute;
using Telerik.Web.Mvc.Extensions;
using Domain.Entities;
using System.Web.Routing;
using Service.Helpers;
using Microsoft.Web.Mvc;
using Website.Areas.Administrator.Models.EmailSetting;
using System.Globalization;
using Website.Helpers;
using Amazon.SimpleEmail;
using System.Configuration;
using System.Collections;
using Amazon.SimpleEmail.Model;
using System.Timers;

namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class EmailSettingController : Controller
    {
        public IEmailSettingService emailSettingService { get; set; }
        public IEmailService emailService { get; set; } public IGroupTemplateService groupTemplateService { get; set; }
        public EmailSettingController(IEmailSettingService emailSettingService, IEmailService emailService, IGroupTemplateService groupTemplateService)
        {
            this.emailSettingService = emailSettingService;
            this.groupTemplateService = groupTemplateService;
            this.emailService = emailService;

        }
        // GET: /Administrator/EmailSetting/


        #region HttpGet
        public ActionResult Index()
        {
            var list = emailSettingService.All.ToList();

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
                    EmailSetting email = emailSettingService.Find(item);
                    if (email != null)
                    {

                        emailSettingService.Delete(email);
                        emailSettingService.Save();
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
            var list = emailSettingService.All;
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
            var email = new CreateEmailSettingModel();
            email.IsActive = true;
            List<GroupTemplate> group = groupTemplateService.All.Where(r => r.IsActive).ToList();
            email.Groups = group.ToSelectListItems(-1);
            return View(email);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            int emailID = Protector.Int(id);
            var email = new EditEmailSettingModel();
            EmailSetting info = emailSettingService.Find(emailID);
            if (info != null)
            {
                ModelCopier.CopyModel(info, email);
                List<GroupTemplate> group = groupTemplateService.All.Where(r => r.IsActive).ToList();
                email.Groups = group.ToSelectListItems(-1);
                email.Date = string.Format("{0:MM/dd/yyyy hh:MM}", info.Date);
                return View(email);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult SentTest(string id)
        {
            int emailID = Protector.Int(id);
            var email = new SentTestModel();
            EmailSetting info = emailSettingService.Find(emailID);
            if (info != null)
            {
                email.Body = info.Body;
                email.From = info.EmailSent;
                email.Subject = info.Subject;

                return View(email);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult SentMail()
        {

            System.Timers.Timer myTimer = new System.Timers.Timer();
            myTimer.Interval = 35000;
            myTimer.AutoReset = true;
            myTimer.Elapsed += new ElapsedEventHandler(myTimer_Elapsed);
            myTimer.Enabled = true;

            return View();
        }
        public void myTimer_Elapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            EmailSetting info = emailSettingService.All.Where(em => em.IsActive && em.IsSuccess == false).FirstOrDefault();
            if (info != null && info.NumberSent <= info.NumberEmail && info.Date <= DateTime.Now && DateTime.Now <= info.Date.AddHours(4))
            {
                List<Email> _email = emailService.All.Where(em => em.IsSent == false && em.IsActive).Take(28).ToList();
                if (_email != null && _email.Count > 0)
                {
                    ArrayList to = new ArrayList();
                    foreach (Email item in _email)
                    {
                        if (!to.Contains(item))
                        {
                            to.Add("dainguyen24_03@yahoo.com");
                        }
                        item.IsSent = true;
                        info.NumberSent++;
                        emailSettingService.Save();
                        emailService.Save();

                    }
                    AmazonSimpleEmailServiceConfig amConfig = new AmazonSimpleEmailServiceConfig();
                    amConfig.UseSecureStringForAwsSecretKey = false;
                    AmazonSimpleEmailServiceClient amzClient = new AmazonSimpleEmailServiceClient(ConfigurationManager.AppSettings["AKID"].ToString(), ConfigurationManager.AppSettings["Secret"].ToString(), amConfig);
                    if (to.Count > 0)
                    {
                        Destination dest = new Destination();
                        dest.WithBccAddresses((string[])to.ToArray(typeof(string)));
                        string body = info.Body;
                        string subject = info.Subject;
                        Body bdy = new Body();
                        bdy.Html = new Amazon.SimpleEmail.Model.Content(body);
                        Amazon.SimpleEmail.Model.Content title = new Amazon.SimpleEmail.Model.Content(subject);
                        Message message = new Message(title, bdy);
                        SendEmailRequest ser = new SendEmailRequest("www.Vietrip.vn<info@vietrip.vn>", dest, message);
                        SendEmailResponse seResponse = amzClient.SendEmail(ser);
                        SendEmailResult seResult = seResponse.SendEmailResult;
                    }

                }

            }
        }
        #endregion
        #region HttpPost
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SentTest(SentTestModel info, string id)
        {
            if (!ModelState.IsValid)
            {
                return View(info);
            }
            AmazonSimpleEmailServiceConfig amConfig = new AmazonSimpleEmailServiceConfig();
            amConfig.UseSecureStringForAwsSecretKey = false;
            AmazonSimpleEmailServiceClient amzClient = new AmazonSimpleEmailServiceClient(ConfigurationManager.AppSettings["AKID"].ToString(), ConfigurationManager.AppSettings["Secret"].ToString(), amConfig);
            ArrayList to = new ArrayList();
            to.Add(info.To);
            Destination dest = new Destination();
            dest.WithBccAddresses((string[])to.ToArray(typeof(string)));
            string body = info.Body;
            string subject = info.Subject;
            Body bdy = new Body();
            bdy.Html = new Amazon.SimpleEmail.Model.Content(body);
            Amazon.SimpleEmail.Model.Content title = new Amazon.SimpleEmail.Model.Content(subject);
            Message message = new Message(title, bdy);
            SendEmailRequest ser = new SendEmailRequest(info.From, dest, message);
            SendEmailResponse seResponse = amzClient.SendEmail(ser);
            SendEmailResult seResult = seResponse.SendEmailResult;
            ModelState.AddModelError("", seResult.MessageId);

            return View(info);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(CreateEmailSettingModel email)
        {
            if (!ModelState.IsValid)
            {
                List<GroupTemplate> group = groupTemplateService.All.Where(r => r.IsActive).ToList();
                email.Groups = group.ToSelectListItems(-1);
                return View(email);
            }
            EmailSetting info = emailSettingService.Get(r => r.Name == email.Name);
            if (info != null)
            {
                List<GroupTemplate> group = groupTemplateService.All.Where(r => r.IsActive).ToList();
                email.Groups = group.ToSelectListItems(-1);
                ModelState.AddModelError("", string.Format("Tên cấu hình email [{0}] đã hiện hữu vui lòng chọn tên khác", email.Name));
                return View(email);
            }
            EmailSetting _email = new EmailSetting();
            ModelCopier.CopyModel(email, _email);
            _email.CreatedDate = DateTime.Now;
            _email.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
            _email.IsSuccess = false;
            _email.NumberSent = 0;
            IFormatProvider culture = new CultureInfo("en-Us", true);
            _email.Date = Convert.ToDateTime(email.Date, culture);
            emailSettingService.Insert(_email);
            emailSettingService.Save();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(EditEmailSettingModel current, string id)
        {
            int emailSettingID = Protector.Int(id);
            EmailSetting info = emailSettingService.Get(d => d.EmailSettingID == emailSettingID);
            if (info != null)
            {
                if (!ModelState.IsValid)
                {
                    List<GroupTemplate> group = groupTemplateService.All.Where(r => r.IsActive).ToList();
                    current.Groups = group.ToSelectListItems(-1);
                    return View(current);
                }
                TryUpdateModel(info);
                IFormatProvider culture = new CultureInfo("en-Us", true);
                info.Date = Convert.ToDateTime(current.Date, culture);
                emailSettingService.Save();
            }
            return RedirectToAction("Index");
        }
        #endregion

    }
}
