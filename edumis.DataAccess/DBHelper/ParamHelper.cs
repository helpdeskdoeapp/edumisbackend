using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edumis.DataAccess.DBHelper
{
    public class ParamHelper : Collection<NpgSqlParam>
    {
        public ParamHelper() : base() { }

        #region Add Parameters to the Collection
        public NpgSqlParam AddParameter(string paramName)
        {
            NpgSqlParam npgSqlParam = new NpgSqlParam();
            npgSqlParam.ParamName = paramName;
            this.Add(npgSqlParam);

            return npgSqlParam;
        }

        public NpgSqlParam AddParameter(string paramName, object paramValue)
        {
            NpgSqlParam npgSqlParam = new NpgSqlParam(paramName, paramValue);
            this.Add(npgSqlParam);

            return npgSqlParam;
        }

        public NpgSqlParam AddParameter(string paramName, NpgsqlDbType dbType, object paramValue)
        {
            NpgSqlParam npgSqlParam = new NpgSqlParam(paramName, dbType, paramValue);
            this.Add(npgSqlParam);

            return npgSqlParam;
        }

        public NpgSqlParam AddParameter(string paramName, NpgsqlDbType dbType, object paramValue, ParameterDirection direction)
        {
            NpgSqlParam npgSqlParam = new NpgSqlParam(paramName, dbType, paramValue, direction);
            this.Add(npgSqlParam);

            return npgSqlParam;
        }

        public NpgSqlParam AddParameter(string paramName, NpgsqlDbType dbType, object paramValue, int paramLength)
        {
            NpgSqlParam npgSqlParam = new NpgSqlParam(paramName, dbType, paramValue, paramLength);
            this.Add(npgSqlParam);

            return npgSqlParam;
        }

        public NpgSqlParam AddParameter(string paramName, NpgsqlDbType dbType, object paramValue, ParameterDirection direction, int paramLength)
        {
            NpgSqlParam npgSqlParam = new NpgSqlParam(paramName, dbType, paramValue, direction, paramLength);
            this.Add(npgSqlParam);

            return npgSqlParam;
        }
        #endregion

        #region Add Output Parameter
        public NpgSqlParam AddOutputParam(string paramName, NpgsqlDbType dbType, int paramLength)
        {
            NpgSqlParam npgSqlParam = new NpgSqlParam(paramName, dbType, null, ParameterDirection.Output, paramLength);
            this.Add(npgSqlParam);
            return npgSqlParam;
        }
        #endregion

        #region Other Methods
        public NpgSqlParam? ParameterItem(string paramName)
        {
            foreach (var item in this)
            {
                if (item.ParamName.ToLower().Equals(paramName.ToLower()))
                    return item;
            }
            return null;
        }

        public NpgSqlParam? ParameterAt(int index)
        {
            if (this.Count > index)
            {
                int i = 0;
                foreach (var obj in this)
                {
                    if (i++ == index)
                        return obj;
                }
            }
            return null;
        }
        #endregion       

        #region Base class Collection Methods overrides
        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);

        }
        protected override void ClearItems()
        {
            base.ClearItems();
        }
        #endregion
    }
}
