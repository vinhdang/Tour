using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.Helpers;
using Service.IServices;
using Service.Attribute;
using System.Web.Routing;
using Domain.Entities;
using Website.Models.Search;

namespace Website.Controllers
{

    public class SearchController : Controller
    {
        public IDistrictService districtService { get; set; }
        public IProvinceService provinceService { get; set; }
        public IHotelService hotelService { get; set; }
        public IBannerService bannerService { get; set; }
        public SearchController(IDistrictService districtService, IProvinceService provinceService, IHotelService hotelService, IBannerService bannerService)
        {
            this.districtService = districtService;
            this.provinceService = provinceService;
            this.hotelService = hotelService;
            this.bannerService = bannerService;
        }
          [ValidateInput(false)]
        public ActionResult Find(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                ViewBag.Title = "Tìm kiếm khách sạn theo Tỉnh / Thành phố";
                SearchHelp info = new SearchHelp();
                info.Name = Server.HtmlEncode(key);
                List<Province> list = provinceService.All.Where(p => p.IsActive).ToList();
                info.Provinces = new List<Province>();
                foreach (Province item in list)
                {
                    string str = UrlHelp.NormalizeStringForUrl(item.Name).Replace("-", " ");
                    if (str.Contains(UrlHelp.NormalizeStringForUrl(info.Name).Replace("-", " ")))
                    {

                        info.Provinces.Add(item);
                    }
                }

                List<Hotel> listHotel = hotelService.All.Where(p => p.IsActive).ToList();
                info.Hotels = new List<Hotel>();
                foreach (Hotel item in listHotel)
                {
                    string str = UrlHelp.NormalizeStringForUrl(item.Name).Replace("-", " ");
                    if (str.Contains(UrlHelp.NormalizeStringForUrl(info.Name).Replace("-", " ")))
                    {

                        info.Hotels.Add(item);
                    }
                }
                info.Banners = bannerService.All.Where(p => p.IsActive && (p.Position & 2) == 2).ToList();
                return View(info);
            }
            return Redirect("/");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Find(SearchHelp current)
        {
            if (current != null && !string.IsNullOrEmpty(current.KeyWord))
            {
                RouteValueDictionary obj = new RouteValueDictionary();
                obj.Add("key", HttpUtility.HtmlEncode(current.KeyWord));
                return RedirectToAction("Find", "Search", obj);
            }
            return View();
        }
        public ActionResult Index(string p, string d, string a, string c, string ht, string type, string hp, string fd, string td, string nr, string ng, string price, string star, string filter, string ftype)
        {
            int provinceID = Protector.Int(p);
            if (provinceID > 0)
            {
                Province info = provinceService.Find(provinceID);
                ViewBag.Title = info.Name;
                ViewBag.KeyWords = info.KeyWord;
                ViewBag.Description = info.Description;

                List<int> _districtID = new List<int>();
                List<int> _areaID = new List<int>();
                List<int> _comfortID = new List<int>();
                List<int> _hotelThemeID = new List<int>();
                List<int> _hotelTypeID = new List<int>();
                List<int> _priceID = new List<int>();
                List<int> _starID = new List<int>();
                int isPromotion = Protector.Int(hp, 0);
                string _filterName = Protector.String(filter);
                string _filterType = Protector.String(ftype);
                DateTime _fromDate = DateTime.Today;
                DateTime _toDate = DateTime.Today;
                int _numberRoom = Protector.Int(nr, 0);
                int _numberGuest = Protector.Int(ng, 0);


                if (!string.IsNullOrEmpty(fd))
                {
                    try
                    {
                        string[] day = fd.Split('_');
                        _fromDate = new DateTime(Protector.Int(day[2]), Protector.Int(day[1]), Protector.Int(day[0]));
                    }
                    catch { }
                }
                if (!string.IsNullOrEmpty(td))
                {
                    try
                    {
                        string[] day = td.Split('_');
                        _toDate = new DateTime(Protector.Int(day[2]), Protector.Int(day[1]), Protector.Int(day[0]));
                    }
                    catch { }
                }
                else
                {
                    _toDate = _fromDate.AddHours(23.99);
                }
                if (!string.IsNullOrEmpty(d))
                {
                    string[] ListdistrictID = d.Split('|');
                    foreach (string id in ListdistrictID)
                    {
                        _districtID.Add(Protector.Int(id));
                    }
                }
                if (!string.IsNullOrEmpty(a))
                {
                    string[] ListAreaID = a.Split('|');
                    foreach (string id in ListAreaID)
                    {
                        _areaID.Add(Protector.Int(id));
                    }
                }
                if (!string.IsNullOrEmpty(c))
                {
                    string[] ListComfortID = c.Split('|');
                    foreach (string id in ListComfortID)
                    {
                        _comfortID.Add(Protector.Int(id));
                    }
                }
                if (!string.IsNullOrEmpty(ht))
                {
                    string[] ListHotelThemeID = ht.Split('|');
                    foreach (string id in ListHotelThemeID)
                    {
                        _hotelThemeID.Add(Protector.Int(id));
                    }
                }
                if (!string.IsNullOrEmpty(type))
                {
                    string[] ListHotelTypeID = type.Split('|');
                    foreach (string id in ListHotelTypeID)
                    {
                        _hotelTypeID.Add(Protector.Int(id));
                    }
                }
                if (!string.IsNullOrEmpty(price))
                {
                    string[] ListPriceID = price.Split('|');
                    foreach (string id in ListPriceID)
                    {
                        _priceID.Add(Protector.Int(id));
                    }
                }
                if (!string.IsNullOrEmpty(star))
                {
                    string[] ListStarID = star.Split('|');
                    foreach (string id in ListStarID)
                    {
                        _starID.Add(Protector.Int(id));
                    }
                }
                var list = hotelService.getHotels(provinceID, _districtID, _areaID, _comfortID, _hotelThemeID, _hotelTypeID, isPromotion, _fromDate, _toDate, _numberRoom, _numberGuest, _priceID, _starID, _filterName, _filterType);
                return View(list);
            }
            return View();
        }
        #region Partial
        [HttpGet]
        public ActionResult SearchByPriceByStar(string p, string d, string a, string c, string ht, string type, string hp, string fd, string td, string nr, string ng, string price, string star)
        {
            int provinceID = Protector.Int(p);
            if (provinceID > 0)
            {
                Website.Models.Search.SearchByPriceByStarModel searchHotel = new Models.Search.SearchByPriceByStarModel();
                if (!string.IsNullOrEmpty(price))
                {
                    searchHotel.ListPriceID = price.Split('|');
                }
                if (!string.IsNullOrEmpty(star))
                {
                    searchHotel.ListStarID = star.Split('|');
                }
                searchHotel.ProvinceID = provinceID;
                searchHotel.DistrictID = Protector.String(d);
                searchHotel.AreaID = Protector.String(a);
                searchHotel.ComfortID = Protector.String(c);
                searchHotel.HotelThemeID = Protector.String(ht);
                searchHotel.HotelTypeID = Protector.String(type);
                searchHotel.HotelPromotion = Protector.String(hp);
                searchHotel.FromDate = Protector.String(fd);
                searchHotel.ToDate = Protector.String(td);
                searchHotel.NumberRoom = Protector.String(nr);
                searchHotel.NumberGuest = Protector.String(ng);
                searchHotel.StarList = GlobalVariables.ListStar;
                searchHotel.PriceList = GlobalVariables.PriceList;
                searchHotel.priceID = Protector.String(price);
                return PartialView("SearchByPriceByStar", searchHotel);
            }
            return PartialView("SearchByPriceByStar");
        }
        [HttpGet]
        public ActionResult FilterBy(string p, string d, string a, string c, string ht, string type, string hp, string fd, string td, string nr, string ng, string price, string star, string filter, string ftype)
        {
            int provinceID = Protector.Int(p);
            if (provinceID > 0)
            {
                Website.Models.Search.FilterByModel searchHotel = new Models.Search.FilterByModel();

                searchHotel.ProvinceID = provinceID;
                searchHotel.DistrictID = Protector.String(d);
                searchHotel.AreaID = Protector.String(a);
                searchHotel.ComfortID = Protector.String(c);
                searchHotel.HotelThemeID = Protector.String(ht);
                searchHotel.HotelTypeID = Protector.String(type);
                searchHotel.HotelPromotion = Protector.String(hp);
                searchHotel.FromDate = Protector.String(fd);
                searchHotel.ToDate = Protector.String(td);
                searchHotel.NumberRoom = Protector.String(nr);
                searchHotel.NumberGuest = Protector.String(ng);
                searchHotel.Filter = Protector.String(filter);
                searchHotel.Price = Protector.String(price);
                searchHotel.Star = Protector.String(star);
                searchHotel.FType = Protector.String(ftype);
                return PartialView("FilterBy", searchHotel);
            }
            return PartialView("FilterBy");
        }
        [HttpPost]
        public ActionResult FilterByPost(string p, string d, string a, string c, string ht, string type, string hp, string fd, string td, string nr, string ng, string price, string star, string filtername, string filtervalue)
        {
            int provinceID = Protector.Int(p);
            if (provinceID > 0)
            {
                RouteValueDictionary obj = new RouteValueDictionary();
                obj.Add("p", provinceID);
                if (!string.IsNullOrEmpty(d))
                {
                    obj.Add("d", d);
                }
                if (!string.IsNullOrEmpty(a))
                {
                    obj.Add("a", a);
                }
                if (!string.IsNullOrEmpty(ht))
                {
                    obj.Add("ht", ht);
                }
                if (!string.IsNullOrEmpty(type))
                {
                    obj.Add("type", type);
                }
                if (!string.IsNullOrEmpty(hp))
                {
                    obj.Add("hp", hp);
                }
                if (!string.IsNullOrEmpty(c))
                {
                    obj.Add("c", c);
                }
                if (!string.IsNullOrEmpty(fd))
                {
                    obj.Add("fd", fd);
                }
                if (!string.IsNullOrEmpty(td))
                {
                    obj.Add("td", td);
                }
                if (!string.IsNullOrEmpty(nr))
                {
                    obj.Add("nr", nr);
                }
                if (!string.IsNullOrEmpty(ng))
                {
                    obj.Add("ng", ng);
                }
                if (!string.IsNullOrEmpty(price))
                {
                    obj.Add("price", price);
                }
                if (!string.IsNullOrEmpty(star))
                {
                    obj.Add("star", star);
                }
                if (!string.IsNullOrEmpty(filtername) && filtername != "0")
                {
                    obj.Add("filter", filtername);
                }
                if (!string.IsNullOrEmpty(filtervalue) && filtervalue != "0")
                {
                    obj.Add("ftype", filtervalue);
                }
                string url = Url.Action("Index", "Search", obj);
                return Redirect(url);
            }
            return PartialView("SearchByPriceByStar");
        }

