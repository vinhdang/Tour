using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Domain.Entities;
using Website.Models.Comfort;
using Service.Helpers;
using System.Web.Routing;

namespace Website.Controllers
{
    public class ComfortController : Controller
    {
        public IComfortService comfortService { get; set; }
        public ComfortController(IComfortService comfortService)
        {
            this.comfortService = comfortService;
        }
        public ActionResult Comforts(string p, string d, string a, string c, string ht, string type, string hp, string fd, string td, string nr, string ng)
        {
            int provinceID = Protector.Int(p);
            if (provinceID > 0)
            {
                Website.Models.Comfort.ComfortModel comforts = new ComfortModel();
                string[] comfortID = null;
                if (!string.IsNullOrEmpty(c))
                {
                    comfortID = c.Split('|');
                }
                else { comfortID = new string[] { }; }
                comforts.Comforts = comfortService.All.Where(cf => cf.IsActive).OrderByDescending(cf => cf.Order).ToList();
                comforts.ProvinceID = provinceID;
                comforts.ListComfortID = comfortID;
                comforts.DistrictID = Protector.String(d);
                comforts.AreaID = Protector.String(a);
                comforts.HotelThemeID = Protector.String(ht);
                comforts.HotelTypeID = Protector.String(type);
                comforts.HotelPromotion = Protector.String(hp);
                comforts.FromDate = Protector.String(fd);
                comforts.ToDate = Protector.String(td);
                comforts.NumberRoom = Protector.String(nr);
                comforts.NumberGuest = Protector.String(ng);
                return PartialView("Comforts", comforts);
            }
            return PartialView("Comforts");
        }
        [HttpPost]
        public ActionResult Comforts(string p, string d, string a, string ht, string type, string hp, string fd, string td, string nr, string ng, string[] ComfortID)
        {

            int provinceID = Protector.Int(p);
            if (provinceID > 0)
            {
                RouteValueDictionary obj = new RouteValueDictionary();
                obj.Add("p", provinceID);
                string comfortID = "";
                if (ComfortID != null && ComfortID.Length > 0)
                {
                    foreach (string str in ComfortID)
                    {
                        comfortID += str + "|";
                    }
                    obj.Add("c", comfortID.Replace("|", " ").Trim().Replace(" ", "|"));

                }
                if (!string.IsNullOrEmpty(d))
                {
                    obj.Add("d", d);
                }
                if (!string.IsNullOrEmpty(a))
                {
                    obj.Add("a", a);
                }
                if (!string.IsNullOrEmpty(ht))
                {
                    obj.Add("ht", ht);
                }
                if (!string.IsNullOrEmpty(type))
                {
                    obj.Add("type", type);
                }
                if (!string.IsNullOrEmpty(hp))
                {
                    obj.Add("hp", hp);
                }
                if (!string.IsNullOrEmpty(fd))
                {
                    obj.Add("fd", fd);
                }
                if (!string.IsNullOrEmpty(td))
                {
                    obj.Add("td", td);
                }
                if (!string.IsNullOrEmpty(nr))
                {
                    obj.Add("nr", nr);
                }
                if (!string.IsNullOrEmpty(ng))
                {
                    obj.Add("ng", ng);
                }
                string url = Url.Action("Index", "Search", obj);
                return Redirect(url);
            }
            return PartialView("Comforts");
        }
        public ActionResult ComfortList(string listComfort)
        {
            List<SelectListItem> comfortList = new List<SelectListItem>();
            List<Comfort> list = new List<Comfort>();
            string[] comfortID = null;
            if (!string.IsNullOrEmpty(listComfort))
            {
                comfortID = listComfort.Split(';');
            }
            else { comfortID = new string[] { }; }
            list = comfortService.All.Where(c => c.IsActive).OrderByDescending(c => c.Order).ToList();
            foreach (Comfort item in list)
            {

                if (comfortID.Contains(item.ComfortID.ToString()))
                {
                    comfortList.Add(new SelectListItem() { Selected = true, Text = item.Name, Value = item.ComfortID.ToString() });
                }
                else
                {
                    comfortList.Add(new SelectListItem() { Selected = false, Text = item.Name, Value = item.ComfortID.ToString() });
                }
            }
            return PartialView("ComfortList", comfortList);
        }
    }
}
