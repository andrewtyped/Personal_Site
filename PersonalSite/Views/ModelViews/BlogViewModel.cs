using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PersonalSite.Models;
using System.Web.Mvc;
using MarkdownSharp;

namespace PersonalSite.Views
{
    public class BlogViewModel
    {
        public IList<BlogPost> Posts { get; set; }
        public BasicBlogStats BlogStats { get; set; }
        public bool OlderNewerNavEnabled { get; set; }
        public IList<TagViewModel> TagCloud { get; private set; }

        public BlogViewModel(IList<BlogPost> posts, IList<CountedTag> countedTags)
        {
            if (posts != null)
            {
                Posts = SetupPosts(posts);
            }

            if (countedTags != null)
            {
                TagCloud = SetupTagCloud(countedTags);
            }
        }

        public IList<BlogPost> SetupPosts(IList<BlogPost> blogPosts )
        {
            foreach (BlogPost post in blogPosts)
            {
                post.Content = ApplyMarkDownFormatting(post.Content);
            }

            return blogPosts;
        }

        public string ApplyMarkDownFormatting(string content)
        {
            var markdown = new Markdown();

            return markdown.Transform(content);
        }

        public List<TagViewModel> SetupTagCloud(IList<CountedTag> countedTags)
        {
            var totalCount = countedTags.Sum(c => c.PostCount);

            var tagViewModels = new List<TagViewModel>();

            foreach (CountedTag countedTag in countedTags)
            {
                var tagCount = countedTag.PostCount;
                var tagViewModel = new TagViewModel() { CountedTag = countedTag };
                tagViewModel.FontSize = SetFontSizeForTagCloudMember(totalCount, tagCount);

                tagViewModels.Add(tagViewModel);
            }

            return tagViewModels;
        }

        public int SetFontSizeForTagCloudMember(int totalPostCount, int tagOccurenceCount)
        {
            decimal maxFont = 40.0M;
            decimal minFont = 9.5M;
            decimal scalingFactor = 100.0M;
            decimal tagPostRatio = (decimal)tagOccurenceCount / (decimal)totalPostCount;

            decimal rawFontSize = scalingFactor * tagPostRatio;
            decimal cappedFontSize = Math.Min(maxFont, rawFontSize);
            decimal finalFontSize = Math.Max(minFont, cappedFontSize);

            return Convert.ToInt32(finalFontSize);
        }
    }
}