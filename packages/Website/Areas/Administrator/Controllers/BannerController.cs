using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service;
using Service.Helpers;
using Microsoft.Web.Mvc;
using Domain.Entities;
using Website.Areas.Administrator.Models;
using System.IO;
using Website.Log;
using Website.Helpers;
using Service.IServices;
using Website.Areas.Administrator.Models.Banner;

namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class BannerController : Controller
    {
        public IBannerService bannerService { get; set; }
        public IProvinceService provinceService { get; set; }
        public BannerController(IBannerService bannerService, IProvinceService provinceService)
        {
            this.bannerService = bannerService;
            this.provinceService = provinceService;
        }

        #region HttpGet
        public ActionResult Index(string type, string ListProvince)
        {
            ViewBag.Type = Service.Helpers.GlobalVariables.BannerType;
            var list = bannerService.All;
            int typeID = Protector.Int(type);
            int provinceID = Protector.Int(ListProvince);
            if (typeID > 0)
            {
                switch (typeID)
                {
                    case 1:
                        list = list.Where(n => (n.Position & 1) == 1);
                        break;
                    case 2:
                        list = list.Where(n => (n.Position & 2) == 2);
                        break;
                    case 4:
                        list = list.Where(n => (n.Position & 4) == 4);
                        break;
                }
            }
            if (typeID > 0)
            {
                list = list.Where(n => n.ProvinceID == provinceID);
            }

            ViewBag.ListProvince = provinceService.All.Where(p => p.IsActive).ToSelectListItems(-1);
            return View(list);
        }
        public ActionResult Create()
        {
            var banner = new CreateBannerModel();

            banner.IsActive = true;
            banner.ListProvince = provinceService.All.Where(p => p.IsActive).ToSelectListItems(-1);
            banner.ListPosition = GlobalVariables.ListBannerPosition;
            return View(banner);
        }
        public ActionResult Edit(string id)
        {
            int BannerID = Protector.Int(id);
            var banner = new EditBannerModel();
            Banner current = bannerService.Find(BannerID);
            if (current != null)
            {
                ModelCopier.CopyModel(current, banner);
                if (current.ProvinceID != null)
                {
                    banner.ListProvince = provinceService.All.Where(p => p.IsActive).ToSelectListItems((int)current.ProvinceID);
                }
                else { banner.ListProvince = provinceService.All.Where(p => p.IsActive).ToSelectListItems(-1); }
                banner.ListPosition = TextHelper.LoadBannerPosition(current.Position);
                banner.FilePath = current.FileName;
                return View(banner);
            }
            return RedirectToAction("Index");
        }

        #endregion
        #region HttpPost
        [HttpPost]
        public ActionResult Create(CreateBannerModel banner_model, string[] Position)
        {
            if (!ModelState.IsValid)
            {
                banner_model.ListProvince = provinceService.All.Where(p => p.IsActive).ToSelectListItems(-1);
                banner_model.ListPosition = GlobalVariables.ListBannerPosition;
                return View(banner_model);
            }

            HttpPostedFileBase fileSmall = Request.Files[0];
            if (fileSmall != null && fileSmall.ContentLength > 0)
            {
                Banner banner = new Banner();
                ModelCopier.CopyModel(banner_model, banner);
                banner.FileName = Path.GetFileName(fileSmall.FileName);
                banner.ContentLength = fileSmall.ContentLength;
                banner.ContentType = fileSmall.ContentType;
                banner.Extension = Path.GetExtension(fileSmall.FileName);
                banner.ProvinceID = banner_model.ProvinceID;
                banner.CreatedDate = DateTime.Now;
                banner.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
                banner.Position = TextHelper.UpdateType(Position);
                bannerService.Insert(banner);
                bannerService.Save();
                FileHelper.SaveProduct_Picture(fileSmall, StorageElement.Banner_PictureFolder, banner.BannerID);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Edit(EditBannerModel banner, string id, string[] Position)
        {
            int BannerID = Protector.Int(id);
            Banner current = bannerService.Find(BannerID);
            if (current != null)
            {
                if (!ModelState.IsValid)
                {
                    if (current.ProvinceID != null)
                    {
                        banner.ListProvince = provinceService.All.Where(p => p.IsActive).ToSelectListItems((int)current.ProvinceID);
                    }
                    else { banner.ListProvince = provinceService.All.Where(p => p.IsActive).ToSelectListItems(-1); }
                    banner.ListPosition = TextHelper.LoadBannerPosition(current.Position);
                    banner.FilePath = current.FileName;
                    return View(banner);
                }
                current.Name = banner.Name;
                current.Link = banner.Link;
                current.Order = banner.Order;
                current.IsActive = banner.IsActive;
                HttpPostedFileBase fileSmall = Request.Files[0];
                if (fileSmall != null && fileSmall.ContentLength > 0)
                {
                    var fullFilePath = FileHelper.getFullFilePath("Banner", current.BannerID, current.FileName);
                    if (FileHelper.imageFileAvailable(fullFilePath))
                    {
                        System.IO.File.Delete(fullFilePath);
                    }
                    FileHelper.SaveProduct_Picture(fileSmall, StorageElement.Banner_PictureFolder, current.BannerID);
                    current.FileName = Path.GetFileName(fileSmall.FileName);
                    current.ContentLength = fileSmall.ContentLength;
                    current.ContentType = fileSmall.ContentType;
                    current.Extension = Path.GetExtension(fileSmall.FileName);
                    current.ProvinceID = banner.ProvinceID;
                    current.Position = TextHelper.UpdateType(Position);
                    bannerService.Save();

                }
                else
                {
                    current.ProvinceID = banner.ProvinceID;
                    current.Position = TextHelper.UpdateType(Position);
                    bannerService.Save();

                }

            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public PartialViewResult Delete(string id)
        {
            int bannerID = Protector.Int(id);
            Banner banner = bannerService.Find(bannerID);
            if (banner != null)
            {
                var fullFilePath = FileHelper.getFullFilePath("Banner", banner.BannerID, banner.FileName);
                if (FileHelper.imageFileAvailable(fullFilePath))
                {
                    System.IO.File.Delete(fullFilePath);
                }
                bannerService.Delete(banner);
                bannerService.Save();
            }
            var pictures = bannerService.All;
            return PartialView("BannerList", pictures);
        }
        #endregion
        #region Private
        private string getFullFilePath(string Path)
        {
            return Server.MapPath(Path);
        }
        #endregion
    }
}
