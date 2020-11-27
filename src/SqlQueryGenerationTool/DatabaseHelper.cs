using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using GenerateSqlScripts.Core.Exceptions;

namespace GenerateSqlScripts
{
    public static class DatabaseHelper
    {
        public static string DbName { get; set; }
        public static bool IsIntegratedSecurity { get; set; }
        public static string DbUsername { get; set; }
        public static string DbPassword { get; set; }

        private static bool _isInitialized =>
            !string.IsNullOrEmpty(DbName) &&
            (IsIntegratedSecurity || (!string.IsNullOrEmpty(DbUsername) && !string.IsNullOrEmpty(DbPassword)));

        public static void Initialize(string dbName, string dbUsername = "", string dbPassword = "", bool isIntegratedSecurity = true)
        {
            DbName = dbName;
            DbUsername = dbUsername;
            DbPassword = dbPassword;
            IsIntegratedSecurity = isIntegratedSecurity;
        }
        
        private static string GetConnectionString()
        {
            if(!_isInitialized) throw new NotInitializedException("Database credentials must be provided before executing any action.");
            
            var connectionString = $"Server=.;Database={DbName};Integrated Security={IsIntegratedSecurity};";

            return connectionString;
        }
        
        public static string[] GetTablesFromDatabase(string dbName)
        {
            if(!_isInitialized) throw new NotInitializedException("Database credentials must be provided before executing any action.");
            
            using var con = new SqlConnection(GetConnectionString());

            con.Open();

            var schemaTables = con.GetSchema("Tables");

            var tables = from DataRow row in schemaTables.Rows select (string) row[2];
            
            con.Close();

            return tables.ToArray();
        }

        public static string[] GetColumnsFromTable(string tableName)
        {
            using var con = new SqlConnection(GetConnectionString());

            var tableColumns = new List<string>();
            
            con.Open();

            string[] restrictionsColumns = new string[4];
            restrictionsColumns[2] = tableName;
            
            var schemaColumns = con.GetSchema("Columns", restrictionsColumns);

            foreach (System.Data.DataRow rowColumn in schemaColumns.Rows)
            {
                tableColumns.Add(rowColumn[3].ToString());
            }
            
            con.Close();

            return tableColumns.ToArray();
        }
    }
}