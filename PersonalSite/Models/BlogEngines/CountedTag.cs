using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalSite.Models
{
    public class CountedTag
    {
        public string Name { get; set; }
        public int PostCount { get; set; }

        public CountedTag(string name, int postCount)
        {
            Name = name;
            PostCount = postCount;
        }
    }
}