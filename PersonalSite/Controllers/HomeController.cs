using DataAccess;
using DataAccess.Interfaces;
using PersonalSite.Globals;
using PersonalSite.Models;
using PersonalSite.Views;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace PersonalSite.Controllers
{
    public class HomeController : Controller
    {
        private IBlogEngine blogEngine;
        private int postsPerPage = Convert.ToInt32(ConfigurationManager.AppSettings["BlogPostsPerPage"]);

        public HomeController()
            : this(new SqlBlogEngine(Data.Sql))
        {
        }

        public HomeController(IBlogEngine blogEngine)
        {
            this.blogEngine = blogEngine;
        }

        public ActionResult Index()
        {
            var posts = blogEngine.GetOlderPosts(int.MaxValue, postsPerPage, true);

            return View(GetBlogViewModel(posts, true));
        }

        public ActionResult GetBlogPostsByTag(string tagName)
        {
            var posts = blogEngine.GetPostsByTag(tagName);

            return View("~/Views/Home/Index.cshtml",GetBlogViewModel(posts, false));
        }

        public ActionResult GetOlderPosts(int maxId)
        {
            var olderPosts = blogEngine.GetOlderPosts(maxId - 1, postsPerPage, false);

            return View("~/Views/Home/Index.cshtml", GetBlogViewModel(olderPosts, true));
        }

        public ActionResult GetNewerPosts(int minId)
        {
            var newerPosts = blogEngine.GetNewerPosts(minId, postsPerPage, false);

            return View("~/Views/Home/Index.cshtml", GetBlogViewModel(newerPosts,true));
        }

        public ActionResult GetBlogPostById(int id)
        {
            var post = new List<BlogPost>(); 
            post.Add(blogEngine.GetPostById(id));

            return View("~/Views/Home/Index.cshtml",GetBlogViewModel(post, false));
        }

        private BlogViewModel GetBlogViewModel(IList<BlogPost> posts, bool enableOlderNewerPostNav)
        {
            var tags = blogEngine.GetTagCounts();
            var viewModel = new BlogViewModel(posts, tags);
            viewModel.OlderNewerNavEnabled = enableOlderNewerPostNav;
            viewModel.BlogStats = blogEngine.GetBlogStats();

            return viewModel;
        }

        public ActionResult Resume()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

    }
}
