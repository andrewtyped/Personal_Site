using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PersonalSite.Models;
using System.Configuration;
using PersonalSite.Globals;
using System.Web;

namespace PersonalSiteTests
{
    [TestFixture]
    public class ConfigTests
    {
        //doesn't work, needs to get config values from other project somehow
        //[Test]
        //public void CanGetConnectionString()
        //{
        //    Assert.IsTrue(ConfigTestability.SqlConnectionIsNamedAppropriately());
        //}
    }
}
