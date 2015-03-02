using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Service.Helpers;
using System.Web.Routing;
using Domain.Entities;

namespace Website.Controllers
{
    public class AreaController : Controller
    {
        public IAreaService areaService { get; set; }
        public IDistrictService districtService { get; set; }
        public AreaController(IAreaService areaService, IDistrictService districtService)
        {
            this.areaService = areaService;
            this.districtService = districtService;
        }

        #region Partial
        [HttpGet]
        public ActionResult Areas(string p, string d, string a, string c, string ht, string type, string hp, string fd, string td, string nr, string ng)
        {
            int provinceID = Protector.Int(p);
            if (provinceID > 0)
            {
                Website.Models.Area.AreasModel areas = new Models.Area.AreasModel();
                if (!string.IsNullOrEmpty(a))
                {
                    areas.ListAreaID = a.Split('|');
                }
                if (!string.IsNullOrEmpty(d))
                {
                    string[] listID = d.Split('|');
                    if (listID != null && listID.Length > 0)
                    {
                        areas.DistrictList = new List<District>();
                        foreach (string item in listID)
                        {
                            int DistrictID = Protector.Int(item);
                            District info = districtService.Find(DistrictID);
                            if (info != null)
                            {

                                areas.DistrictList.Add(info);
                            }
                        }
                    }
                }
                areas.ProvinceID = provinceID;
                areas.DistrictID = Protector.String(d);
                areas.ComfortID = Protector.String(c);
                areas.HotelThemeID = Protector.String(ht);
                areas.HotelTypeID = Protector.String(type);
                areas.HotelPromotion = Protector.String(hp);
                areas.FromDate = Protector.String(fd);
                areas.ToDate = Protector.String(td);
                areas.NumberRoom = Protector.String(nr);
                areas.NumberGuest = Protector.String(ng);
                return PartialView("Areas", areas);
            }
            return PartialView("Areas");
        }
        [HttpPost]
        public ActionResult Areas(string p, string d, string a, string c, string ht, string type, string hp, string fd, string td, string nr, string ng, string[] AreaID)
        {

            int provinceID = Protector.Int(p);
            if (provinceID > 0)
            {
                RouteValueDictionary obj = new RouteValueDictionary();
                obj.Add("p", provinceID);
                string areaID = "";
                if (AreaID != null && AreaID.Length > 0)
                {
                    foreach (string str in AreaID)
                    {
                        areaID += str + "|";
                    }
                    obj.Add("a", areaID.Replace("|", " ").Trim().Replace(" ", "|"));

                }
                if (!string.IsNullOrEmpty(d))
                {
                    obj.Add("d", d);
                }
                if (!string.IsNullOrEmpty(c))
                {
                    obj.Add("c", c);
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

            return PartialView("Districts");
        }
        #endregion

    }
}
