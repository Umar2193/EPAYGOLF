using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DapperManager
    {
        
        private string ConnectionString = "";
        public DapperManager() {
            ConnectionString = ConfigurationHelper.GetConnectionString("DefaultConnection");
        }

        public bool CheckConnection()
        {
            bool result;
            using (var connection = new SqlConnection(ConnectionString))
            {

                try
                {
                    connection.Open();
                    connection.Close();
                    result = true;

                }
                catch (SqlException ex)
                {
                    result = false;
                    connection.Close();
                    Helpers.ApplicationExceptions.SaveAppError(ex);
                    throw ex;
                }
            };
            return result;
        }
        public IEnumerable<T> Get<T>(string spname, dynamic parameter = null, IDbTransaction transaction = null, bool buffered = true, int? timeout = null, CommandType? commandtype = null) where T : class
        {
            IEnumerable<T> result = new List<T>();
            using (var connection = new SqlConnection(ConnectionString))
            {

                try
                {
                    connection.Open();
                    result = SqlMapper.Query<T>(connection, spname,
                                                 param: (object)parameter,
                                                 transaction: transaction,
                                                 commandTimeout: timeout,
                                                 commandType: commandtype);
                    connection.Close();
                }
                catch (SqlException ex)
                {
                    connection.Close();
					Helpers.ApplicationExceptions.SaveAppError(ex);
					throw ex;
                }
            };

            return result;
        }

        public int Insert<T>(string spname, dynamic parameter, IDbTransaction transaction = null, bool buffered = true, int? timeout = null, CommandType? commandtype = null) where T : struct
        {
            int result = 0;
            using (var connection = new SqlConnection(ConnectionString))
            {

                try
                {
                    connection.Open();
                    result = connection.Execute(spname,
                              param: (object)parameter,
                              transaction: transaction,
                              commandTimeout: timeout,
                              commandType: commandtype);
                    connection.Close();

                }
                catch (SqlException ex)
                {
                    connection.Close();
					Helpers.ApplicationExceptions.SaveAppError(ex);
					throw ex;
                }
            }
            return result;
        }

        public int Execute<T>(string spname, dynamic parameter = null, IDbTransaction transaction = null, bool buffered = true, int? timeout = null, CommandType? commandtype = null) where T : struct
        {
            int result = 0;

            using (var connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    result = connection.Execute(spname,
                              param: (object)parameter,
                              transaction: transaction,
                              commandTimeout: timeout,
                              commandType: commandtype);
                    connection.Close();
                }
                catch (SqlException ex)
                {
                    connection.Close();
					Helpers.ApplicationExceptions.SaveAppError(ex);
					throw ex;
                }
            }

            return result;
        }

        public int Update<T>(string spname, dynamic parameter, IDbTransaction transaction = null, bool buffered = true, int? timeout = null, CommandType? commandtype = null) where T : struct
        {
            int result = 0;
            using (var connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    result = connection.Execute(spname,
                              param: (object)parameter,
                              transaction: transaction,
                              commandTimeout: timeout,
                              commandType: commandtype);
                    connection.Close();
                }
                catch (SqlException ex)
                {
                    connection.Close();
					Helpers.ApplicationExceptions.SaveAppError(ex);
					throw ex;
                }
            }
            return result;
        }

        public int Delete<T>(string spname, dynamic parameter, IDbTransaction transaction = null, bool buffered = true, int? timeout = null, CommandType? commandtype = null) where T : struct
        {
            int result = 0;
            using (var connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    result = connection.Execute(spname,
                              param: (object)parameter,
                              transaction: transaction,
                              commandTimeout: timeout,
                              commandType: commandtype);
                    connection.Close();
                }
                catch (SqlException ex)
                {
                    connection.Close();
					Helpers.ApplicationExceptions.SaveAppError(ex);
					throw ex;
                }
            }
            return result;
        }

        public Tuple<List<T1>, List<T2>> QueryMultiple<T1, T2>(string spname, dynamic parameter, IDbTransaction transaction = null, bool buffered = true, int? timeout = null, CommandType? commandtype = null)
        {
            Tuple<List<T1>, List<T2>> result;
            var obj1 = new List<T1>();
            var obj2 = new List<T2>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    using (var reader = connection.QueryMultiple(spname,
                              param: (object)parameter,
                              transaction: transaction,
                              commandTimeout: timeout,
                              commandType: commandtype))
                    {

                        obj1 = (List<T1>)reader.Read<T1>().ToList();
                        obj2 = (List<T2>)reader.Read<T2>().ToList();
                        result = new Tuple<List<T1>, List<T2>>(obj1, obj2);
                    }
                    connection.Close();
                }
                catch (SqlException ex)
                {
                    connection.Close();
					Helpers.ApplicationExceptions.SaveAppError(ex);
					throw ex;
                }
            }
            return result;
        }
        public IEnumerable<T> Query<T>(string spname, dynamic parameter = null, IDbTransaction transaction = null, bool buffered = true, int? timeout = null, CommandType? commandtype = null) where T : class
        {
            IEnumerable<T> result = new List<T>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    result = SqlMapper.Query<T>(connection, spname,
                                                 param: (object)parameter,
                                                 transaction: transaction,
                                                 commandTimeout: timeout,
                                                 commandType: commandtype);
                    connection.Close();

                }
                catch (SqlException ex)
                {
                    connection.Close();
					Helpers.ApplicationExceptions.SaveAppError(ex);
					throw ex;
                }
            };
            return result;
        }
    }
}
