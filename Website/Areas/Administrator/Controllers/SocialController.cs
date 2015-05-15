using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Domain;
using Microsoft.Web.Mvc;
using Service.Helpers;
using Service.Attribute;
using System.Web.Routing;
using Telerik.Web.Mvc.Extensions;
using Website.Areas.Administrator.Models.Social;
using System.IO;

namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class SocialController : Controller
    {
        public ISocialService socialService { get; set; }
        public SocialController(ISocialService socialService)
        {
            this.socialService = socialService;
        }

        public ActionResult Index()
        {
            List<Social> list = socialService.All.OrderByDescending(s => s.Order).ToList();
            return View(list);
        }
        [HttpGet]
        public ActionResult Create()
        {

            var social = new CreateSocialModel();
            social.IsActive = true;
            return View(social);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            int socialID = Protector.Int(id);
            var _social = socialService.Find(socialID);
            if (_social != null)
            {
                EditSocialModel edit = new EditSocialModel();
                ModelCopier.CopyModel(_social, edit);
                edit.FilePath = _social.ICon;
                edit.SocialID = _social.ID;
                return View(edit);
            }
            return RedirectToAction("Error", "Administrator");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(CreateSocialModel current)
        {
            if (!ModelState.IsValid)
            {
                return View(current);
            }
            Social _social = new Social();
            ModelCopier.CopyModel(current, _social);
            _social.CreatedDate = DateTime.Now;
            _social.ICon = _social.Name;
            _social.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
            socialService.Insert(_social);
            socialService.Save();
            HttpPostedFileBase fileSmall = Request.Files[0];
            if (fileSmall != null && fileSmall.ContentLength > 0)
            {
                string fileNameS = DateTime.Now.ToString("HHmmss_ddMMyyyy_") + TextHelper.HandleTitleStringForImg(Path.GetFileName(fileSmall.FileName));
                FileHelper.SaveApartment_Picture(fileSmall, fileNameS, StorageElement.Social_PictureFolder, _social.ID);
                _social.ICon = fileNameS;
                socialService.Save();
            }
            return RedirectToAction("Index");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(EditSocialModel current, string id)
        {
            int socialID = Protector.Int(id);
            var _social = socialService.Find(socialID);
            if (_social != null)
            {
                if (!ModelState.IsValid)
                {
                    return View(current);
                }
                if (TryUpdateModel(_social))
                {
                    HttpPostedFileBase fileSmall = Request.Files[0];
                    if (fileSmall != null && fileSmall.ContentLength > 0)
                    {
                        var fullFilePath = FileHelper.getFullFilePath("Social", _social.ID, _social.ICon);
                        if (FileHelper.imageFileAvailable(fullFilePath))
                        {
                            System.IO.File.Delete(fullFilePath);
                        }
                        string fileName = DateTime.Now.ToString("HHmmss_ddMMyyyy_") + TextHelper.HandleTitleStringForImg(Path.GetFileName(fileSmall.FileName));
                        _social.ICon = fileName;
                        FileHelper.SaveApartment_Picture(fileSmall, fileName, StorageElement.Social_PictureFolder, _social.ID);
                    }
                    socialService.Save();
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Error", "Administrator");
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
                    Social _social = socialService.Find(item);
                    if (_social != null)
                    {
                        socialService.Delete(_social);
                        socialService.Save();
                        FileHelper.DeleteDirectory("Social", Protector.Int(_social.ID));
                    }
                }
            }
            RouteValueDictionary routeValues = this.GridRouteValues();
            return RedirectToAction("Index", routeValues);
        }
    }
}
