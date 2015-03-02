using System;
using System.Collections.Generic;
using System.IO;
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
using Website.Areas.Administrator.Models.TourProvider;

namespace Website.Areas.Administrator.Controllers
{
    public class TourProviderController : Controller
    {
       public ITourProviderService tourProviderService { get; set; }
       public INationalService nationalService { get; set; }
       public IProvinceService provinceService { get; set; }
       public IProviderPictureService providerPictureService { get; set; }
       public TourProviderController(ITourProviderService tourProviderService, INationalService nationalService,
           IProvinceService provinceService, IProviderPictureService providerPictureService)
        {
            this.tourProviderService = tourProviderService;
            this.nationalService = nationalService;
            this.provinceService = provinceService;
            this.providerPictureService = providerPictureService;
        }
        
        //
        // GET: /Administrator/TourType/
        public ActionResult Index()
        {
            List<TourProvider> list = tourProviderService.All.Where(p => !((bool)p.IsDeleted)).ToList();

            var result = new List<TourProviderDisplay>();

            foreach (var provider in list)
            {
                var model = new TourProviderDisplay();
                ModelCopier.CopyModel(provider, model);

                model.IsActive = provider.IsActive;
                model.Tours = new List<Tour>();
                model.National = nationalService.All.SingleOrDefault(n => n.ID == provider.NationalID);
                model.Province = provinceService.All.SingleOrDefault(p => p.ID == provider.ProvinceID);

                var pic = providerPictureService.Get(p => p.ProviderID == model.ID);
                if(pic != null)
                {
                    model.Avatar = pic.FileName;
                }

                result.Add(model);
            }

            return View(result);
        }

        [HttpPost]
        [ActionName("Index")]
        [AcceptButton(ButtonName = "Search")]
        public ActionResult IndexSearch(string key)
        {
            var list = tourProviderService.All;
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
            var tourProvider = new TourProviderModel();
            tourProvider.IsActive = true;

            ViewData["National"] = nationalService.All.Where(n => n.IsActive).ToList();
            ViewData["Province"] = provinceService.All.Where(n => n.IsActive).ToList();
            return View(tourProvider);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            int tourProID = Protector.Int(id);
            var tourPro= tourProviderService.Find(tourProID);
            var model = new TourProviderModel();
            ModelCopier.CopyModel(tourPro, model);

            ViewData["National"] = nationalService.All.Where(n => n.IsActive).ToList();
            ViewData["Province"] = provinceService.All.Where(n => n.IsActive && n.NationalID == model.NationalID).ToList();
            return View(model);
        }

        [HttpGet]
        public JsonResult LoadProvinceByNational(int nationalId)
        {
            var result = provinceService.All.Where(p => p.NationalID == nationalId).Select(n => new { ID = n.ID, Name = n.Name }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
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
                    int providerId = Protector.Int(record);
                    var result = tourProviderService.All.SingleOrDefault(p => p.ID == providerId);
                    if (result != null)
                    {
                        result.IsDeleted = true;
                        if(TryUpdateModel(result))
                        {
                            tourProviderService.Save();   
                        }
                    }
                }
            }
            RouteValueDictionary routeValues = this.GridRouteValues();
            return RedirectToAction("Index", routeValues);
        }

