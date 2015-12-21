using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalSite.Controllers;
using PersonalSite.Models;
using NUnit.Framework;
using Moq;
using System.Web.Mvc;
using PersonalSite.Views;

namespace PersonalSiteTests
{
    [TestFixture]
    public class ControllerTests
    {
        IBlogEngine blogEngine;

        [TestFixtureSetUp]
        public void SetupMockBlogEngine()
        {
            Mock<IBlogEngine> mockBlog = new Mock<IBlogEngine>();
            //mockBlog.Setup(m => m.GetBlogPosts(It.IsAny<int?>(),It.IsAny<string>()))
            //    .Returns(getTenBlogPosts());

            blogEngine = mockBlog.Object;
        }

        private IList<BlogPost> getTenBlogPosts()
        {
            var posts = new List<BlogPost>();

            for (int i = 0; i < 10; i++)
            {
                posts.Add(new BlogPost("Test", "Test test test", new DateTime(2013, 01, 01), i));
            }

            return posts;
        }


        [Test]
        public void HomeControllerShouldReturnBlogPosts()
        {
            var controller = new HomeController(blogEngine);
            var result = controller.Index() as ViewResult;
            var model = result.Model as BlogViewModel;

            Assert.IsTrue(model.Posts is IList<BlogPost>);
            Assert.IsTrue(model.Posts.Count == 10);
        }
    }
}
