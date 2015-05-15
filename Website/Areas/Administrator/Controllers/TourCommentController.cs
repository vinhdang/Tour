using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Domain;
using Microsoft.Web.Mvc;
using Service.Attribute;
using Service.Helpers;
using Service.IServices;
using Telerik.Web.Mvc.Extensions;
using Website.Areas.Administrator.Models.Comment;

namespace Website.Areas.Administrator.Controllers
{
    public class TourCommentController : Controller
    {
        private ITourCommentService tourCommentService { get; set; }
        private ITourService tourService { get; set; }

        public TourCommentController(ITourCommentService tourCommentService, ITourService tourService)
        {
            this.tourCommentService = tourCommentService;
            this.tourService = tourService;
        } 
        
        //
        // GET: /Administrator/TourComment/

        #region HttpGet
        public ActionResult Index(string id)
        {
            var tourID = Protector.Int(id ?? Request["tourid"]);
            var list = tourCommentService.All.Where(a => a.TourID == tourID).OrderByDescending(t => t.CreatedDate).ToList();
            var tour = tourService.Get(t => t.ID == tourID);
            if(tour != null)
            {
                ViewBag.TourName = tour.Name;
            }

            ViewBag.TourID = id;
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
                foreach (var record in checkedRecords)
                {
                    tourCommentService.Delete(record);
                }

                tourCommentService.Save();
            }
            RouteValueDictionary routeValues = this.GridRouteValues();
            return RedirectToAction("Index", routeValues);
        }

        [HttpPost]
        [ActionName("Index")]
        [AcceptButton(ButtonName = "Search")]
        public ActionResult IndexSearch(string key, FormCollection data)
        {
            var list = tourCommentService.All;
            if (!string.IsNullOrEmpty(key))
            {
                list = list.Where(r => r.Email.Contains(key));
            }
            ViewBag.Key = key;
            ViewBag.TourID = data["TourID"];
            return View(list.ToList());
        }


        [HttpGet]
        public ActionResult Create()
        {
            int tourId = Protector.Int(Request.QueryString["tourid"]);

            ViewBag.TourID = tourId;
            var comment = new CommentModel();

            comment.IsEnable = true;
            comment.TourID = Protector.Int(Request.QueryString["tourid"]);

            return View(comment);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            int tourAgendaID = Protector.Int(id);
            var comment = tourCommentService.Find(tourAgendaID);
            var model = new CommentModel();
            ModelCopier.CopyModel(comment, model);


            ViewBag.TourID = Request.QueryString["tourid"];
            return View(model);
        }
        #endregion

        #region HttpPost
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(CommentModel model, FormCollection data)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var info = tourCommentService.Get(r => r.Email == model.Email && r.Content == model.Content && r.TourID == model.TourID);
            if (info != null)
            {
                ModelState.AddModelError("", string.Format("Nhận xét [{0}] đã hiện hữu vui lòng chọn email khác", model.Email));
                return View(model);
            }
            var comment = new Comment();

            ModelCopier.CopyModel(model, comment);

            comment.CreatedDate = DateTime.Now;
            comment.IsDeleted = false;
            //comment.Level = Protector.Int(data["Level"]);

            tourCommentService.Insert(comment);
            tourCommentService.Save();

            return RedirectToAction("Index", "TourComment", new { id = comment.TourID });
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(string id, CommentModel current, FormCollection data)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TourID = Request.QueryString["tourid"];
                return View(current);
            }
            int commentID = Protector.Int(current.ID);
            var comment = tourCommentService.Get(r => r.ID == commentID);
            if (comment != null)
            {
                comment = tourCommentService.Find(commentID);
                //comment.Level = Protector.Int(data["Level"]);
                if (TryUpdateModel(comment))
                {
                    tourCommentService.Save();
                    return RedirectToAction("Index", "TourComment", new { id = comment.TourID });
                }
            }

            return View(current);
        }
        #endregion

    }
}
