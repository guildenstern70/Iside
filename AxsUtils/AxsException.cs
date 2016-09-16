using System;
using System.Runtime.Serialization;

namespace AxsUtils
{
    /// <summary>
    ///     AxsException.
    /// </summary>
    [Serializable]
    public class AxsException : Exception
    {
        public AxsException()
            : base("AxsUtils General Exception")
        {
        }

        public AxsException(string message)
            : base(message)
        {
        }

        public AxsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected AxsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}