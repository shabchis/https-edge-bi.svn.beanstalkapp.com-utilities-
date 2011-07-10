using System;
using System.Data;
using System.Data.Common;

namespace BackofficeDemo
{
    public static class DbExtentions
    {
        public static void AddParameter(this DbCommand dbCmd, ParameterDirection direction, DbType dbType, string name, Object value)
        {
            DbParameter param = dbCmd.CreateParameter();
            param.Direction = direction;
            param.DbType = dbType;
            param.ParameterName = name;
            param.Value = value;
            dbCmd.Parameters.Add(param);
        }

        public static void AddParameter(this DbCommand dbCmd, DbType dbType, string name, Object value)
        {
            AddParameter(dbCmd, ParameterDirection.Input, dbType, name, value);
        }
    }
}
