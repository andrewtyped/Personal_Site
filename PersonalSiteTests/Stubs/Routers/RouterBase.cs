using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalSiteTests.Stubs.Routers
{
    public abstract class RouterBase
    {
        public IDataResult RouteWithReturnData(string spName, IEnumerable<IDataParameter> parms)
        {
            IDataResult result;

            switch (spName)
            {
                case "usp_GetBlogPost":
                    result = GetBlogPost(parms);
                    break;
                default:
                    throw new NotImplementedException();
            }

            return result;
        }

        public IResult RouteWithNoReturnData(string spName, IEnumerable<IDataParameter> parms)
        {
            IResult result;

            switch (spName)
            {
                case "usp_AddEditBlogPost":
                    result = AddEditBlogPost(parms);
                    break;
                case "usp_DeleteBlogPost":
                    result = DeleteBlogPost(parms);
                    break;
                default:
                    throw new NotImplementedException();
            }

            return result;
        }

        protected abstract IDataResult GetBlogPost(IEnumerable<IDataParameter> parms);
        protected abstract IResult AddEditBlogPost(IEnumerable<IDataParameter> parms);
        protected abstract IResult DeleteBlogPost(IEnumerable<IDataParameter> parms);
    }
}
