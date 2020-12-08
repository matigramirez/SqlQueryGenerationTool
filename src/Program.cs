using System;
using System.Collections.Generic;
using GenerateSqlScripts.Core;

namespace GenerateSqlScripts
{
    class Program
    {
        private static Dictionary<ProgramArg, string> _programArgs = new Dictionary<ProgramArg, string>();
        
        static void Main(string[] args)
        {
            ArgParser.Parse(args, ref _programArgs);

            if (_programArgs.TryGetValue(ProgramArg.Version, out _))
            {
                Console.WriteLine("sqgt v1.0");
                return;
            }

            if (!_programArgs.TryGetValue(ProgramArg.DbName, out var dbName))
            {
                Console.WriteLine("Must provide database name.");
                return;
            }

            if (!_programArgs.ContainsKey(ProgramArg.UseWindowsAuth))
            {
                _programArgs.TryGetValue(ProgramArg.DbUsername, out var dbUsername);
                _programArgs.TryGetValue(ProgramArg.DbPassword, out var dbPassword);

                if (string.IsNullOrEmpty(dbUsername) || string.IsNullOrEmpty(dbPassword))
                {
                    Console.WriteLine("Must provide database username and password");
                    return;
                }

                Console.WriteLine($"Connecting to database with user: {dbUsername} and pw: {dbPassword}");
                    
                DatabaseHelper.Initialize(dbName, dbUsername, dbPassword);
            }
            else
            {
                Console.WriteLine("Connecting to database with windows authentication...");
                DatabaseHelper.Initialize(dbName);
            }

            var tables = DatabaseHelper.GetTablesFromDatabase(dbName);

            Console.WriteLine("Select tables");
            Console.WriteLine("You can select multiple by separating values with ',' (1,2,3,..):");
            Console.WriteLine("0: All");
            
            for (int i = 0; i < tables.Length; i++)
            {
                Console.WriteLine($"{ i+1 }: {tables[i]}");
            }
            
            Console.Write("Selected table/s: ");

            string selectedTablesString = Console.ReadLine();

            if (selectedTablesString?.Length == 1)
            {
                // 0 = All
                if (selectedTablesString == "0")
                {
                    foreach (var table in tables)
                    {
                        CreateScriptForTable(table);
                    }
                }
                else
                {
                    // Non 0 values
                    bool isInt = int.TryParse(selectedTablesString, out int index);
                
                    if(isInt) CreateScriptForTable(tables[index-1]);
                }
            }
            else
            {
                foreach (var num in GetNumbersFromCommaSeparatedString(selectedTablesString))
                {
                    CreateScriptForTable(tables[num - 1]);
                }
            }
        }

        static void CreateScriptForTable(string tableName)
        {
            var columns = DatabaseHelper.GetColumnsFromTable(tableName);
                
            var insertScript = ScriptGenerator.Generate(ScriptType.Insert, tableName, columns);
            
            try
            {
                var fileName = $"{tableName}_Insert.sql";
                FileWriter.SaveFileInCurrentDirectory(fileName,insertScript);
                Console.WriteLine($"{fileName} created.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        static IEnumerable<int> GetNumbersFromCommaSeparatedString(string str)
        {
            foreach (string s in str.Split(','))
            {
                if (int.TryParse(s, out int num))
                {
                    yield return num;
                }
            }
        }
    }
}