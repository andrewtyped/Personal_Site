using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class StringResult : IResult
    {
        protected bool success;
        protected string result;

        public bool Success
        {
            get
            {
                return success;
            }
        }

        public string Result
        {
            get
            {
                return result;
            }
        }

        public StringResult(bool success, string result)
        {
            this.success = success;
            this.result = result;
        }


    }
}
