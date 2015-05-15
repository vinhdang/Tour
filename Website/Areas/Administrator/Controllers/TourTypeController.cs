using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Domain;
using Service.Attribute;
using Service.Helpers;
using Service.IServices;
using Telerik.Web.Mvc.Extensions;
using Website.Areas.Administrator.Models.TourType;

namespace Website.Areas.Administrator.Controllers
{
    public class TourTypeController : Controller
    {
        public ITourTypeService tourTypeService { get; set; }
        public TourTypeController(ITourTypeService roomTypeService)
        {
            this.tourTypeService = roomTypeService;
        }
        
        //
        // GET: /Administrator/TourType/

        public ActionResult Index()
        {
            List<TourType> list = tourTypeService.All.OrderByDescending( t => t.Order).ToList();
            return View(list);
        }

        [HttpPost]
        [ActionName("Index")]
        [AcceptButton(ButtonName = "Search")]
        public ActionResult IndexSearch(string key)
        {
            var list = tourTypeService.All;
            if (!string.IsNullOrEmpty(key))
            {
                list = list.Where(r => r.Name.Contains(key));
            }
            ViewBag.Key = key;
            return View(list.ToList());
        }


        [HttpGet]
        public ActionResult Create()
        {
            var tourType = new TourTypeModel();
            tourType.IsActive = true;
            return View(tourType);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            int tourTypeID = Protector.Int(id);
            var tourType = tourTypeService.Find(tourTypeID);

            return View(ConvertTourTypeData(tourType));
        }

        [HttpGet]
        public JsonResult Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids)) return Json(false, JsonRequestBehavior.AllowGet);

            var items = ids.Split(new char[] {','}, StringSplitOptions.None);

            if(items.Length > 0)
            {
                try
                {
                    foreach (var item in items)
                    {
                        tourTypeService.Delete(Protector.Long(item));
                    }

                    tourTypeService.Save();
                }
                catch (Exception)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(TourTypeModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            TourType info = tourTypeService.Get(r => r.Name == model.Name);
            if (info != null)
            {
                ModelState.AddModelError("", string.Format("Tên loại tour [{0}] đã hiện hữu vui lòng chọn tên khác", model.Name));
                return View(model);
            }
            var tourTypeData = new TourType()
                                   {
                                       CreatedDate = DateTime.Now,
                                       CreatedBy = Protector.Int(HttpContext.User.Identity.Name),
                                       Name = model.Name,
                                       Description = model.Description,
                                       IsActive = model.IsActive,
                                       Order = model.Order,
                                       Tours = null
                                   };

            tourTypeService.Insert(tourTypeData);
            tourTypeService.Save();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(string id, TourTypeModel current)
        {
            if (!ModelState.IsValid)
            {
                return View(current);
            }
            int tourTypeID = Protector.Int(id);
            TourType tourType = tourTypeService.Get(r => System.String.CompareOrdinal(r.Name.ToLower(), current.Name.ToLower()) == 0 && r.ID != tourTypeID);
            if (tourType != null)
            {
                ModelState.AddModelError("", string.Format("Loại Tour [{0}] đã hiện hữu vui lòng chọn tên khác", current.Name));
                return View(current);
            }
            tourType = tourTypeService.Find(tourTypeID);
            if (TryUpdateModel(tourType))
            {
                tourTypeService.Save();
                return RedirectToAction("Index");
            }
            else return View(current);
        }

        private TourTypeModel ConvertTourTypeData(TourType data)
        {
            if (data == null) return new TourTypeModel();

            return new TourTypeModel()
                       {
                           ID = data.ID,
                           Name = data.Name,
                           CreatedDate = data.CreatedDate,
                           CreatedBy = data.CreatedBy,
                           Description = data.Description,
                           IsActive = data.IsActive,
                           Order = data.Order
                       };
        }

        //[HttpPost]
        //[ActionName("Index")]
        //[AcceptButton(ButtonName = "Delete")]
        //public ActionResult IndexDelete(int[] checkedRecords)
        //{
        //    checkedRecords = checkedRecords ?? new int[] { };
        //    if (checkedRecords != null && checkedRecords.Length > 0)
        //    {

        //    }
        //    RouteValueDictionary routeValues = this.GridRouteValues();
        //    return RedirectToAction("Index", routeValues);
        //}
    }
}
