using DataAccess;
using PersonalSite.Models;
using PersonalSite.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PersonalSite.Controllers
{
    [Authorize(Roles = "admin")]
    public class ContentController : Controller
    {
        private IBlogEngine dataAccess;

        public ContentController()
            : this(new SqlBlogEngine())
        {
        }
     
        public ContentController(IBlogEngine dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public ActionResult Content()
        {
            return View();
        }

        public ActionResult EditContent(int id)
        {
            var post = dataAccess.GetPostById(id);
            var viewModel = new BlogPostViewModel(post);

            return View("Content",viewModel);
        }

        [HttpPost]
        public ActionResult AddEditBlogPost(string title, string tags, string content, int? id)
        {
            StringResult jsonData;

            if (string.IsNullOrEmpty(title))
            {
                jsonData = new StringResult(false,"Please enter a title");
            }
            else if (string.IsNullOrEmpty(content))
            {
                jsonData = new StringResult(false,"Please enter content");
            }
            else
            {
                jsonData = AddEditBlogPostData(title, tags, content, id);
            }

            return Json(jsonData);
        }

        private StringResult AddEditBlogPostData(string title, string tags, string content, int? id)
        {
            var post = new BlogPost(title, content, System.DateTime.Now, id);

            if (!string.IsNullOrEmpty(tags))
            {
                var tagList = GetTagList(tags);
                post.Tags.AddRange(tagList);
            }

            try
            {
                dataAccess.AddEditBlogPost(post);
            }
            catch
            {
                return new StringResult(false, "Sorry, this entry was not posted successfully.");
            }

            string resultText = id == null ? "New post added!" : "Post edited successfully!";

            return new StringResult(true, resultText);
        }

        private List<Tag> GetTagList(string tags)
        {
            var tagsList = new List<Tag>();

            foreach (string tag in tags.Split(' '))
            {
                tagsList.Add(new Tag(tag));
            }

            return tagsList;
        }
    }
}
