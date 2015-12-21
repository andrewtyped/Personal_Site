using PersonalSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace PersonalSite.Views
{
    public class BlogPostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Tags { get; set; }
        public string Content { get; set; }

        public BlogPostViewModel()
        {
        }

        public BlogPostViewModel(BlogPost post)
        {
            Id = (int)post.Id;
            Title = post.Title;
            Content = post.Content;
            Tags = SetTags(post.Tags);
        }

        private string SetTags(List<Tag> tags)
        {
            StringBuilder spacedTags = new StringBuilder();

            foreach (Tag tag in tags)
            {
                spacedTags.Append(tag.Name + " ");
            }

            return spacedTags.ToString().Trim();
        }
    }
}