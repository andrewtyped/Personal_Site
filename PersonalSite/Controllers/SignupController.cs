using DataAccess;
using PersonalSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PersonalSite.Controllers
{
    public class SignupController : Controller
    {
        private UserMemberProvider provider = (UserMemberProvider)Membership.Provider;

        private IUserDataAccess dataAccess;

        public SignupController()
            :this(new SqlUserDataAccess())
        {
        }

        public SignupController(IUserDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        //[RequireHttps]
        public ActionResult CreateUser(string username, string email, string password, string confirmPassword)
        {
            StringResult result = ValidateNewUser(username, email, password, confirmPassword);

            if(result.Success)
            {
                var user = provider.CreateUser(username, password, email);
                result = new StringResult(true, "Signup successful! Welcome to the site.");
            }

            return Json(result);
        }

        private StringResult ValidateNewUser(string username, string email, string password, string confirmPassword)
        {
            StringResult result;

            result = validateUsername(username);

            if (!result.Success)
            {
                return result;
            }

            result = validateEmail(email);

            if (!result.Success)
            {
                return result;
            }

            result = validatePassword(password, confirmPassword);

            return result;
        }

        private StringResult validateUsername(string username)
        {
            StringResult result;

            if (username.Length < 6)
            {
                result = new StringResult(false, "User name must be at least six characters");
            }
            else if (UserExists(username))
            {
                result = new StringResult(false, "This user name is already taken.");
            }
            else
            {
                result = new StringResult(true, "");
            }

            return result;
        }

        private bool UserExists(string userName)
        {
            var user = dataAccess.GetUserByName(userName);

            if (user.UserName != "FAIL")
            {
                return true;
            }

            return false;
        }

        private StringResult validateEmail(string email)
        {
            StringResult result;

            if (!email.Contains("@") || !email.Contains("."))
            {
                result = new StringResult(false, "Invalid email");
            }
            else
            {
                result = new StringResult(true, "");
            }

            return result;
        }

        private StringResult validatePassword(string password, string confirmPassword)
        {
            StringResult result;

            if (password.Length < 8)
            {
                result = new StringResult(false, "Password must be at least 8 characters");
            }
            else if (!string.Equals(password, confirmPassword))
            {
                result = new StringResult(false, "These passwords don't match");
            }
            else
            {
                result = new StringResult(true, "");
            }

            return result;
        }

    }
}
