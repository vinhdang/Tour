using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain;
using Microsoft.Web.Mvc;
using Service.IServices;
using Service.Helpers;
using Website.Helpers;
using Website.Model.Tour;

namespace Website.Controllers
{
    public class ProvinceController : Controller
    {
         public ITourService tourService { get; set; }
        public ISupportService supportService { get; set; }
        public IProvinceService provinceService { get; set; }
        public ISocialService socialService { get; set; }
        public ITourPriceService tourPriceService { get; set; }
        public IConfigService configService { get; set; }
        public INationalService nationalService { get; set; }
        public ITourPictureService pictureService { get; set; }
        public ITourAgendaService agendaService { get; set; }
        public ITourUtilitiesService activityService { get; set; }
        public ITourCommentService commentService { get; set; }
        public IProviderPictureService providerPictureService { get; set; }
        public IContactService contactService { get; set; }
        public ITourTypeService typeService { get; set; }
        public ITourThemeService themeService { get; set; }

        public ProvinceController(ISupportService supportService, ITourService apartmentService, IProvinceService provinceService,
            ISocialService socialService, ITourPriceService apartmentPriceService, IConfigService configService,
            INationalService nationalService, ITourPictureService pictureService, ITourAgendaService agendaService,
            ITourUtilitiesService activityService, ITourCommentService commentService, IProviderPictureService providerPictureService,
            IContactService contactService, ITourTypeService typeService, ITourThemeService themeService)
        {
            this.tourService = apartmentService;
            this.nationalService = nationalService;
            this.supportService = supportService;
            this.provinceService = provinceService;
            this.socialService = socialService;
            this.tourPriceService = apartmentPriceService;
            this.configService = configService;
            this.pictureService = pictureService;
            this.agendaService = agendaService;
            this.activityService = activityService;
            this.commentService = commentService;
            this.providerPictureService = providerPictureService;
            this.contactService = contactService;
            this.typeService = typeService;
            this.themeService = themeService;
        }

        public ActionResult Index(string provinceName, string type, string fd, string td, string theme, string district, string area, string price, string filter, string g)
        {

            if (!string.IsNullOrEmpty(provinceName))
            {
                string name = provinceName.ToLower();
                Province info = provinceService.All.FirstOrDefault(p => p.Url == name);

                var breadcrumbs = new List<Breadcrumb>();

                var breadcrumb = new Breadcrumb()
                {
                    Key = "Home",
                    Display = "Trang Chủ",
                    Url = Url.Action("Index", "Home")
                };
                breadcrumbs.Add(breadcrumb);

                if (info != null)
                {
                    ViewBag.Title = info.Name;
                    ViewBag.KeyWords = info.KeyWord;
                    ViewBag.Description = info.Description;

                    breadcrumb = new Breadcrumb()
                                     {
                                         Key = "National",
                                         Display = info.National.Name,
                                         Url = Request.Url == null ? "#" : Request.Url.AbsoluteUri
                                     };

                    breadcrumbs.Add(breadcrumb);

                    breadcrumb = new Breadcrumb()
                    {
                        Key = "Province",
                        Display = info.Name,
                        Url = Request.Url == null ? "#" : Request.Url.AbsoluteUri
                    };

                    breadcrumbs.Add(breadcrumb);

                   
                }else
                {
                    breadcrumb = new Breadcrumb()
                    {
                        Key = "National",
                        Display = "Việt Nam",
                        Url = Request.Url == null ? "#" : Request.Url.AbsoluteUri
                    };

                    breadcrumbs.Add(breadcrumb);
                }

                var title = breadcrumbs.Where(b => b.Key != "Home").Select(b => b.Display).ToList();

                ViewBag.Title = string.Join(" >> ", title);
                ViewData["Breabcrumb"] = breadcrumbs;
                ViewBag.FreeAndEasyUrl = UrlHelp.getProvinceUrl("viet-nam", "all", -1);
                ViewBag.ForeignUrl = UrlHelp.getForeignUrl("foreign", "all", -1);
            }
            return View();
        }

        public ActionResult Search(string provinceName, string fd, string td, string g, string departProvince)
        {

            string name = provinceName.ToLower();
            departProvince = string.IsNullOrEmpty(departProvince) ? "Ho-Chi-Minh" : departProvince;
            Province destinfo = provinceService.All.FirstOrDefault(p => p.Url == name);
            var depart = provinceService.All.FirstOrDefault(p => p.Url == departProvince.ToLower());

            if(destinfo != null && depart != null)
            {
                ViewBag.DestProvinceName = destinfo.Name;
                ViewBag.DepartProvinceName = depart.Name;
            }
            return PartialView("Search");
        }
        
