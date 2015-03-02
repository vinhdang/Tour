using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Domain;
using Service.Helpers;
using Website.Models.Hotel;

namespace Website.Controllers
{
    public class ApartmentController : Controller
    {
        //public IComfortService comfortService { get; set; }
        public ITourService apartmentService { get; set; }
        public ISupportService supportService { get; set; }
        public IProvinceService provinceService { get; set; }

        public ITourPriceService apartmentPriceService { get; set; }
        public IConfigService configService { get; set; }
        public ApartmentController(ISupportService supportService, ITourService apartmentService, IProvinceService provinceService
            , ITourPriceService apartmentPriceService, IConfigService configService)
        {
            this.apartmentService = apartmentService;
            //this.comfortService = comfortService;
            this.supportService = supportService;
            this.provinceService = provinceService;

            this.apartmentPriceService = apartmentPriceService;
            this.configService = configService;
        }

        public ActionResult Index(string apartmentName)
        {
            string name = Protector.String(apartmentName).ToLower();
            if (!string.IsNullOrEmpty(name))
            {
                Tour info = apartmentService.All.Where(h => h.IsActive && h.Url == name).FirstOrDefault();
                if (info != null)
                {
                    AddToShoppingCart(info.ID);
                    ViewBag.Title = info.PageTitle;
                    ViewBag.Description = info.Description;
                    ViewBag.KeyWords = info.KeyWord;
                    return View(info);
                }
            }
            return View();
        }
        public ActionResult ComfortList(string listComfort)
        {
            List<SelectListItem> comfortList = new List<SelectListItem>();
            List<Comfort> list = new List<Comfort>();
            string[] comfortID = null;
            if (!string.IsNullOrEmpty(listComfort))
            {
                comfortID = listComfort.Split(';');
            }
            else { comfortID = new string[] { }; }
            list = comfortService.All.Where(c => c.IsActive).OrderByDescending(c => c.Order).ToList();
            foreach (Comfort item in list)
            {

                if (comfortID.Contains(item.ComfortID.ToString()))
                {
                    comfortList.Add(new SelectListItem() { Selected = true, Text = item.Name, Value = item.ComfortID.ToString() });
                }
                else
                {
                    comfortList.Add(new SelectListItem() { Selected = false, Text = item.Name, Value = item.ComfortID.ToString() });
                }
            }
            return PartialView("ComfortList", comfortList);
        }

        public ActionResult Support()
        {
            List<Support> lstYahoo = supportService.All.Where(s => s.IsActive && (s.Type & 1) == 1).OrderByDescending(s => s.Order).ThenByDescending(s => s.CreatedDate).ToList();
            List<Support> lstEmail = supportService.All.Where(s => s.IsActive && (s.Type & 8) == 8).OrderByDescending(s => s.Order).ThenByDescending(s => s.CreatedDate).ToList();
            List<Support> lstSkype = supportService.All.Where(s => s.IsActive && (s.Type & 2) == 2).OrderByDescending(s => s.Order).ThenByDescending(s => s.CreatedDate).ToList();
            List<Support> lstPhone = supportService.All.Where(s => s.IsActive && (s.Type & 4) == 4).OrderByDescending(s => s.Order).ThenByDescending(s => s.CreatedDate).ToList();
            ViewBag.Phone = lstPhone;
            ViewBag.Yahoo = lstYahoo;
            ViewBag.Skype = lstSkype;
            ViewBag.Email = lstEmail;
            return PartialView("Support");
        }
        public ActionResult OtherApartment(string apartmentName)
        {
            string name = Protector.String(apartmentName).ToLower();
            if (!string.IsNullOrEmpty(name))
            {
                Apartment info = apartmentService.All.Where(h => h.IsActive && h.Url == name).FirstOrDefault();
                if (info != null)
                {
                    var list = apartmentService.All.Where(h => h.IsActive && h.ApartmentID != info.ApartmentID && h.ProvinceID == info.ProvinceID).Take(10).ToList();
                    return PartialView("OtherApartment", list);
                }
            }
            return PartialView("OtherApartment");
        }
        public ActionResult PriceList(string apartmentName)
        {
            string name = Protector.String(apartmentName).ToLower();
            if (!string.IsNullOrEmpty(name))
            {
                Apartment info = apartmentService.All.Where(h => h.IsActive && h.Url == name).FirstOrDefault();
                if (info != null)
                {
                    var list = apartmentPriceService.All.Where(h => h.IsActive && h.ApartmentID == info.ApartmentID && h.Date >= DateTime.Today).OrderBy(c => c.Date).Take(30).ToList();
                    return PartialView("PriceList", list);
                }
            }
            return PartialView("PriceList");
        }
        public ActionResult ListApartmentInCookies()
        {
            List<Apartment> list = new List<Apartment>();
            if (Request.Cookies["ShoppingCart"] == null)
            {
                //Do nothing
            }
            else
            {
                HttpCookie oCookie = (HttpCookie)Request.Cookies["ShoppingCart"];
                char[] sep = { ',' };
                oCookie.Expires = DateTime.Now.AddDays(2);
                string[] arrCookie = oCookie.Value.Split(sep);

                if (arrCookie != null && arrCookie.Length > 0)
                {
                    foreach (string id in arrCookie)
                    {
                        long apartmentID = Protector.Long(id);
                        Apartment hotel = apartmentService.All.Where(h => h.IsActive && h.ApartmentID == apartmentID).FirstOrDefault();
                        if (hotel != null)
                        {
                            list.Add(hotel);
                            { }
                        }
                    }

                }
            }
            return PartialView("ListApartmentInCookies", list);
        }
        private void AddToShoppingCart(long ProductID)
        {

            if (Request.Cookies["ShoppingCart"] == null)
            {

                HttpCookie oCookie = new HttpCookie("ShoppingCart");

                //Set Cookie to expire in 3 hours

                oCookie.Expires = DateTime.Now.AddDays(1);

                oCookie.Value = ProductID.ToString();

                Response.Cookies.Add(oCookie);

            }

            else
            {

                bool bExists = false;

                char[] sep = { ',' };

                HttpCookie oCookie = (HttpCookie)Request.Cookies["ShoppingCart"];

                //Set Cookie to expire in 3 hours

                oCookie.Expires = DateTime.Now.AddDays(1);

                //Check if Cookie already contain same item

                string sProdID = oCookie.Value.ToString();

                string[] arrCookie = sProdID.Split(sep);



                for (int i = 0; i < arrCookie.Length; i++)
                {

                    if (arrCookie[i].Trim() == ProductID.ToString().Trim())
                    {

                        bExists = true;

                    }

                }

                if (!bExists)
                {

                    if (oCookie.Value.Length == 0)
                    {

                        oCookie.Value = ProductID.ToString();

                    }

                    else
                    {

                        if (arrCookie.Length >= 10) { }
                        else
                        {
                            oCookie.Value = oCookie.Value + "," + ProductID;
                        }

                    }

                }

                //Add back into  the Response Objects.

                Response.Cookies.Add(oCookie);

            }

        }




        public class JsonResult
        {
            public int value { get; set; }
            public string label { get; set; }
            public int desc { get; set; }
        }

    }
}
