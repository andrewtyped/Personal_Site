using DataAccess;
using DataAccess.Interfaces;
using PersonalSite.Globals;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PersonalSite.Models
{
    public class SqlUserDataAccess : IUserDataAccess
    {
        private IDataAccess dataAccess;

        private User failUser = new User(0, "FAIL", "FAIL USER", "", new Role("FAIL"));

        public SqlUserDataAccess()
        {
            this.dataAccess = Data.Sql;
        }

        public SqlUserDataAccess(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public User GetUserByName(string userName)
        {
            var parms = new List<IDataParameter>();
            parms.Add(new SqlParameter("@Name",userName));

            var result = dataAccess.ExecProcWithReturnData("usp_GetUserByName", parms);

            User user;

            if (result.Success && result.Data.Rows.Count == 1)
            {
                var row = result.Data.Rows[0];
                var id = (int)row["Id"];
                var name = (string)row["Name"];
                var email = (string)row["Email"];
                var password = (string)row["Password"];
                var role = (string)row["Role"];
                user = new User(id, name, email, password, new Role(role));
            }
            else
            {
                user = failUser;
            }

            return user;
        }

        public int GetUserCount()
        {
            int userCount;

            var result = dataAccess.ExecScalar("usp_GetUserCount");

            if (result.Success && result.Scalar != null)
            {
                userCount = (int)result.Scalar;

                return userCount;
            }
            else
            {
                throw new Exception(result.Result);
            }
        }

        public User CreateUser(string userName, string password, string email, string role)
        {
            var parms = new List<SqlParameter>();
            Action<string, object> addParm = (parm, value) => { parms.Add(new SqlParameter(parm, value)); };
            addParm("@Name", userName);
            addParm("@Email", email);
            addParm("@Password", password);
            addParm("@Role", role);

            var result = dataAccess.ExecProcWithReturnData("usp_CreateUser", parms);

            User user;

            if (result.Success)
            {
                user = new User(int.Parse(result.Data.Rows[0][0].ToString()), userName, email, password, new Role(role));
            }
            else
            {
                user = failUser;
            }

            return user;

        }
    }
}