        public ActionResult PostSearch(string txtLocal, string FromDate, string ToDate, string txtClients, string provinceID)
        {
            int _provinceID = Protector.Int(provinceID);
            string url = "";
            if (_provinceID > 0)
            {
                Province _province = provinceService.Find(_provinceID);
                if (_province != null)
                {
                    url += "/" + UrlHelp.NormalizeStringForUrl(_province.National.Name) + "/" + UrlHelp.NormalizeStringForUrl(_province.Url) + "/";
                }

                url += "p/";
                if (!string.IsNullOrEmpty(FromDate) && FromDate != "Ngày đi")
                {
                    url += "?fd=" + HttpUtility.UrlEncode(FromDate.Trim().Replace("/", "_"));
                    if (!string.IsNullOrEmpty(ToDate) && ToDate != "Ngày về")
                    {
                        url += "&td=" + HttpUtility.UrlEncode(ToDate.Trim().Replace("/", "_"));
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(ToDate) && ToDate != "Ngày về")
                    {
                        url += "?td=" + HttpUtility.UrlEncode(ToDate.Trim().Replace("/", "_"));
                    }
                }
                if (!string.IsNullOrEmpty(txtClients) && txtClients != "Số khách")
                {
                    if (url.Contains("?"))
                    {
                        url += "&g=" + HttpUtility.UrlEncode(txtClients.Trim());
                    }
                    else { url += "?g=" + HttpUtility.UrlEncode(txtClients.Trim()); }
                }

                return Redirect(url);

            }
            else
            {
                return Redirect("/");
            }
            return PartialView("Search");
        }
        
        public ActionResult Price(string provinceName, string type, string fd, string td, string theme, string district, string area, string comfort, string star, string price)
        {
            string name = provinceName.ToLower();
            Province info = provinceService.All.Where(p => p.Url == name).FirstOrDefault();
            if (info != null)
            {
                ViewBag.ProvinceName = provinceName;
                ViewBag.HotelType = type;
                ViewBag.FromDate = fd;
                ViewBag.ToDate = td;
                ViewBag.HotelTheme = theme;
                ViewBag.District = district;
                ViewBag.Area = area;
                ViewBag.Comfort = comfort;
                ViewBag.Star = star;
                ViewBag.Price = price;
                List<SelectListItem> list = GlobalVariables.PriceList;
                return PartialView("Price", list);
            }
            return PartialView("Price");
        }

        public ActionResult Filter(string provinceName, string type, string fd, string td, string theme, string district, string area, string comfort, string star, string price, string filter)
        {
            var themes = themeService.All.Where(dt => dt.IsActive).OrderByDescending(t => t.Order).ThenByDescending(t => t.CreatedDate).ToList();

            var months = WebHelpers.GetMonths(DateTime.Now.AddYears(1));
            
            WebHelpers.SessionMonthCriteria = months;

            ViewData["Themes"] = themes;
            ViewData["Month"] = months;
            return PartialView("Filter");
        }

