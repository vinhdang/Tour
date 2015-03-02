using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Microsoft.Practices.ServiceLocation;

namespace Website.Log
{
    public class LogErrorsAttribute : FilterAttribute, IExceptionFilter
    {
        #region IExceptionFilter Members
        void IExceptionFilter.OnException(ExceptionContext filterContext)
        {
            if (filterContext != null && filterContext.Exception != null)
            {
                string controller = filterContext.RouteData.Values["controller"].ToString();
                string action = filterContext.RouteData.Values["action"].ToString();
                string loggerName = string.Format("{0}Controller.{1}", controller, action);
                string userID = filterContext.HttpContext.User.Identity.Name.ToString();
                DateTime timeStamp = filterContext.HttpContext.Timestamp;
                string Id = string.Empty;
                if (filterContext.RouteData.Values["id"] != null)
                {
                    Id = filterContext.RouteData.Values["id"].ToString();
                }
                StringBuilder message = new StringBuilder();
                message.Append("UserName=");
                message.Append(userID + "|");
                message.Append("Controller=");
                message.Append(controller + "|");
                message.Append("Action=");
                message.Append(action + "|");
                message.Append("TimeStamp=");
                message.Append(timeStamp.ToString() + "|");
                message.Append("Error=");
                message.Append(filterContext.Exception.Message + "|");
                if (!string.IsNullOrEmpty(Id))
                {
                    message.Append("RouteId=");
                    message.Append(Id);
                }
                var log = ServiceLocator.Current.GetInstance<ILoggingService>();
                log.Error(message.ToString());
            }

        }

        #endregion
    }
}