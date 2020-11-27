using System.Collections.Generic;
using System.Linq;
using GenerateSqlScripts.Core;

namespace GenerateSqlScripts
{
    public static class ArgParser
    {
        public static void Parse(string[] args, ref Dictionary<ProgramArg, string> argDictionary)
        {
            for (int i = 0; i < args.Length; i++)
            {
                // If arg is empty or doesn't start with '-' or "--", do nothing
                if (string.IsNullOrEmpty(args[i])) continue;
                if (!IsSingleDash(args[i]) && !IsDoubleDash(args[i])) continue;
                
                // -arg argValue
                if (IsSingleDash(args[i]))
                {
                    // Check that argValue's index isn't out of the array's range
                    if (i + 1 < args.Length)
                    {
                        // Get the argument type from the argument string name, without the '-' character
                        var argType = GetArgType(args[i][1..]);

                        // Arg type doesn't exist
                        if (argType == null) continue;

                        // Arg value should exist since we checked that the range is valid
                        var argValue = args[i + 1];
                        
                        argDictionary.Add((ProgramArg)argType, argValue);
                    }
                }
                
                // --arg
                if (IsDoubleDash(args[i]))
                {
                    // Get the argument type from the argument string name, without the "--" string
                    var argType = GetArgType(args[i][2..]);

                    // Arg type doesn't exist
                    if (argType == null) continue;
                    
                    argDictionary.Add((ProgramArg)argType, null);
                }
            }

        }

        private static bool IsSingleDash(string arg) => arg[0] == '-' && arg[1] != '-';
        private static bool IsDoubleDash(string arg) => arg[..2] == "--";

        private static ProgramArg? GetArgType(string arg) => 
            arg switch
            {
                "version" => ProgramArg.Version,
                "db" => ProgramArg.DbName,
                "u" => ProgramArg.DbUsername,
                "pw" => ProgramArg.DbPassword,
                "integrated" => ProgramArg.UseWindowsAuth,
                _ => null
            };
    }
}