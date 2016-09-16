using System;
using System.Security.Cryptography;
using LLCryptoLib.Security.Resources;

namespace LLCryptoLib.Security.Cryptography
{
    /// <summary>
    ///     Represents the abstract class from which all implementations of the MD4 hash algorithm inherit.
    /// </summary>
    /// <remarks>
    ///     Warning: The MD4 algorithm is a broken algorithm. It should <i>only</i> be used for compatibility with older
    ///     systems.
    /// </remarks>
    public abstract class MD4 : HashAlgorithm
    {
        /// <summary>
        ///     Initializes a new instance of <see cref="MD4" />.
        /// </summary>
        /// <remarks>
        ///     You cannot create an instance of an abstract class. Application code will create a new instance of a derived
        ///     class.
        /// </remarks>
        protected MD4()
        {
            this.HashSizeValue = 128;
        }

        /// <summary>
        ///     Creates an instance of the default implementation of the <see cref="MD4" /> hash algorithm.
        /// </summary>
        /// <returns>A new instance of the MD4 hash algorithm.</returns>
        public new static MD4 Create()
        {
            return Create("MD4");
        }

        /// <summary>
        ///     Creates an instance of the specified implementation of the <see cref="MD4" /> hash algorithm.
        /// </summary>
        /// <param name="hashName">The name of the specific implementation of MD4 to use.</param>
        /// <returns>A new instance of the specified implementation of MD4.</returns>
        /// <exception cref="CryptographicException">An error occurs while initializing the hash.</exception>
        public new static MD4 Create(string hashName)
        {
            if (string.Equals(hashName, "MD4", StringComparison.InvariantCultureIgnoreCase) ||
                string.Equals(hashName, "LLCryptoLib.Security.cryptography.md4cryptoserviceprovider",
                    StringComparison.InvariantCultureIgnoreCase))
                return new MD4CryptoServiceProvider();
            throw new ArgumentException(ResourceController.GetString("Error_ParamInvalid"));
        }
    }
}