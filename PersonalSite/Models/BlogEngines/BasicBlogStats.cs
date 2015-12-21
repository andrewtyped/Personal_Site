using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalSite.Models
{
    public class BasicBlogStats
    {
        public int MaxPostId { get; set; }
        public int MinPostId { get; set; }
        public DateTime MaxPostDate { get; set; }
        public DateTime MinPostDate { get; set; }
        public int TotalPostCount { get; set; }
    }
}