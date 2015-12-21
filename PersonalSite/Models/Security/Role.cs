using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalSite.Models
{
    public class Role
    {
        public string RoleName { get; set; }

        public List<string> Rights { get; set; }

        public Role(string roleName)
        {
            RoleName = roleName;
        }
    }
}