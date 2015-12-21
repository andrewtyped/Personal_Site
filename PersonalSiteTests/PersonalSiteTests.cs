using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using DataAccess.Interfaces;
using PersonalSite.Models;
using PersonalSite.Views;
using System.Data.SqlClient;
using DataAccess;
using System.Data;
using PersonalSiteTests.Stubs;
using PersonalSiteTests.Stubs.Routers;

namespace PersonalSiteTests
{
    [TestFixture]
    public class PersonalSiteTests
    {
        private DataAccessStub<DataAccessRouter> dataAccessStub = new DataAccessStub<DataAccessRouter>();
        private DataAccessStub<FaultyDataAccessRouter> faultyDataAccesStub = new DataAccessStub<FaultyDataAccessRouter>();

        [Test]
        public void CanGetSingleBlogPost()
        {
            SqlBlogEngine blogEngine = new SqlBlogEngine(dataAccessStub);
            //var post = blogEngine.GetBlogPosts(1).ToList();

            //Assert.AreEqual(1, post[0].Id);
            //Assert.AreEqual("Test", post[0].Title);
            //Assert.AreEqual("Test test test", post[0].Content);
            //Assert.AreEqual(new DateTime(2012, 01, 01), post[0].DateCreated);
        }

        [Test]
        public void CanGetMultipleBlogPosts()
        {
            var blogEngine = new SqlBlogEngine(dataAccessStub);
            //var post = blogEngine.GetBlogPosts();
            //Assert.AreEqual(5, post.Count);
        }

        [Test]
        public void CanGetSinglePageOfFiveBlogPosts()
        {
            var blogEngine = new SqlBlogEngine(dataAccessStub);
            //var post = blogEngine.GetOlderPosts();
        }

        [Test]
        public void LackOfBlogPostsShouldNotThrowException()
        {
            SqlBlogEngine blogEngine = new SqlBlogEngine(faultyDataAccesStub);

            //var post = blogEngine.GetBlogPosts().ToList();

            //Assert.AreEqual(post[0].Id, 0);
            //Assert.AreEqual(post[0].Title, "Sorry");
            //Assert.AreEqual(post[0].Content, "No blog posts yet.");
            //Assert.AreEqual(post[0].DateCreated, new DateTime(1900, 01, 01));
        }

        [Test]
        public void CanSaveBlogPost()
        {
            var post = new BlogPost("SaveTest", "Test test test", new DateTime(2013, 01, 01));
            post.Tags.Add(new Tag("Test"));
            var blogEngine = new SqlBlogEngine(dataAccessStub);

            var result = blogEngine.AddEditBlogPost(post);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.Result, "");
        }

        [Test]
        public void CanDeleteBlogPost()
        {
            var post = new BlogPost("DeleteTest", "Test test test", new DateTime(1900,01,01),1);
            var blogEngine = new SqlBlogEngine(dataAccessStub);

            var result = blogEngine.DeleteBlogPost(post);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.Result, "");
        }

        [Test]
        public void CanGetTagCloud()
        {
            var cSharpTag = new Tag("C#");
            var codeTag = new Tag("Code");
            var testTag = new Tag("Test");

            var tags = new List<CountedTag>()
            {
                new CountedTag("C#",4),
                new CountedTag("Code",4),
                new CountedTag("Test",4)
            };

            var posts = new List<BlogPost>();

            var blogViewModel = new BlogViewModel(posts,tags);

            var tagViewModels = blogViewModel.SetupTagCloud(tags);

            var cSharpTagViewModel = tagViewModels.Where(t => t.CountedTag.Name == "C#").Single();
            var codeTagViewModel = tagViewModels.Where(t => t.CountedTag.Name == "Code").Single();
            var testTagViewModel = tagViewModels.Where(t => t.CountedTag.Name == "Test").Single();

            Assert.AreEqual(3, tagViewModels.Count);
            Assert.AreEqual(4, cSharpTagViewModel.CountedTag.PostCount);
            Assert.AreEqual(4, codeTagViewModel.CountedTag.PostCount);
            Assert.AreEqual(4, testTagViewModel.CountedTag.PostCount);

            Assert.AreEqual(33, cSharpTagViewModel.FontSize);
        }
    }
}
