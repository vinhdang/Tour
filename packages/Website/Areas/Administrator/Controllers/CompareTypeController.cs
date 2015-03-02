using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Domain.Entities;
using Website.Areas.Administrator.Models.CompareType;
using Microsoft.Web.Mvc;
using Service.Helpers;
using System.IO;

namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class CompareTypeController : Controller
    {
        public ICompareTypeService compareTypeService { get; set; }
        public CompareTypeController(ICompareTypeService compareTypeService)
        {
            this.compareTypeService = compareTypeService;
        }
        #region HttpGet
        public ActionResult Index()
        {
            List<CompareType> list = compareTypeService.All.ToList();

            return View(list);
        }
        [HttpGet]
        public ActionResult Create()
        {
            var compareType = new CreatCompareTypeModel();
            compareType.IsActive = true;
            return View(compareType);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            int compareTypeID = Protector.Int(id);
            var compareType = new CreatCompareTypeModel();
            CompareType info = compareTypeService.Find(compareTypeID);
            if (info != null)
            {
                ModelCopier.CopyModel(info, compareType);
                return View(compareType);
            }
            return RedirectToAction("Index");
        }
        #endregion
        #region HttpPost

        [HttpPost]
        public ActionResult Create(CreatCompareTypeModel compareType)
        {
            if (!ModelState.IsValid)
            {
                return View(compareType);
            }
            CompareType info = compareTypeService.Get(r => r.Name == compareType.Name);
            if (info != null)
            {
                ModelState.AddModelError("", string.Format("Tên so sánh [{0}] đã hiện hữu vui lòng chọn tên khác", compareType.Name));
                return View(compareType);
            }
            CompareType _compareType = new CompareType();
            ModelCopier.CopyModel(compareType, _compareType);
            _compareType.CreatedDate = DateTime.Now;
            _compareType.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
            compareTypeService.Insert(_compareType);
            compareTypeService.Save();
            HttpPostedFileBase file = Request.Files[0];
            if (file != null && file.ContentLength > 0)
            {
                string fileName = DateTime.Now.ToString("HHmmss_ddMMyyyy_") + TextHelper.HandleTitleStringForImg(Path.GetFileName(file.FileName));
                _compareType.Logo = fileName;
                compareTypeService.Save();
                FileHelper.SaveHotel_Picture(file, _compareType.Logo, StorageElement.CompareType_PictureFolder, _compareType.CompareTypeID);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Edit(CreatCompareTypeModel current, string id)
        {
            int compareTypeID = Protector.Int(id);
            CompareType info = compareTypeService.Get(r => r.CompareTypeID == compareTypeID);
            if (info != null)
            {
                if (!ModelState.IsValid)
                {
                 
                    return View(current);
                }
                TryUpdateModel(info);
              
                HttpPostedFileBase file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    if (!string.IsNullOrEmpty(info.Logo))
                    {
                        var fullFilePath = FileHelper.getFullFilePath("CompareTypes", info.CompareTypeID, info.Logo);
                        if (FileHelper.imageFileAvailable(fullFilePath))
                        {
                            System.IO.File.Delete(fullFilePath);
                        }
                    }
                    string fileName = DateTime.Now.ToString("HHmmss_ddMMyyyy_") + TextHelper.HandleTitleStringForImg(Path.GetFileName(file.FileName));
                    info.Logo = fileName;
                    FileHelper.SaveHotel_Picture(file, info.Logo, StorageElement.CompareType_PictureFolder, info.CompareTypeID);

                }
                compareTypeService.Save();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Delete(string id)
        {
            int compareTypeId = Protector.Int(id);
            CompareType compareType = compareTypeService.Find(compareTypeId);
            if (compareType != null)
            {
                compareTypeService.Delete(compareType);
                compareTypeService.Save();
            }
            var list = compareTypeService.All;
            return PartialView("CompareTypeList", list);
        }
        #endregion

    }
}
