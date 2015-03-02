using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Domain.Entities;
using Service.Attribute;
using System.Web.Routing;
using Telerik.Web.Mvc.Extensions;
using Service.Helpers;
using Microsoft.Web.Mvc;

namespace Website.Areas.Ticket.Controllers
{
    [Authorize]
    public class TicketTypeController : Controller
    {
        public ITicketTypeService ticketTypeService { get; set; }
        public TicketTypeController(ITicketTypeService ticketTypeService)
        {
            this.ticketTypeService = ticketTypeService;
        }
        #region HttpGet
        public ActionResult Index()
        {
            List<TicketType> list = ticketTypeService.All.ToList();
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
                foreach (int item in checkedRecords)
                {
                    TicketType ticketType = ticketTypeService.Find(item);
                    if (ticketType != null)
                    {

                        ticketTypeService.Delete(ticketType);
                        ticketTypeService.Save();
                    }
                }
            }
            RouteValueDictionary routeValues = this.GridRouteValues();
            return RedirectToAction("Index", routeValues);
        }

        [HttpPost]
        [ActionName("Index")]
        [AcceptButton(ButtonName = "Search")]
        public ActionResult IndexSearch(string key)
        {
            var list = ticketTypeService.All;
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
            var ticketType = new TicketType();
            ticketType.IsActive = true;

            return View(ticketType);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            int ticketTypeID = Protector.Int(id);
            TicketType info = ticketTypeService.Find(ticketTypeID);
            if (info != null)
            {

                return View(info);
            }
            return RedirectToAction("Index");
        }
        #endregion
        #region HttpPost

        [HttpPost]
        public ActionResult Create(TicketType ticketType)
        {
            if (!ModelState.IsValid)
            {
                return View(ticketType);
            }
            TicketType info = ticketTypeService.Get(r => r.Name == ticketType.Name);
            if (info != null)
            {
                ModelState.AddModelError("", string.Format("Tên loại vé [{0}] đã hiện hữu vui lòng chọn tên khác", ticketType.Name));
                return View(ticketType);
            }
            ticketType.CreatedDate = DateTime.Now;
            ticketType.CreatedBy = Protector.Int(HttpContext.User.Identity.Name);
            ticketTypeService.Insert(ticketType);
            ticketTypeService.Save();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(TicketType current, string id)
        {
            int ticketTypeID = Protector.Int(id);
            TicketType ticketType = ticketTypeService.Get(r => r.Name == current.Name && r.TicketTypeID != ticketTypeID);
            if (ticketType != null)
            {
                ModelState.AddModelError("", string.Format("Tên loại vé [{0}] đã hiện hữu vui lòng chọn tên khác", current.Name));
                return View(current);
            }
            else
            {
                ticketType = ticketTypeService.Find(ticketTypeID);
                if (TryUpdateModel(ticketType))
                {
                    ticketTypeService.Save();
                    return RedirectToAction("Index");
                }
                else return View(current);
            }
        }




        #endregion

    }
}