        public ActionResult Tour(string provinceName, string departProvince)
        {

            National national = new National();
            var tours = new List<Tour>();
            national = nationalService.Get(n => n.KeyWord == "viet-nam");
            if (national != null)
            {
                tours = tourService.All.Where(t => !(bool)t.IsDeleted && t.IsActive && t.DestinationNationalID == national.ID).ToList();
            }
            //tours = tourService.All.Where(t => !(bool)t.IsDeleted && t.IsActive).ToList();

            var depart = string.IsNullOrEmpty(departProvince) ? "Ho-Chi-Minh" : departProvince;

            if (!string.IsNullOrEmpty(provinceName) && provinceName != "all")
            {
                tours = FilterTourByDestProvince(provinceName, tours);
            }


            if (!string.IsNullOrEmpty(depart))
            {
                tours = FilterTourByDepartProvince(depart, tours);
            }

            var tourResult = new List<TourDisplay>();

            tourResult = ConvertTour2ListDisplay(tours);

            var result = new TourResult()
                             {
                                 PageIndex = 1,
                                 TotalRecords = tourResult.Count,
                                 PageSize = 9,
                                 Tours = tourResult
                             };


            WebHelpers.SessionTourResult = result;

            var toDisplay = new TourResult();
            ModelCopier.CopyModel(result, toDisplay);
            toDisplay.Tours = toDisplay.Tours.Take(9).ToList();

            ViewData["Tours"] = toDisplay;
            
            return PartialView("Tour");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult FilterData(string duration, string price, string themes, string months, 
            string comments, string depart, string dest)
        {
            var tours = tourService.All.Where(t => t.IsActive && !(bool) t.IsDeleted).ToList();

            if (!string.IsNullOrEmpty(dest))
            {
                tours = FilterTourByDestProvince(dest, tours);
            }

            if (!string.IsNullOrEmpty(depart))
            {
                tours = FilterTourByDepartProvince(depart, tours);
            }
            
            if(!string.IsNullOrEmpty(themes))
            {
                tours = FilterTourByTheme(themes, tours);
            }

            if(!string.IsNullOrEmpty(months))
            {
                tours = FilterTourByMonth(months, tours);
            }

            if(!string.IsNullOrEmpty(duration))
            {
                tours = FilterTourByDuration(duration, tours); 
            }
            
            if(!string.IsNullOrEmpty(price))
            {
                tours = FilterTourByPrice(price, tours); 
            }
            
            if(!string.IsNullOrEmpty(comments))
            {
                tours = FilterTourByComment(comments, tours); 
            }

            var tourResult = new TourResult()
                                 {
                                     Tours = ConvertTour2ListDisplay(tours),
                                     PageIndex = 1,
                                     PageSize = 9,
                                     TotalRecords = tours.Count
                                 };

            WebHelpers.SessionTourResult = tourResult;

            var toDisplay = new TourResult();
            ModelCopier.CopyModel(tourResult, toDisplay);
            toDisplay.Tours = toDisplay.Tours.Take(9).ToList();

            ViewData["Tours"] = toDisplay;
            var html = RenderTemplate("Tour");

            return Json(new { success = true, html = html }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangePage(string index)
        {
            var page = Protector.Int(index);

            if (WebHelpers.SessionTourResult == null) return Json(new {success = false}, JsonRequestBehavior.AllowGet);

            var result = WebHelpers.SessionTourResult;
            var pageSize = result.PageSize;

            var toDisplay = new TourResult();
            ModelCopier.CopyModel(result, toDisplay);
            toDisplay.Tours = toDisplay.Tours.Skip(pageSize * (page - 1)).Take(pageSize).ToList();
            toDisplay.PageIndex = page;

            ViewData["Tours"] = toDisplay;
            var html = RenderTemplate("Tour");

            return Json(new { success = true, html = html }, JsonRequestBehavior.AllowGet); ;
        }

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
        
        public class JsonResult
        {
            public int value { get; set; }
            public string label { get; set; }
            public int desc { get; set; }
        }

        private List<TourDisplay> ConvertTour2ListDisplay(List<Tour> tours)
        {
            var nationals = nationalService.All.Where(p => p.IsActive).ToList();
            var provinces = provinceService.All.Where(p => p.IsActive).ToList();

            var result = new List<TourDisplay>();
            if (tours != null)
            {
                foreach (var tour in tours)
                {
                    var model = ConvertTour2Display(tour, nationals, provinces);
                    result.Add(model);
                }
            }

            return result;
        }

        private TourDisplay ConvertTour2Display(Tour tour, List<National> nationals, List<Province> provinces)
        {


            var model = new TourDisplay();

            ModelCopier.CopyModel(tour, model);
            var nation = nationals.SingleOrDefault(n => n.ID == model.DestinationNationalID);
            var province = provinces.SingleOrDefault(n => n.ID == model.DestinationProvinceID);
            var picture = tour.TourPictures.FirstOrDefault(p => p.IsActive && p.IsAvartar);

            model.AvatarPic = picture;
            if(nation != null && province != null)
            {
                model.LinkToDetail = UrlHelp.getHotelUrl(nation.Name, province.Name, model.Url, model.ID, model.DestinationProvinceID);
            }

            if(picture != null)
            {
                model.AvatarUrl = FileHelper.GetApartment_Picture(picture.FileName, picture.TourID); 
            }

            var tourCmt = tour.Comments.Where(c => !(bool)c.IsDeleted && (bool)c.IsEnable).ToList();
            decimal avg = 0;
            if (tourCmt.Any())
            {
                avg = ((Decimal)tourCmt.Sum(c => c.Level) / (Decimal)tourCmt.Count());
            }
            var rating_url = string.Format("~/Content/FrontEnd/images/user-rating-{0}.png", tourCmt.Count() > 0 ? tourCmt.Sum(c => c.Level) / tourCmt.Count(): 0);
            
            model.AVGComment = avg;
            model.Rating_Url = Url.Content(rating_url);
            model.CommentCount = tourCmt.Count();

            if (model.Description.Length > 144)
            {
                model.Description = string.Concat(model.Description.Substring(0, 144), "...");
            }

            return model;
        }

        protected String RenderTemplate(String templateName)
        {
            var content = String.Empty;
            var templateView = ViewEngines.Engines.FindPartialView(this.ControllerContext, templateName);
            using (var writer = new StringWriter())
            {
                var context = new ViewContext(this.ControllerContext, templateView.View, ViewData, TempData, writer);
                templateView.View.Render(context, writer);
                writer.Flush();
                content = writer.ToString();
            }
            return content;
        }

        private List<Tour> FilterTourByTheme(string themes, List<Tour> tours)
        {
            if (string.IsNullOrEmpty(themes)) return tours;

            var themeIds = themes.Split(new[] {','}, StringSplitOptions.None).ToList();

            tours = tours.Where(t => themeIds.Contains(t.TourThemeID.ToString())).ToList();

            return tours;
        }

        private List<Tour> FilterTourByDuration(string duration, List<Tour> tours)
        {
            if (string.IsNullOrEmpty(duration)) return tours;

            var tourDuration = Protector.Int(duration.Split(new []{';'}, StringSplitOptions.None)[0]);

            tours = tours.Where(t => t.Duration <= (tourDuration + 1) && t.Duration >= (tourDuration - 1)).ToList();

            return tours;
        }

        private List<Tour> FilterTourByPrice(string price, List<Tour> tours)
        {
            if (string.IsNullOrEmpty(price)) return tours;

            var priceRange = price.Split(new[] { ';' }, StringSplitOptions.None);
            var floor = priceRange.Length > 0 ? Protector.Decimal(priceRange[0]) : -1;
            var round = priceRange.Length > 0 ? Protector.Decimal(priceRange[1]) : -1;

            tours = tours.Where(t => t.TourPrices.Any(p => (p.Price >= round && p.Price <= floor) || p.Price == 0)).ToList();

            return tours;
        }

        private List<Tour> FilterTourByMonth(string month, List<Tour> tours)
        {
            if (string.IsNullOrEmpty(month)) return tours;

            var months = month.Split(new[] { ',' }, StringSplitOptions.None);
            var lst = new List<DateTime>();
            var sesMonth = WebHelpers.SessionMonthCriteria;

            foreach (var value in months)
            {
                var instance = sesMonth.SingleOrDefault(m => m.Value == Protector.Int(value));
                if(instance != null)
                {
                    lst.Add(instance.OriginalValue);
                }
            }

            tours = tours.Where(t => lst.Any(d => d.Month == ((DateTime)t.Startdate).Month || d.Month == ((DateTime)t.Enddate).Month)).ToList();

            return tours;
        }

        private List<Tour> FilterTourByComment(string comments, List<Tour> tours)
        {
            if (string.IsNullOrEmpty(comments)) return tours;

            var commentAvgs = comments.Split(new[] { ',' }, StringSplitOptions.None);
            var lst = new List<Tour>();
            var listCmtLevel = commentAvgs.Select(item => Protector.Decimal(item)).ToList();

            foreach (var tour in tours)
            {
                if(tour.Comments.Count == 0)continue;

                var tourCmt = tour.Comments.Where(c => !(bool) c.IsDeleted && (bool) c.IsEnable).ToList();
                decimal avg = ((Decimal)tourCmt.Sum(c => c.Level) / (Decimal)tourCmt.Count());

                if(listCmtLevel.Any(l => l <= avg))
                {
                    lst.Add(tour);
                }
            }

            return lst;
        }

        private List<Tour> FilterTourByDestProvince(string destProvince, List<Tour> tours)
        {
            if (string.IsNullOrEmpty(destProvince)) return tours;

            destProvince = UrlHelp.NormalizeStringForUrl(destProvince);
            var destProvinceUrl = Protector.String(destProvince);
            var province = provinceService.All.FirstOrDefault(p => p.Url.ToLower().Contains(destProvinceUrl.ToLower()));

            if(province == null) return tours;

            tours = tours.Where(t => t.DestinationProvinceID == province.ID).ToList();

            return tours;
        }
        
        private List<Tour> FilterTourByDepartProvince(string destProvince, List<Tour> tours)
        {
            if (string.IsNullOrEmpty(destProvince)) return tours;

            destProvince = UrlHelp.NormalizeStringForUrl(destProvince);
            var destProvinceUrl = Protector.String(destProvince);
            var province = provinceService.All.FirstOrDefault(p => p.Url.ToLower().Contains(destProvinceUrl.ToLower()));

            if(province == null) return tours;

            tours = tours.Where(t => t.DepartureProvinceID == province.ID).ToList();

            return tours;
        }
    }
}
