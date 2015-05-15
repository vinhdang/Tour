using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Reflection;

namespace Service.Attribute
{
    public class AcceptButtonAttribute : ActionMethodSelectorAttribute
    {
        public string ButtonName { get; set; }

        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            var req = controllerContext.RequestContext.HttpContext.Request;
            return !string.IsNullOrEmpty(req.Form[this.ButtonName]);
        }
    }
}
