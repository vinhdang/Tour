using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Domain.Entities;
using Website.Helpers;
using Service.Attribute;
using System.Web.Routing;
using Telerik.Web.Mvc.Extensions;
using Service.Helpers;
using Website.Areas.Administrator.Models.District;
using Microsoft.Web.Mvc;
using Website.Areas.Administrator.Models.Province;
using Website.Helpers;
namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class CompareController : Controller
    {
        public IHotelService hotelService { get; set; }
        public ICompareService compareService { get; set; }
        public ICompareTypeService compareTypeService { get; set; }
        public CompareController(ICompareService compareService, IHotelService hotelService, ICompareTypeService compareTypeService)
        {

            this.compareService = compareService;
            this.hotelService = hotelService;
            this.compareTypeService = compareTypeService;

        }
        #region HttpGet




        [HttpGet]
        public ActionResult Create(string id)
        {
            int hotelID = Protector.Int(id);
            var info = hotelService.Get(h => h.HotelID == hotelID);
            if (info != null)
            {
                var compare = new CreateCompareModel();
                compare.IsActive = true;
                compare.Hotel = info;
                compare.Types = compareTypeService.All.Where(c => c.IsActive).ToSelectListItems(-1);
                compare.Compares = compareService.All.Where(c => c.HotelID == hotelID).ToList();
                return View(compare);
            }
            return RedirectToAction("Index", "Hotel");
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            int compareID = Protector.Int(id);
            var info = compareService.Find(compareID);
            if (info != null)
            {
                var compare = new CreateCompareModel();
                ModelCopier.CopyModel(info, compare);
                compare.Hotel = info.Hotel;
                compare.Types = compareTypeService.All.Where(c => c.IsActive).ToSelectListItems(info.CompareTypeID);
                compare.Compares = compareService.All.Where(c => c.HotelID == info.HotelID).ToList();
                return View(compare);
            }
            return RedirectToAction("Index", "Hotel");
        }
        #endregion

        #region HttpPost
        [HttpPost]
        public ActionResult Create(CreateCompareModel compare, string id)
        {
            int hotelID = Protector.Int(id);
            var hotel = hotelService.Get(h => h.HotelID == hotelID);
            if (hotel != null)
            {
                if (!ModelState.IsValid)
                {
                    compare.Hotel = hotel;
                    compare.Types = compareTypeService.All.Where(c => c.IsActive).ToSelectListItems(-1);
                    compare.Compares = compareService.All.Where(c => c.HotelID == hotelID).ToList();
                    return View(compare);
                }
                Compare info = compareService.Get(r => r.Name == compare.Name && r.HotelID == hotel.HotelID);
                if (info != null)
                {
                    compare.Hotel = hotel;
                    compare.Types = compareTypeService.All.Where(c => c.IsActive).ToSelectListItems(info.CompareTypeID);
                    compare.Compares = compareService.All.Where(c => c.HotelID == hotelID).ToList();
                    ModelState.AddModelError("", string.Format("Tên so sánh [{0}] đã hiện hữu vui lòng chọn tên khác", compare.Name));
                    return View(compare);
                }
                info = new Compare();
                ModelCopier.CopyModel(compare, info);
                info.CreatedDate = DateTime.Now;
                info.HotelID = hotelID;
                info.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
                compareService.Insert(info);
                compareService.Save();
                return RedirectToAction("Create", new { id = hotelID });
            }
            return RedirectToAction("Hotel", "Index");
        }

        [HttpPost]
        public ActionResult Edit(string id, CreateCompareModel current)
        {
            int compareID = Protector.Int(id);
            Compare info = compareService.Find(compareID);
            if (info != null)
            {
                if (!ModelState.IsValid)
                {
                    current.Hotel = current.Hotel;
                    current.Types = compareTypeService.All.Where(c => c.IsActive).ToSelectListItems(info.CompareTypeID);
                    current.Compares = compareService.All.Where(p => p.HotelID == info.HotelID).ToList();
                    return View(current);
                }
                TryUpdateModel(info);
                compareService.Save();
                return RedirectToAction("Create", new { id = info.HotelID });
            }
            return RedirectToAction("Hotel", "Index");

        }
        [HttpPost]
        public ActionResult Delete(string id, string hotelID)
        {
            int hID = Protector.Int(hotelID);
            int compareID = Protector.Int(id);
            Compare compare = compareService.Find(compareID);
            if (compare != null)
            {
                compareService.Delete(compare);
                compareService.Save();
            }
            var compares = compareService.All.Where(c => c.HotelID == hID);
            return PartialView("CompareList", compares);
        }
        #endregion

    }
}
