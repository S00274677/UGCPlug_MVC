using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UGCPlug_MVC.Filters
{
    public class NoCacheAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var response = filterContext.HttpContext.Response;

            response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.Cache.SetNoStore();
            response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);

            response.AppendHeader("Pragma", "no-cache");
            response.AppendHeader("Cache-Control", "no-store, no-cache, must-revalidate, max-age=0");
            response.AppendHeader("Expires", "0");

            base.OnResultExecuting(filterContext);
        }

    }
}