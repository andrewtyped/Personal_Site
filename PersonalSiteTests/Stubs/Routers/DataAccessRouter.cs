using DataAccess;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalSiteTests.Stubs.Routers
{
    public class DataAccessRouter :RouterBase
    {
        protected override IDataResult GetBlogPost(IEnumerable<IDataParameter> parms)
        {
            var table = GetBlogPostTable();
            var desiredPostId = (int?)parms.Where(p => p.ParameterName == "@Id").Single().Value;

            if (desiredPostId != null)
                table.Rows.Add((int)desiredPostId, "Test", "Test test test", new DateTime(2012, 01, 01), "C#");
            else
            {
                table.Rows.Add(1, "Test1", "Test test test", new DateTime(2012, 01, 01), "C#");
                table.Rows.Add(2, "Test2", "Test test test", new DateTime(2012, 01, 01), "C#");
                table.Rows.Add(3, "Test3", "Test test test", new DateTime(2012, 01, 01), "C#");
                table.Rows.Add(4, "Test4", "Test test test", new DateTime(2012, 01, 01), "C#");
                table.Rows.Add(5, "Test5", "Test test test", new DateTime(2012, 01, 01), "C#");
            }

            var result = new DataResult(true, "", table);
            return result;
        }

        private DataTable GetBlogPostTable()
        {
            var table = new DataTable();
            table.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Id",typeof(int)),
                new DataColumn("Title"),
                new DataColumn("Content"),
                new DataColumn("DateCreated"),
                new DataColumn("Name") //tag name... badly named column
            });

            return table;
        }

        protected override IResult AddEditBlogPost(IEnumerable<IDataParameter> parms)
        {
            return new StringResult(true, "");
        }

        protected override IResult DeleteBlogPost(IEnumerable<IDataParameter> parms)
        {
            return new StringResult(true, "");
        }
    }
}
