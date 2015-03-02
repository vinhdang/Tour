using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Service.Helpers;
using Domain.Entities;
using Website.Models.Comment;
using System.Web.Routing;

namespace Website.Controllers
{
    public class CommentController : Controller
    {
        public IHotelService hotelService { get; set; }
        public ICommentService commentService { get; set; }
        public IConfigService configService { get; set; }
        public CommentController(IHotelService hotelService, ICommentService commentService, IConfigService configService)
        {
            this.hotelService = hotelService;
            this.commentService = commentService;
            this.configService = configService;
        }

        public ActionResult CommentList(string h, string p)
        {
            int HotelID = Protector.Int(h);
            List<Comment> list = new List<Comment>();
            Hotel hotel = hotelService.Find(HotelID);
            if (hotel != null)
            {
                list = commentService.All.Where(c => c.HotelID == hotel.HotelID && c.IsActive).OrderByDescending(c => c.CreatedDate).ToList();
            }
            return PartialView("CommentList", list);
        }
        public ActionResult Comment(string h, string p)
        {
            int HotelID = Protector.Int(h);
            CreateCommentModel info = null;
            Hotel hotel = hotelService.Find(HotelID);
            if (hotel != null)
            {
                info = new CreateCommentModel();
                info.HotelID = hotel.HotelID;
                return PartialView("Comment", info);
            }
            return PartialView("Comment", info);
        }
        public ActionResult CommentResult(string h)
        {
            int HotelID = Protector.Int(h);
            MyHotel info = hotelService.getByID(HotelID);
            if (info != null)
            {
                ViewBag.Title = Protector.String(info.PageTitle);
                ViewBag.Description = Protector.String(info.Description);
                ViewBag.KeyWords = Protector.String(info.KeyWord);
                Config config = configService.All.Where(c => c.IsActive && c.Name == "CommentResult").FirstOrDefault();
                if (config != null)
                {
                    return View(config);
                }

            }
            return View();
        }
        [HttpPost]
        public ActionResult Comment(CreateCommentModel model, string CaptchaValue, string InvisibleCaptchaValue, string h, string p, string rate1, string rate2, string rate3, string rate4, string rate5, string rate6)
        {
            int HotelID = Protector.Int(h);
            Hotel hotel = hotelService.Find(HotelID);
            if (hotel != null)
            {
                bool cv = CaptchaController.IsValidCaptchaValue(CaptchaValue.ToUpper());
                bool icv = InvisibleCaptchaValue == "";

                if (!cv || !icv)
                {
                    return Redirect("/");
                }
                if (!ModelState.IsValid)
                {
                    model.HotelID = hotel.HotelID;
                    return PartialView("Comment", model);
                }
                Comment info = new Domain.Entities.Comment();
                info.IP = TextHelper.GetIPAddress();
                info.Content = model.Content;
                info.Name = model.Name;
                info.HotelID = hotel.HotelID;
                info.Email = model.Email;
                info.Title = model.Title;
                info.IsActive = false;
                info.IsActiveBy = null;
                info.Rate1 = Protector.Int(rate1);
                info.Rate2 = Protector.Int(rate2);
                info.Rate3 = Protector.Int(rate3);
                info.Rate4 = Protector.Int(rate4);
                info.Rate5 = Protector.Int(rate5);
                info.Rate6 = Protector.Int(rate6);
                info.CreatedDate = DateTime.Now;
                commentService.Insert(info);
                commentService.Save();
                Config configEmail = configService.All.Where(c => c.IsActive && c.Name == "EmailTo").FirstOrDefault();
                List<string> emailTo1 = new List<string>();
                if (configEmail != null && !string.IsNullOrEmpty(configEmail.Value))
                {
                    string[] emails = configEmail.Value.Split(';');
                    foreach (string e in emails)
                    {
                        emailTo1.Add(e.Trim());
                    }
                }
                string subject1 = "Cảm ơn khách hàng đã viết đánh giá đến Vietrip";
                Config commentTemplate = configService.All.Where(c => c.IsActive && c.Name == "CommentTemplate").FirstOrDefault();
                string body1 = string.Format(commentTemplate.Value, info.Name);
                List<string> emailTo = new List<string>();
                emailTo.Add(info.Email.Trim());
                EmailHelper.SMTPSendEmail(emailTo, string.Empty, subject1, string.Empty, body1);


                string subject = "www.Vietrip.vn thong bao đánh giá mới chờ duyệt";
                string body = "";
                body += "Tên:" + info.Name + "<br/>";
                body += "Email :" + info.Email + "<br/>";
                body += "Tiêu đề :" + info.Title + "<br/>";
                body += "Nội dung :" + info.Content + "<br/>";
                body += "Nhận xét chung :" + info.Rate1 + "<br/>";
                body += "Chất lượng dịch vụ :" + info.Rate2 + "<br/>";
                body += "Chất lượng phòng :" + info.Rate3 + "<br/>";
                body += "Sạch sẽ :" + info.Rate4 + "<br/>";
                body += "Nhân viên phục vụ :" + info.Rate5 + "<br/>";
                body += "Tiện nghi phòng :" + info.Rate6 + "<br/>";
                body += "Gởi yêu cầu lúc :" + info.CreatedDate + "<br/>";
                body += "Khách sạn :" + info.Hotel.Name + "<br/>";
                int success1 = EmailHelper.SMTPSendEmail(emailTo1, string.Empty, subject, string.Empty, body);
                RouteValueDictionary obj = new RouteValueDictionary();
                obj.Add("h", info.HotelID);
                return RedirectToAction("CommentResult", "Comment", obj);
            }
            return Redirect("/");
        }

    }
}
