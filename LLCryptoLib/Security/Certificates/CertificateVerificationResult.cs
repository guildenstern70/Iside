using System;
using System.Threading;

namespace LLCryptoLib.Security.Certificates
{
    /// <summary>
    ///     Represents the status of an asynchronous certificate chain verification operation.
    /// </summary>
    internal class CertificateVerificationResult : IAsyncResult
    {
        /// <summary>Holds the value of the AsyncState property.</summary>
        private readonly object m_AsyncState;

        /// <summary>Holds the value of the Callback property.</summary>
        private readonly AsyncCallback m_Callback;

        /// <summary>Holds the value of the Chain property.</summary>
        private readonly CertificateChain m_Chain;

        /// <summary>Holds the value of the Flags property.</summary>
        private readonly VerificationFlags m_Flags;

        /// <summary>Holds the value of the HasEnded property.</summary>
        private bool m_HasEnded;

        /// <summary>Holds the value of the IsCompleted property.</summary>
        private bool m_IsCompleted;

        /// <summary>Holds the value of the Server property.</summary>
        private readonly string m_Server;

        /// <summary>Holds the value of the Status property.</summary>
        private CertificateStatus m_Status;

        /// <summary>Holds the value of the ThrowException property.</summary>
        private Exception m_ThrowException;

        /// <summary>Holds the value of the Type property.</summary>
        private readonly AuthType m_Type;

        /// <summary>Holds the value of the WaitHandle property.</summary>
        private ManualResetEvent m_WaitHandle;

        /// <summary>
        ///     Initializes a new CertificateVerificationResult instance.
        /// </summary>
        /// <param name="chain">The <see cref="CertificateChain" /> that has to be verified.</param>
        /// <param name="server">The server to which the <see cref="Certificate" /> has been issued.</param>
        /// <param name="type">One of the <see cref="AuthType" /> values.</param>
        /// <param name="flags">One of the <see cref="VerificationFlags" /> values.</param>
        /// <param name="callback">The delegate to call when the verification finishes.</param>
        /// <param name="asyncState">User-defined state data.</param>
        public CertificateVerificationResult(CertificateChain chain, string server, AuthType type,
            VerificationFlags flags, AsyncCallback callback, object asyncState)
        {
            this.m_Chain = chain;
            this.m_Server = server;
            this.m_Type = type;
            this.m_Flags = flags;
            this.m_AsyncState = asyncState;
            this.m_Callback = callback;
            this.m_WaitHandle = null;
            this.m_HasEnded = false;
        }

        /// <summary>
        ///     Gets the associated certificate chain.
        /// </summary>
        /// <value>
        ///     A <see cref="CertificateChain" /> instance.
        /// </value>
        public CertificateChain Chain
        {
            get { return this.m_Chain; }
        }

        /// <summary>
        ///     Gets the associated server name.
        /// </summary>
        /// <value>
        ///     A string that holds the server name.
        /// </value>
        public string Server
        {
            get { return this.m_Server; }
        }

        /// <summary>
        ///     Gets the associated authentication type.
        /// </summary>
        /// <value>
        ///     One of the <see cref="AuthType" /> values.
        /// </value>
        public AuthType Type
        {
            get { return this.m_Type; }
        }

        /// <summary>
        ///     Gets the associated verification flags.
        /// </summary>
        /// <value>
        ///     One of the <see cref="VerificationFlags" /> values.
        /// </value>
        public VerificationFlags Flags
        {
            get { return this.m_Flags; }
        }

        /// <summary>
        ///     Gets or sets a value that indicates whether the user has called EndVerifyChain for this object.
        /// </summary>
        /// <value>
        ///     <b>true</b> if the user has called EndVerifyChain, <b>false</b> otherwise.
        /// </value>
        public bool HasEnded
        {
            get { return this.m_HasEnded; }
            set { this.m_HasEnded = value; }
        }

        /// <summary>
        ///     Gets an exception that has occurred while verifying the certificate chain or a null reference (<b>Nothing</b> in
        ///     Visual Basic) if the verification succeeded.
        /// </summary>
        /// <value>
        ///     A <see cref="Exception" /> instance.
        /// </value>
        public Exception ThrowException
        {
            get { return this.m_ThrowException; }
        }

        /// <summary>
        ///     Gets the status of the <see cref="CertificateChain" />.
        /// </summary>
        /// <value>
        ///     One of the <see cref="CertificateStatus" /> values.
        /// </value>
        public CertificateStatus Status
        {
            get { return this.m_Status; }
        }

        /// <summary>
        ///     Gets an indication of whether the asynchronous operation completed synchronously.
        /// </summary>
        /// <value>Always <b>false</b>.</value>
        public bool CompletedSynchronously
        {
            get { return false; }
        }

        /// <summary>
        ///     Gets a boolean value that indicates whether the operation has finished.
        /// </summary>
        /// <value>
        ///     <b>true</b> if the verification of the chain has been completed, <b>false</b> otherwise.
        /// </value>
        public bool IsCompleted
        {
            get { return this.m_IsCompleted; }
        }

        /// <summary>
        ///     Gets a <see cref="WaitHandle" /> that is used to wait for an asynchronous operation to complete.
        /// </summary>
        /// <value>
        ///     A WaitHandle that is used to wait for an asynchronous operation to complete.
        /// </value>
        public WaitHandle AsyncWaitHandle
        {
            get
            {
                if (this.m_WaitHandle == null)
                    this.m_WaitHandle = new ManualResetEvent(false);
                return this.m_WaitHandle;
            }
        }

        /// <summary>
        ///     Gets a user-defined object that qualifies or contains information about an asynchronous operation.
        /// </summary>
        /// <value>
        ///     A user-defined object that qualifies or contains information about an asynchronous operation.
        /// </value>
        public object AsyncState
        {
            get { return this.m_AsyncState; }
        }

        /// <summary>
        ///     Sets the WaitHandle to signalled and calls the appropriate delegate.
        /// </summary>
        /// <param name="error">An exception that may have occurred.</param>
        /// <param name="status">The status of the certificate chain.</param>
        internal void VerificationCompleted(Exception error, CertificateStatus status)
        {
            this.m_ThrowException = error;
            this.m_Status = status;
            this.m_IsCompleted = true;
            if (this.m_Callback != null)
                this.m_Callback(this);
            if (this.m_WaitHandle != null)
                this.m_WaitHandle.Set();
        }
    }
}