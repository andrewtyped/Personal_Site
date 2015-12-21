using DataAccess;
using DataAccess.Interfaces;
using PersonalSite.Globals;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace PersonalSite.Models
{
    public class SqlBlogEngine : IBlogEngine
    {
        private IDataAccess dataAccess;
        public IDataAccess DataAccess
        {
            get { return dataAccess; }
        }

        public SqlBlogEngine()
        {
            this.dataAccess = Data.Sql;
        }

        public SqlBlogEngine(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public BlogPost GetPostById(int id)
        {
            var parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id",id));

            var result = dataAccess.ExecProcWithReturnData("usp_GetBlogPost",parms);
            var posts = new List<BlogPost>();

            if (result.Success && result.Data.Rows.Count > 0)
                posts = BuildPostSetWithTagsAndContent(result.Data).ToList();
            else
                posts.Add(new BlogPost("Sorry", "No blog posts yet.", new DateTime(1900, 01, 01), 0));

            return posts.Single();
        }

        private IEnumerable<BlogPost> BuildPostSetWithTagsAndContent(DataTable table)
        {
            var posts = new List<BlogPost>();
            var postIds = new HashSet<int>();
            var tags = new List<Tag>();

            for (int i = 0; i < table.Rows.Count; i++)
            {
                int postId = (int)table.Rows[i]["Id"];
                string tag = (string)table.Rows[i]["Name"];

                if (!string.IsNullOrEmpty(tag))
                    tags.Add(new Tag(tag, postId));

                if (postIds.Contains(postId))
                    continue;

                postIds.Add(postId);

                string title = (string)table.Rows[i]["Title"];
                string content = (string)table.Rows[i]["Content"];
                DateTime dateCreated = Convert.ToDateTime(table.Rows[i]["DateCreated"]);

                posts.Add(new BlogPost(title, content, dateCreated, postId));
            }

            AddTagsToPosts(posts, tags);

            return posts;
        }

        private void AddTagsToPosts(IEnumerable<BlogPost> posts, IEnumerable<Tag> tags)
        {
            foreach (BlogPost post in posts)
            {
                var postTags = tags.Where(t => t.BlogPostId == (int)post.Id);

                if (postTags.Count() > 0)
                    post.Tags.AddRange(postTags);
            }
        }

        public IList<BlogPost> GetOlderPosts(int maxId, int numResults, bool showContent = false)
        {
            var parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@MaxId", maxId));
            parms.Add(new SqlParameter("@NumResults", numResults));
            if (showContent)
                parms.Add(new SqlParameter("@ReturnContent", 1));

            var result = dataAccess.ExecProcWithReturnData("usp_GetOlderPosts", parms);
            var posts = new List<BlogPost>();

            if (result.Success && result.Data.Rows.Count > 0)
                posts = BuildSimplePostSetOfTitlesAndDates(result.Data).ToList();
            else
                posts.Add(new BlogPost("Sorry", "No blog posts yet.", new DateTime(1900, 01, 01), 0));

            return posts;
        }

        public IList<BlogPost> GetNewerPosts(int maxId, int numResults, bool showContent = false)
        {
            var parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@MinId", maxId));
            parms.Add(new SqlParameter("@NumResults", numResults));
            if (showContent)
                parms.Add(new SqlParameter("@ReturnContent", 1));

            var result = dataAccess.ExecProcWithReturnData("usp_GetNewerPosts", parms);
            var posts = new List<BlogPost>();

            if (result.Success && result.Data.Rows.Count > 0)
                posts = BuildSimplePostSetOfTitlesAndDates(result.Data).ToList();
            else
                posts.Add(new BlogPost("Sorry", "No blog posts yet.", new DateTime(1900, 01, 01), 0));

            return posts;
        }


        public IList<BlogPost> GetPostsByTag(string tagName)
        {
            var parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@TagName", tagName));

            var result = dataAccess.ExecProcWithReturnData("usp_GetBlogPostsByTag", parms);
            var posts = new List<BlogPost>();

            if (result.Success && result.Data.Rows.Count > 0)
                posts = BuildSimplePostSetOfTitlesAndDates(result.Data).ToList();
            else
                posts.Add(new BlogPost("Sorry", "No blog posts yet.", new DateTime(1900, 01, 01), 0));

            return posts;
        }

        private IEnumerable<BlogPost> BuildSimplePostSetOfTitlesAndDates(DataTable table)
        {
            var posts = new List<BlogPost>();

            var contentIncluded = table.Columns.Contains("Content");

            for (int i = 0; i < table.Rows.Count; i++)
            {
                int postId = (int)table.Rows[i]["Id"];
                string title = (string)table.Rows[i]["Title"];
                DateTime dateCreated = Convert.ToDateTime(table.Rows[i]["DateCreated"]);
                string content = "";

                if (contentIncluded)
                    content = (string)table.Rows[i]["Content"];

                posts.Add(new BlogPost(title, content, dateCreated, postId));
            }

            return posts;
        }

        public IList<Tag> GetTags()
        {
            var result = dataAccess.ExecProcWithReturnData("usp_GetTags",new List<SqlParameter>());
            var tags = new List<Tag>();

            if (result.Success && result.Data.Rows.Count > 0)
            {
                var table = result.Data;

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    var tagId = (int)table.Rows[i]["Id"];
                    var tagName = (string)table.Rows[i]["Name"];

                    tags.Add(new Tag(tagName,null, tagId));
                }
            }

            return tags;
        }

        public IList<CountedTag> GetTagCounts()
        {
            var result = dataAccess.ExecProcWithReturnData("usp_GetTagCounts", new List<SqlParameter>());
            var countedTags = new List<CountedTag>();

            if (result.Success && result.Data.Rows.Count > 0)
            {
                var table = result.Data;

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    var tagName = (string)table.Rows[i]["Name"];
                    var postCount = (int)table.Rows[i]["PostCount"];

                    countedTags.Add(new CountedTag(tagName, postCount));
                }
            }

            return countedTags;
        }

        public IResult AddEditBlogPost(BlogPost post)
        {
            List<SqlParameter> parms = GetAddEditParms(post);
            
            var result = dataAccess.ExecProcNoReturnData("usp_AddEditBlogPost", parms);

            if (result.Success)
                return result;
            else
                throw new Exception(result.Result);
        }

        private List<SqlParameter> GetAddEditParms(BlogPost post)
        {
            var parms = new List<SqlParameter>();

            parms.Add(new SqlParameter("@Id", post.Id));
            parms.Add(new SqlParameter("@Title", post.Title));
            parms.Add(new SqlParameter("@Content", post.Content));
            parms.Add(new SqlParameter("@DateCreated", post.DateCreated));

            var tagTable = new DataTable("TagType");
            tagTable.Columns.Add("Name", typeof(string));
            tagTable.Columns.Add("BlogPostId", typeof(int));

            foreach (Tag tag in post.Tags)
                tagTable.Rows.Add(tag.Name, null);

            parms.Add(new SqlParameter("@TagTable", tagTable) { SqlDbType = SqlDbType.Structured });

            return parms;
        }

        public IResult DeleteBlogPost(BlogPost post)
        {
            var parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id",post.Id));

            var result = dataAccess.ExecProcNoReturnData("usp_DeleteBlogPost", parms);

            return result;
        }

        public BasicBlogStats GetBlogStats()
        {
            var parms = new List<SqlParameter>();
            var result = dataAccess.ExecProcWithReturnData("usp_GetBlogStats", parms);

            var stats = new BasicBlogStats();

            if (result.Success && result.Data.Rows.Count > 0)
            {
                var row = result.Data.Rows[0];
                stats.MaxPostId = (int)row["MaxPostId"];
                stats.MaxPostDate = Convert.ToDateTime(row["MaxPostDate"]);
                stats.MinPostId = (int)row["MinPostId"];
                stats.MinPostDate = Convert.ToDateTime(row["MinPostDate"]);
                stats.TotalPostCount = (int)row["PostCount"];
            }
            else
            {
                stats.MaxPostId = 0;
                stats.MaxPostDate = new DateTime(1900, 01, 01);
                stats.MinPostId = 0;
                stats.MinPostDate = new DateTime(1900, 01, 01);
                stats.TotalPostCount = 0;
            }

            return stats;
        }
    }
}