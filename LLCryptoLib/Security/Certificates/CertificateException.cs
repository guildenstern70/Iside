using System;
using System.Runtime.Serialization;

namespace LLCryptoLib.Security.Certificates
{
    /// <summary>
    ///     The exception that is thrown when a certificate error is detected.
    /// </summary>
    [Serializable]
    public class CertificateException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the CertificateException class with default properties.
        /// </summary>
        public CertificateException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the CertificateException class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public CertificateException(string message) : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the CertificateException class with a specified error message and a reference to the
        ///     inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">
        ///     The exception that is the cause of the current exception. If the inner parameter is not a null
        ///     reference (<b>Nothing</b> in Visual Basic), the current exception is raised in a catch block that handles the inner
        ///     exception.
        /// </param>
        public CertificateException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the CertificateException class with serialized data.
        /// </summary>
        /// <param name="info">
        ///     The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being
        ///     thrown.
        /// </param>
        /// <param name="context">
        ///     The <see cref="StreamingContext" /> that contains contextual information about the source or
        ///     destination.
        /// </param>
        protected CertificateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}