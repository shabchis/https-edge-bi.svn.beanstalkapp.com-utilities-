using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Security.Principal;


namespace BackofficeDemo
{
    public abstract class Database
    {
        protected DbConnection CreateConnection(string dbName)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[dbName];
            string provider = settings.ProviderName;
            string conString = settings.ConnectionString;
            
            
            DbProviderFactory factory = DbProviderFactories.GetFactory(provider);
            DbConnection conn = factory.CreateConnection();
            conn.ConnectionString = conString;

            return conn;
        }

        protected DbCommand CreateCommand(string dbName, string cmdText, CommandType cmdType)
        {
            DbConnection db = CreateConnection(dbName);
            DbCommand command = db.CreateCommand();
            command.CommandText = cmdText;
            command.CommandType = cmdType;
            return command;
        }
    }
}
