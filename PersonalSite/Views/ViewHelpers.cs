using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonalSite.Views
{
    public static class ViewHelpers
    {
        public static MvcHtmlString If(this MvcHtmlString value, bool evaluation)
        {
            return evaluation ? value : MvcHtmlString.Empty;
        }

        public static string PrettifyDateTime(this DateTime value)
        {
            return value.ToString("dddd, MMMM dd yyyy", CultureInfo.CreateSpecificCulture("en-US"));
        }
    }
}