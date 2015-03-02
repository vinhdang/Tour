using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Domain.Entities;
using Website.Helpers;
using Service.Helpers;

namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {

        public ICommentService commentService { get; set; }
        public IProvinceService provinceService { get; set; }
        public INationalService nationalService { get; set; }
        public IDistrictService districtService { get; set; }
        public CommentController(ICommentService commentService, IProvinceService provinceService, INationalService nationalService, IDistrictService districtService)
        {
            this.commentService = commentService;
            this.provinceService = provinceService;
            this.nationalService = nationalService;
            this.districtService = districtService;
            this.districtService = districtService;

        }

        #region HttpGet
        public ActionResult Waiting()
        {

            List<Comment> list = commentService.All.Where(p => p.IsActive == false && p.IsActiveBy == null).ToList();
            IEnumerable<National> nationals = nationalService.All.Where(r => r.IsActive).OrderByDescending(r => r.Order);
            ViewBag.NationalList = nationals.ToSelectListItems(-1);
            List<Province> provinces = new List<Province>();
            ViewBag.ProvinceList = provinces.ToSelectListItems(-1);

            List<District> districts = new List<District>();
            ViewBag.DistrictList = districts.ToSelectListItems(-1);
            return View(list);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            int commentID = Protector.Int(id);
            Comment info = commentService.Find(commentID);
            if (info != null)
            {

                return View(info);
            }
            return RedirectToAction("Waiting");
        }
        #endregion

    }
}
