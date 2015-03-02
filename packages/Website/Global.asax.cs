using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Website
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
          
            routes.MapRoute("Category", "{name}/{id}/c", new { controller = "Category", action = "Index", name = UrlParameter.Optional, id = UrlParameter.Optional }, new string[] { "Website.Controllers" });
            routes.MapRoute("Contact", "{name}/{id}/ct", new { controller = "Contact", action = "Index", name = UrlParameter.Optional, id = UrlParameter.Optional }, new string[] { "Website.Controllers" });

            routes.MapRoute("Product", "{name}/{id}/pl", new { controller = "Product", action = "Index", name = UrlParameter.Optional, id = UrlParameter.Optional }, new string[] { "Website.Controllers" });

            routes.MapRoute("New", "{CatName}/{name}/{newID}/{id}/n", new { controller = "New", action = "Detail", name = UrlParameter.Optional, CatName = UrlParameter.Optional, newID = UrlParameter.Optional, id = UrlParameter.Optional }, new string[] { "Website.Controllers" });

            routes.MapRoute("Province", "{nationalName}/{provinceName}/{p}/{n}/p", new { controller = "Search", action = "Index", name = UrlParameter.Optional, nationalName = UrlParameter.Optional, provinceName = UrlParameter.Optional, p = UrlParameter.Optional, n = UrlParameter.Optional }, new string[] { "Website.Controllers" });

            routes.MapRoute("HotelDetail", "{provinceName}/{hotelName}/{h}/{p}/h", new { controller = "Hotel", action = "Detail", provinceName = UrlParameter.Optional, hotelName = UrlParameter.Optional, h = UrlParameter.Optional, p = UrlParameter.Optional }, new string[] { "Website.Controllers" });

            routes.MapRoute("HomeHotel", "khach-san", new { controller = "Hotel", action = "Index", id = 2 }, new string[] { "Website.Controllers" });
            routes.MapRoute("Order", "{provinceName}/{hotelName}/{h}/{p}/booking", new { controller = "Order", action = "Index", provinceName = UrlParameter.Optional, hotelName = UrlParameter.Optional, h = UrlParameter.Optional, p = UrlParameter.Optional }, new string[] { "Website.Controllers" });

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "Website.Controllers" });




        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            Bootstrapper.Initialise();
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}