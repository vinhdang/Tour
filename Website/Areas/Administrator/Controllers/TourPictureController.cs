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
using Website.Areas.Administrator.Models.TourPicture;

namespace Website.Areas.Administrator.Controllers
{
    public class TourPictureController : Controller
    {
        public ITourService tourService { get; set; }
        public ITourPictureService tourPictureService { get; set; }

        public TourPictureController(ITourService tourService, ITourPictureService tourPictureService)
        {
            this.tourService = tourService;
            this.tourPictureService = tourPictureService;
        }
        #region HttpGet
        [HttpGet]
        public ActionResult Index(string id)
        {
            int tourID = Protector.Int(id);
            List<TourPicture> list = tourPictureService.All.Where(c => c.TourID == tourID).ToList();
            var info = tourService.Find(tourID);
            if (info != null)
            {
                ViewBag.TourName = info.Name;
                ViewBag.TourID = info.ID;
            }
            return View(list);
        }
        [HttpGet]
        public ActionResult Create(string id)
        {
            int tourID = Protector.Int(id);
            var info = tourService.Get(h => h.ID == tourID);
            if (info != null)
            {
                TourPictureModel picture = new TourPictureModel();
                picture.IsActive = true;
                picture.Alt = info.Name;
                ViewBag.TourName = info.Name;
                ViewBag.TourID = info.ID;
                return View(picture);
            }
            return RedirectToAction("Index", "Tour");
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            int pictureID = Protector.Int(id);
            var picture = new EditTourPictureModel();
            var current = tourPictureService.Find(pictureID);
            if (current != null)
            {
                ModelCopier.CopyModel(current, picture);
                ViewBag.FilePath = current.FileName;
                ViewBag.TourName = current.Tour.Name;
                ViewBag.TourID = current.Tour.ID;
                return View(picture);
            }
            return RedirectToAction("Index", "Tour");
        }
        #endregion

        #region HttpPost
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(TourPictureModel tour, string id)
        {
            int tourID = Protector.Int(id);
            var info = tourService.Get(h => h.ID == tourID);
            if (info != null)
            {
                if (!ModelState.IsValid)
                {
                    tour.Alt = info.Name;
                    ViewBag.TourName = info.Name;
                    ViewBag.TourID = info.ID;
                    return View(tour);
                }
                HttpPostedFileBase file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    var picture = new TourPicture();
                    ModelCopier.CopyModel(tour, picture);
                    if (picture.IsAvartar)
                    {

                        List<TourPicture> list = tourPictureService.All.Where(p => p.TourID == tourID && p.IsAvartar).ToList();
                        foreach (var item in list)
                        {
                            item.IsAvartar = false;
                            tourPictureService.Save();
                        }
                    }
                    string fileName = DateTime.Now.ToString("HHmmss_ddMMyyyy_") + TextHelper.HandleTitleStringForImg(Path.GetFileName(file.FileName));
                    picture.FileName = fileName;
                    picture.ContentLength = file.ContentLength;
                    picture.ContentType = file.ContentType;
                    picture.Extension = Path.GetExtension(fileName);
                    picture.TourID = tourID;
                    picture.CreatedDate = DateTime.Now;
                    picture.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
                    tourPictureService.Insert(picture);
                    tourPictureService.Save();
                    FileHelper.SaveApartment_Picture(file, picture.FileName, StorageElement.Tour_PictureFolder, info.ID);
                }
                return RedirectToAction("Index", new { id = tourID });
            }
            return RedirectToAction("Index", "Tour");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(EditTourPictureModel new_picture, string id)
        {
            int pictureID = Protector.Int(id);
            var current = tourPictureService.Find(pictureID);
            if (current != null)
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.TourName = current.Tour.Name;
                    ViewBag.TourID = current.Tour.ID;
                    ViewBag.FilePath = current.FileName;
                    return View(new_picture);
                }
                HttpPostedFileBase file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    var fullFilePath = FileHelper.getFullFilePath("Tour", current.TourID, current.FileName);
                    if (FileHelper.imageFileAvailable(fullFilePath))
                    {
                        System.IO.File.Delete(fullFilePath);
                    }
                    if (current.IsAvartar)
                    {

                        List<TourPicture> list = tourPictureService.All.Where(p => p.TourID == current.TourID && p.IsAvartar).ToList();
                        foreach (var item in list)
                        {
                            item.IsAvartar = false;
                            tourPictureService.Save();
                        }
                    }
                    string fileName = DateTime.Now.ToString("HHmmss_ddMMyyyy_") + TextHelper.HandleTitleStringForImg(Path.GetFileName(file.FileName));
                    FileHelper.SaveApartment_Picture(file, fileName, StorageElement.Tour_PictureFolder, current.TourID);
                    current.FileName = fileName;
                    current.ContentLength = file.ContentLength;
                    current.ContentType = file.ContentType;
                    current.Extension = Path.GetExtension(fileName);
                    current.Order = Protector.Int(new_picture.Order);
                    current.Alt = new_picture.Alt;
                    current.IsActive = new_picture.IsActive;
                    current.IsAvartar = new_picture.IsAvartar;
                    tourPictureService.Save();
                    return RedirectToAction("Index", new { id = current.TourID });
                }
                else
                {
                    if (new_picture.IsAvartar)
                    {

                        List<TourPicture> list = tourPictureService.All.Where(p => p.TourID == current.TourID && p.IsAvartar).ToList();
                        foreach (var item in list)
                        {
                            item.IsAvartar = false;
                            tourPictureService.Save();
                        }
                    }
                    current.Order = Protector.Int(new_picture.Order);
                    current.Alt = new_picture.Alt;
                    current.IsActive = new_picture.IsActive;
                    current.IsAvartar = new_picture.IsAvartar;
                    tourPictureService.Save();
                    return RedirectToAction("Index", new { id = current.TourID });
                }

            }
            return RedirectToAction("Index", "Tour");
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
                    var picture = tourPictureService.Find(item);
                    if (picture != null)
                    {
                        tourPictureService.Delete(picture);
                        tourPictureService.Save();
                        FileHelper.DeleteApartment_Picture(picture.FileName, Protector.Int(picture.TourID));
                    }
                }
            }
            RouteValueDictionary routeValues = this.GridRouteValues();
            return RedirectToAction("Index", routeValues);
        }
        #endregion
    }
}
