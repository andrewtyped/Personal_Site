using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ScalarResult : StringResult, IScalarResult
    {
        public object Scalar { get; private set; }

        public ScalarResult(bool success, string message, object scalar)
            :base(success, message)
        {
            this.Scalar = scalar;
        }
    }
}
