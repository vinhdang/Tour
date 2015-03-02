using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Domain;
using Microsoft.Web.Mvc;
using Service.Helpers;
using Service.IServices;
using Website.Areas.Administrator.Models.TourProvider;
using Website.Helpers;
using Website.Model.Tour;

namespace Website.Controllers
{
    public class TourController : Controller
    {
        //public IComfortService comfortService { get; set; }
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

        public TourController(ISupportService supportService, ITourService apartmentService, IProvinceService provinceService,
            ISocialService socialService, ITourPriceService apartmentPriceService, IConfigService configService,
            INationalService nationalService, ITourPictureService pictureService, ITourAgendaService agendaService,
            ITourUtilitiesService activityService, ITourCommentService commentService, IProviderPictureService providerPictureService,
            IContactService contactService)
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
        }

        public ActionResult Index(string tourName)
        {
            string name = Protector.String(tourName).ToLower();
            var breadcrumbs = new List<Breadcrumb>();

            var breadcrumb = new Breadcrumb()
            {
                Key = "Home",
                Display = "Trang Chủ",
                Url = Url.Action("Index", "Home")
            };
            breadcrumbs.Add(breadcrumb);

            if (!string.IsNullOrEmpty(name))
            {
                Tour info = tourService.All.FirstOrDefault(h => h.IsActive && h.Url == name);
                
                if (info != null)
                {
                    WebHelpers.SessionTourID = info.ID;
                    WebHelpers.SessionTour = info;
                    AddToCookie(info.ID);

                    var picture = info.TourPictures.FirstOrDefault(p => p.IsActive && p.IsAvartar);
                    ViewBag.Title = info.PageTitle;
                    ViewBag.Description = info.Description;
                    ViewBag.KeyWords = info.KeyWord;
                    if (picture != null)
                    {
                        ViewBag.Image = FileHelper.GetApartment_Picture(picture.FileName, picture.TourID);
                    }

                    var national = nationalService.Get(n => n.ID == info.DestinationNationalID);
                    if(national !=  null)
                    {
                        breadcrumb = new Breadcrumb()
                        {
                            Key = "National",
                            Display = national.Name,
                            Url = "/" + UrlHelp.NormalizeStringForUrl(national.KeyWord) + "/all/"
                        };
                        if(national.KeyWord == "viet-nam")
                        {
                            breadcrumb.Url += "p/";
                        }else
                        {
                            breadcrumb.Url += "f/";
                        }
                        breadcrumbs.Add(breadcrumb); 
                    }

                    var province = provinceService.Get(p => p.ID == info.DestinationProvinceID);

                    if(province != null)
                    {
                        breadcrumb = new Breadcrumb()
                        {
                            Key = "Province",
                            Display = province.Name,
                            Url = "/" + UrlHelp.NormalizeStringForUrl(province.National.KeyWord) + "/" + UrlHelp.NormalizeStringForUrl(province.Name) + "/"
                        };
                        if (province.National.KeyWord == "viet-nam")
                        {
                            breadcrumb.Url += "p/";
                        }
                        else
                        {
                            breadcrumb.Url += "f/";
                        }
                        breadcrumbs.Add(breadcrumb);
                    }

                    //
                    breadcrumb = new Breadcrumb()
                    {
                        Key = "Province",
                        Display = info.Name,
                        Url = "#"
                    };

                    breadcrumbs.Add(breadcrumb);
                }

                ViewData["Breabcrumb"] = breadcrumbs;
            }
            return View();
        }