        [HttpPost]
        public ActionResult Create(TourProviderModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var info = tourProviderService.Get(r => r.Name == model.Name);
            if (info != null)
            {
                ModelState.AddModelError("", string.Format("Tên loại tour [{0}] đã hiện hữu vui lòng chọn tên khác", model.Name));
                return View(model);
            }
            var tourProData = new TourProvider()
                                   {
                                       Name = model.Name,
                                       Description = model.Description,
                                       Email = model.Email,
                                       Website = model.Website,
                                       Fax = model.Fax,
                                       PhoneNumber = model.PhoneNumber,
                                       NationalID = model.NationalID,
                                       ProvinceID = model.ProvinceID,
                                       DistrictID = model.DistrictID,
                                       AreaID = model.AreaID,
                                       Address = model.Address,
                                       IsActive = model.IsActive,
                                       IsDeleted = false,
                                       CreatedBy = Protector.Int(HttpContext.User.Identity.Name),
                                       CreatedDate = DateTime.Now
                                   };

            tourProviderService.Insert(tourProData);
            tourProviderService.Save();

            HttpPostedFileBase file = Request.Files[0];
            if (file != null && file.ContentLength > 0)
            {
                var picture = new ProviderPicture();
                string fileName = DateTime.Now.ToString("HHmmss_ddMMyyyy_") + TextHelper.HandleTitleStringForImg(Path.GetFileName(file.FileName));
                picture.FileName = fileName;
                picture.ContentLength = file.ContentLength;
                picture.ContentType = file.ContentType;
                picture.Extension = Path.GetExtension(fileName);
                picture.ProviderID = tourProData.ID;
                picture.CreatedDate = DateTime.Now;
                picture.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
                providerPictureService.Insert(picture);
                providerPictureService.Save();
                FileHelper.SaveApartment_Picture(file, picture.FileName, StorageElement.Provider_PictureFolder, tourProData.ID); 
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(string id, TourProviderModel current)
        {
            if (!ModelState.IsValid)
            {
                return View(current);
            }
            int tourProID = Protector.Int(id);
            var tourProvider = tourProviderService.Get(r => System.String.CompareOrdinal(r.Name.ToLower(), current.Name.ToLower()) == 0 && r.ID != tourProID);
            if (tourProvider != null)
            {
                ModelState.AddModelError("", string.Format("Loại Tour [{0}] đã hiện hữu vui lòng chọn tên khác", current.Name));
                return View(current);
            }
            tourProvider= tourProviderService.Find(tourProID);

            tourProvider.ModifiedBy = Protector.Int(HttpContext.User.Identity.Name);
            tourProvider.ModifiedDate = DateTime.Now;
            if (TryUpdateModel(tourProvider))
            {
                tourProviderService.Save();

                HttpPostedFileBase file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    var picture = new ProviderPicture();

                    picture = providerPictureService.Get(p => p.ProviderID == tourProvider.ID);

                    if(picture != null)
                    {
                        string fileName = DateTime.Now.ToString("HHmmss_ddMMyyyy_") + TextHelper.HandleTitleStringForImg(Path.GetFileName(file.FileName));
                        picture.FileName = fileName;
                        picture.ContentLength = file.ContentLength;
                        picture.ContentType = file.ContentType;
                        picture.Extension = Path.GetExtension(fileName);
                        picture.ProviderID = tourProvider.ID;
                        
                        providerPictureService.Update(picture);
                        providerPictureService.Save();
                    } else
                    {
                        picture = new ProviderPicture();
                        string fileName = DateTime.Now.ToString("HHmmss_ddMMyyyy_") + TextHelper.HandleTitleStringForImg(Path.GetFileName(file.FileName));
                        picture.FileName = fileName;
                        picture.ContentLength = file.ContentLength;
                        picture.ContentType = file.ContentType;
                        picture.Extension = Path.GetExtension(fileName);
                        picture.ProviderID = tourProvider.ID;
                        
                        providerPictureService.Insert(picture);
                        providerPictureService.Save(); 
                    }

                    FileHelper.SaveApartment_Picture(file, picture.FileName, StorageElement.Provider_PictureFolder, tourProvider.ID);
                }

                return RedirectToAction("Index");
            }
            else return View(current);
        }

        private TourProviderModel ConvertTourProviderData(TourProvider data)
        {
            if (data == null) return new TourProviderModel();

            return new TourProviderModel()
                       {
                           ID = data.ID,
                           Name = data.Name,
                           Description = data.Description,
                           IsActive = (bool)data.IsActive,
                       };
        }
    }
}
