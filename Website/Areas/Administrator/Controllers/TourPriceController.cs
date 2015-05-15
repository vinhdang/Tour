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
using Website.Areas.Administrator.Models.TourPrice;
using Website.Helpers;

namespace Website.Areas.Administrator.Controllers
{
    public class TourPriceController : Controller
    {
        public ITourService tourService { get; set; }
        public ITourPriceService tourPriceService { get; set; }
        public ITourProviderService tourProvider { get; set; }

        public TourPriceController(ITourService tourService, ITourPriceService tourPriceService, ITourProviderService tourProvider)
        {
            this.tourService = tourService;
            this.tourPriceService = tourPriceService;
            this.tourProvider = tourProvider;
        }
        #region HttpGet
        public ActionResult Index(string id)
        {
            int tourID = Protector.Int(id);
            var info = tourService.Find(tourID);
            if (info != null)
            {
                ViewBag.TourName = info.Name;
                List<TourPrice> list = tourPriceService.All.Where(p => p.TourID == tourID).OrderByDescending( p => p.Sequence).ToList();
                ViewBag.TourID = tourID;
                return View(list);
            }
            return RedirectToAction("Index", "Tour");
        }
        [HttpPost]
        [ActionName("Index")]
        [AcceptButton(ButtonName = "Delete")]
        public ActionResult IndexDelete(int[] checkedRecords, string id, string From, string To)
        {
            checkedRecords = checkedRecords ?? new int[] { };
            if (checkedRecords != null && checkedRecords.Length > 0)
            {
                foreach (int item in checkedRecords)
                {
                    var price = tourPriceService.Find(item);
                    if (price != null)
                    {

                        tourPriceService.Delete(price);
                        tourPriceService.Save();
                    }
                }
            }
            ViewBag.From = Protector.String(From);
            ViewBag.To = Protector.String(To);
            int tourID = Protector.Int(id);
            ViewBag.TourID = tourID;
            RouteValueDictionary routeValues = this.GridRouteValues();
            return RedirectToAction("Index", routeValues);
        }

        [HttpPost]
        [ActionName("Index")]
        [AcceptButton(ButtonName = "Search")]
        public ActionResult IndexSearch(string id, string From, string To)
        {
            int tourID = Protector.Int(id);
            ViewBag.ApartmentID = tourID;
            IQueryable<TourPrice> list = tourPriceService.All.Where(p => p.TourID == tourID);

            if (!string.IsNullOrEmpty(From))
            {
                DateTime FromDate = Protector.DateTime(From);
                list = list.Where(r => r.Date >= FromDate);
                ViewBag.From = From;
            }
            if (!string.IsNullOrEmpty(To))
            {
                DateTime ToDate = Protector.DateTime(To);
                list = list.Where(r => r.Date <= ToDate);
                ViewBag.To = To;
            }

            return View(list.ToList());
        }

        [HttpGet]
        public ActionResult Create(string id)
        {
            int tourID = Protector.Int(id);
            var tour = tourService.Find(tourID);
            if (tour != null)
            {
                var price = new TourPriceModel();
                ViewBag.ApartmentName = tour.Name;
                price.TourID = tourID;
                price.IsActive = true;
                price.TourProviders = tourProvider.All.Where(p => (bool)p.IsActive && !(bool)p.IsDeleted).ToSelectListItems(-1).ToList();
                return View(price);
            }
            return RedirectToAction("Index", "Tour", null);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            long priceID = Protector.Int(id);
            var price = new TourPriceModel();
            var info = tourPriceService.Find(priceID);
            if (info != null)
            {
                ModelCopier.CopyModel(info, price);
                var tour = tourService.All.SingleOrDefault(h => h.ID == info.TourID);
                if (tour != null)
                {
                    ViewBag.TourName = tour.Name;
                    ViewBag.TourID = tour.ID;
                    price.Price = Convert.ToInt64(info.Price);
                }
                price.TourProviders = tourProvider.All.Where(p => (bool)p.IsActive && !(bool)p.IsDeleted).ToSelectListItems(info.ProviderID.HasValue ? (int)info.ProviderID : -1).ToList();
                return View(price);
            }
            return RedirectToAction("Index", "Tour");
        }
        #endregion

        #region HttpPost
        [HttpPost]
        public ActionResult Create(TourPriceModel tourPrice, string id, string[] dl)
        {
            int tourID = Protector.Int(id ?? tourPrice.TourID.ToString());
            var tour = tourService.Find(tourID);
            if (tour != null)
            {
                if (!ModelState.IsValid)
                {
                    tourPrice.TourProviders = tourProvider.All.Where(p => (bool)p.IsActive && !(bool)p.IsDeleted).ToSelectListItems((int)tourPrice.ProviderID).ToList(); ;
                    return View(tourPrice);
                }

                var persisted = tourPriceService.All.SingleOrDefault(p => p.ProviderID == tourPrice.ProviderID && p.TourID == tour.ID);
                if(persisted != null)
                {
                    tourPrice.TourProviders = tourProvider.All.Where(p => (bool)p.IsActive && !(bool)p.IsDeleted).ToSelectListItems((int)tourPrice.ProviderID).ToList(); ;
                    ModelState.AddModelError("", string.Format("Giá của nhà cung cấp này đã hiện hữu vui lòng chọn nhà cung cấp khác"));
                    return View(tourPrice);
                }

                var  checkPrice = new TourPrice();
                checkPrice.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
                checkPrice.CreatedDate = DateTime.Now;
                checkPrice.TourID = tour.ID;
                checkPrice.IsActive = tourPrice.IsActive;
                checkPrice.NumberGuest = Protector.Int(tourPrice.NumberGuest);
                checkPrice.Description = tourPrice.Description;
                checkPrice.IsContact = tourPrice.IsContact;
                checkPrice.ProviderID = tourPrice.ProviderID;
                checkPrice.Sequence = tourPrice.Sequence;
                checkPrice.LinkBooking = tourPrice.LinkBooking;
                checkPrice.Price = tourPrice.Price;

                tourPriceService.Insert(checkPrice);
                tourPriceService.Save();

                return RedirectToAction("Index", "TourPrice", new {id = tourID});
                
            }
            return RedirectToAction("Index", "Tour");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(TourPriceModel current, string id)
        {
            int priceID = Protector.Int(id);
            var info = tourPriceService.All.FirstOrDefault(p => p.ID == priceID);
            if (info != null)
            {
                if (!ModelState.IsValid)
                {
                    var tour = tourService.Find(info.TourID);
                    if (tour != null)
                    {
                        ViewBag.TourName = tour.Name;
                        ViewBag.TourID = tour.ID;
                        ViewBag.TourID = tour.ID;
                        current.Price = Convert.ToInt64(info.Price);
                    }
                    current.TourProviders = tourProvider.All.Where(p => (bool)p.IsActive && !(bool)p.IsDeleted).ToSelectListItems((int)current.ProviderID).ToList(); ;
                    return View(current);
                }
                TryUpdateModel(info);
                tourPriceService.Save();
                return RedirectToAction("Index", "TourPrice", new { id = info.TourID });

            }
            return RedirectToAction("Index", "Tour");
        }
        #endregion

        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

    }
}
