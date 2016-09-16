/*
 * LLCryptoLib - Advanced .NET Encryption and Hashing Library
 * v.$id$
 * 
 * The contents of this file are subject to the license distributed with
 * the package (the License). This file cannot be distributed without the 
 * original LittleLite Software license file. The distribution of this
 * file is subject to the agreement between the licensee and LittleLite
 * Software.
 * 
 * Customer that has purchased Source Code License may alter this
 * file and distribute the modified binary redistributables with applications. 
 * Except as expressly authorized in the License, customer shall not rent,
 * lease, distribute, sell, make available for download of this file. 
 * 
 * This software is not Open Source, nor Free. Its usage must adhere
 * with the License obtained from LittleLite Software.
 * 
 * The source code in this file may be derived, all or in part, from existing
 * other source code, where the original license permit to do so.
 * 
 * Copyright (C) 2003-2014 LittleLite Software
 * 
 */

using System;
using System.Runtime.Serialization;

namespace LLCryptoLib.Security
{
    /// <summary>
    /// The exception that is thrown when a security error is detected.
    /// </summary>
    [Serializable]
    public class SecurityException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the SecurityException class with default properties.
        /// </summary>
        public SecurityException() : base() { }
        /// <summary>
        /// Initializes a new instance of the SecurityException class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public SecurityException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the SecurityException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception. If the <paramref name="inner"/> parameter is not a null reference (<b>Nothing</b> in Visual Basic), the current exception is raised in a catch block that handles the inner exception.</param>
        public SecurityException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Initializes a new instance of the SecurityException class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected SecurityException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}