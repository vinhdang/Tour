using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service;
using Website.Areas.Administrator.Models;
using Service.Helpers;
using Domain;
using Microsoft.Web.Mvc;
using Website.Log;
using System.IO;
using Service.IServices;
using Website.Areas.Administrator.Models.Config;

namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class ConfigController : Controller
    {
        public IConfigService configService { get; set; }
        public ConfigController(IConfigService configService)
        {
            this.configService = configService;
        }
        #region HttpGet
        public ActionResult Index()
        {
            var list = configService.All;
            return View(list);
        }
        public ActionResult Create()
        {
            var config = new CreateConfigModel();
            config.ListType = GlobalVariables.ConfigType;
            config.IsActive = true;
            return View(config);
        }
        public ActionResult Edit(int id)
        {
            var config = new CreateConfigModel();
            Config current = configService.Find(id);
            if (current != null)
            {
                ModelCopier.CopyModel(current, config);
                return View(config);
            }
            return RedirectToAction("Index");
        }
        public ActionResult Email()
        {
            var config = new CreateConfigEmailModel();
            config.Email = configService.All.Where(c => c.Name == "Email").FirstOrDefault().Value;
            config.HostName = configService.All.Where(c => c.Name == "Hostname").FirstOrDefault().Value;
            config.UserName = configService.All.Where(c => c.Name == "Username").FirstOrDefault().Value;
            config.Password = configService.All.Where(c => c.Name == "Password").FirstOrDefault().Value;
            config.Port = Protector.Int(configService.All.Where(c => c.Name == "Port").FirstOrDefault().Value);
            config.Timeout = Protector.Int(configService.All.Where(c => c.Name == "Timeout").FirstOrDefault().Value);
            config.EmailCC = Protector.String(configService.All.Where(c => c.Name == "EmailCC").FirstOrDefault().Value);
            return View(config);
        }
        public ActionResult Social()
        {
            var config = new CreateConfigSocialModel();
            config.Facebook = configService.All.Where(c => c.Name == "Facebook").FirstOrDefault().Value;
            config.Twitter = configService.All.Where(c => c.Name == "Twitter").FirstOrDefault().Value;
            config.GooglePlus = configService.All.Where(c => c.Name == "GooglePlus").FirstOrDefault().Value;
            config.Youtube = configService.All.Where(c => c.Name == "Youtube").FirstOrDefault().Value;
            return View(config);
        }
        public ActionResult Meta()
        {
            var config = new CreateConfigMetaModel();
            config.IndexTitle = configService.All.Where(c => c.Name == "IndexTitle").FirstOrDefault().Value;
            config.IndexDescription = configService.All.Where(c => c.Name == "IndexDescription").FirstOrDefault().Value;
            config.IndexKeyWords = configService.All.Where(c => c.Name == "IndexKeyWords").FirstOrDefault().Value;
            return View(config);
        }
        public ActionResult Analytics()
        {
            var config = new Config();
            config = configService.Get(c => c.Name == "GOOGLEANALYTICS");
            return View(config);
        }
        public ActionResult BookingTemplate()
        {
            var config = new Config();
            config = configService.Get(c => c.Name == "BookingTemplate");
            return View(config);
        }
        public ActionResult CompleteBooking()
        {
            var config = new Config();
            config = configService.Get(c => c.Name == "CompleteBooking");
            return View(config);
        }
        public ActionResult EmailTemplateNhanGiaPhong()
        {
            var config = new Config();
            config = configService.Get(c => c.Name == "EmailTemplateNhanGiaPhong");
            return View(config);
        }
        public ActionResult Online()
        {
            var config = new Config();
            config = configService.Get(c => c.Name == "Online");
            return View(config);
        }

        public ActionResult Script()
        {
            var config = new Config();
            config = configService.Get(c => c.Name == "Script");
            return View(config);
        }
        public ActionResult Logo()
        {
            var config = new CreateLogoModel();
            Config cf = configService.Get(c => c.Name == "Logo");
            if (cf != null)
            {
                config.FilePath = cf.Value;
                config.ConfigID = cf.ID;
            }
            return View(config);
        }
        public ActionResult Recruitment()
        {
            var config = new CreateRecruitmentModel();
            Config cf = configService.Get(c => c.Name == "RecruitmentFORM");
            if (cf != null)
            {
                config.Value = cf.Value;
                config.ConfigID = cf.ID;
            }
            return View(config);
        }

        public ActionResult CompanyPolicy()
        {
            var config = new CreateRecruitmentModel();
            Config cf = configService.Get(c => c.Name == "CompanyPolicy");
            if (cf != null)
            {
                config.Value = cf.Value;
                config.ConfigID = cf.ID;
            }
            return View(config);
        }
        public ActionResult Contact()
        {
            var config = new Config();
            config = configService.Get(c => c.Name == "Contact");
            return View(config);
        }
        public ActionResult Company()
        {
            var config = new Config();
            config = configService.Get(c => c.Name == "Company");
            return View(config);
        }
        #endregion
        #region HttpPost
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Email(CreateConfigEmailModel config, string ProtocolValue)
        {
            if (!ModelState.IsValid)
            {
                config.Email = configService.All.Where(c => c.Name == "Email").FirstOrDefault().Value;
                config.HostName = configService.All.Where(c => c.Name == "HostName").FirstOrDefault().Value;
                config.UserName = configService.All.Where(c => c.Name == "UserName").FirstOrDefault().Value;
                config.Password = configService.All.Where(c => c.Name == "Password").FirstOrDefault().Value;
                config.Port = Protector.Int(configService.All.Where(c => c.Name == "Port").FirstOrDefault().Value);
                config.Timeout = Protector.Int(configService.All.Where(c => c.Name == "Timeout").FirstOrDefault().Value);
                config.EmailCC = Protector.String(configService.All.Where(c => c.Name == "EmailCC").FirstOrDefault().Value);
                return View(config);
            }
            Config configEmail = configService.All.Where(c => c.Name == "Email").FirstOrDefault();
            if (configEmail != null)
            {
                configEmail.Value = config.Email;
                configService.Save();
            }

            Config configHostName = configService.All.Where(c => c.Name == "HostName").FirstOrDefault();
            if (configHostName != null)
            {
                configHostName.Value = config.HostName;
                configService.Save();
            }
            Config configUserName = configService.All.Where(c => c.Name == "UserName").FirstOrDefault();
            if (configUserName != null)
            {
                configUserName.Value = config.UserName;
                configService.Save();
            }
            Config configPassword = configService.All.Where(c => c.Name == "Password").FirstOrDefault();
            if (configPassword != null && !string.IsNullOrEmpty(config.Password))
            {
                configPassword.Value = Protector.String(config.Password);
                configService.Save();
            }
            Config configPort = configService.All.Where(c => c.Name == "Port").FirstOrDefault();
            if (configPort != null)
            {
                configPort.Value = config.Port.ToString();
                configService.Save();
            }
            Config configTimeout = configService.All.Where(c => c.Name == "Timeout").FirstOrDefault();
            if (configTimeout != null)
            {
                configTimeout.Value = config.Timeout.ToString();
                configService.Save();
            }
            Config configEmailCC = configService.All.Where(c => c.Name == "EmailCC").FirstOrDefault();
            if (configEmailCC != null)
            {
                configEmailCC.Value = Protector.String(config.EmailCC, "");
                configService.Save();
            }
            return Redirect(Request.RawUrl);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Social(CreateConfigSocialModel config)
        {
            if (!ModelState.IsValid)
            {
                config.Facebook = configService.All.Where(c => c.Name == "Facebook").FirstOrDefault().Value;
                config.Twitter = configService.All.Where(c => c.Name == "Twitter").FirstOrDefault().Value;
                config.GooglePlus = configService.All.Where(c => c.Name == "GooglePlus").FirstOrDefault().Value;
                config.Youtube = configService.All.Where(c => c.Name == "Youtube").FirstOrDefault().Value;
                return View(config);
            }
            Config configFacebook = configService.All.Where(c => c.Name == "Facebook").FirstOrDefault();
            if (configFacebook != null)
            {
                configFacebook.Value = Protector.String(config.Facebook);
                configService.Save();
            }
            Config configTwitter = configService.All.Where(c => c.Name == "Twitter").FirstOrDefault();
            if (configTwitter != null)
            {
                configTwitter.Value = config.Twitter.ToString();
                configService.Save();
            }
            Config configGooglePlus = configService.All.Where(c => c.Name == "GooglePlus").FirstOrDefault();
            if (configGooglePlus != null)
            {
                configGooglePlus.Value = config.GooglePlus.ToString();
                configService.Save();
            }
            Config configYoutube = configService.All.Where(c => c.Name == "Youtube").FirstOrDefault();
            if (configYoutube != null)
            {
                configYoutube.Value = config.Youtube.ToString();
                configService.Save();
            }
            return Redirect(Request.RawUrl);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Meta(CreateConfigMetaModel config)
        {
            if (!ModelState.IsValid)
            {
                config.IndexTitle = configService.All.Where(c => c.Name == "IndexTitle").FirstOrDefault().Value;
                config.IndexDescription = configService.All.Where(c => c.Name == "IndexDescription").FirstOrDefault().Value;
                config.IndexKeyWords = configService.All.Where(c => c.Name == "IndexKeyWords").FirstOrDefault().Value;
                return View(config);
            }
            Config configIndexTitle = configService.All.Where(c => c.Name == "IndexTitle").FirstOrDefault();
            if (configIndexTitle != null)
            {
                configIndexTitle.Value = Protector.String(config.IndexTitle);
                configService.Save();
            }
            Config configIndexDescription = configService.All.Where(c => c.Name == "IndexDescription").FirstOrDefault();
            if (configIndexDescription != null)
            {
                configIndexDescription.Value = config.IndexDescription.ToString();
                configService.Save();
            }
            Config configIndexKeyWords = configService.All.Where(c => c.Name == "IndexKeyWords").FirstOrDefault();
            if (configIndexKeyWords != null)
            {
                configIndexKeyWords.Value = config.IndexKeyWords.ToString();
                configService.Save();
            }
            return Redirect(Request.RawUrl);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Analytics(Config config)
        {
            if (!ModelState.IsValid)
            {
                config = configService.Get(c => c.Name == "GOOGLEANALYTICS");
                return View(config);
            }
            Config current = configService.Get(c => c.Name == "GOOGLEANALYTICS");
            if (current != null)
            {
                current.Value = Protector.String(config.Value);
                configService.Save();
            }

            return Redirect(Request.RawUrl);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult BookingTemplate(Config config)
        {
            if (!ModelState.IsValid)
            {
                config = configService.Get(c => c.Name == "BookingTemplate");
                return View(config);
            }
            Config current = configService.Get(c => c.Name == "BookingTemplate");
            if (current != null)
            {
                current.Value = Protector.String(config.Value);
                configService.Save();
            }

            return Redirect(Request.RawUrl);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CompleteBooking(Config config)
        {
            if (!ModelState.IsValid)
            {
                config = configService.Get(c => c.Name == "CompleteBooking");
                return View(config);
            }
            Config current = configService.Get(c => c.Name == "CompleteBooking");
            if (current != null)
            {
                current.Value = Protector.String(config.Value);
                configService.Save();
            }

            return Redirect(Request.RawUrl);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EmailTemplateNhanGiaPhong(Config config)
        {
            if (!ModelState.IsValid)
            {
                config = configService.Get(c => c.Name == "EmailTemplateNhanGiaPhong");
                return View(config);
            }
            Config current = configService.Get(c => c.Name == "EmailTemplateNhanGiaPhong");
            if (current != null)
            {
                current.Value = Protector.String(config.Value);
                configService.Save();
            }

            return Redirect(Request.RawUrl);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Online(Config config)
        {
            if (!ModelState.IsValid)
            {
                config = configService.Get(c => c.Name == "Online");
                return View(config);
            }
            Config current = configService.Get(c => c.Name == "Online");
            if (current != null)
            {
                current.Value = Protector.Int(config.Value.Trim()).ToString();
                configService.Save();
            }

            return Redirect(Request.RawUrl);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Script(Config config)
        {
            if (!ModelState.IsValid)
            {
                config = configService.Get(c => c.Name == "Script");
                return View(config);
            }
            Config current = configService.Get(c => c.Name == "Script");
            if (current != null)
            {
                current.Value = Protector.String(config.Value);
                configService.Save();
            }

            return Redirect(Request.RawUrl);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Logo(CreateLogoModel config)
        {
            if (!ModelState.IsValid)
            {
                Config cf = configService.Get(c => c.Name == "Logo");
                if (cf != null)
                {
                    config.FilePath = cf.Value;
                    config.ConfigID = cf.ID;
                }
                return View(config);
            }
            Config current = configService.Get(c => c.Name == "Logo");
            if (current != null)
            {
                HttpPostedFileBase fileSmall = Request.Files[0];
                if (fileSmall != null && fileSmall.ContentLength > 0)
                {
                    var fullFilePath = FileHelper.getFullFilePath("Config", current.ID, current.Value);
                    if (FileHelper.imageFileAvailable(fullFilePath))
                    {
                        System.IO.File.Delete(fullFilePath);
                    }
                    string fileName = DateTime.Now.ToString("HHmmss_ddMMyyyy_") + TextHelper.HandleTitleStringForImg(Path.GetFileName(fileSmall.FileName));
                    FileHelper.SaveApartment_Picture(fileSmall, fileName, StorageElement.Config_PictureFolder, current.ID);
                    current.Value = fileName;
                    configService.Save();
                }

            }
            return Redirect(Request.RawUrl);
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Recruitment(CreateRecruitmentModel config)
        {
            if (!ModelState.IsValid)
            {
                Config cf = configService.Get(c => c.Name == "RecruitmentFORM");
                if (cf != null)
                {
                    config.FilePath = cf.Value;
                    config.ConfigID = cf.ID;
                }
                return View(config);
            }
            Config current = configService.Get(c => c.Name == "RecruitmentFORM");
            if (current != null)
            {
                //
                current.Value = config.Value;
                current.CreatedDate = DateTime.Now;

                configService.Update(current);
                configService.Save();

            }else
            {
                current = new Config()
                              {
                                  Name = "RecruitmentFORM",
                                  CreatedDate = DateTime.Now,
                                  Type = 3,
                                  IsActive = true,
                                  Value = config.Value
                              };

                configService.Insert(current);
                configService.Save();
            }
            return View(config);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CompanyPolicy(CreateRecruitmentModel config)
        {
            if (!ModelState.IsValid)
            {
                Config cf = configService.Get(c => c.Name == "CompanyPolicy");
                if (cf != null)
                {
                    config.FilePath = cf.Value;
                    config.ConfigID = cf.ID;
                }
                return View(config);
            }
            Config current = configService.Get(c => c.Name == "CompanyPolicy");
            if (current != null)
            {
                //
                current.Value = config.Value;
                current.CreatedDate = DateTime.Now;

                configService.Update(current);
                configService.Save();

            }
            else
            {
                current = new Config()
                {
                    Name = "CompanyPolicy",
                    CreatedDate = DateTime.Now,
                    Type = 3,
                    IsActive = true,
                    Value = config.Value
                };

                configService.Insert(current);
                configService.Save();
            }
            return View(config);
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Contact(Config config)
        {
            if (!ModelState.IsValid)
            {
                config = configService.Get(c => c.Name == "Contact");
                return View(config);
            }
            Config current = configService.Get(c => c.Name == "Contact");
            if (current != null)
            {
                current.Value = Protector.String(config.Value);
                configService.Save();
            }

            return Redirect(Request.RawUrl);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(CreateConfigModel config_model)
        {
            if (!ModelState.IsValid)
            {
                config_model.ListType = GlobalVariables.ConfigType;
                return View(config_model);
            }
            Config config = new Config();
            ModelCopier.CopyModel(config_model, config);
            config.CreatedDate = DateTime.Now;
            configService.Insert(config);
            configService.Save();
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(CreateConfigModel config_model, int id)
        {
            if (!ModelState.IsValid)
            {
                return View(config_model);
            }
            Config config = configService.Find(id);
            if (config != null)
            {
                TryUpdateModel(config);
                switch (config.Type)
                {
                    case 4:
                        string fullFile = this.getFullFilePath(config.Value);
                        if (FileHelper.imageFileAvailable(fullFile))
                        {
                            FileHelper.DeleteConfig_Picture(config.Value, config.ID);
                            FileInfo file = new FileInfo(fullFile);
                            config.Value = file.Name;
                            FileHelper.SaveConfig_Picture(config.Value, fullFile, StorageElement.Config_PictureFolder, config.ID);
                        }
                        else
                        {
                            config.Value = config_model.Value;
                        }

                        break;
                }
                configService.Save();
                return RedirectToAction("Index");

            }
            return RedirectToAction("Error", "Administrator");
        }
        private string getFullFilePath(string Path)
        {
            return Server.MapPath(Path);
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Company(Config config)
        {
            if (!ModelState.IsValid)
            {
                config = configService.Get(c => c.Name == "Company");
                return View(config);
            }
            Config current = configService.Get(c => c.Name == "Company");
            if (current != null)
            {
                current.Value = Protector.String(config.Value);
                configService.Save();
            }

            return Redirect(Request.RawUrl);
        }
        #endregion
    }
}
