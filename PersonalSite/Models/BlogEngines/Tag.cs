using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalSite.Models
{
    public class Tag
    {
        public int? Id { get; private set; }
        public string Name { get; set; }
        public int? BlogPostId { get; set; }

        public Tag(string name, int? blogPostId = null, int? id = null)
        {
            Id = id;
            Name = name;
            BlogPostId = blogPostId;
        }
    }
}