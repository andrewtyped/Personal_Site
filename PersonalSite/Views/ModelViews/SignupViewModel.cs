﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalSite.Views
{
    public class SignupViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}