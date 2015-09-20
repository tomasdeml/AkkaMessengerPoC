using System;

namespace AkkaMessenger.Runtime
{
    class Failure : Result
    {
        public string ErrorMessage { get; } 
        public Exception Exception { get; }

        public Failure(string errorMessage, Exception exception = null)
        {
            ErrorMessage = errorMessage;
            Exception = exception;
        }

        public override string ToString()
        {
            return ErrorMessage + Environment.NewLine + Exception;
        } 
    }
}