        [HttpPost]
        public ActionResult SearchByPrice(string p, string d, string a, string c, string ht, string type, string hp, string fd, string td, string nr, string ng, string[] priceID)
        {
            int provinceID = Protector.Int(p);
            if (provinceID > 0)
            {
                RouteValueDictionary obj = new RouteValueDictionary();
                obj.Add("p", provinceID);
                string listID = "";
                if (priceID != null && priceID.Length > 0)
                {
                    foreach (string str in priceID)
                    {
                        listID += str + "|";
                    }
                    obj.Add("price", listID.Replace("|", " ").Trim().Replace(" ", "|"));

                }
                if (!string.IsNullOrEmpty(d))
                {
                    obj.Add("d", d);
                }
                if (!string.IsNullOrEmpty(a))
                {
                    obj.Add("a", a);
                }
                if (!string.IsNullOrEmpty(ht))
                {
                    obj.Add("ht", ht);
                }
                if (!string.IsNullOrEmpty(type))
                {
                    obj.Add("type", type);
                }
                if (!string.IsNullOrEmpty(hp))
                {
                    obj.Add("hp", hp);
                }
                if (!string.IsNullOrEmpty(c))
                {
                    obj.Add("c", c);
                }
                if (!string.IsNullOrEmpty(fd))
                {
                    obj.Add("fd", fd);
                }
                if (!string.IsNullOrEmpty(td))
                {
                    obj.Add("td", td);
                }
                if (!string.IsNullOrEmpty(nr))
                {
                    obj.Add("nr", nr);
                }
                if (!string.IsNullOrEmpty(ng))
                {
                    obj.Add("ng", ng);
                }
                string url = Url.Action("Index", "Search", obj);
                return Redirect(url);
            }
            return PartialView("SearchByPriceByStar");
        }
        [HttpPost]
        public ActionResult SearchByStar(string p, string d, string a, string c, string ht, string type, string hp, string price, string fd, string td, string nr, string ng, string[] starID)
        {
            int provinceID = Protector.Int(p);
            if (provinceID > 0)
            {
                RouteValueDictionary obj = new RouteValueDictionary();
                obj.Add("p", provinceID);
                string listID = "";
                if (starID != null && starID.Length > 0)
                {
                    foreach (string str in starID)
                    {
                        listID += str + "|";
                    }
                    obj.Add("star", listID.Replace("|", " ").Trim().Replace(" ", "|"));

                }
                if (!string.IsNullOrEmpty(d))
                {
                    obj.Add("d", d);
                }
                if (!string.IsNullOrEmpty(a))
                {
                    obj.Add("a", a);
                }
                if (!string.IsNullOrEmpty(ht))
                {
                    obj.Add("ht", ht);
                }
                if (!string.IsNullOrEmpty(type))
                {
                    obj.Add("type", type);
                }
                if (!string.IsNullOrEmpty(price))
                {
                    obj.Add("price", price);
                }
                if (!string.IsNullOrEmpty(hp))
                {
                    obj.Add("hp", hp);
                }
                if (!string.IsNullOrEmpty(c))
                {
                    obj.Add("c", c);
                }
                if (!string.IsNullOrEmpty(fd))
                {
                    obj.Add("fd", fd);
                }
                if (!string.IsNullOrEmpty(td))
                {
                    obj.Add("td", td);
                }
                if (!string.IsNullOrEmpty(nr))
                {
                    obj.Add("nr", nr);
                }
                if (!string.IsNullOrEmpty(ng))
                {
                    obj.Add("ng", ng);
                }
                string url = Url.Action("Index", "Search", obj);
                return Redirect(url);
            }
            return PartialView("SearchByPriceByStar");
        }


