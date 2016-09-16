namespace LLCryptoLib.Security.Certificates
{
    /// <summary>
    ///     Certificate file format
    /// </summary>
    public enum CertificateFormat
    {
        /// <summary>
        ///     Base64
        /// </summary>
        BASE64,

        /// <summary>
        ///     DER
        /// </summary>
        DER,

        /// <summary>
        ///     PEM
        /// </summary>
        PEM,

        /// <summary>
        ///     PKCS7
        /// </summary>
        PKCS7,

        /// <summary>
        ///     PKCS12
        /// </summary>
        PKCS12,

        /// <summary>
        ///     Unknown format
        /// </summary>
        UNKNOWN
    }
}