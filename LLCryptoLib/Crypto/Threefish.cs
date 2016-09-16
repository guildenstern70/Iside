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
 * 
 * Copyright (C) 2003-2014 LittleLite Software
 * 
 */

using System.Security.Cryptography;

namespace LLCryptoLib.Crypto
{
    /// <summary>
    /// Threefish is a tweakable block cipher designed as part of the Skein hash function, an entry 
    /// in the NIST hash function competition. Threefish uses no S-boxes or other table lookups in order 
    /// to avoid cache timing attacks its nonlinearity comes from alternating additions with exclusive ORs. 
    /// In that respect, it's similar to Salsa20, TEA, and the SHA-3 candidates CubeHash and BLAKE.
    /// Threefish and the Skein hash function were designed by Bruce Schneier, Niels Ferguson, Stefan Lucks, 
    /// Doug Whiting, Mihir Bellare, Tadayoshi Kohno, Jon Callas, and Jesse Walker.
    /// </summary>
    public class Threefish : SymmetricAlgorithm
    {
        const int DefaultCipherSize = 256;

        /// <summary>
        /// Initializes a new instance of the <see cref="Threefish"/> class.
        /// </summary>
        /// <exception cref="T:System.Security.Cryptography.CryptographicException">
        /// The implementation of the class derived from the symmetric algorithm is not valid.
        /// </exception>
        public Threefish()
        {
            // Set up supported key and block sizes for Threefish
            KeySizes[] supportedSizes = 
            {
                new KeySizes(256, 512, 256),  // Supported key sizes
                new KeySizes(1024, 1024, 0)   // Supported block sizes
            };

            base.LegalBlockSizesValue = supportedSizes;
            base.LegalKeySizesValue   = supportedSizes;

            // Set up default sizes
            base.KeySizeValue   = DefaultCipherSize;
            base.BlockSizeValue = DefaultCipherSize;

            // ECB is the default for the other ciphers in
            // the standard library I think
            base.ModeValue = CipherMode.ECB;
        }

        /// <summary>
        /// When overridden in a derived class, creates a symmetric decryptor object with the specified <see cref="P:System.Security.Cryptography.SymmetricAlgorithm.Key"/> property and initialization vector (<see cref="P:System.Security.Cryptography.SymmetricAlgorithm.IV"/>).
        /// </summary>
        /// <param name="rgbKey">The secret key to use for the symmetric algorithm.</param>
        /// <param name="rgbIV">The initialization vector to use for the symmetric algorithm.</param>
        /// <returns>A symmetric decryptor object.</returns>
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            return new ThreefishTransform(rgbKey, rgbIV, ThreefishTransformType.Decrypt, ModeValue, PaddingValue);
        }

        /// <summary>
        /// When overridden in a derived class, creates a symmetric encryptor object with the specified <see cref="P:System.Security.Cryptography.SymmetricAlgorithm.Key"/> property and initialization vector (<see cref="P:System.Security.Cryptography.SymmetricAlgorithm.IV"/>).
        /// </summary>
        /// <param name="rgbKey">The secret key to use for the symmetric algorithm.</param>
        /// <param name="rgbIV">The initialization vector to use for the symmetric algorithm.</param>
        /// <returns>A symmetric encryptor object.</returns>
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            return new ThreefishTransform(rgbKey, rgbIV, ThreefishTransformType.Encrypt, ModeValue, PaddingValue);
        }

        /// <summary>
        /// When overridden in a derived class, generates a random initialization vector (<see cref="P:System.Security.Cryptography.SymmetricAlgorithm.IV"/>) to use for the algorithm.
        /// </summary>
        public override void GenerateIV()
        {
            base.IVValue = GenerateRandomBytes(base.BlockSizeValue / 8);
        }

        /// <summary>
        /// When overridden in a derived class, generates a random key (<see cref="P:System.Security.Cryptography.SymmetricAlgorithm.Key"/>) to use for the algorithm.
        /// </summary>
        public override void GenerateKey()
        {
            base.KeyValue = GenerateRandomBytes(base.KeySizeValue / 8);
        }

        /// <summary>
        /// Generates the random bytes.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        static byte[] GenerateRandomBytes(int amount)
        {
            var rngCrypto = new RNGCryptoServiceProvider();

            var bytes = new byte[amount];
            rngCrypto.GetBytes(bytes);

            return bytes;
        }
    }
}
