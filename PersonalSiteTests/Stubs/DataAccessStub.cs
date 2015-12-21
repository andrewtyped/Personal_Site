using DataAccess.Interfaces;
using PersonalSiteTests.Stubs.Routers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalSiteTests.Stubs
{
    public class DataAccessStub<T> : IDataAccess where T : RouterBase, new()
    {
        private T router = new T();

        IDataResult IDataAccess.ExecProcWithReturnData(string procedureName, IEnumerable<IDataParameter> parms)
        {
            return router.RouteWithReturnData(procedureName, parms);
        }

        IScalarResult IDataAccess.ExecScalar(string procedureName, IEnumerable<IDataParameter> parms)
        {
            throw new NotImplementedException();
        }

        IScalarResult IDataAccess.ExecScalar(string procedureName)
        {
            throw new NotImplementedException();
        }

        IResult IDataAccess.ExecProcNoReturnData(string procedureName, IEnumerable<IDataParameter> parms)
        {
            return router.RouteWithNoReturnData(procedureName, parms);
        }
    }
}
