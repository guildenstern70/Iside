using System;
using System.Runtime.Serialization;

namespace LLCryptoLib
{
    /// <summary>
    ///     LLCryptoLibException.
    /// </summary>
    [Serializable]
    public class LLCryptoLibException : Exception
    {
        /// <summary>
        ///     LLCryptoLibException object
        /// </summary>
        public LLCryptoLibException() : base("LLCryptoLib General Exception")
        {
        }

        /// <summary>
        ///     LLCryptoLibException object
        /// </summary>
        /// <param name="message">Exception message</param>
        public LLCryptoLibException(string message) : base(message)
        {
        }

        /// <summary>
        ///     LLCryptoLibException object
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception object</param>
        public LLCryptoLibException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        ///     LLCryptoLibException object
        /// </summary>
        /// <param name="info">Serialization Info handle</param>
        /// <param name="context">Streaming context</param>
        protected LLCryptoLibException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}