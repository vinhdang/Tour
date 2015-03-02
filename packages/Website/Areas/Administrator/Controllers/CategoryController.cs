using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Domain.Entities;
using Service.Helpers;
using Website.Areas.Administrator.Models.Category;
using Microsoft.Web.Mvc;
using Website.Log;
using Website.Helpers;
namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        public ICategoryService categoryService { get; set; }
        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        #region HttpGet
        public ActionResult Index(string status, string TypeList)
        {
            var list = categoryService.GetAll();

            return View(list);
        }
        /// <summary>
        /// get create category
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            var category = new CreateCategoryModel();
            category.IsActive = true;
            category.PageSize = 10;
            category.ListParent = categoryService.GetAll().ToSelectListItems(-1);
            category.ListPosition = GlobalVariables.ListPosition;
            return View(category);
        }
        /// <summary>
        /// get edit category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            int catID = Protector.Int(id);
            var category = categoryService.Find(catID);
            if (category != null)
            {
                EditCategoryModel edit = new EditCategoryModel();
                ModelCopier.CopyModel(category, edit);
                edit.ListPosition = TextHelper.LoadPosition(edit.Position);
                edit.ListParent = categoryService.GetAll().ToSelectListItems((int)category.ParentID);
                return View(edit);
            }
            return RedirectToAction("Index");
        }
        #endregion
        #region  HttpPost
        /// <summary>
        /// post create category
        /// </summary>
        /// <param name="cat_model"></param>
        /// <param name="Cattype"></param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(CreateCategoryModel current, string[] Position)
        {
            if (!ModelState.IsValid)
            {
                current.ListParent = categoryService.GetAll().ToSelectListItems(-1);
                current.ListPosition = GlobalVariables.ListPosition;
                return View(current);
            }
            Category category = new Category();
            ModelCopier.CopyModel(current, category);
            category.CreatedDate = DateTime.Now;
            category.Position = TextHelper.UpdateType(Position);
            category.Type = 1;
            GetLevel(category);
            categoryService.Insert(category);
            categoryService.Save();
            return RedirectToAction("Index");
        }
       [HttpPost, ValidateInput(false)]
        public ActionResult Edit(EditCategoryModel current, string id, string[] Position)
        {
            if (!ModelState.IsValid)
            {
                current.ListPosition = TextHelper.LoadPosition(current.Position);
                current.ListParent = categoryService.GetAll().ToSelectListItems((int)current.ParentID);
                return View(current);
            }
            int catId = Protector.Int(id);
            var info = categoryService.Find(catId);
            if (info != null)
            {
                if (TryUpdateModel(info))
                {
                    info.Position = TextHelper.UpdateType(Position);
                    GetLevel(info);
                    categoryService.Save();
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Delete(string id)
        {
            int catId = Protector.Int(id);
            Category cat = categoryService.Find(catId);
            if (cat != null)
            {
                categoryService.Delete(cat);
                categoryService.Save();
            }
            var list = categoryService.GetAll();
            return PartialView("CategoryList", list);
        }

        #endregion


        #region  Private
        private void GetLevel(Category category)
        {
            if (category.ParentID > 0)
            {
                Category parent_Category = categoryService.Find(Protector.Int(category.ParentID));
                if (parent_Category != null)
                {
                    category.Level = parent_Category.Level + 1;
                    if (!string.IsNullOrEmpty(parent_Category.ListParentID))
                    {
                        category.ListParentID = parent_Category.ListParentID + "," + parent_Category.CategoryID.ToString();
                    }
                    else
                    {
                        category.ListParentID = parent_Category.CategoryID.ToString();
                    }
                }
                else
                {
                    category.ListParentID = string.Empty;
                    category.Level = 1;
                }
            }
            else
            {
                category.Level = 1;
                category.ParentID = -1;
                category.ListParentID = string.Empty;
            }
        }
        #endregion
    }
}
