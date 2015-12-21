using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalSite.Models
{
    public interface IBlogEngine
    {
        IDataAccess DataAccess {get;}
        BlogPost GetPostById(int id);
        BasicBlogStats GetBlogStats();
        IList<BlogPost> GetOlderPosts(int maxId, int numResults, bool showContent = false);
        IList<BlogPost> GetNewerPosts(int minId, int numResults, bool showContent = false);
        IList<BlogPost> GetPostsByTag(string tagName);
        IList<Tag> GetTags();
        IList<CountedTag> GetTagCounts();
        IResult AddEditBlogPost(BlogPost post);
        IResult DeleteBlogPost(BlogPost post);
    }
}
