using System;
using System.Text;

namespace LLCryptoLib.Security.Certificates
{
    /// <summary>
    ///     Revocation item structure class.
    ///     It associates a certificate with a boolean value (revoked or not).
    /// </summary>
    [Serializable]
    public class RevocationItem
    {
        private string certificateID;
        private string certificateOwner;
        private bool revokedStatus;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RevocationItem" /> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="revoked">if set to <c>true</c> [revoked].</param>
        public RevocationItem(string id, string owner, bool revoked)
        {
            this.certificateID = id;
            this.certificateOwner = owner;
            this.revokedStatus = revoked;
        }

        /// <summary>
        ///     Gets or sets the certificate serial number.
        /// </summary>
        /// <value>The certificate serial number.</value>
        public string Certificate
        {
            get { return this.certificateID; }

            set { this.certificateID = value; }
        }

        /// <summary>
        ///     Gets or sets the certificate owner.
        /// </summary>
        /// <value>The certificate owner.</value>
        public string Owner
        {
            get { return this.certificateOwner; }

            set { this.certificateOwner = value; }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this Certificate is revoked.
        /// </summary>
        /// <value><c>true</c> if revoked; otherwise, <c>false</c>.</value>
        public bool Revoked
        {
            get { return this.revokedStatus; }

            set { this.revokedStatus = value; }
        }

        /// <summary>
        ///     Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            StringBuilder rName = new StringBuilder(this.Owner);
            rName.Append(" [");
            rName.Append(this.Certificate);
            rName.Append("]");
            return rName.ToString();
        }
    }
}