using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalSite.Models
{
    public interface ICommentEngine
    {
        IList<Comment> GetCommentsByPostId(int postId);
        IResult AddEditComment(Comment comment);
        IResult DeleteComment(int commentId);
    }
}
