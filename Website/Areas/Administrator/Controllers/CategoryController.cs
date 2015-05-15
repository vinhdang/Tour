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
using Website.Areas.Administrator.Models.Category;
using Website.Helpers;
using System.IO;
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
        public ActionResult Index()
        {
            var list = categoryService.GetAll();
            return View(list);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var category = new CreateCategoryModel();
            category.IsActive = true;
            List<Category> list = categoryService.GetAll();
            ViewBag.List = list;
            return View(category);
        }

        public ActionResult Edit(string id)
        {
            int catID = Protector.Int(id);
            var category = categoryService.Find(catID);
            if (category != null)
            {
                EditCategoryModel edit = new EditCategoryModel();
                ModelCopier.CopyModel(category, edit);
                List<Category> list = categoryService.GetAll().Where(p => p.ID != category.ID).ToList();
                ViewBag.List = list;
                return View(edit);
            }
            return RedirectToAction("Error", "Administrator");
        }



        #endregion
        #region  HttpPost
        [HttpPost]
        [ActionName("Index")]
        [AcceptButton(ButtonName = "Delete")]
        public ActionResult IndexDelete(int[] checkedRecords)
        {
            if (checkedRecords != null && checkedRecords.Length > 0)
            {
                foreach (int item in checkedRecords)
                {
                    Category category = categoryService.Find(item);
                    if (category != null)
                    {
                        List<Category> list = categoryService.All.Where(c => c.ParentID == category.ID).ToList();
                        if (list != null && list.Count > 0)
                        {

                        }
                        else
                        {
                            try
                            {

                                categoryService.Delete(category);
                                categoryService.Save();
                            }
                            catch
                            { }
                        }
                    }
                }
            }
            RouteValueDictionary routeValues = this.GridRouteValues();
            return RedirectToAction("Index", routeValues);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(CreateCategoryModel current, string[] p)
        {
            if (!ModelState.IsValid)
            {
                return View(current);
            }


            Category category = new Category();
            ModelCopier.CopyModel(current, category);
            category.CreatedDate = DateTime.Now;
            category.Position = 0;
            category.Type = 1;
            GetLevel(category);
            category.Position = 0;
            if (p != null && p.Length > 0)
            {
                foreach (string item in p)
                {
                    category.Position += Protector.Int(item);
                }


            }
            else { category.Position = 0; }

            categoryService.Insert(category);
            categoryService.Save();



            return RedirectToAction("Index");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(EditCategoryModel current, string id, string[] p)
        {

            int catId = Protector.Int(id);
            var info = categoryService.Find(catId);
            if (info != null)
            {
                if (!ModelState.IsValid)
                {
                    List<Category> list = categoryService.GetAll().Where(c => c.ID != info.ID).ToList();
                    ViewBag.List = list;
                    return View(current);
                }

                if (TryUpdateModel(info))
                {
                    GetLevel(info);
                    if (p != null && p.Length > 0)
                    {
                        info.Position = 0;
                        foreach (string item in p)
                        {
                            info.Position += Protector.Int(item);
                        }


                    }
                    else { info.Position = 0; }
                    categoryService.Save();
                    return RedirectToAction("Index");

                }
            }
            return RedirectToAction("Error", "Administrator");
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
                        category.ListParentID = parent_Category.ListParentID + "," + parent_Category.ID.ToString();
                    }
                    else
                    {
                        category.ListParentID = parent_Category.ID.ToString();
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
