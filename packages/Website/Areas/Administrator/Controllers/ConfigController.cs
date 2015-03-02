using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service;
using Website.Areas.Administrator.Models;
using Service.Helpers;
using Domain.Entities;
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
        #endregion
        #region HttpPost
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
                            FileHelper.DeleteConfig_Picture(config.Value, config.ConfigID);
                            FileInfo file = new FileInfo(fullFile);
                            config.Value = file.Name;
                            FileHelper.SaveConfig_Picture(config.Value, fullFile, StorageElement.Config_PictureFolder, config.ConfigID);
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
            return RedirectToAction("Index");
        }
        private string getFullFilePath(string Path)
        {
            return Server.MapPath(Path);
        }
        #endregion
    }
}
