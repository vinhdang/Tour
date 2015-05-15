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
            routes.MapRoute("Province", "{nationalName}/{provinceName}/p", new { controller = "Province", action = "Index", nationalName = UrlParameter.Optional, provinceName = UrlParameter.Optional }, new string[] { "Website.Controllers" });
            routes.MapRoute("Foreign", "{nationalName}/{provinceName}/f", new { controller = "Foreign", action = "Index", nationalName = UrlParameter.Optional, provinceName = UrlParameter.Optional }, new string[] { "Website.Controllers" });
            routes.MapRoute("Tour", "{nationalName}/{provinceName}/{tourName}/t/", new { controller = "Tour", action = "Index", nationalName = UrlParameter.Optional, provinceName = UrlParameter.Optional, tourName = UrlParameter.Optional }, new string[] { "Website.Controllers" });
            routes.MapRoute("Login", "site/login", new { controller = "User", action = "LogOn" }, new string[] { "Website.Controllers" });
            routes.MapRoute("/huong-dan", "huong-dan", new { controller = "Category", action = "Detail", id = 2 }, new string[] { "Website.Controllers" });
            routes.MapRoute("/lien-he", "lien-he", new { controller = "Contact", action = "Index", id = 3 }, new string[] { "Website.Controllers" });
            routes.MapRoute("Booking", "{nationalName}/{provinceName}/{hotelName}/{hotelID}/{provinceID}/booking", new { controller = "Order", action = "Index", nationalName = UrlParameter.Optional, provinceName = UrlParameter.Optional, hotelName = UrlParameter.Optional, hotelID = UrlParameter.Optional, provinceID = UrlParameter.Optional }, new string[] { "Website.Controllers" });
            routes.MapRoute("Order", "{nationalName}/{provinceName}/{hotelName}/{orderID}/r", new { controller = "Order", action = "Result", nationalName = UrlParameter.Optional, provinceName = UrlParameter.Optional, hotelName = UrlParameter.Optional, orderID = UrlParameter.Optional }, new string[] { "Website.Controllers" });
            routes.MapRoute("Recruiment", "tuyen-dung", new { controller = "Account", action = "Recruitment"}, new string[] { "Website.Controllers" });
            routes.MapRoute("CompanyPolicy", "noi-quy", new { controller = "Account", action = "CompanyPolicy" }, new string[] { "Website.Controllers" });

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                //new { controller = "Administrator", action = "Index", id = UrlParameter.Optional },
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