using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Service.Attribute;
using System.Web.Routing;
using Domain.Entities;
using Telerik.Web.Mvc.Extensions;
using Service.Helpers;
using Website.Helpers;
using Microsoft.Web.Mvc;
using Website.Areas.Administrator.Models.Province;
using System.IO;

namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class ProvinceController : Controller
    {
        public INationalService nationalService { get; set; }

        public IProvinceService provinceService { get; set; }
        public ProvinceController(INationalService nationalService, IProvinceService provinceService)
        {

            this.nationalService = nationalService;
            this.provinceService = provinceService;
        }
        #region HttpGet
        public ActionResult Index()
        {
            List<Province> list = provinceService.All.ToList();
            IEnumerable<National> nationals = nationalService.All.Where(r => r.IsActive).OrderByDescending(r => r.Order);
            ViewBag.NationalList = nationals.ToSelectListItems(-1);
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

            }
            RouteValueDictionary routeValues = this.GridRouteValues();
            return RedirectToAction("Index", routeValues);
        }

        [HttpPost]
        [ActionName("Index")]
        [AcceptButton(ButtonName = "Search")]
        public ActionResult IndexSearch(string key, string NationalList)
        {
            var list = provinceService.All;
            int NationalID = Protector.Int(NationalList);
            if (!string.IsNullOrEmpty(key))
            {
                list = list.Where(r => r.Name.Contains(key));
            }
            if (NationalID > 0)
            {
                list = list.Where(r => r.NationalID == NationalID);

            }
            ViewBag.Key = key;
            IEnumerable<National> nationals = nationalService.All.Where(r => r.IsActive).OrderByDescending(r => r.Order);
            ViewBag.NationalList = nationals.ToSelectListItems(NationalID);
            return View(list.ToList());
        }


        [HttpGet]
        public ActionResult Create()
        {
            var province = new CreateProvinceModel();
            province.IsActive = true;
            province.Nationals = nationalService.All.Where(n => n.IsActive).ToSelectListItems(-1);
            province.Types = GlobalVariables.ProvinceType;
            province.Positions = GlobalVariables.ProvincePosition;
            province.SiteList = GlobalVariables.SiteList;
            return View(province);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            int provinceID = Protector.Int(id);
            var province = new CreateProvinceModel();
            Province info = provinceService.Find(provinceID);
            if (info != null)
            {
                ModelCopier.CopyModel(info, province);
                province.Nationals = nationalService.All.Where(n => n.IsActive).ToSelectListItems(info.NationalID);
                province.Types = GlobalVariables.ProvinceType;
                province.Positions = GlobalVariables.ProvincePosition;
                province.SiteList = GlobalVariables.SiteList;
                return View(province);
            }
            return RedirectToAction("Index");
        }
        #endregion
        #region HttpPost

        [HttpPost]
        public ActionResult Create(CreateProvinceModel province, string[] p, string[] s)
        {
            if (!ModelState.IsValid)
            {
                province.Nationals = nationalService.All.Where(n => n.IsActive).ToSelectListItems(-1);
                province.Types = GlobalVariables.ProvinceType;
                province.Positions = GlobalVariables.ProvincePosition;
                province.SiteList = GlobalVariables.SiteList;
                return View(province);
            }
            Province info = provinceService.Get(r => r.Name == province.Name && r.NationalID == province.NationalID);
            if (info != null)
            {
                province.Nationals = nationalService.All.Where(n => n.IsActive).ToSelectListItems(-1);
                province.Types = GlobalVariables.ProvinceType;
                province.Positions = GlobalVariables.ProvincePosition;
                province.SiteList = GlobalVariables.SiteList;
                ModelState.AddModelError("", string.Format("Tên tỉnh thành [{0}] đã hiện hữu vui lòng chọn tên khác", province.Name));
                return View(province);
            }
            Province _province = new Province();
            ModelCopier.CopyModel(province, _province);
            _province.CreatedDate = DateTime.Now;
            _province.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
            _province.Position = TextHelper.UpdateType(p);
            _province.SiteID = TextHelper.UpdateType(s);
            provinceService.Insert(_province);
            provinceService.Save();
            HttpPostedFileBase file = Request.Files[0];
            if (file != null && file.ContentLength > 0)
            {
                string fileName = DateTime.Now.ToString("HHmmss_ddMMyyyy_") + TextHelper.HandleTitleStringForImg(Path.GetFileName(file.FileName));
                _province.Avartar = fileName;
                provinceService.Save();
                FileHelper.SaveHotel_Picture(file, _province.Avartar, StorageElement.Province_PictureFolder, _province.ProvinceID);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Edit(CreateProvinceModel current, string id, string[] p, string[] s)
        {
            int provinceID = Protector.Int(id);
            Province info = provinceService.Get(r => r.ProvinceID == provinceID);
            if (info != null)
            {
                if (!ModelState.IsValid)
                {
                    current.Nationals = nationalService.All.Where(n => n.IsActive).ToSelectListItems(info.NationalID);
                    current.Types = GlobalVariables.ProvinceType;
                    current.Positions = GlobalVariables.ProvincePosition;
                    current.SiteList = GlobalVariables.SiteList;
                    return View(current);
                }
                TryUpdateModel(info);
                info.Position = TextHelper.UpdateType(p);
                info.SiteID = TextHelper.UpdateType(s);
                HttpPostedFileBase file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    if (!string.IsNullOrEmpty(info.Avartar))
                    {
                        var fullFilePath = FileHelper.getFullFilePath("Provinces", info.ProvinceID, info.Avartar);
                        if (FileHelper.imageFileAvailable(fullFilePath))
                        {
                            System.IO.File.Delete(fullFilePath);
                        }
                    }
                    string fileName = DateTime.Now.ToString("HHmmss_ddMMyyyy_") + TextHelper.HandleTitleStringForImg(Path.GetFileName(file.FileName));
                    info.Avartar = fileName;
                    FileHelper.SaveHotel_Picture(file, info.Avartar, StorageElement.Province_PictureFolder, info.ProvinceID);

                }
                provinceService.Save();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        #endregion
    }
}
