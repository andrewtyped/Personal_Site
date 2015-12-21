using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace PersonalSite.Globals
{
    public static class ConfigTestability
    {
        public static bool SqlConnectionIsNamedAppropriately()
        {
            try
            {
                //When called from test project, the test project's app config is being referenced,
                //NOT the web.config of the class' home project.
                string connection = ConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}