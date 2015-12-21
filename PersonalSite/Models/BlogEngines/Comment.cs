using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalSite.Models
{
    public class Comment
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public int? RepliesTo { get; set; }
        public string Content { get; set; }
        public bool InModeration { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastModified { get; set; }
    }
}