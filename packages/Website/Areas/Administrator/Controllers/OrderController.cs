using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Service.Helpers;
using Website.Areas.Administrator.Models.Order;
using Microsoft.Web.Mvc;
using Website.Helpers;
using Domain.Entities;
using Service.Attribute;
using System.Web.Routing;
using Telerik.Web.Mvc.Extensions;

namespace Website.Areas.Administrator.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        public ICategoryService categoryService { get; set; }
        public IOrderService orderService { get; set; }
        public IOrderInfoService orderInfoService { get; set; }
        public IStatusService statusService { get; set; }
        public IPaymentService paymentService { get; set; }
        public INationalService nationalService { get; set; }
        public IProvinceService provinceService { get; set; }
        public IDistrictService districtService { get; set; }
        public OrderController(ICategoryService categoryService, IOrderService orderService, IStatusService statusService, IPaymentService paymentService,
            INationalService nationalService, IProvinceService provinceService, IDistrictService districtService, IOrderInfoService orderInfoService)
        {
            this.categoryService = categoryService;
            this.orderService = orderService;
            this.statusService = statusService;
            this.paymentService = paymentService;
            this.nationalService = nationalService;
            this.provinceService = provinceService;
            this.districtService = districtService;
            this.orderInfoService = orderInfoService;
        }
        public ActionResult Index()
        {
            List<Order> list = orderService.All.ToList();
            IEnumerable<National> nationals = nationalService.All.Where(r => r.IsActive).OrderByDescending(r => r.Order);
            ViewBag.NationalList = nationals.ToSelectListItems(-1);
            List<Province> provinces = new List<Province>();
            ViewBag.ProvinceList = provinces.ToSelectListItems(-1);

            List<District> districts = new List<District>();
            ViewBag.DistrictList = districts.ToSelectListItems(-1);
            return View(list);
        }
        [HttpPost]
        [ActionName("Index")]
        [AcceptButton(ButtonName = "Delete")]
        public ActionResult IndexDelete(int[] checkedRecords, string key, string NationalList, string ProvinceList, string DistrictList)
        {
            checkedRecords = checkedRecords ?? new int[] { };
            if (checkedRecords != null && checkedRecords.Length > 0)
            {
                foreach (int item in checkedRecords)
                {
                    Order order = orderService.Find(item);
                    if (order != null)
                    {
                        List<OrderInfo> lst = orderInfoService.All.Where(o => o.OrderID == order.OrderID).ToList();
                        foreach (OrderInfo oritem in lst)
                        {
                            orderInfoService.Delete(oritem);
                            orderInfoService.Save();
                        }
                        orderService.Delete(order);
                        orderService.Save();
                    }
                }
            }
            RouteValueDictionary routeValues = this.GridRouteValues();
            return RedirectToAction("Index", routeValues);
        }

        [HttpPost]
        [ActionName("Index")]
        [AcceptButton(ButtonName = "Search")]
        public ActionResult IndexSearch(string key, string NationalList, string ProvinceList, string DistrictList)
        {
            var list = orderService.All;
            int NationalID = Protector.Int(NationalList);
            int ProvinceID = Protector.Int(ProvinceList);
            int DistrictID = Protector.Int(DistrictList);
            if (!string.IsNullOrEmpty(key))
            {
                list = list.Where(r => r.FullName.Contains(key) || r.OrderCode.Contains(key) || r.Phone.Contains(key) || r.Address.Contains(key));
            }
            if (NationalID > 0)
            {
                list = list.Where(r => r.NationalID == NationalID);

            }
            if (ProvinceID > 0)
            {
                list = list.Where(r => r.ProvinceID == ProvinceID);

            }
            if (DistrictID > 0)
            {
                list = list.Where(r => r.DistrictID == DistrictID);

            }
            ViewBag.Key = key;
            IEnumerable<National> nationals = nationalService.All.Where(r => r.IsActive).OrderByDescending(r => r.Order);
            ViewBag.NationalList = nationals.ToSelectListItems(NationalID);
            IEnumerable<Province> provinces = provinceService.All.Where(r => r.IsActive && r.NationalID == NationalID).OrderByDescending(r => r.Order);
            ViewBag.ProvinceList = provinces.ToSelectListItems(ProvinceID);

            IEnumerable<District> districts = districtService.All.Where(r => r.IsActive && r.ProvinceID == ProvinceID).OrderByDescending(r => r.Order);
            ViewBag.DistrictList = districts.ToSelectListItems(DistrictID);
            return View(list.ToList());
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            int orderID = Protector.Int(id);
            var order = new EditOrderModel();
            Order current = orderService.Find(orderID);
            if (current != null)
            {
                ModelCopier.CopyModel(current, order);
                order.OrderInfoList = current.OrderInfos.ToList();
                order.Hotel = current.Hotel;
                order.Payments = paymentService.All.Where(p => p.IsActive).ToList();
                order.StatusList = statusService.All.Where(s => s.IsActive).ToSelectListItems(current.StatusID);
                return View(order);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Edit(string id, EditOrderModel current)
        {
            int orderID = Protector.Int(id);
            Order order = orderService.Find(orderID);
            if (!ModelState.IsValid)
            {
                ModelCopier.CopyModel(order, current);
                current.OrderInfoList = order.OrderInfos.ToList();
                current.Hotel = current.Hotel;
                current.Payments = paymentService.All.Where(p => p.IsActive).ToList();
                current.StatusList = statusService.All.Where(s => s.IsActive).ToSelectListItems(current.StatusID);
                return View(current);
            }

            if (TryUpdateModel(order))
            {
                orderService.Save();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        #region Json Action
        public ActionResult SelectProvince(string nationalID)
        {
            List<Province> list = new List<Province>();
            int nID = Protector.Int(nationalID);
            list = provinceService.All.Where(p => p.NationalID == nID).OrderByDescending(p => p.Order).ToList();
            return Json(list.ToSelectListItems(-1), JsonRequestBehavior.AllowGet);
        }
        public ActionResult SelectDistrict(string provinceID)
        {
            List<District> list = new List<District>();
            int pr = Protector.Int(provinceID);
            list = districtService.All.Where(p => p.ProvinceID == pr).OrderByDescending(p => p.Order).ToList();
            return Json(list.ToSelectListItems(-1), JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}
