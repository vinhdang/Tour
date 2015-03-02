using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Web.Mvc;
using Service.IServices;
using Domain;
using Service.Helpers;
using Website.Model.Tour;

namespace Website.Controllers
{
    public class HomeController : Controller
    {
        public ISupportService supportService { get; set; }
        public IProvinceService provinceService { get; set; }
        public ITourService tourService { get; set; }
        public ICategoryService categoryService { get; set; }
        public ISocialService socialService { get; set; }
        public INationalService nationalService { get; set; }
        public IDistrictService districtService { get; set; }

        public HomeController(ISupportService supportService, IProvinceService provinceService,
            ITourService tourService, ICategoryService categoryService, ISocialService socialService,
            INationalService nationalService, IDistrictService districtService)
        {
            this.supportService = supportService;
            this.provinceService = provinceService;
            this.tourService = tourService;
            this.categoryService = categoryService;
            this.socialService = socialService;
            this.nationalService = nationalService;
            this.districtService = districtService;
        }
        
        public ActionResult Index()
        {
            int catID = 1;
            if (catID > 0)
            {
                Category info = categoryService.All.FirstOrDefault(h => h.IsActive && h.ID == catID);
                if (info != null)
                {
                    ViewBag.Title = info.PageTitle;
                    ViewBag.Description = info.Description;
                    ViewBag.KeyWords = info.Keyword;
                    return View(info);
                }
            }
            return View();
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
        public ActionResult Search()
        {
            List<Province> lstTypical = provinceService.All.Where(p => p.IsActive && (p.Position & 1) == 1).OrderByDescending(s => s.Order).ThenByDescending(s => s.CreatedDate).ToList();
            List<Province> lstABCD = provinceService.All.Where(p => p.IsActive && (p.Position & 4) == 4).OrderByDescending(s => s.Order).ThenByDescending(s => s.CreatedDate).ToList();
            List<Province> lstEFGH = provinceService.All.Where(p => p.IsActive && (p.Position & 8) == 8).OrderByDescending(s => s.Order).ThenByDescending(s => s.CreatedDate).ToList();
            List<Province> lstIJKL = provinceService.All.Where(p => p.IsActive && (p.Position & 16) == 16).OrderByDescending(s => s.Order).ThenByDescending(s => s.CreatedDate).ToList();
            List<Province> lstMNOP = provinceService.All.Where(p => p.IsActive && (p.Position & 32) == 32).OrderByDescending(s => s.Order).ThenByDescending(s => s.CreatedDate).ToList();
            List<Province> lstQRST = provinceService.All.Where(p => p.IsActive && (p.Position & 64) == 64).OrderByDescending(s => s.Order).ThenByDescending(s => s.CreatedDate).ToList();
            List<Province> lstUVWXYZ = provinceService.All.Where(p => p.IsActive && (p.Position & 128) == 128).OrderByDescending(s => s.Order).ThenByDescending(s => s.CreatedDate).ToList();

            ViewBag.Typical = lstTypical;
            ViewBag.ABCD = lstABCD;
            ViewBag.EFGH = lstEFGH;
            ViewBag.IJKL = lstIJKL;
            ViewBag.MNOP = lstMNOP;
            ViewBag.QRST = lstQRST;
            ViewBag.UVWXYZ = lstUVWXYZ;

            return PartialView("Search");
        }
        public ActionResult Province()
        {
            List<Province> list = provinceService.All.Where(p => p.IsActive && (p.Position & 256) == 256).OrderByDescending(s => s.Order).ThenByDescending(s => s.CreatedDate).ToList();

            return PartialView("Province", list);
        }

        [HttpPost]
        public ActionResult PostSearch(string txtLocal, string province, FormCollection data)
        {
            var provinceShort = Protector.String(data["home-province"] ?? province);
            var country = UrlHelp.NormalizeStringForUrl(data["home-national"]);
            var local = UrlHelp.NormalizeStringForUrl(data["home-district"]);
            var searchtext = UrlHelp.NormalizeStringForUrl(data["search-text"]);

            //remove prefix
            if(provinceShort.Contains("province") || provinceShort.Contains("Province"))
            {
                provinceShort = provinceShort.ToLower().Replace("province", "").Trim();
            }

            provinceShort = UrlHelp.NormalizeStringForUrl(provinceShort);

            var national = nationalService.All.FirstOrDefault(n => n.KeyWord.Contains(country));
            //var _province = provinceService.Get(p => p.Url == provinceShort);
            var _province = provinceService.Get(p => p.Url.Contains(provinceShort) && !string.IsNullOrEmpty(provinceShort));
            string url = "";

            if (_province != null)
            {
                national = _province.National;
            }

            if (national != null)
            {
                if (national.KeyWord == "viet-nam")
                {
                    if (_province != null)
                    {
                        url += "/" + UrlHelp.NormalizeStringForUrl(_province.National.KeyWord) + "/" + UrlHelp.NormalizeStringForUrl(_province.Url) + "/";
                        url += "p/";
                    }
                    else
                    {
                        url += "/viet-nam/all/p/";
                    }
                }
                else
                {
                    if (_province != null)
                    {
                        url += "/" + UrlHelp.NormalizeStringForUrl(_province.National.KeyWord) + "/" + UrlHelp.NormalizeStringForUrl(_province.Url) + "/";
                        url += "f/";
                    }else
                    {
                        url += "/" + UrlHelp.NormalizeStringForUrl(national.KeyWord) + "/all/f/";
                    }
                }

                return Redirect(url);
            }
            else
            {
                url += "/viet-nam/all/p/";
                return Redirect(url);
            }

            return Redirect("/");
        }

        public ActionResult Navigation()
        {
            List<Province> lstNavigation = provinceService.All.Where(p => p.IsActive && (p.Position & 2) == 2).OrderByDescending(s => s.Order).ThenByDescending(s => s.CreatedDate).ToList();

            return PartialView("Navigation", lstNavigation);
        }

        public ActionResult Apartment()
        {
            List<Tour> lstTour= tourService.All.Where(p => p.IsActive && (p.Type & 1) == 1).OrderByDescending(s => s.Order).ThenByDescending(s => s.CreatedDate).ToList();

            return PartialView("Apartment", lstTour);
        }

        public ActionResult Tour()
        {
            var provinces = provinceService.All.Where(p => p.IsActive && p.Position == 1).ToList();

            if (provinces != null && provinces.Count > 0)
            {
                foreach (var pro in provinces)
                {
                    var browseUrl = "/" + UrlHelp.NormalizeStringForUrl(pro.National.KeyWord) + "/" + UrlHelp.NormalizeStringForUrl(pro.Url) + "/";
                    pro.Avartar = FileHelper.GetProvince_Picture(pro.Avartar, pro.ID);
                    pro.Url = "/" + UrlHelp.NormalizeStringForUrl(pro.National.KeyWord) + "/" + UrlHelp.NormalizeStringForUrl(pro.Url) + "/";

                    if(pro.National.KeyWord == "viet-nam")
                    {
                        browseUrl += "p/";
                    }else
                    {
                        browseUrl += "f/";
                    }

                    pro.Url = browseUrl;
                }
            }

            return PartialView("Tour", provinces);
        }

        //public ActionResult Tour()
        //{
        //    var lstTour = tourService.All.Where(p => p.IsActive && !(bool)p.IsDeleted && (p.Type & 1) == 1).OrderByDescending(s => s.Order).ThenByDescending(s => s.CreatedDate).ToList();
        //    var nationals = nationalService.All.Where(p => p.IsActive).ToList();
        //    var provinces = provinceService.All.Where(p => p.IsActive).ToList();

        //    var result = new List<TourDisplay>();
        //    if (lstTour != null)
        //    {
        //        foreach (var tour in lstTour)
        //        {
        //            var model = new TourDisplay();

        //            ModelCopier.CopyModel(tour, model);
        //            var nation = nationals.SingleOrDefault(n => n.ID == model.DestinationNationalID);
        //            var province = provinces.SingleOrDefault(n => n.ID == model.DestinationProvinceID);
        //            var picture = tour.TourPictures.FirstOrDefault(p => p.IsActive && p.IsAvartar);

        //            model.AvatarPic = picture;
        //            model.LinkToDetail = UrlHelp.getHotelUrl(nation.Name, province.Name, model.Url, model.ID, model.DestinationProvinceID);
        //            model.AvatarUrl = FileHelper.GetApartment_Picture(picture.FileName, picture.TourID);
        //            if (model.Description.Length > 144)
        //            {
        //                model.Description = string.Concat(model.Description.Substring(0, 144), "...");
        //            }

        //            if (result.Count < 6)
        //            {
        //                result.Add(model);
        //            }
        //            else
        //            {
        //                break;
        //            }

        //        }
        //    }

        //    return PartialView("Tour", result);
        //}

        public ActionResult Footer()
        {
            var lstSocial = socialService.All.Where(s => s.IsActive).OrderByDescending(s => s.Order).ToList();

            foreach (var social in lstSocial)
            {
                social.ICon = FileHelper.GetSocial_Picture(social.ICon, social.ID);
            }

            ViewBag.Phone = supportService.All.Where(s => s.IsActive && (s.Type & 4) == 4).OrderByDescending(s => s.Order).ThenByDescending(s => s.CreatedDate).ToList();
            return PartialView("Footer", lstSocial);
        }

        public ActionResult SearchProvince(string query)
        {
            var keyword = Request["query"] ?? query;
            List<JsonResult> result = new List<JsonResult>();

            if(string.IsNullOrEmpty(keyword)) return Json(result, JsonRequestBehavior.AllowGet);

            keyword = keyword.ToLower().Trim();
            var keywordStandard = UrlHelp.NormalizeStringForUrl(keyword);

            var temp =
                provinceService.All.Where(
                    p => p.Url.ToLower().Contains(keywordStandard) 
                        || p.Districts.Any(d => d.Name.ToLower().Contains(keyword))
                        || p.Areas.Any(a => a.Name.ToLower().Contains(keyword)));

            if (temp == null || temp.Count() == 0) return Json(result, JsonRequestBehavior.AllowGet);

            foreach (var item in temp)
            {
                var district = item.Districts.FirstOrDefault(d => d.Name.ToLower().Contains(keyword));
                var strDistrict = district != null ? string.Format("{0}, ", district.Name) : string.Empty;
                var urlDistrict = district != null ? UrlHelp.NormalizeStringForUrl(district.Name) : string.Empty;
                var value = string.Format("{0},{1},{2}", urlDistrict, item.Url, UrlHelp.NormalizeStringForUrl(item.National.Name));

                var str = string.Format("{0}{1}, {2}", strDistrict, item.Name, item.National.Name);
                result.Add(new JsonResult { value = value, label = str});
                //result.Add(new JsonResult { value = value, label = str, natid = item.Url, proid = item.Url, distid = urlDistrict });
            }
            if (result.Count > 15)
            {
                result = result.Take(15).ToList();
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public class JsonResult
        {
            public string value { get; set; }
            public string label { get; set; }
            //public int desc { get; set; }
            //public string natid { get; set; }
            //public string proid { get; set; }
            //public string distid { get; set; }
        }
    }
}
