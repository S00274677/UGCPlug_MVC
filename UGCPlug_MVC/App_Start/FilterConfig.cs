using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UGCPlug_MVC.Filters;

namespace UGCPlug_MVC.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new BusinessAuthoriseGlobalFilter());
            filters.Add(new HandleErrorAttribute());

        }
    }
}