        [HttpGet]
        public ActionResult SearchHotel(string p, string fd, string td, string nr, string ng, string h)
        {
            int provinceID = Protector.Int(p);
            int hotelID = Protector.Int(h);
            if (provinceID > 0)
            {
                Province info = provinceService.Find(provinceID);
                if (info != null)
                {
                    Website.Models.Search.SearchHotelModel searchHotel = new Models.Search.SearchHotelModel();
                    if (!string.IsNullOrEmpty(fd))
                    {
                        searchHotel.FDate = Protector.String(fd.Replace("_", "/").Trim());
                    }
                    if (!string.IsNullOrEmpty(td))
                    {
                        searchHotel.TDate = Protector.String(td.Replace("_", "/").Trim());
                    }
                    searchHotel.NumberRoom = Protector.Int(nr, 1);
                    searchHotel.NumberGuest = Protector.Int(ng, 1);
                    searchHotel._ProvinceID = provinceID;
                    searchHotel.ProvinceName = info.Name + " , " + info.National.Name;

                    return PartialView("SearchHotel", searchHotel);
                }
            }
            return PartialView("SearchHotel");
        }
        [HttpPost]
        public ActionResult SearchHotel(string ProvinceName, string provinceID, string HotelName, string hotelID, string FDate, string TDate, string NumberGuest, string NumberRoom)
        {
            int _provinceID = Protector.Int(provinceID);
            int _hotelID = Protector.Int(hotelID);
            if (_provinceID > 0)
            {
                RouteValueDictionary obj = new RouteValueDictionary();
                obj.Add("p", _provinceID);
                if (!string.IsNullOrEmpty(FDate))
                {
                    obj.Add("fd", HttpUtility.UrlEncode(FDate.Trim().Replace("/", "_")));
                }
                if (!string.IsNullOrEmpty(TDate))
                {
                    obj.Add("td", HttpUtility.UrlEncode(TDate.Trim().Replace("/", "_")));
                }
                if (!string.IsNullOrEmpty(NumberGuest))
                {
                    obj.Add("ng", NumberGuest);
                }
                if (!string.IsNullOrEmpty(NumberRoom))
                {
                    obj.Add("nr", NumberRoom);
                }
                if (_hotelID > 0)
                {
                    string url = UrlHelp.getHotelUrl(Protector.String(ProvinceName), Protector.String(HotelName), _hotelID, _provinceID);
                    if (!string.IsNullOrEmpty(FDate))
                    {
                        url += "?fd=" + HttpUtility.UrlEncode(FDate.Trim().Replace("/", "_"));
                        if (!string.IsNullOrEmpty(TDate))
                        {
                            url += "&td=" + HttpUtility.UrlEncode(TDate.Trim().Replace("/", "_"));
                        }
                        if (!string.IsNullOrEmpty(NumberRoom))
                        {
                            url += "&nr=" + HttpUtility.UrlEncode(NumberRoom);
                        }
                        if (!string.IsNullOrEmpty(NumberGuest))
                        {
                            url += "&ng=" + HttpUtility.UrlEncode(NumberGuest);
                        }
                    }

                    else
                    {
                        if (!string.IsNullOrEmpty(TDate))
                        {
                            url += "?td=" + HttpUtility.UrlEncode(TDate.Trim().Replace("/", "_"));
                            if (!string.IsNullOrEmpty(NumberRoom))
                            {
                                url += "&nr=" + HttpUtility.UrlEncode(NumberRoom);
                            }
                            if (!string.IsNullOrEmpty(NumberGuest))
                            {
                                url += "&ng=" + HttpUtility.UrlEncode(NumberGuest);
                            }
                        }
                    }
                    return Redirect(url);
                }

                return RedirectToAction("Index", "Search", obj);
            }
            else
            {
            }
            return View();
        }
        [HttpGet]
        public ActionResult HotelPromotion(string p, string d, string a, string c, string ht, string type, string hp, string fd, string td, string nr, string ng)
        {
            int provinceID = Protector.Int(p);
            if (provinceID > 0)
            {
                Website.Models.Search.HotelPromotionModel hotelPromotion = new Models.Search.HotelPromotionModel();
                if (!string.IsNullOrEmpty(hp))
                {
                    hotelPromotion.ListHotelPromotionID = hp.Split('|');
                }
                hotelPromotion.ProvinceID = provinceID;
                hotelPromotion.DistrictID = Protector.String(d);
                hotelPromotion.AreaID = Protector.String(a);
                hotelPromotion.ComfortID = Protector.String(c);
                hotelPromotion.HotelThemeID = Protector.String(ht);
                hotelPromotion.HotelTypeID = Protector.String(type);
                hotelPromotion.FromDate = Protector.String(fd);
                hotelPromotion.ToDate = Protector.String(td);
                hotelPromotion.NumberRoom = Protector.String(nr);
                hotelPromotion.NumberGuest = Protector.String(ng);
                hotelPromotion.HotelPromotionList = GlobalVariables.ListHotelPromotion;
                return PartialView("HotelPromotion", hotelPromotion);
            }
            return PartialView("HotelPromotion");
        }
        [HttpPost]
        public ActionResult HotelPromotion(string p, string d, string a, string c, string ht, string type, string hp, string fd, string td, string nr, string ng, string[] HotelPromotionID)
        {
            int provinceID = Protector.Int(p);
            if (provinceID > 0)
            {
                RouteValueDictionary obj = new RouteValueDictionary();
                obj.Add("p", provinceID);
                string hotelPromotionID = "";
                if (HotelPromotionID != null && HotelPromotionID.Length > 0)
                {
                    foreach (string str in HotelPromotionID)
                    {
                        hotelPromotionID += str + "|";
                    }
                    obj.Add("hp", hotelPromotionID.Replace("|", " ").Trim().Replace(" ", "|"));

                }
                if (!string.IsNullOrEmpty(d))
                {
                    obj.Add("d", d);
                }
                if (!string.IsNullOrEmpty(a))
                {
                    obj.Add("a", a);
                }
                if (!string.IsNullOrEmpty(ht))
                {
                    obj.Add("ht", ht);
                }
                if (!string.IsNullOrEmpty(type))
                {
                    obj.Add("type", type);
                }
                if (!string.IsNullOrEmpty(c))
                {
                    obj.Add("c", c);
                }
                if (!string.IsNullOrEmpty(fd))
                {
                    obj.Add("fd", fd);
                }
                if (!string.IsNullOrEmpty(td))
                {
                    obj.Add("td", td);
                }
                if (!string.IsNullOrEmpty(nr))
                {
                    obj.Add("nr", nr);
                }
                if (!string.IsNullOrEmpty(ng))
                {
                    obj.Add("ng", ng);
                }
                string url = Url.Action("Index", "Search", obj);
                return Redirect(url);
            }
            return PartialView("HotelPromotion");
        }