        public ActionResult Info(string tourName)
        {
            var tourId = Protector.Int(WebHelpers.SessionTourID);
            var tour = tourService.Get(t => t.ID == tourId);
            if (tour == null) return PartialView("Info");

            var tourPrices = tourPriceService.All.Where(p => p.TourID == tourId && p.IsActive).OrderBy(a => a.Sequence).ToList();
            var tourAgenda = agendaService.All.Where(a => a.TourID == tourId && a.IsActivate).OrderBy(a => a.Sequence).ToList();
            var tourSuggestion = activityService.All.Where(a => a.TourID == tourId && (bool)a.IsActive).OrderBy(a => a.Order).ToList();
            var tourComment = commentService.All.Where(a => a.TourID == tourId && (bool)a.IsEnable).OrderBy(a => a.CreatedDate).ToList();
            var lstEmail = supportService.All.Where(s => s.IsActive && (s.Type & 8) == 8).OrderByDescending(s => s.Order).ThenByDescending(s => s.CreatedDate).ToList();
            var lstPhone = supportService.All.Where(s => s.IsActive && (s.Type & 4) == 4).OrderByDescending(s => s.Order).ThenByDescending(s => s.CreatedDate).ToList();

            var pricesConverted = new List<PriceDisplay>();
            var emailSupport = string.Empty;
            var phoneSupport = string.Empty;
            if(tourPrices != null && tourPrices.Count > 0)
            {
                foreach (var price in tourPrices)
                {
                    var model = new PriceDisplay();
                    ModelCopier.CopyModel(price, model);
                    model.TourProvider = Convert2ProviderDisplay(price.TourProvider);

                    pricesConverted.Add(model);
                }
            }

            if(lstEmail.Count > 0)
            {
                emailSupport = string.Join(" - ", lstEmail.Select(e => e.Value));
            }
            if(lstPhone.Count > 0)
            {
                phoneSupport = string.Join(" - ", lstPhone.Select(e => e.Value));
            }

            ViewData["Prices"] = pricesConverted;
            ViewData["Agenda"] = tourAgenda;
            ViewData["Suggestion"] = tourSuggestion;
            ViewData["Comment"] = tourComment;
            ViewData["EmailSupport"] = emailSupport;
            ViewData["PhoneSupport"] = phoneSupport;
            ViewData["TourId"] = tourId;
             

            return PartialView("Info");
        }

