using System;
using System.Runtime.Serialization;

namespace LLCryptoLib.Security
{
    /// <summary>
    ///     The exception that is thrown when a security error is detected.
    /// </summary>
    [Serializable]
    public class SecurityException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the SecurityException class with default properties.
        /// </summary>
        public SecurityException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the SecurityException class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public SecurityException(string message) : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the SecurityException class with a specified error message and a reference to the
        ///     inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">
        ///     The exception that is the cause of the current exception. If the <paramref name="inner" />
        ///     parameter is not a null reference (<b>Nothing</b> in Visual Basic), the current exception is raised in a catch
        ///     block that handles the inner exception.
        /// </param>
        public SecurityException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the SecurityException class with serialized data.
        /// </summary>
        /// <param name="info">
        ///     The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being
        ///     thrown.
        /// </param>
        /// <param name="context">
        ///     The <see cref="StreamingContext" /> that contains contextual information about the source or
        ///     destination.
        /// </param>
        protected SecurityException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}