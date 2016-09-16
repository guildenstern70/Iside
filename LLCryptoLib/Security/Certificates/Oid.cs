namespace LLCryptoLib.Security.Certificates
{
    /// <summary>
    ///     OID
    /// </summary>
    public class Oid
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Oid" /> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="description">The description.</param>
        public Oid(string id, string description)
        {
            this.OidId = id;
            this.Description = description;
        }

        /// <summary>
        ///     Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; }

        /// <summary>
        ///     Gets the oid id.
        /// </summary>
        /// <value>The oid id.</value>
        public string OidId { get; }

        /// <summary>
        ///     Gets the server authentication.
        /// </summary>
        /// <value>The server authentication.</value>
        public static Oid ServerAuthentication
        {
            get { return new Oid("1.3.6.1.5.5.7.3.1", "Server authentication"); }
        }

        /// <summary>
        ///     Gets the client authentication.
        /// </summary>
        /// <value>The client authentication.</value>
        public static Oid ClientAuthentication
        {
            get { return new Oid("1.3.6.1.5.5.7.3.2", "Client authentication"); }
        }

        /// <summary>
        ///     Gets the code signing.
        /// </summary>
        /// <value>The code signing.</value>
        public static Oid CodeSigning
        {
            get { return new Oid("1.3.6.1.5.5.7.3.3", "Code signing"); }
        }
    }

    /// <summary>
    ///     An OID container class
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
         "CA1053:StaticHolderTypesShouldNotHaveConstructors")]
    public class OidContainer
    {
        private static readonly Oid[] oids =
        {
            // Algorithms	
            new Oid("1.2.840.113549.2.2", "MD2"),
            new Oid("1.2.840.113549.2.5", "MD5"),
            new Oid("1.3.36.3.2.1", "RIPEMD_160"),
            new Oid("1.3.14.3.2.26", "SHA-1"),
            new Oid("1.2.840.10040.4.1", "ANSI DSA"),
            new Oid("1.2.840.10046.2.1", "dh KeyAgreement (deprecated)"),
            new Oid("1.2.840.10046.2.1", "dh KeyAgreementx942"),
            new Oid("1.2.840.113549.1.1.1", "RSA Encryption"),
            new Oid("1.2.840.10045.2.1", "ANSI ECDSA"),
            new Oid("1.2.372.980001.4.1", "OAEPMsgFormatWithSHA-1 (Baltimore)"),
            new Oid("1.2.372.980001.1.2.1.3", "Baltimore_OID_Rijndael_CBC"),
            new Oid("1.2.372.980001.1.2.1.4", "Baltimore_OID_Rijndael_CBC_PAD"),
            new Oid("1.2.372.980001.1.2.1.1", "Baltimore_OID_Rijndael_ECB"),
            new Oid("1.2.372.980001.1.2.1.2", "Baltimore_OID_Rijndael_ECB_PAD"),
            new Oid("1.3.14.3.2.7", "DESCBC"),
            new Oid("1.3.36.3.1.1.1", "DESECB_PAD"),
            new Oid("1.2.840.10046.1.2", "DES3CBC"),
            new Oid("1.2.840.113549.3.7", "DES3EDECBC"),
            new Oid("1.2.840.113549.3.2", "RC2CBC"),
            new Oid("1.2.840.113549.3.4", "RC4"),
            new Oid("1.2.840.10040.4.3", "DSAWithSHA-1"),
            new Oid("1.2.840.113549.1.1.2", "MD2WithRSAEncryption"),
            new Oid("1.3.14..3.2.24", "MD2 with RSA signature"),
            new Oid("1.2.840.113549.1.1.4", "MD5 With RSA Encryption"),
            new Oid("1.3.14..3.2.25", "MD5 With RSA Signature"),
            new Oid("1.2.840.113549.1.1.5", "SHA-1 With RSA Encryption"),
            new Oid("1.3.14..3.2.29", "SHA-1 With RSA Signature"),
            new Oid("1.2.372.980001.1.3.98", "TLS With RSA Encryption"),
            new Oid("1.2.840.10045.4.1", "ANSI ECDSA SHA-1"),
            new Oid("1.2.372.980001.4.2", "BaltimoreMofNShamir"),
            new Oid("1.2.372.980001.4.4", "BaltimoreXORSplitting"),
            new Oid("1.2.372.980001.1.2.1.3", "Baltimore_OID_Rijndael_CBC"),
            new Oid("1.2.372.980001.1.2.1.4", "Baltimore_OID_Rijndael_CBC_PAD"),
            new Oid("1.2.372.980001.1.2.1.1", "Baltimore_OID_Rijndael_ECB"),
            new Oid("1.2.372.980001.1.2.1.2", "Baltimore_OID_Rijndael_ECB_PAD"),
            new Oid("1.3.14.3.2.7", "DES CBC"),
            new Oid("1.3.36.3.1.1.1", "DES ECB_PAD"),
            new Oid("1.2.840.10046.1.2", "DES3CBC"),
            new Oid("1.2.840.113549.3.7", "DES3EDECBC"),
            new Oid("1.3.36.3.1.3.1.1", "DES3ECB_PAD"),
            new Oid("1.2.840.113549.3.2", "RC2 CBC"),
            new Oid("1.2.840.113549.3.4", "RC4"),
            new Oid("1.3.6.1.4.1.188.7.1.1.1", "IDEA ECB"),
            new Oid("1.3.6.1.4.1.188.7.1.1.2", "IDEA CBC"),
            new Oid("1.2.840.113549.1.1.1", "RSA Encryption"),
            new Oid("1.2.840.113549.1.1.7", "RSA Encryption With OAEP Padding"),
            new Oid("1.2.840.113549.1.1.6", "RSA Encryption With OAEP PaddingSET"),
            new Oid("1.2.840.113549.1.5.3", "pbe With MD5 And DES CBC"),
            new Oid("1.2.840.113549.1.12.1.1", "pbe With SHA1 And 128 Bit RC4"),
            new Oid("1.2.840.113549.1.12.1.2", "pbe With SHA1 And 40 Bit RC4"),
            new Oid("1.2.840.113549.1.12.1.3", "pbe With SHA1 And 3_Key TripleDES CBC"),
            new Oid("1.2.840.113549.1.12.1.4", "pbe With SHA1 And 2_Key TripleDES CBC"),
            new Oid("1.2.840.113549.1.12.1.5", "pbe With SHA1 And 128Bit RC2 CBC"),
            new Oid("1.2.840.113549.1.12.1.6", "pbe With SHA1 And 40Bit RC2 CBC"),
            new Oid("1.2.840.113533.7.66.12", "pbe With MD5 And CAST5 CBC"),
            // Usages
            new Oid("1.3.6.1.5.5.7.3.1", "Server authentication"),
            new Oid("1.3.6.1.5.5.7.3.2", "Client authentication"),
            new Oid("1.3.6.1.5.5.7.3.3", "Code signing"),
            new Oid("1.3.6.1.5.5.7.3.4", "Email"),
            new Oid("1.3.6.1.5.5.7.3.5", "IPSec end system"),
            new Oid("1.3.6.1.5.5.7.3.6", "IPSec tunnel"),
            new Oid("1.3.6.1.5.5.7.3.7", "IPSec user"),
            new Oid("1.3.6.1.5.5.7.3.8", "Timestamping"),
            new Oid("1.3.6.1.5.5.7.3.9", "OCSP Signing")
        };

        /// <summary>
        ///     Froms the string.
        /// </summary>
        /// <param name="s">A string containing OID</param>
        /// <returns>The OID read from the string</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static Oid FromString(string s)
        {
            foreach (Oid o in oids)
            {
                if (s.IndexOf(o.Description) != -1)
                {
                    return o;
                }
            }

            return null;
        }

        /// <summary>
        ///     Gets the usages.
        /// </summary>
        /// <returns>The usages of this OID</returns>
        public static string[] GetUsages()
        {
            return new string[10]
            {
                "1.3.6.1.5.5.7.3.1 - Server authentication", "1.3.6.1.5.5.7.3.2 - Client authentication",
                "1.3.6.1.5.5.7.3.3 - Code signing", "1.3.6.1.5.5.7.3.4 - Email",
                "1.3.6.1.5.5.7.3.5 - IPSec end system", "1.3.6.1.5.5.7.3.6 - IPSec tunnel",
                "1.3.6.1.5.5.7.3.7 - IPSec user", "1.3.6.1.5.5.7.3.8 - Timestamping",
                "1.3.6.1.5.5.7.3.9 - OCSP Signing", "Unknown"
            };
        }

        /// <summary>
        ///     Gets the description.
        /// </summary>
        /// <param name="oid">The oid.</param>
        /// <returns>The description of the OID</returns>
        public static string GetDescription(string oid)
        {
            foreach (Oid o in oids)
            {
                if (o.OidId.Equals(oid))
                {
                    return o.Description;
                }
            }

            return "Unknown";
        }
    }
}