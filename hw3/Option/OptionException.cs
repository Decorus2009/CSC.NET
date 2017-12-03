using System;
using System.Runtime.Serialization;

namespace Option
{
    [Serializable]
    public class OptionException : Exception
    {
        public OptionException()
        {
        }

        public OptionException(string message) : base(message)
        {
        }

        public OptionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected OptionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {    
        }
    }
}