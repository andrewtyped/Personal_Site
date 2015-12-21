using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalSite.Models
{
    public interface IUserDataAccess
    {
        User GetUserByName(string userName);
        int GetUserCount();
        User CreateUser(string userName, string password, string email, string role);
    }
}
