using System;
using System.Security.Cryptography;
using LLCryptoLib.Security.Resources;

namespace LLCryptoLib.Security.Cryptography
{
    /// <summary>
    ///     Represents the abstract class from which all implementations of the MD2 hash algorithm inherit.
    /// </summary>
    public abstract class MD2 : HashAlgorithm
    {
        /// <summary>
        ///     Initializes a new instance of <see cref="MD2" />.
        /// </summary>
        /// <remarks>
        ///     You cannot create an instance of an abstract class. Application code will create a new instance of a derived
        ///     class.
        /// </remarks>
        protected MD2()
        {
            this.HashSizeValue = 128;
        }

        /// <summary>
        ///     Creates an instance of the default implementation of the <see cref="MD2" /> hash algorithm.
        /// </summary>
        /// <returns>A new instance of the MD2 hash algorithm.</returns>
        public new static MD2 Create()
        {
            return Create("MD2");
        }

        /// <summary>
        ///     Creates an instance of the specified implementation of the <see cref="MD2" /> hash algorithm.
        /// </summary>
        /// <param name="hashName">The name of the specific implementation of MD2 to use.</param>
        /// <returns>A new instance of the specified implementation of MD2.</returns>
        /// <exception cref="CryptographicException">An error occurs while initializing the hash.</exception>
        /// <exception cref="ArgumentException"></exception>
        public new static MD2 Create(string hashName)
        {
            if (string.Equals(hashName, "MD2", StringComparison.InvariantCultureIgnoreCase) ||
                string.Equals(hashName, "LLCryptoLib.Security.cryptography.md2cryptoserviceprovider",
                    StringComparison.InvariantCultureIgnoreCase))
                return new MD2CryptoServiceProvider();
            throw new ArgumentException(ResourceController.GetString("Error_ParamInvalid"));
        }
    }
}