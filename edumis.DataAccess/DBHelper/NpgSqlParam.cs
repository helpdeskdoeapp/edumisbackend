using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edumis.DataAccess.DBHelper
{
    public class NpgSqlParam
    {
        #region Properties Definition
        /// <summary>
        /// Parameter name
        /// </summary>
        public string ParamName { get; set; }

        /// <summary>
        /// Parameter Value (may be string, int, bool etc.)
        /// </summary>
        public object ParamValue { get; set; }

        /// <summary>
        /// Size or lingth of the parameter
        /// </summary>
        public int ParamLength { get; set; }

        /// <summary>
        /// Direction in out (Default is Input)
        /// </summary>
        private ParameterDirection paramDirection = ParameterDirection.Input;
        public ParameterDirection ParamDirection { get { return paramDirection; } set { paramDirection = value; } }

        /// <summary>
        /// Type of parameter(Default is varchar)
        /// </summary>
        private NpgsqlDbType dbType = NpgsqlDbType.Varchar;
        public NpgsqlDbType DBType { get { return dbType; } set { dbType = value; } }

        /// <summary>
        /// Type of parameter (Default is  Text)
        /// </summary>
        private NpgsqlDbType sqldbType = NpgsqlDbType.Text;
        public NpgsqlDbType SqlDbType { get { return sqldbType; } set { sqldbType = value; } }
        #endregion

        #region All Constructors
        public NpgSqlParam() { }

        public NpgSqlParam(string paramName, object paramValue)
        {
            this.ParamName = paramName;
            this.ParamValue = paramValue;
        }

        public NpgSqlParam(string paramName, NpgsqlDbType npgsqlDbType, object paramValue)
        {
            this.ParamName = paramName;
            this.ParamValue = paramValue;
            this.DBType = npgsqlDbType;
        }

        public NpgSqlParam(string paramName, NpgsqlDbType npgsqlDbType, object paramValue, ParameterDirection paramDirection)
        {
            this.ParamName = paramName;
            this.ParamValue = paramValue;
            this.DBType = npgsqlDbType;
            this.ParamDirection = paramDirection;
        }

        public NpgSqlParam(string paramName, NpgsqlDbType npgsqlDbType, object paramValue, int paramLength)
        {
            this.ParamName = paramName;
            this.ParamValue = paramValue;
            this.DBType = npgsqlDbType;
            this.ParamLength = paramLength;
        }

        public NpgSqlParam(string paramName, NpgsqlDbType npgsqlDbType, object paramValue, ParameterDirection paramDirection, int paramLength)
        {
            this.ParamName = paramName;
            this.ParamValue = paramValue;
            this.DBType = npgsqlDbType;
            this.ParamDirection = paramDirection;
            this.ParamLength = paramLength;
        }

        #endregion

        #region Clear All Values in case defaults are required
        internal void ClearAllValues()
        {
            ParamName = string.Empty;
            dbType = NpgsqlDbType.Varchar;
            ParamValue = null;
            paramDirection = ParameterDirection.Input;
            ParamLength = 0;
        }
        #endregion
    }
}
