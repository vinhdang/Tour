using System.Web.Mvc;

namespace Website.Areas.Ticket
{
    public class TicketAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Ticket";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
"Ticket",
"Ticket",
new { controller = "Ticket", action = "Index" },
new[] { "Website.Areas.Ticket.Controllers" }
);
            context.MapRoute(
                "Ticket_default",
                "Ticket/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
