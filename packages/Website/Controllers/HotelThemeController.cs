using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Service.Helpers;
using Website.Models.HotelTheme;
using System.Web.Routing;

namespace Website.Controllers
{
    public class HotelThemeController : Controller
    {
        public IHotelThemeService hotelThemeService { get; set; }
        public HotelThemeController(IHotelThemeService hotelThemeService)
        {
            this.hotelThemeService = hotelThemeService;
        }


        public ActionResult HotelThemes(string p, string d, string a, string c, string ht, string type, string hp, string fd, string td, string nr, string ng)
        {
            int provinceID = Protector.Int(p);
            if (provinceID > 0)
            {
                Website.Models.HotelTheme.HotelThemeModel hotelTheme = new HotelThemeModel();
                string[] hotelThemeID = null;
                if (!string.IsNullOrEmpty(ht))
                {
                    hotelThemeID = ht.Split('|');
                }
                else
                {
                    hotelThemeID = new string[] { };
                }
                hotelTheme.HotelThemeList = hotelThemeService.All.Where(cf => cf.IsActive && cf.ProvinceID == provinceID).OrderByDescending(cf => cf.Order).ToList();
                hotelTheme.ProvinceID = provinceID;
                hotelTheme.ListHotelThemeID = hotelThemeID;
                hotelTheme.DistrictID = Protector.String(d);
                hotelTheme.AreaID = Protector.String(a);
                hotelTheme.ComfortID = Protector.String(c);
                hotelTheme.HotelTypeID = Protector.String(type);
                hotelTheme.HotelPromotion = Protector.String(hp);
                hotelTheme.FromDate = Protector.String(fd);
                hotelTheme.ToDate = Protector.String(td);
                hotelTheme.NumberRoom = Protector.String(nr);
                hotelTheme.NumberGuest = Protector.String(ng);
                return PartialView("HotelThemes", hotelTheme);
            }
            return PartialView("HotelThemes");
        }
        [HttpPost]
        public ActionResult HotelThemes(string p, string d, string a, string c, string type, string hp, string fd, string td, string nr, string ng, string[] HotelThemeID)
        {
            int provinceID = Protector.Int(p);
            if (provinceID > 0)
            {
                RouteValueDictionary obj = new RouteValueDictionary();
                obj.Add("p", provinceID);
                string hotelThemeID = "";
                if (HotelThemeID != null && HotelThemeID.Length > 0)
                {
                    foreach (string str in HotelThemeID)
                    {
                        hotelThemeID += str + "|";
                    }
                    obj.Add("ht", hotelThemeID.Replace("|", " ").Trim().Replace(" ", "|"));

                }
                if (!string.IsNullOrEmpty(d))
                {
                    obj.Add("d", d);
                }
                if (!string.IsNullOrEmpty(a))
                {
                    obj.Add("a", a);
                }
                if (!string.IsNullOrEmpty(c))
                {
                    obj.Add("c", c);
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
            return PartialView("HotelThemes");
        }
    }
}
