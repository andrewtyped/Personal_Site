using DataAccess.Interfaces;
using PersonalSite.Globals;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PersonalSite.Models
{
    public class SqlCommentEngine : ICommentEngine
    {
        public IDataAccess DataAccess { get; private set; }

        public SqlCommentEngine()
        {
            DataAccess = Data.Sql;
        }

        public SqlCommentEngine(IDataAccess dataAccess)
        {
            DataAccess = dataAccess;
        }

        public IList<Comment> GetCommentsByPostId(int postId)
        {
            var parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@PostId",postId));

            var result = DataAccess.ExecProcWithReturnData("usp_GetCommentsByPost", parms);
            var comments = new List<Comment>();

            if (result.Success && result.Data.Rows.Count > 0)
            {
                var rowCount = result.Data.Rows.Count;
                var table = result.Data;

                for (int i = 0; i < rowCount; i++)
                {
                    var row = table.Rows[i];
                    var comment = new Comment()
                    {
                        Id = (int)row["Id"],
                        UserId = (int)row["UserId"],
                        RepliesTo = (int?)row["RepliesTo"],
                        Content = row["Content"].ToString(),
                        InModeration = Convert.ToBoolean(row["InModeration"]),
                        DateCreated = Convert.ToDateTime(row["DateCreated"]),
                        LastModified = Convert.ToDateTime(row["LastModified"]),
                    };

                    comments.Add(comment);
                }
            }

            return comments;
        }

        public IResult AddEditComment(Comment comment)
        {
            var parms = new List<SqlParameter>()
            {
                new SqlParameter("@Id",comment.Id),
                new SqlParameter("@UserId", comment.UserId),
                new SqlParameter("@PostId", comment.PostId),
                new SqlParameter("@InReplyTo", comment.RepliesTo),
                new SqlParameter("@Content", comment.Content),
                new SqlParameter("@InModeration", comment.InModeration)
            };

            var result = DataAccess.ExecProcNoReturnData("usp_AddEditComment",parms);

            if (result.Success)
                return result;
            else
                throw new Exception(result.Result);
        }

        public IResult DeleteComment(int commentId)
        {
            var parms = new List<SqlParameter>()
            {
                new SqlParameter("@Id",commentId),
            };

            var result = DataAccess.ExecProcNoReturnData("usp_DeleteComment", parms);

            if (result.Success)
                return result;
            else
                throw new Exception(result.Result);
        }
    }
}