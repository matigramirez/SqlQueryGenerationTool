using System;

namespace GenerateSqlScripts.Core.Exceptions
{
    public class NotInitializedException : Exception
    {
        public override string Message { get; }

        public NotInitializedException(string message)
        {
            Message = message;
        }
    }
}