using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IDataAccess
    {
        IDataResult ExecProcWithReturnData(string procedureName, IEnumerable<IDataParameter> parms);
        IScalarResult ExecScalar(string procedureName, IEnumerable<IDataParameter> parms);
        IScalarResult ExecScalar(string procedureName);
        IResult ExecProcNoReturnData(string procedureName, IEnumerable<IDataParameter> parms);
    }
}
