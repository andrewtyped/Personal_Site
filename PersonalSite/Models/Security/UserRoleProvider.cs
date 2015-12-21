using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace PersonalSite.Models
{
public class UserRoleProvider : RoleProvider
   {
       private IUserDataAccess repository;

       public UserRoleProvider()
           : this(new SqlUserDataAccess()) 
       {
            
       }

       public UserRoleProvider(IUserDataAccess repository) : base()
        {
            this.repository = repository ?? new SqlUserDataAccess();
        }

       public override bool IsUserInRole(string username, string roleName)
       {
           User user = repository.GetUserByName(username);
           if(user!=null)
                return user.IsInRole(roleName);
           else
               return false;
       }

       public override string ApplicationName
       {
           get
           {
               throw new NotImplementedException();
           }
           set
           {
               throw new NotImplementedException();
           }
       }
       public override void AddUsersToRoles(string[] usernames, string[] roleNames)
       {
            throw new NotImplementedException();
        }
       public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
       {
           throw new NotImplementedException();
       }
       public override void CreateRole(string roleName)
       {
           throw new NotImplementedException();
       }
       public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
       {
           throw new NotImplementedException();
       }
       public override bool RoleExists(string roleName)
       {
           throw new NotImplementedException();
       }
       public override string[] GetRolesForUser(string username)
       {
           User user = repository.GetUserByName(username);

           string[] roles = new string[1];

           roles[0] = user.Role.RoleName;

           return roles;
       }
       public override string[] GetUsersInRole(string roleName)
       {
           throw new NotImplementedException();
       }
       public override string[] FindUsersInRole(string roleName, string usernameToMatch)
       {
           throw new NotImplementedException();
       }
       public override string[] GetAllRoles()
       {
           throw new NotImplementedException();
       }
   }
}