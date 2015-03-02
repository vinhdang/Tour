using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Service.Helpers;
using Website.Helpers;
using Domain.Entities;
using Microsoft.Web.Mvc;
using System.Web.Routing;
using Service.Attribute;
using Telerik.Web.Mvc.Extensions;

namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        public IPaymentService paymentService { get; set; }

        public PaymentController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }
        #region HttpGet
        public ActionResult Index()
        {
            List<Payment> list = paymentService.All.ToList();
            return View(list);
        }

        [HttpPost]
        [ActionName("Index")]
        [AcceptButton(ButtonName = "Delete")]
        public ActionResult IndexDelete(int[] checkedRecords)
        {
            checkedRecords = checkedRecords ?? new int[] { };
            if (checkedRecords != null && checkedRecords.Length > 0)
            {

            }
            RouteValueDictionary routeValues = this.GridRouteValues();
            return RedirectToAction("Index", routeValues);
        }

        [HttpPost]
        [ActionName("Index")]
        [AcceptButton(ButtonName = "Search")]
        public ActionResult IndexSearch(string key)
        {
            var list = paymentService.All;
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
            var payment = new Payment();
            payment.IsActive = true;
            return View(payment);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            int paymentID = Protector.Int(id);
            var payment = paymentService.Find(paymentID);
            return View(payment);
        }
        #endregion
        #region HttpPost
        [HttpPost]
        public ActionResult Create(Payment payment)
        {
            if (!ModelState.IsValid)
            {
                return View(payment);
            }
            Payment info = paymentService.Get(r => r.Name == payment.Name);
            if (info != null)
            {
                ModelState.AddModelError("", string.Format("Tên loại thanh toán [{0}] đã hiện hữu vui lòng chọn tên khác", payment.Name));
                return View(payment);
            }
            payment.CreatedDate = DateTime.Now;
            payment.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
            paymentService.Insert(payment);
            paymentService.Save();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(string id, Payment current)
        {
            if (!ModelState.IsValid)
            {
                return View(current);
            }
            int paymentlID = Protector.Int(id);
            Payment payment = paymentService.Get(r => r.Name == current.Name && r.PaymentID != paymentlID);
            if (payment != null)
            {
                ModelState.AddModelError("", string.Format("Tên loại thanh toán [{0}] đã hiện hữu vui lòng chọn tên khác", current.Name));
                return View(payment);
            }
            payment = paymentService.Find(paymentlID);
            if (TryUpdateModel(payment))
            {
                paymentService.Save();
                return RedirectToAction("Index");
            }
            else return View(payment);

        }
        #endregion
    }
}
