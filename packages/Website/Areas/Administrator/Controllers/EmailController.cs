using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Domain.Entities;
using Service.Helpers;
using Microsoft.Web.Mvc;
using Service.Attribute;
using System.Web.Routing;
using Telerik.Web.Mvc.Extensions;
using Website.Areas.Administrator.Models.Email;
using System.Data.OleDb;
using System.IO;

namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class EmailController : Controller
    {

        public IEmailService emailService { get; set; }
        public EmailController(IEmailService emailService)
        {
            this.emailService = emailService;
        }
        #region HttpGet
        public ActionResult Index()
        {
            var list = emailService.All;

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
                    Email email = emailService.Find(item);
                    if (email != null)
                    {

                        emailService.Delete(email);
                        emailService.Save();
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
            var list = emailService.All;
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
            var email = new Email();
            email.IsActive = true;
            return View(email);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            int emailID = Protector.Int(id);

            Email info = emailService.Find(emailID);
            if (info != null)
            {
                return View(info);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Import()
        {
            var email = new ImportMailModel();
            return View(email);
        }
        #endregion
        #region HttpPost
        [HttpPost]
        public ActionResult Import(ImportMailModel email)
        {
            if (!ModelState.IsValid)
            {

                return View(email);
            }
            HttpPostedFileBase fileSmall = Request.Files[0];
            if (fileSmall != null && fileSmall.ContentLength > 0)
            {
                string fileName = DateTime.Now.ToString("HHmmss_ddMMyyyy_") + TextHelper.HandleTitleStringForImg(Path.GetFileName(fileSmall.FileName));
                FileHelper.SaveHotel_Picture(fileSmall, fileName, StorageElement.Import_Email, 1);
                string extension = Path.GetExtension(fileName).ToLower();
                string provider = "Microsoft.ACE.OLEDB.12.0";
                if (extension == ".xls")
                {
                    provider = "Microsoft.Jet.OLEDB.4.0";
                }
                var path = Path.Combine(Server.MapPath(StorageElement.Import_Email), "1");
                string sourcePath = Path.Combine(path, fileName);

                OleDbConnection oconn = new OleDbConnection(@"Provider=" + provider + ";Data Source=" + sourcePath + ";Extended Properties=Excel 8.0;");
                try
                {
                    OleDbCommand ocmd = new OleDbCommand("select * from [Sheet1$]", oconn);
                    oconn.Open();
                    OleDbDataReader odr = ocmd.ExecuteReader();

                    while (odr.Read())
                    {
                        string name = Protector.String(valid(odr, 0)).Trim();
                        if (EmailHelper.IsEmail(name))
                        {
                            Email current = emailService.All.Where(e => e.Name == name).FirstOrDefault();
                            if (current == null)
                            {
                                Email _email = new Email();
                                _email.CreatedDate = DateTime.Now;
                                _email.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
                                _email.IsSent = false;
                                _email.IsActive = true;
                                _email.Name = name;
                                emailService.Insert(_email);
                                emailService.Save();
                            }
                        }
                    }
                }
                catch { }
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Create(Email email)
        {
            if (!ModelState.IsValid)
            {

                return View(email);
            }
            Email info = emailService.Get(r => r.Name == email.Name);
            if (info != null)
            {

                ModelState.AddModelError("", string.Format("Tên email [{0}] đã hiện hữu vui lòng chọn tên khác", email.Name));
                return View(email);
            }
            Email _email = new Email();
            ModelCopier.CopyModel(email, _email);
            _email.CreatedDate = DateTime.Now;
            _email.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
            _email.IsSent = false;
            emailService.Insert(_email);
            emailService.Save();
            ModelState.AddModelError("", "Tạo thành công");
            return View(email);
        }
        #endregion
        protected string valid(OleDbDataReader myreader, int stval)//if any columns are found null then they are replaced by zero
        {
            object val = myreader[stval];
            if (val != DBNull.Value)
                return val.ToString();
            else
                return Convert.ToString(0);
        }
    }
}
