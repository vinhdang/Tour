using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Service.Helpers;
using Website.Areas.Administrator.Models.HotelPicture;
using Domain.Entities;
using Microsoft.Web.Mvc;
using System.IO;

namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class HotelPictureController : Controller
    {
        public IHotelService hotelService { get; set; }
        public IHotelPictureService hotelPictureService { get; set; }

        public HotelPictureController(IHotelService hotelService, IHotelPictureService hotelPictureService)
        {
            this.hotelService = hotelService;
            this.hotelPictureService = hotelPictureService;
        }
        #region HttpGet
        [HttpGet]
        public ActionResult Create(string id)
        {
            int hotelID = Protector.Int(id);
            var info = hotelService.Get(h => h.HotelID == hotelID);
            if (info != null)
            {
                CreateHotelPictureModel picture = new CreateHotelPictureModel();
                picture.Hotel = info;
                picture.IsActive = true;
                picture.HotelID = hotelID;
                picture.Alt = info.Name;
                picture.HotelPictures = hotelPictureService.All.Where(p => p.HotelID == hotelID);
                return View(picture);
            }
            return RedirectToAction("Index", "Hotel");
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            int pictureID = Protector.Int(id);
            var picture = new EditHotelPictureModel();
            HotelPicture current = hotelPictureService.Find(pictureID);
            if (current != null)
            {
                ModelCopier.CopyModel(current, picture);
                ViewBag.FilePath = current.FileName;
                picture.Hotel = current.Hotel;
                picture.HotelPictures = hotelPictureService.All.Where(p => p.HotelID == current.HotelID);
                return View(picture);
            }
            return RedirectToAction("Index", "Hotel");
        }
        #endregion
        #region HttpPost
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(CreateHotelPictureModel hotel, string id)
        {
            int hotelID = Protector.Int(id);
            var info = hotelService.Get(h => h.HotelID == hotelID);
            if (info != null)
            {
                if (!ModelState.IsValid)
                {
                    hotel.Hotel = info;
                    hotel.IsActive = true;
                    hotel.HotelPictures = hotelPictureService.All.Where(p => p.HotelID == info.HotelID);
                    return View(hotel);
                }
                HttpPostedFileBase file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    HotelPicture picture = new HotelPicture();
                    ModelCopier.CopyModel(hotel, picture);
                    if (picture.IsAvartar)
                    {

                        List<HotelPicture> list = hotelPictureService.All.Where(p => p.HotelID == hotelID && p.IsAvartar).ToList();
                        foreach (HotelPicture item in list)
                        {
                            item.IsAvartar = false;
                            hotelPictureService.Save();
                        }
                    }
                    string fileName = DateTime.Now.ToString("HHmmss_ddMMyyyy_") + TextHelper.HandleTitleStringForImg(Path.GetFileName(file.FileName));
                    picture.FileName = fileName;
                    picture.ContentLength = file.ContentLength;
                    picture.ContentType = file.ContentType;
                    picture.Extension = Path.GetExtension(fileName);
                    picture.HotelID = hotelID;
                    picture.CreatedDate = DateTime.Now;
                    picture.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
                    hotelPictureService.Insert(picture);
                    hotelPictureService.Save();
                    FileHelper.SaveHotel_Picture(file, picture.FileName, StorageElement.Hotel_PictureFolder, info.HotelID);
                }
                return RedirectToAction("Create", new { id = hotelID });
            }
            return RedirectToAction("Hotel", "Index");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(EditHotelPictureModel new_picture, string id)
        {
            int pictureID = Protector.Int(id);
            HotelPicture current = hotelPictureService.Find(pictureID);
            if (current != null)
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.FilePath = current.FileName;
                    new_picture.Hotel = current.Hotel;
                    new_picture.HotelPictures = hotelPictureService.All.Where(p => p.HotelID == current.HotelID);
                    return View(new_picture);
                }
                HttpPostedFileBase file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    var fullFilePath = FileHelper.getFullFilePath("Hotels", current.HotelID, current.FileName);
                    if (FileHelper.imageFileAvailable(fullFilePath))
                    {
                        System.IO.File.Delete(fullFilePath);
                    }
                    if (current.IsAvartar)
                    {

                        List<HotelPicture> list = hotelPictureService.All.Where(p => p.HotelID == current.HotelID && p.IsAvartar).ToList();
                        foreach (HotelPicture item in list)
                        {
                            item.IsAvartar = false;
                            hotelPictureService.Save();
                        }
                    }
                    string fileName = DateTime.Now.ToString("HHmmss_ddMMyyyy_") + TextHelper.HandleTitleStringForImg(Path.GetFileName(file.FileName));
                    FileHelper.SaveHotel_Picture(file, fileName, StorageElement.Hotel_PictureFolder, current.HotelID);
                    current.FileName = fileName;
                    current.ContentLength = file.ContentLength;
                    current.ContentType = file.ContentType;
                    current.Extension = Path.GetExtension(fileName);
                    current.Order = Protector.Int(new_picture.Order);
                    current.Alt = new_picture.Alt;
                    current.IsActive = new_picture.IsActive;
                    current.IsAvartar = new_picture.IsAvartar;
                    hotelPictureService.Save();
                    return RedirectToAction("Create", new { id = current.HotelID });
                }
                else
                {
                    if (new_picture.IsAvartar)
                    {

                        List<HotelPicture> list = hotelPictureService.All.Where(p => p.HotelID == current.HotelID && p.IsAvartar).ToList();
                        foreach (HotelPicture item in list)
                        {
                            item.IsAvartar = false;
                            hotelPictureService.Save();
                        }
                    }
                    current.Order = Protector.Int(new_picture.Order);
                    current.Alt = new_picture.Alt;
                    current.IsActive = new_picture.IsActive;
                    current.IsAvartar = new_picture.IsAvartar;
                    hotelPictureService.Save();
                    return RedirectToAction("Create", new { id = current.HotelID });
                }

            }
            return RedirectToAction("Hotel", "Index");
        }
        [HttpPost]
        public ActionResult Delete(string id, string HotelID)
        {
            int hotelID = Protector.Int(HotelID);
            int pictureID = Protector.Int(id);
            HotelPicture picture = hotelPictureService.Find(pictureID);
            if (picture != null)
            {
                FileHelper.DeleteHotel_Picture(picture.FileName, Protector.Int(picture.HotelID));
                hotelPictureService.Delete(picture);
                hotelPictureService.Save();
            }
            var pictures = hotelPictureService.All.Where(c => c.HotelID == hotelID);
            return PartialView("HotelPictureList", pictures);
        }
        #endregion


    }
}
