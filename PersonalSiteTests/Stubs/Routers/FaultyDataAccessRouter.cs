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
    public sealed class FaultyDataAccessRouter : RouterBase
    {
        protected override IDataResult GetBlogPost(IEnumerable<IDataParameter> parms)
        {
            return new DataResult(false, "", null);
        }

        protected override IResult AddEditBlogPost(IEnumerable<IDataParameter> parms)
        {
            return new StringResult(false, "error");
        }

        protected override IResult DeleteBlogPost(IEnumerable<IDataParameter> parms)
        {
            return new StringResult(false, "error");
        }
    }
}
