using System;
using System.Runtime.Serialization;

namespace AkkaMessenger.Runtime.Recipients
{
    [Serializable]
    public class ParsingFailedException : Exception
    { 
        public ParsingFailedException()
        {
        }

        public ParsingFailedException(string message) : base(message)
        {
        }

        public ParsingFailedException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ParsingFailedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}