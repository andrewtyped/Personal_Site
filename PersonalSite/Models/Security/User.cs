using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace PersonalSite.Models
{
    public class User : IPrincipal
    {
        protected User() { }

        public User(int userId, string userName, string email, string password, Role role)
        {
            UserId = userId;
            UserName = userName;
            Email = email;
            Password = password;
            Role = role;
        }

        public virtual int UserId { get; set; }

        public virtual string UserName { get; set; }

        public virtual string Email { get; set; }

        public virtual string Password { get; set; }

        public virtual IIdentity Identity { get; set; }

        public virtual Role Role {get; set;}

        public virtual bool IsInRole(string role)
        {
            bool isInRole = false;

            if (string.Equals(Role.RoleName, role, StringComparison.OrdinalIgnoreCase))
            {
                isInRole = true;
            }

            return isInRole;
        }
    }
}