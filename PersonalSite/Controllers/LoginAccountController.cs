using PersonalSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PersonalSite.Controllers
{
    public class LoginAccountController : Controller
    {
        private IUserDataAccess dataAccess;
        private UserMemberProvider provider = (UserMemberProvider)Membership.Provider;

        public LoginAccountController()
            :this(new SqlUserDataAccess())
        {
        }

        public LoginAccountController(IUserDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public ActionResult Login()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LogOn(string userName, string password, string returnUrl)
        {
            if (!ValidateLogOn(userName, password))
            {
                return View("Login");
            }

            var user = dataAccess.GetUserByName(userName);

            FormsAuthentication.SetAuthCookie(user.UserName, false);

            if (!String.IsNullOrEmpty(returnUrl) && returnUrl != "/")
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private bool ValidateLogOn(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName))
            {
                ModelState.AddModelError("Login", "You must specify a username.");
            }
            else if (String.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("Login", "You must specify a password.");
            }
            else if (!provider.ValidateUser(userName, password))
            {
                ModelState.AddModelError("Login", "The username or password provided is incorrect.");
            }
            return ModelState.IsValid;
        }

    }
}
