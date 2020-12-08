using System;
using System.Text;

namespace GenerateSqlScripts
{
    public enum ScriptType
    {
        Insert,
        Update
    }
    
    public static class ScriptGenerator
    {
        public static string Generate(ScriptType type, string tableName, string[] columnNames)
        {
            if (columnNames == null || columnNames.Length < 1)
            {
                throw new ArgumentException("Columns can't be empty.");
            }

            if (tableName == null) throw new ArgumentException("Table can't be null.");

            return type switch
            {
                ScriptType.Insert => GenerateInsert(tableName, columnNames),
                ScriptType.Update => GenerateUpdate(tableName, columnNames),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Type must be provided.")
            };
        }

        private static string GenerateInsert(string tableName, string[] columnNames)
        {
            var insertString = new StringBuilder();

            insertString.Append($"INSERT INTO {tableName} (");

            foreach (var columnName in columnNames)
            {
                insertString.Append(columnName + ", ");
            }

            insertString[^2] = ')';
            insertString[^1] = '\n';

            insertString.Append("VALUES (");
            
            foreach (var columnName in columnNames)
            {
                insertString.Append($"@{columnName}, ");
            }
            
            insertString[^2] = ')';
            insertString[^1] = '\n';

            insertString.Append("SELECT CAST(SCOPE_IDENTITY() AS INT)");
            insertString[^1] = '\n';

        return insertString.ToString();
        }

        private static string GenerateUpdate(string tableName, string[] columns)
        {
            return "";
        }
    }
}