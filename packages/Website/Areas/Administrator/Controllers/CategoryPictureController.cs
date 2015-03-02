using System.Collections.Generic;
using System.Web.Mvc;
using Service;
using Domain.Entities;
using System.Globalization;
using Website.Helpers;
using Service.Helpers;
using Website.Areas.Administrator.Models;
using System.IO;
using Microsoft.Web.Mvc;
using System;
using Website.Log;
using Service.IServices;
using Website.Areas.Administrator.Models.CategoryPicture;
using System.Web;

namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class CategoryPictureController : Controller
    {
        public ICategoryPictureService categoryPictureService { get; set; }
        public ICategoryService categoryService { get; set; }
        public CategoryPictureController(ICategoryPictureService categoryPictureService, ICategoryService categoryService)
        {
            this.categoryPictureService = categoryPictureService;
            this.categoryService = categoryService;
        }
        #region HttpGet

        [HttpGet]
        public ActionResult Create(string id)
        {
            int CatID = Protector.Int(id);
            var info = categoryService.Get(h => h.CategoryID == CatID);
            if (info != null)
            {
                CreateCategoryPictureModel picture = new CreateCategoryPictureModel();
                picture.Category = info;
                picture.ListPictures = categoryPictureService.GetMany(d => d.CategoryID == info.CategoryID);
                picture.IsActive = true;
                return View(picture);
            }
            return RedirectToAction("Index", "Product", null);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            int pictureID = Protector.Int(id);
            var picture = new EditCategoryPictureModel();
            CategoryPicture current = categoryPictureService.Find(pictureID);
            if (current != null)
            {
                ModelCopier.CopyModel(current, picture);
                picture.ListPictures = categoryPictureService.GetMany(c => c.CategoryID == current.CategoryID);
                picture.FilePath = current.FileName;
                return View(picture);
            }
            return RedirectToAction("Index", "Product");
        }
        #endregion
        #region HttpPost

        [HttpPost]
        public ActionResult Create(CreateCategoryPictureModel new_picture, string id)
        {
            int categoryID = Protector.Int(id);
            var info = categoryService.Get(h => h.CategoryID == categoryID);
            if (info != null)
            {
                if (!ModelState.IsValid)
                {
                    new_picture.ListPictures = categoryPictureService.GetMany(d => d.CategoryID == categoryID);
                    return View(new_picture);
                }
                HttpPostedFileBase fileSmall = Request.Files[0];
                if (fileSmall != null && fileSmall.ContentLength > 0)
                {
                    CategoryPicture picture = new CategoryPicture();
                    ModelCopier.CopyModel(new_picture, picture);
                    picture.FileName = Path.GetFileName(fileSmall.FileName);
                    picture.ContentLength = fileSmall.ContentLength;
                    picture.ContentType = fileSmall.ContentType;
                    picture.Extension = Path.GetExtension(fileSmall.FileName);
                    picture.CategoryID = categoryID;
                    picture.CreatedDate = DateTime.Now;
                    picture.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
                    categoryPictureService.Insert(picture);
                    categoryPictureService.Save();
                    FileHelper.SaveProduct_Picture(fileSmall, StorageElement.Category_PictureFolder, info.CategoryID);
                    return RedirectToAction("Create", new { id = categoryID });
                }
            }
            return RedirectToAction("Index", "Category");
        }

        [HttpPost]
        public ActionResult Edit(EditCategoryPictureModel new_picture, string id, string[] type)
        {
            int pictureID = Protector.Int(id);
            CategoryPicture current = categoryPictureService.Find(pictureID);
            if (current != null)
            {
                if (!ModelState.IsValid)
                {
                    new_picture.ListPictures = categoryPictureService.GetMany(c => c.CategoryID == current.CategoryID);
                    new_picture.FilePath = current.FileName;
                    return View(new_picture);
                }
                HttpPostedFileBase fileSmall = Request.Files[0];
                current.Alt = new_picture.Alt;
                current.Order = new_picture.Order;
                current.IsActive = new_picture.IsActive;
                if (fileSmall != null && fileSmall.ContentLength > 0)
                {
                    var fullFilePath = FileHelper.getFullFilePath("Category", current.CategoryID, current.FileName);
                    if (FileHelper.imageFileAvailable(fullFilePath))
                    {
                        System.IO.File.Delete(fullFilePath);
                    }
                    FileHelper.SaveProduct_Picture(fileSmall, StorageElement.Category_PictureFolder, current.CategoryID);
                    current.FileName = Path.GetFileName(fileSmall.FileName);
                    current.ContentLength = fileSmall.ContentLength;
                    current.ContentType = fileSmall.ContentType;
                    current.Extension = Path.GetExtension(fileSmall.FileName);
                    categoryPictureService.Save();
                    return RedirectToAction("Create", new { id = current.CategoryID });
                }
                else
                {
                    categoryPictureService.Save();
                    return RedirectToAction("Create", new { id = current.CategoryID });
                }

            }
            return RedirectToAction("Product", "Index");
        }

        [HttpPost]
        public PartialViewResult Delete(string id, string CategoryID)
        {
            int categoryID = Protector.Int(CategoryID);
            int pictureID = Protector.Int(id);
            CategoryPicture picture = categoryPictureService.Find(pictureID);
            if (picture != null)
            {
                var fullFilePath = FileHelper.getFullFilePath("Category", picture.CategoryID, picture.FileName);
                if (FileHelper.imageFileAvailable(fullFilePath))
                {
                    System.IO.File.Delete(fullFilePath);
                }
                categoryPictureService.Delete(picture);
                categoryPictureService.Save();
            }
            var pictures = categoryPictureService.GetMany(c => c.CategoryID == categoryID);
            return PartialView("CategoryPictureList", pictures);
        }

        #endregion
        #region Private Method


        #endregion
    }
}
