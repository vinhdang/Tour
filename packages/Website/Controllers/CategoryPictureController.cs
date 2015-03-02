using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Service.Helpers;
using Domain.Entities;

namespace Website.Controllers
{
    public class CategoryPictureController : Controller
    {
        public ICategoryService categoryService { get; set; }
        public ICategoryPictureService categoryPictureService { get; set; }
        public CategoryPictureController(ICategoryService categoryService, ICategoryPictureService categoryPictureService)
        {
            this.categoryService = categoryService;
            this.categoryPictureService = categoryPictureService;
        }
         [OutputCache(Duration = 100000)]
        public ActionResult LoadPictureByCategoryID(string id)
        {
            int CatID = Protector.Int(id);
            CategoryPicture info = categoryPictureService.All.Where(c => c.CategoryID == CatID && c.IsActive).OrderBy(c => c.Order).FirstOrDefault();
            return PartialView("LoadPictureByCategoryID", info);
        }

    }
}
