using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace PersonalSite
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            
            if(Convert.ToBoolean(ConfigurationManager.AppSettings["RequireSSL"]))
                filters.Add(new RequireHttpsAttribute());
        }
    }
}