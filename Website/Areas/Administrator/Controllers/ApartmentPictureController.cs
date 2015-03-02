using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Service.Helpers;
using Website.Areas.Administrator.Models.ApartmentPicture;
using Domain;
using Microsoft.Web.Mvc;
using System.IO;
using Service.Attribute;
using System.Web.Routing;
using Telerik.Web.Mvc.Extensions;

namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class ApartmentPictureController : Controller
    {
        public ITourService tourService { get; set; }
        public ITourPictureService tourPictureService { get; set; }

        public ApartmentPictureController(ITourService tourService, ITourPictureService tourPictureService)
        {
            this.tourService = tourService;
            this.tourPictureService = tourPictureService;
        }
        #region HttpGet
        [HttpGet]
        public ActionResult Index(string id)
        {
            int apartmentID = Protector.Int(id);
            List<ApartmentPicture> list = apartmentPictureService.All.Where(c => c.ApartmentID == apartmentID).ToList();
            Apartment info = apartmentService.Find(apartmentID);
            if (info != null)
            {
                ViewBag.ApartmentName = info.Name;
                ViewBag.ApartmentID = info.ApartmentID;
            }
            return View(list);
        }
        [HttpGet]
        public ActionResult Create(string id)
        {
            int apartmentID = Protector.Int(id);
            var info = apartmentService.Get(h => h.ApartmentID == apartmentID);
            if (info != null)
            {
                CreateApartmentPictureModel picture = new CreateApartmentPictureModel();
                picture.IsActive = true;
                picture.Alt = info.Name;
                ViewBag.ApartmentName = info.Name;
                ViewBag.ApartmentID = info.ApartmentID;
                return View(picture);
            }
            return RedirectToAction("Index", "Apartment");
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            int pictureID = Protector.Int(id);
            var picture = new EditApartmentPictureModel();
            ApartmentPicture current = apartmentPictureService.Find(pictureID);
            if (current != null)
            {
                ModelCopier.CopyModel(current, picture);
                ViewBag.FilePath = current.FileName;
                ViewBag.ApartmentName = current.Apartment.Name;
                ViewBag.ApartmentID = current.Apartment.ApartmentID;
                return View(picture);
            }
            return RedirectToAction("Index", "Apartment");
        }
        #endregion
        #region HttpPost
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(CreateApartmentPictureModel apartment, string id)
        {
            int apartmentID = Protector.Int(id);
            var info = apartmentService.Get(h => h.ApartmentID == apartmentID);
            if (info != null)
            {
                if (!ModelState.IsValid)
                {
                    apartment.Alt = info.Name;
                    ViewBag.ApartmentName = info.Name;
                    ViewBag.ApartmentID = info.ApartmentID;
                    return View(apartment);
                }
                HttpPostedFileBase file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    ApartmentPicture picture = new ApartmentPicture();
                    ModelCopier.CopyModel(apartment, picture);
                    if (picture.IsAvartar)
                    {

                        List<ApartmentPicture> list = apartmentPictureService.All.Where(p => p.ApartmentID == apartmentID && p.IsAvartar).ToList();
                        foreach (ApartmentPicture item in list)
                        {
                            item.IsAvartar = false;
                            apartmentPictureService.Save();
                        }
                    }
                    string fileName = DateTime.Now.ToString("HHmmss_ddMMyyyy_") + TextHelper.HandleTitleStringForImg(Path.GetFileName(file.FileName));
                    picture.FileName = fileName;
                    picture.ContentLength = file.ContentLength;
                    picture.ContentType = file.ContentType;
                    picture.Extension = Path.GetExtension(fileName);
                    picture.ApartmentID = apartmentID;
                    picture.CreatedDate = DateTime.Now;
                    picture.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
                    apartmentPictureService.Insert(picture);
                    apartmentPictureService.Save();
                    FileHelper.SaveApartment_Picture(file, picture.FileName, StorageElement.Apartment_PictureFolder, info.ApartmentID);
                }
                return RedirectToAction("Index", new { id = apartmentID });
            }
            return RedirectToAction("Apartment", "Index");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(EditApartmentPictureModel new_picture, string id)
        {
            int pictureID = Protector.Int(id);
            ApartmentPicture current = apartmentPictureService.Find(pictureID);
            if (current != null)
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ApartmentName = current.Apartment.Name;
                    ViewBag.ApartmentID = current.Apartment.ApartmentID;
                    ViewBag.FilePath = current.FileName;
                    return View(new_picture);
                }
                HttpPostedFileBase file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    var fullFilePath = FileHelper.getFullFilePath("Apartment", current.ApartmentID, current.FileName);
                    if (FileHelper.imageFileAvailable(fullFilePath))
                    {
                        System.IO.File.Delete(fullFilePath);
                    }
                    if (current.IsAvartar)
                    {

                        List<ApartmentPicture> list = apartmentPictureService.All.Where(p => p.ApartmentID == current.ApartmentID && p.IsAvartar).ToList();
                        foreach (ApartmentPicture item in list)
                        {
                            item.IsAvartar = false;
                            apartmentPictureService.Save();
                        }
                    }
                    string fileName = DateTime.Now.ToString("HHmmss_ddMMyyyy_") + TextHelper.HandleTitleStringForImg(Path.GetFileName(file.FileName));
                    FileHelper.SaveApartment_Picture(file, fileName, StorageElement.Apartment_PictureFolder, current.ApartmentID);
                    current.FileName = fileName;
                    current.ContentLength = file.ContentLength;
                    current.ContentType = file.ContentType;
                    current.Extension = Path.GetExtension(fileName);
                    current.Order = Protector.Int(new_picture.Order);
                    current.Alt = new_picture.Alt;
                    current.IsActive = new_picture.IsActive;
                    current.IsAvartar = new_picture.IsAvartar;
                    apartmentPictureService.Save();
                    return RedirectToAction("Index", new { id = current.ApartmentID });
                }
                else
                {
                    if (new_picture.IsAvartar)
                    {

                        List<ApartmentPicture> list = apartmentPictureService.All.Where(p => p.ApartmentID == current.ApartmentID && p.IsAvartar).ToList();
                        foreach (ApartmentPicture item in list)
                        {
                            item.IsAvartar = false;
                            apartmentPictureService.Save();
                        }
                    }
                    current.Order = Protector.Int(new_picture.Order);
                    current.Alt = new_picture.Alt;
                    current.IsActive = new_picture.IsActive;
                    current.IsAvartar = new_picture.IsAvartar;
                    apartmentPictureService.Save();
                    return RedirectToAction("Index", new { id = current.ApartmentID });
                }

            }
            return RedirectToAction("Hotel", "Index");
        }

        [HttpPost]
        [ActionName("Index")]
        [AcceptButton(ButtonName = "Delete")]
        public ActionResult IndexDelete(int[] checkedRecords)
        {
            if (checkedRecords != null && checkedRecords.Length > 0)
            {
                foreach (int item in checkedRecords)
                {
                    ApartmentPicture picture = apartmentPictureService.Find(item);
                    if (picture != null)
                    {
                        apartmentPictureService.Delete(picture);
                        apartmentPictureService.Save();
                        FileHelper.DeleteApartment_Picture(picture.FileName, Protector.Int(picture.ApartmentID));
                    }
                }
            }
            RouteValueDictionary routeValues = this.GridRouteValues();
            return RedirectToAction("Index", routeValues);
        }
        #endregion


    }
}
