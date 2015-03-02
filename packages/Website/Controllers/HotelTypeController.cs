using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.IServices;
using Service.Helpers;
using System.Web.Routing;

namespace Website.Controllers
{
    public class HotelTypeController : Controller
    {
        public IHotelTypeService hotelTypeService { get; set; }
        public HotelTypeController(IHotelTypeService hotelTypeService)
        {
            this.hotelTypeService = hotelTypeService;
        }

        public ActionResult HotelTypes(string p, string d, string a, string c, string ht, string type, string hp, string fd, string td, string nr, string ng)
        {
            int provinceID = Protector.Int(p);
            if (provinceID > 0)
            {

                Website.Models.HotelType.HotelTypeModel hotelTypes = new Models.HotelType.HotelTypeModel();
                var list = hotelTypeService.All.Where(dt => dt.IsActive).OrderByDescending(t => t.Order).ToList();
                if (!string.IsNullOrEmpty(type))
                {
                    hotelTypes.ListHotelTypeID = type.Split('|');
                }
                hotelTypes.HotelTypeList = list;
                hotelTypes.ProvinceID = provinceID;
                hotelTypes.DistrictID = Protector.String(d);
                hotelTypes.AreaID = Protector.String(a);
                hotelTypes.ComfortID = Protector.String(c);
                hotelTypes.HotelThemeID = Protector.String(ht);
                hotelTypes.HotelPromotion = Protector.String(hp);
                hotelTypes.FromDate = Protector.String(fd);
                hotelTypes.ToDate = Protector.String(td);
                hotelTypes.NumberRoom = Protector.String(nr);
                hotelTypes.NumberGuest = Protector.String(ng);
                return PartialView("HotelTypes", hotelTypes);
            }

            return PartialView("HotelTypes");
        }
        [HttpPost]
        public ActionResult HotelTypes(string p, string d, string a, string c, string ht, string hp, string fd, string td, string nr, string ng, string[] HotelTypeID)
        {
            int provinceID = Protector.Int(p);
            if (provinceID > 0)
            {
                RouteValueDictionary obj = new RouteValueDictionary();
                obj.Add("p", provinceID);
                string hotelTypeID = "";
                if (HotelTypeID != null && HotelTypeID.Length > 0)
                {
                    foreach (string str in HotelTypeID)
                    {
                        hotelTypeID += str + "|";
                    }

                    obj.Add("type", hotelTypeID.Replace("|", " ").Trim().Replace(" ", "|"));
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
                if (!string.IsNullOrEmpty(ht))
                {
                    obj.Add("ht", ht);
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

            return PartialView("HotelTypes");
        }

    }
}
