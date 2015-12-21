using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class SqlDataAccess :IDataAccess
    {
        private string connectionString;

        public SqlDataAccess(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public IDataResult ExecProcWithReturnData(string procedureName, IEnumerable<IDataParameter> parms)
        {
            SqlDataAdapter adapter = null;
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand command = GetSqlCommand(procedureName, parms, conn);
                    adapter = new SqlDataAdapter(command);
                    conn.Open();
                    adapter.Fill(dt);

                    return new DataResult(true, string.Empty, dt);
                }
            }
            catch(Exception ex)
            {
                return new DataResult(false, ex.Message, null);
            }
            finally
            {
                adapter.Dispose();
            }
        }

        private SqlCommand GetSqlCommand(string procedureName, IEnumerable<IDataParameter> parms, SqlConnection conn)
        {
            SqlCommand command = new SqlCommand(procedureName, conn);
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            if (parms != null)
            {
                foreach (IDataParameter parm in parms)
                {
                    object value = parm.Value ?? DBNull.Value;
                    command.Parameters.Add(new SqlParameter(parm.ParameterName, value));
                }
            }

            return command;
        }

        public IResult ExecProcNoReturnData(string procedureName, IEnumerable<IDataParameter> parms)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand command = GetSqlCommand(procedureName, parms, conn);
                    conn.Open();
                    command.ExecuteNonQuery();

                    return new StringResult(true, string.Empty);
                }
            }
            catch (Exception ex)
            {
                return new StringResult(false, ex.Message);
            }
        }

        public IScalarResult ExecScalar(string procedureName)
        {
            return ExecScalar(procedureName, null);
        }

        public IScalarResult ExecScalar(string procedureName, IEnumerable<IDataParameter> parms)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand command = GetSqlCommand(procedureName, parms, conn);
                    conn.Open();
                    object result = command.ExecuteScalar();

                    return new ScalarResult(true, string.Empty,result);
                }
            }
            catch (Exception ex)
            {
                return new ScalarResult(false, ex.Message,null);
            }
        }
    }
}
