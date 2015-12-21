using DataAccess;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace PersonalSite.Globals
{
    public static class Data
    {
        public static IDataAccess Sql {get; private set ;}

        static Data()
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["UseProductionDatabase"]))
                Sql = new SqlDataAccess(ConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString);
            else
                Sql = new SqlDataAccess(ConfigurationManager.ConnectionStrings["testConnection"].ConnectionString);
        }
    }
}