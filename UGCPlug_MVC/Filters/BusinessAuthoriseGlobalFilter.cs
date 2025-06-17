using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UGCPlug_MVC.Filters
{
    public class BusinessAuthoriseGlobalFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var action = filterContext.ActionDescriptor.ActionName;
            var session = filterContext.HttpContext.Session;

            // Allow access to public controllers/actions
            var publicControllers = new[] { "Auth", "Home" };
            var publicActions = new[] { "Index", "HowItWorks", "Contact", "Login", "Register" };

            if (publicControllers.Contains(controller, StringComparer.OrdinalIgnoreCase) &&
                publicActions.Contains(action, StringComparer.OrdinalIgnoreCase))
            {
                return;
            }

            // Redirect to login if session is missing
            if (session["BusinessId"] == null)
            {
                filterContext.Result = new RedirectResult("/Auth/Login");
            }
        }



        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // Nothing needed here
        }
    }
}