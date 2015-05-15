using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Domain;
using Service.Helpers;
using Service.Attribute;
using Website.Areas.Administrator.Models.ApartmentPrice;
using Website.Helpers;
using Microsoft.Web.Mvc;
using System.Web.Routing;
using Telerik.Web.Mvc.Extensions;

namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class ApartmentPriceController : Controller
    {
        public ITourService tourService { get; set; }
        public ITourPriceService tourPriceService { get; set; }
        public ApartmentPriceController(ITourService tourService, ITourPriceService tourPriceService)
        {
            this.tourService = tourService;
            this.tourPriceService = tourPriceService;
        }
        #region HttpGet
        public ActionResult Index(string id)
        {
            List<ApartmentPrice> list1 = apartmentPriceService.All.Where(r => r.Date < DateTime.Today).ToList();
            if (list1 != null && list1.Count > 0)
            {
                foreach (ApartmentPrice item in list1)
                {
                    apartmentPriceService.Delete(item);
                    apartmentPriceService.Save();
                }
            }
            int apartmentID = Protector.Int(id);
            Apartment info = apartmentService.Find(apartmentID);
            if (info != null)
            {
                ViewBag.ApartmentName = info.Name;
                List<ApartmentPrice> list = apartmentPriceService.All.Where(p => p.ApartmentID == apartmentID).ToList();
                ViewBag.ApartmentID = apartmentID;
                return View(list);
            }
            return RedirectToAction("Index", "Apartment");
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
                    ApartmentPrice apartmentPrice = apartmentPriceService.Find(item);
                    if (apartmentPrice != null)
                    {

                        apartmentPriceService.Delete(apartmentPrice);
                        apartmentPriceService.Save();
                    }
                }
            }
            ViewBag.From = Protector.String(From);
            ViewBag.To = Protector.String(To);
            int apartmentID = Protector.Int(id);
            ViewBag.ApartmentID = apartmentID;
            RouteValueDictionary routeValues = this.GridRouteValues();
            return RedirectToAction("Index", routeValues);
        }

        [HttpPost]
        [ActionName("Index")]
        [AcceptButton(ButtonName = "Search")]
        public ActionResult IndexSearch(string id, string From, string To)
        {
            int apartmentID = Protector.Int(id);
            ViewBag.ApartmentID = apartmentID;
            IQueryable<ApartmentPrice> list = apartmentPriceService.All.Where(p => p.ApartmentID == apartmentID);

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
            int apartmentID = Protector.Int(id);
            Apartment apartment = apartmentService.Find(apartmentID);
            if (apartment != null)
            {
                var price = new CreateApartmentPriceModel();
                ViewBag.ApartmentName = apartment.Name;
                price.IsActive = true;
                price.DateList = GlobalVariables.DateList;
                return View(price);
            }
            return RedirectToAction("Index", "Hotel", null);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            long roomPriceID = Protector.Int(id);
            var roomPrice = new EditApartmentPriceModel();
            ApartmentPrice info = apartmentPriceService.Find(roomPriceID);
            if (info != null)
            {
                ModelCopier.CopyModel(info, roomPrice);
                var apartment = apartmentService.All.Where(h => h.ApartmentID == info.ApartmentID).FirstOrDefault();
                if (apartment != null)
                {
                    ViewBag.ApartmentName = apartment.Name;
                    ViewBag.ApartmentID = apartment.ApartmentID;
                    roomPrice.Price = Convert.ToInt64(info.Price);
                }
                return View(roomPrice);
            }
            return RedirectToAction("Index", "Hotel");
        }
        #endregion
        #region HttpPost
        [HttpPost]
        public ActionResult Create(CreateApartmentPriceModel apartmentPrice, string id, string[] dl)
        {
            int apartmentID = Protector.Int(id);
            var apartment = apartmentService.Find(apartmentID);
            if (apartment != null)
            {
                if (!ModelState.IsValid)
                {
                    apartmentPrice.IsActive = true;
                    apartmentPrice.DateList = GlobalVariables.DateList;
                    return View(apartmentPrice);
                }
                foreach (DateTime day in EachDay(Protector.DateTime(apartmentPrice.FromDate), Protector.DateTime(apartmentPrice.ToDate)))
                {
                    DateTime startdate = day.Date;
                    DateTime enddate = startdate.AddHours(23.99);
                    ApartmentPrice checkPrice = apartmentPriceService.All.Where(r => r.ApartmentID == apartment.ApartmentID && r.Date >= startdate && r.Date <= enddate).FirstOrDefault();
                    if (checkPrice != null)
                    {
                        checkPrice.NumberGuest = Protector.Int(apartmentPrice.NumberGuest);
                        checkPrice.IsActive = apartmentPrice.IsActive;
                        checkPrice.CreatedDate = DateTime.Now;
                        checkPrice.Description = apartmentPrice.Description;
                        checkPrice.IsContact = apartmentPrice.IsContact;
                        checkPrice.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
                        if (dl != null)
                        {
                            switch (day.DayOfWeek.ToString())
                            {
                                case "Friday":
                                    if (dl.Contains("Friday"))
                                    {
                                        checkPrice.Price = Protector.Decimal(apartmentPrice.Price1);
                                    }
                                    else
                                    {
                                        checkPrice.Price = Protector.Decimal(apartmentPrice.Price);
                                    }
                                    break;
                                case "Saturday":
                                    if (dl.Contains("Saturday"))
                                    {
                                        checkPrice.Price = Protector.Decimal(apartmentPrice.Price1);
                                    }
                                    else
                                    {
                                        checkPrice.Price = Protector.Decimal(apartmentPrice.Price);
                                    }
                                    break;
                                case "Sunday":
                                    if (dl.Contains("Sunday"))
                                    {
                                        checkPrice.Price = Protector.Decimal(apartmentPrice.Price1);
                                    }
                                    else
                                    {
                                        checkPrice.Price = Protector.Decimal(apartmentPrice.Price);
                                    }
                                    break;
                                default:
                                    checkPrice.Price = Protector.Decimal(apartmentPrice.Price);
                                    break;
                            }
                        }
                        else
                        {
                            checkPrice.Price = Protector.Decimal(apartmentPrice.Price);

                        }

                        apartmentPriceService.Save();
                    }
                    else
                    {
                        checkPrice = new ApartmentPrice();
                        checkPrice.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
                        checkPrice.CreatedDate = DateTime.Now;

                        checkPrice.Date = startdate;
                        if (dl != null)
                        {
                            switch (day.DayOfWeek.ToString())
                            {
                                case "Friday":
                                    if (dl.Contains("Friday"))
                                    {
                                        checkPrice.Price = Protector.Decimal(apartmentPrice.Price1);
                                    }
                                    else
                                    {
                                        checkPrice.Price = Protector.Decimal(apartmentPrice.Price);
                                    }
                                    break;
                                case "Saturday":
                                    if (dl.Contains("Saturday"))
                                    {
                                        checkPrice.Price = Protector.Decimal(apartmentPrice.Price1);
                                    }
                                    else
                                    {
                                        checkPrice.Price = Protector.Decimal(apartmentPrice.Price);
                                    }
                                    break;
                                case "Sunday":
                                    if (dl.Contains("Sunday"))
                                    {
                                        checkPrice.Price = Protector.Decimal(apartmentPrice.Price1);
                                    }
                                    else
                                    {
                                        checkPrice.Price = Protector.Decimal(apartmentPrice.Price);
                                    }
                                    break;
                                default:
                                    checkPrice.Price = Protector.Decimal(apartmentPrice.Price);
                                    break;
                            }

                        }
                        else
                        {
                            checkPrice.Price = Protector.Decimal(apartmentPrice.Price);
                        }
                        checkPrice.ApartmentID = apartment.ApartmentID;
                        checkPrice.IsActive = apartmentPrice.IsActive;
                        checkPrice.NumberGuest = Protector.Int(apartmentPrice.NumberGuest);
                        checkPrice.Description = apartmentPrice.Description;
                        checkPrice.IsContact = apartmentPrice.IsContact;
                        apartmentPriceService.Insert(checkPrice);
                        apartmentPriceService.Save();
                    }
                }
                return RedirectToAction("Index", new { id = apartmentID });
            }
            return RedirectToAction("Index", "Apartment");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(EditApartmentPriceModel current, string id)
        {
            int apartmentPriceID = Protector.Int(id);
            ApartmentPrice info = apartmentPriceService.All.Where(p => p.ApartmentPriceID == apartmentPriceID).FirstOrDefault();
            if (info != null)
            {
                if (!ModelState.IsValid)
                {
                    var apartment = apartmentService.Find(info.ApartmentID);
                    if (apartment != null)
                    {
                        ViewBag.ApartmentName = apartment.Name;
                        ViewBag.ApartmentID = apartment.ApartmentID;
                        current.Price = Convert.ToInt64(info.Price);
                    }
                    return View(current);
                }
                TryUpdateModel(info);
                apartmentPriceService.Save();
                return RedirectToAction("Index", "ApartmentPrice", new { id = info.ApartmentID });

            }
            return RedirectToAction("Index", "Apartment");
        }
        #endregion
        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}
