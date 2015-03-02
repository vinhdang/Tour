using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Domain.Entities;
using Service.Helpers;
using System.Web.Routing;
using Website.Helpers;
namespace Website.Controllers
{
    public class DistrictController : Controller
    {
        public IDistrictService districtService { get; set; }
        public DistrictController(IDistrictService districtService)
        {
            this.districtService = districtService;
        }
        public ActionResult Index(string p)
        {
            return View();
        }
        #region Partial
        [HttpGet]
        public ActionResult Districts(string p, string d, string a, string c, string ht, string type, string hp, string fd, string td, string nr, string ng)
        {
            int provinceID = Protector.Int(p);
            if (provinceID > 0)
            {
                Website.Models.District.DistrictsModel districts = new Models.District.DistrictsModel();
                if (!string.IsNullOrEmpty(d))
                {
                    districts.ListDistrictID = d.Split('|');
                }

                var list = districtService.All.Where(dt => dt.IsActive && dt.ProvinceID == provinceID).ToList();
                districts.DistrictList = list;
                districts.ProvinceID = provinceID;
                districts.AreaID = Protector.String(a);
                districts.ComfortID = Protector.String(c);
                districts.HotelThemeID = Protector.String(ht);
                districts.HotelTypeID = Protector.String(type);
                districts.HotelPromotion = Protector.String(hp);
                districts.FromDate = Protector.String(fd);
                districts.ToDate = Protector.String(td);
                districts.NumberRoom = Protector.String(nr);
                districts.NumberGuest = Protector.String(ng);
                return PartialView("Districts", districts);
            }
            return PartialView("Districts");
        }
        [HttpPost]
        public ActionResult Districts(string p, string c, string ht, string type, string hp, string fd, string td, string nr, string ng, string[] DistrictID)
        {
            int provinceID = Protector.Int(p);
            if (provinceID > 0)
            {
                RouteValueDictionary obj = new RouteValueDictionary();
                obj.Add("p", provinceID);
                string districtID = "";
                if (DistrictID != null && DistrictID.Length > 0)
                {
                    foreach (string str in DistrictID)
                    {
                        districtID += str + "|";
                    }

                    obj.Add("d", districtID.Replace("|", " ").Trim().Replace(" ", "|"));
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
