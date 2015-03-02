using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Service.Attribute;
using System.Web.Routing;
using Telerik.Web.Mvc.Extensions;
using Domain.Entities;
using Website.Areas.Ticket.Models.Ticket;

namespace Website.Areas.Ticket.Controllers
{
    public class TicketController : Controller
    {
        public IAirlineService airlineService { get; set; }
        public ISeatTypeService seatTypeService { get; set; }
        public ITicketTypeService ticketTypeService { get; set; }
        public ITicketService ticketService { get; set; }
        public TicketController(IAirlineService airlineService, ISeatTypeService seatTypeService, ITicketTypeService ticketTypeService, ITicketService ticketService)
        {
            this.airlineService = airlineService;
            this.seatTypeService = seatTypeService;
            this.ticketTypeService = ticketTypeService;
            this.ticketService = ticketService;
        }
        #region HttpGet
        public ActionResult Index()
        {
            List<Domain.Entities.Ticket> list = ticketService.All.ToList();
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
                    Domain.Entities.Ticket ticket = ticketService.Find(item);
                    if (ticket != null)
                    {

                        ticketService.Delete(ticket);
                        ticketService.Save();
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
            var list = ticketService.All;

            ViewBag.Key = key;
            return View(list.ToList());
        }
        [HttpGet]
        public ActionResult Create()
        {
            CreateTicketModel info = new CreateTicketModel();
            info.IsActive = true;
            return View(info);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {

            return RedirectToAction("Index");
        }
        #endregion

    }
}
