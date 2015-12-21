using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DataResult : StringResult, IDataResult
    {
        private DataTable data;
        public DataTable Data
        {
            get
            {
                return data;
            }
        }

        public DataResult(bool success, string result, DataTable data)
            : base(success,result)
        {
            this.data = data;
        }


    }
}