        public ActionResult GetListAgenda(string tourid)
        {
            var tourId = Protector.Int(tourid);
            var tourAgenda = agendaService.All.Where(a => a.TourID == tourId && a.IsActivate).OrderBy(a => a.Sequence).ToList();

            var result = new List<JsonAgenda>();
            foreach (var agenda in tourAgenda)
            {
                var model = new JsonAgenda();
                ModelCopier.CopyModel(agenda, model);

                result.Add(model);
            }
            return Json(new { agenda = result }, JsonRequestBehavior.AllowGet);
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

        public ActionResult Breadcrumb()
        {
            return PartialView("Breadcrumb");
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

        public ActionResult Gallery(string tourName)
        {
            var tourPic = pictureService.All.Where(p => p.TourID == WebHelpers.SessionTourID && p.IsActive).ToList();
            var result = new List<TourPictureDisplay>();

            if(tourPic != null)
            {
                foreach (var picture in tourPic)
                {
                    var model = new TourPictureDisplay();
                    ModelCopier.CopyModel(picture, model);

                    model.PictureUrl = FileHelper.GetApartment_Picture(picture.FileName, picture.TourID);

                    result.Add(model);
                }
            }

            ViewData["Tour"] = WebHelpers.SessionTour;

            ViewBag.Phone = supportService.All.Where(s => s.IsActive && (s.Type & 4) == 4).OrderByDescending(s => s.Order).ThenByDescending(s => s.CreatedDate).ToList();
           
            return PartialView("Gallery", result);
        }

        public ActionResult MayYouLike(string tourName)
        {
            string name = Protector.String(tourName).ToLower();
            if (!string.IsNullOrEmpty(name))
            {
                Tour info = tourService.All.FirstOrDefault(h => h.IsActive && !(bool)h.IsDeleted && h.Url == name);
                
                if (info != null)
                {
                    var list = tourService.All.Where(h => !(bool)h.IsDeleted && h.IsActive && h.ID != info.ID && h.DestinationProvinceID == info.DestinationProvinceID).Take(10).ToList();

                    var result = ConvertTour2ListDisplay(list);

                    return PartialView("MayYouLike", result);
                }
            }

            return PartialView("MayYouLike");
        }

        [HttpPost]
        public ActionResult SubmitComment(string un, string msg, string tid, string level, string email)
        {
            if (string.IsNullOrEmpty(un) || string.IsNullOrEmpty(msg) || string.IsNullOrEmpty(tid))
                return Json(new {success = false, exist = false, html = string.Empty}, JsonRequestBehavior.AllowGet);

            var tourId = Protector.Int(tid);

            var persisted =
                commentService.Get(c => c.TourID == tourId && c.Email.ToLower() == un.ToLower() && c.Content.ToLower() == msg.ToLower());

            if(persisted != null)
            {
                return Json(new { success = false, exist = true, html = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            var comment = new Comment();

            comment.TourID = tourId;
            comment.Email = un;
            comment.Content = msg;
            comment.Level = Protector.Int(level);
            comment.CreatedDate = DateTime.Now;
            comment.IsDeleted = false;
            comment.IsEnable = true;
            
            commentService.Insert(comment);
            commentService.Save();

            var contact = new Contact()
                              {
                                  TourID = tourId,
                                  Content = string.Empty,
                                  Address = string.Empty,
                                  CreatedDate = DateTime.Now,
                                  Email = email,
                                  FullName = un,
                                  Phone = string.Empty,
                                  Isprocessing = false,
                                  Type = enumContactType.Comment.GetHashCode()
                              };

            contactService.Insert(contact);
            contactService.Save();

            ViewData["Comment"] = commentService.All.Where(a => a.TourID == tourId && (bool)a.IsEnable).ToList();
            var html = RenderTemplate("Comment");

            return Json(new { success = true, html = html }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, ValidateInput(false)]
        public ActionResult OrderTour(string em, string p, string tourid, string note)
        {
            if(string.IsNullOrEmpty(em) || string.IsNullOrEmpty(p) || string.IsNullOrEmpty(tourid))
            {
                return Json(new {success = false}, JsonRequestBehavior.AllowGet);
            }
            var tourId = Protector.Int(tourid);
            //var persisted = contactService.Get(c => c.Email == em && c.Phone == p && c.TourID == tourId);
            //if(persisted != null)
            //{
            //    return Json(new { success = false , exist = true}, JsonRequestBehavior.AllowGet);
            //}

            try
            {
                var contact = new Contact()
                {
                    CreatedDate = DateTime.Now,
                    FullName = em,
                    Email = em,
                    Phone = p,
                    Address = string.Empty,
                    Isprocessing = false,
                    TourID = tourId,
                    Content = note,
                    Type = enumContactType.Order.GetHashCode()
                };

                if (DateTime.Today.Day > 9)
                {
                    contact.OrderCode = DateTime.Today.Day.ToString();
                }
                else { contact.OrderCode = "0" + DateTime.Today.Day.ToString(); }
                if (DateTime.Today.Month > 9)
                {
                    contact.OrderCode += DateTime.Today.Month.ToString();
                }
                else { contact.OrderCode += "0" + DateTime.Today.Month.ToString(); }
                contact.OrderCode += DateTime.Now.Year.ToString() + contact.ID.ToString();

                contactService.Insert(contact);
                contactService.Save();

                var tour = tourService.All.SingleOrDefault(t => t.ID == tourId);
                var nationals = nationalService.All.ToList();
                var provinces = provinceService.All.ToList();
                var tourDisplay = ConvertTour2Display(tour, nationals, provinces);

                SendEmail(contact, tourDisplay);
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            return Json(new {success = true}, JsonRequestBehavior.AllowGet);
        }
        
        //public ActionResult PriceList(string apartmentName)
        //{
        //    string name = Protector.String(apartmentName).ToLower();
        //    if (!string.IsNullOrEmpty(name))
        //    {
        //        Apartment info = apartmentService.All.Where(h => h.IsActive && h.Url == name).FirstOrDefault();
        //        if (info != null)
        //        {
        //            var list = apartmentPriceService.All.Where(h => h.IsActive && h.ApartmentID == info.ApartmentID && h.Date >= DateTime.Today).OrderBy(c => c.Date).Take(30).ToList();
        //            return PartialView("PriceList", list);
        //        }
        //    }
        //    return PartialView("PriceList");
        //}

        public ActionResult History()
        {
            var list = new List<TourDisplay>();
            if (Request.Cookies["TourHistory"] == null)
            {
                //Do nothing
            }
            else
            {
                HttpCookie oCookie = (HttpCookie)Request.Cookies["TourHistory"];
                char[] sep = { ',' };
                oCookie.Expires = DateTime.Now.AddDays(2);
                string[] arrCookie = oCookie.Value.Split(sep);

                if (arrCookie != null && arrCookie.Length > 0)
                {
                    var nationals = nationalService.All.Where(p => p.IsActive).ToList();
                    var provinces = provinceService.All.Where(p => p.IsActive).ToList();
                    foreach (string id in arrCookie)
                    {
                        long tourID = Protector.Long(id);
                        var tour = tourService.All.FirstOrDefault(h => h.IsActive && !(bool)h.IsDeleted && h.ID == tourID);
                        if (tour != null)
                        {
                            list.Add(ConvertTour2Display(tour, nationals, provinces));
                        }
                    }
                }
            }
            return PartialView("History", list);
        }

        private void AddToCookie(long ProductID)
        {
            if (Request.Cookies["TourHistory"] == null)
            {
                HttpCookie oCookie = new HttpCookie("TourHistory");

                //Set Cookie to expire in 3 hours
                oCookie.Expires = DateTime.Now.AddDays(1);
                oCookie.Value = ProductID.ToString();
                Response.Cookies.Add(oCookie);
            }
            else
            {
                bool bExists = false;
                char[] sep = { ',' };
                HttpCookie oCookie = (HttpCookie)Request.Cookies["TourHistory"];

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
            model.LinkToDetail = UrlHelp.getHotelUrl(nation.Name, province.Name, model.Url, model.ID, model.DestinationProvinceID);
            model.AvatarUrl = FileHelper.GetApartment_Picture(picture.FileName, picture.TourID);
            if (model.Description.Length > 144)
            {
                model.Description = string.Concat(model.Description.Substring(0, 144), "...");
            }

            return model;
        } 

        private TourProviderDisplay Convert2ProviderDisplay(TourProvider provider)
        {
            var model = new TourProviderDisplay();
            ModelCopier.CopyModel(provider, model);

            model.IsActive = provider.IsActive;
            model.Tours = new List<Tour>();
            model.National = nationalService.All.SingleOrDefault(n => n.ID == provider.NationalID);
            model.Province = provinceService.All.SingleOrDefault(p => p.ID == provider.ProvinceID);

            var pic = providerPictureService.Get(p => p.ProviderID == model.ID);
            if (pic != null)
            {
                model.Avatar = FileHelper.GetProvider_Picture(pic.FileName, Protector.Int(pic.ProviderID));
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

        private void SendEmail(Contact contact, TourDisplay tour)
        {
            if (contact == null) return;

            Config config = configService.All.FirstOrDefault(c => c.IsActive && c.Name == "BookingTemplate");
            if (config != null)
            {
                List<string> emailTo = new List<string>();
                emailTo.Add(contact.Email);
                Config from = configService.All.FirstOrDefault(c => c.Name == "Email");
                Config cc = configService.All.FirstOrDefault(c => c.Name == "EmailCC");
                Config port = configService.All.FirstOrDefault(c => c.Name == "Port");
                Config timeout = configService.All.FirstOrDefault(c => c.Name == "Timeout");
                Config hostName = configService.All.FirstOrDefault(c => c.Name == "HostName");
                Config userName = configService.All.FirstOrDefault(c => c.Name == "UserName");
                Config password = configService.All.FirstOrDefault(c => c.Name == "Password");
                List<string> emailCC = new List<string>();
                if (cc != null && !string.IsNullOrEmpty(cc.Value))
                {
                    string[] emails = cc.Value.Split(';');
                    foreach (string e in emails)
                    {
                        emailCC.Add(e);
                    }
                }


                string subject = "100Tour thông báo booking " + contact.OrderCode;
                string tr = "";

                tr += "<tr>"
                      +
                      "<td style='border: solid 1px #c2c2c2; color: #000000; margin: 0; padding: 5px; text-align: center;border-top: 0px; border-right: 0px;'><a href='http://vietrip.vn" +
                      tour.LinkToDetail + "'>" + tour.Name + "</a></td>"
                      +
                      "<td style='border: solid 1px #c2c2c2; color: #000000; margin: 0; padding: 5px; text-align: center;border-top: 0px; border-right: 0px;'>N/A</td>"
                      +
                      "<td style='border: solid 1px #c2c2c2; color: #000000; margin: 0; padding: 5px; text-align: center;border-top: 0px; border-right: 0px;'>N/A</td>";

                string body = string.Format(config.Value, contact.FullName, contact.OrderCode, String.Format("{0:HH:mm:ss}", contact.CreatedDate), String.Format("{0:dd/MM/yyyy}", contact.CreatedDate), tr, contact.OrderCode, Protector.String(contact.FullName), Protector.String(contact.Phone), Protector.String(contact.Email), Protector.String(contact.Address), string.Empty, Protector.String(contact.Content), "Thanh toán tại công ty", string.Empty, string.Empty, string.Empty);
                int success = EmailHelper.SMTPSendEmail(Protector.Int(port.Value, 465), Protector.String(hostName.Value.Trim()), Protector.Int(timeout.Value.Trim(), 10000), Protector.String(userName.Value.Trim()), Protector.String(password.Value.Trim()), Protector.String(userName.Value), emailTo, emailCC, subject, string.Empty, body);
            }
        }
    }
}
