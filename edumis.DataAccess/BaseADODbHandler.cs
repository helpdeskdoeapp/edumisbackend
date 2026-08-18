using edumis.DataAccess.DBHelper;
using edumis.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;

namespace edumis.DataAccess
{
    public class BaseADODbHandler
    {
        protected string ConnectionString;
        private readonly ApplicationDBContext dbContext;
        protected BaseADODbHandler(ApplicationDBContext applicationDBContext)
        {
            dbContext = applicationDBContext;
            ConnectionString = applicationDBContext.Database.GetDbConnection().ConnectionString;
        }


        protected NpgsqlConnection GetPGSQLNpgsqlConnection(string? ConStr = null)
        {
            return string.IsNullOrEmpty(ConStr) ? new NpgsqlConnection(ConnectionString) : new NpgsqlConnection(ConStr);
        }
        protected void CloseConnection(NpgsqlCommand command)
        {
            if (command != null && command.Connection.State == ConnectionState.Open)
            {
                command.Connection.Close();
            }
        }

        protected void OpenConnection(NpgsqlCommand command)
        {
            if (command != null && command.Connection.State == ConnectionState.Closed)
            {
                command.Connection.Open();
            }
        }

        #region Execute Stored Procedures
        public async Task<bool> ExecStoredProcedureWithTrans(string storedProcName, ParamHelper paramList, ErrorModel? error)
        {
            using (NpgsqlConnection con = GetPGSQLNpgsqlConnection())
            {
                con.Open();
                NpgsqlTransaction npgsqlTransaction = con.BeginTransaction();
                using (NpgsqlCommand command = new NpgsqlCommand(storedProcName, con))
                {
                    command.CommandText = storedProcName;
                    command.CommandType = CommandType.Text;
                    try
                    {
                        if (paramList != null)
                        {
                            foreach (var parameter in paramList)
                            {
                                command.Parameters.AddWithValue(parameter.ParamName, parameter.DBType, parameter.ParamValue);
                            }
                        }
                        int result = await command.ExecuteNonQueryAsync();
                        await npgsqlTransaction.CommitAsync();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        error = new ErrorModel()
                        {
                            Message = ex.Message,
                            InnerExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : string.Empty,
                            StackTrace = ex.StackTrace
                        };

                        await npgsqlTransaction.RollbackAsync();
                        throw ex;
                        //return false;
                    }
                    finally
                    {
                        CloseConnection(command);
                    }
                }
            }
        }

        public async Task<object?> ExecNonQueryTransSingle(string storedProcName, ParamHelper paramList, ErrorModel error)
        {
            object? result = null;
            using (NpgsqlConnection con = GetPGSQLNpgsqlConnection())
            {
                con.Open();
                NpgsqlTransaction npgsqlTransaction = con.BeginTransaction();
                using (NpgsqlCommand command = new NpgsqlCommand(storedProcName, con))
                {
                    command.CommandText = storedProcName;
                    command.CommandType = CommandType.Text;
                    try
                    {
                        if (paramList != null)
                        {
                            foreach (var parameter in paramList)
                            {
                                command.Parameters.AddWithValue(parameter.ParamName, parameter.DBType, parameter.ParamValue);
                            }
                        }
                        result = await command.ExecuteScalarAsync();
                        await npgsqlTransaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        error = new ErrorModel()
                        {
                            Message = ex.Message,
                            InnerExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : string.Empty,
                            StackTrace = ex.StackTrace
                        };

                        await npgsqlTransaction.RollbackAsync();
                        throw ex;
                    }
                    finally
                    {
                        CloseConnection(command);
                    }
                }
            }
            return result;
        }
        #endregion

        #region Reader Methods
        public async Task<List<T>> ExecuteDBQuery<T>(string SqlQuery, ParamHelper paramList, Func<NpgsqlDataReader, List<T>> PopulateDataFromReader, ErrorModel? error)
        {
            NpgsqlConnection con = GetPGSQLNpgsqlConnection();
            NpgsqlDataReader ResultReader = null;
            List<T> ResultList = new List<T>();
            using (NpgsqlCommand cmd = new NpgsqlCommand(SqlQuery, con))
            {
                cmd.CommandType = CommandType.Text;
                if (paramList != null)
                {
                    foreach (var param in paramList)
                    {
                        cmd.Parameters.AddWithValue(param.ParamName, param.DBType, param.ParamValue);
                    }
                }

                try
                {
                    OpenConnection(cmd);
                    ResultReader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleResult);
                    ResultList = PopulateDataFromReader(ResultReader);
                }
                catch (Exception ex)
                {
                    error = new ErrorModel()
                    {
                        Message = ex.Message,
                        InnerExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : string.Empty,
                        StackTrace = ex.StackTrace
                    };
                    throw ex;
                }
                finally
                {
                    CloseConnection(cmd);
                }

            }
            return ResultList;
        }
        public async Task<List<T>> ExecuteSPReader<T>(string StoredProcName, ParamHelper paramList, Func<NpgsqlDataReader, List<T>> PopulateDataFromReader, ErrorModel? error)
        {
            NpgsqlConnection con = GetPGSQLNpgsqlConnection();
            NpgsqlDataReader ResultReader = null;
            List<T> ResultList = new List<T>();
            using (NpgsqlCommand cmd = new NpgsqlCommand(StoredProcName, con))
            {
                cmd.CommandType = CommandType.Text;
                if (paramList != null)
                {
                    foreach (var param in paramList)
                    {
                        cmd.Parameters.AddWithValue(param.ParamName, param.DBType, param.ParamValue);
                    }
                }

                try
                {
                    OpenConnection(cmd);
                    ResultReader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleResult);
                    ResultList = PopulateDataFromReader(ResultReader);
                }
                catch (Exception ex)
                {
                    error = new ErrorModel()
                    {
                        Message = ex.Message,
                        InnerExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : string.Empty,
                        StackTrace = ex.StackTrace
                    };
                    throw ex;
                }
                finally
                {
                    CloseConnection(cmd);
                }

            }
            return ResultList;
        }
        #endregion
    }
}