        public ActionResult SelectGuest(string nroom)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            int nID = Protector.Int(nroom);
            for (int i = nID; i <= nID * 4; i++)
            {
                list.Add(new SelectListItem { Text = i.ToString() + " khách", Value = i.ToString(), Selected = false });
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getProvince(string term)
        {
            var list = provinceService.All.Where(p => p.IsActive).OrderByDescending(p => p.Order).ToList();
            List<JsonResult> result = new List<JsonResult>();
            foreach (Province item in list)
            {
                string str = UrlHelp.NormalizeStringForUrl(item.Name).Replace("-", " ");
                if (str.Contains(UrlHelp.NormalizeStringForUrl(term).Replace("-", " ")))
                {
                    result.Add(new JsonResult { value = item.ProvinceID, label = item.Name, desc = 1 });
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getHotel(string term, string provinceID)
        {
            int _provinceID = Protector.Int(provinceID);
            var list = hotelService.All.Where(h => h.ProvinceID == _provinceID && h.IsActive).OrderByDescending(h => h.Order);
            List<JsonResult> result = new List<JsonResult>();
            foreach (Hotel item in list)
            {
                string str = UrlHelp.NormalizeStringForUrl(item.Name).Replace("-", " ");
                if (str.Contains(UrlHelp.NormalizeStringForUrl(term).Replace("-", " ")))
                {
                    result.Add(new JsonResult { value = item.HotelID, label = item.Name, desc = item.Star });
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
