using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonalSite.Models
{
    public class BlogPost
    {
        public int? Id { get; private set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public List<Tag> Tags { get; set; }

        public BlogPost(string title, string content, DateTime dateCreated, int? id = null)
        {
            Title = title;
            Content = content;
            Id = id;
            DateCreated = dateCreated;

            Tags = new List<Tag>();
        }
    }
}