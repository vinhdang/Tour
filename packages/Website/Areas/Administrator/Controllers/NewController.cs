using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.Log;
using Service.IServices;
using Service.Helpers;
using Website.Areas.Administrator.Models.New;
using Microsoft.Web.Mvc;
using Website.Helpers;
using Domain.Entities;
using System.IO;
namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class NewController : Controller
    {
        public ICategoryService categoryService { get; set; }
        public INewService newService { get; set; }
        public NewController(ICategoryService categoryService, INewService newService)
        {
            this.categoryService = categoryService;
            this.newService = newService;
        }
        #region HttpGet
        public ActionResult Index(string CategoryList, string status, string key)
        {
            var list = newService.All;
            int categoryID = Protector.Int(CategoryList, -1);
            if (categoryID > 0)
            {

                list = list.Where(n => n.CategoryID == categoryID);
            }
            if (!string.IsNullOrEmpty(status))
            {
                switch (status)
                {
                    case "1":
                        list = list.Where(c => c.IsActive == true);
                        break;
                    case "0":
                        list = list.Where(c => c.IsActive == false);
                        break;
                }
            }
            if (!string.IsNullOrEmpty(key))
            {
                list = list.Where(c => c.Name.Contains(key));

            }
            ViewBag.CategoryList = categoryService.GetAllByType(1).ToSelectListItems(categoryID);
            ViewBag.Status = Service.Helpers.GlobalVariables.Active;
            return View(list);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var category = new CreateNewModel();
            category.IsActive = true;
            category.Categories = categoryService.GetAllByType(1).ToSelectListItems(-1);
            category.ListType = GlobalVariables.TypeList;
            return View(category);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            int newID = Protector.Int(id);
            var _new = newService.Find(newID);
            if (_new != null)
            {
                EditNewModel edit = new EditNewModel();
                ModelCopier.CopyModel(_new, edit);
                edit.Categories = categoryService.GetAllByType(1).ToSelectListItems(_new.CategoryID);
                edit.ListType = TextHelper.LoadType(_new.Type);
                return View(edit);
            }
            return RedirectToAction("Index");
        }
        #endregion
        #region  HttpPost

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(CreateNewModel current, string[] Type)
        {
            if (!ModelState.IsValid)
            {
                current.Categories = categoryService.GetAllByType(1).ToSelectListItems(current.CategoryID);
                current.ListType = GlobalVariables.TypeList;
                return View(current);
            }
            New _new = new New();
            ModelCopier.CopyModel(current, _new);
            _new.CreatedDate = DateTime.Now;
            _new.Type = TextHelper.UpdateType(Type);
            newService.Insert(_new);
            newService.Save();
            HttpPostedFileBase file = Request.Files[0];
            if (file != null && file.ContentLength > 0)
            {
                _new.Avartar = Path.GetFileName(file.FileName);
                FileHelper.SaveProduct_Picture(file, StorageElement.News_PictureFolder, _new.NewID);
                newService.Save();
            }
            return RedirectToAction("Index");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(EditNewModel current, string id, string[] Type)
        {
            if (!ModelState.IsValid)
            {
                current.Categories = categoryService.GetAllByType(1).ToSelectListItems(current.CategoryID);
                current.ListType = TextHelper.LoadType(current.Type);
                return View(current);
            }
            int newID = Protector.Int(id);
            var info = newService.Find(newID);
            if (info != null)
            {
                if (TryUpdateModel(info))
                {
                    HttpPostedFileBase file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        var fullFilePath = FileHelper.getFullFilePath("New", info.NewID, info.Avartar);
                        if (FileHelper.imageFileAvailable(fullFilePath))
                        {
                            System.IO.File.Delete(fullFilePath);
                        }
                        info.Avartar = Path.GetFileName(file.FileName);
                        FileHelper.SaveProduct_Picture(file, StorageElement.News_PictureFolder, info.NewID);
                    }
                    info.Type = TextHelper.UpdateType(Type);
                    newService.Save();
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Delete(string id)
        {
            int newID = Protector.Int(id);
            New _new = newService.Find(newID);
            if (_new != null)
            {
                newService.Delete(_new);
                newService.Save();
                FileHelper.DeleteDirectory("New", newID);
            }
            var list = newService.All;
            ViewBag.CategoryList = categoryService.GetAllByType(1).ToSelectListItems(_new.CategoryID);
            ViewBag.Status = Service.Helpers.GlobalVariables.Active;
            return PartialView("NewsList", list);
        }

        #endregion
    }
}
