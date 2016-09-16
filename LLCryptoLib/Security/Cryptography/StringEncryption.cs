using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using LLCryptoLib.Security.Resources;

namespace LLCryptoLib.Security.Cryptography
{
    /// <summary>
    ///     Defines a number of easy-to-use methods to perform string-based encryption.
    /// </summary>
    public sealed class StringEncryption
    {
        private SymmetricAlgorithm m_BulkCipher;
        private HashAlgorithm m_Hash;
        private byte[] m_IV;
        private byte[] m_Key;

        /// <summary>
        ///     Initializes a new StringEncryption instance.
        /// </summary>
        /// <remarks>The default bulk cipher algorithm is Rijndael and the default hash algorithm is RIPEMD-160.</remarks>
        public StringEncryption()
        {
            this.Init(Rijndael.Create(), RIPEMD160.Create());
        }

        /// <summary>
        ///     Initializes a new StringEncryption instance.
        /// </summary>
        /// <param name="bulkCipher">The name of the bulk cipher algorithm to use.</param>
        /// <param name="hash">The name of the hash algorithm to use.</param>
        public StringEncryption(string bulkCipher, string hash)
        {
            if ((bulkCipher == null) || (bulkCipher.Length == 0))
                bulkCipher = "Rijndael";
            if ((hash == null) || (hash.Length == 0))
                hash = "RIPEMD160";
            this.Init(SymmetricAlgorithm.Create(bulkCipher), HashAlgorithm.Create(hash));
        }

        /// <summary>
        ///     Initializes a new StringEncryption instance.
        /// </summary>
        /// <param name="bulkCipher">The bulk cipher algorithm to use.</param>
        /// <param name="hash">The hash algorithm to use.</param>
        /// <exception cref="ArgumentNullException">One of the parameters is a null reference.</exception>
        public StringEncryption(SymmetricAlgorithm bulkCipher, HashAlgorithm hash)
        {
            if (bulkCipher == null)
                throw new ArgumentNullException("bulkCipher", ResourceController.GetString("Error_ParamNull"));
            if (hash == null)
                throw new ArgumentNullException("hash", ResourceController.GetString("Error_ParamNull"));
            this.Init(bulkCipher, hash);
        }

        /// <summary>
        ///     Gets or sets the key of the bulk cipher algorithm.
        /// </summary>
        /// <value>An array of bytes that contains the key of the bulk cipher algorithm.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
             "CA1819:PropertiesShouldNotReturnArrays")]
        public byte[] Key
        {
            get { return this.m_Key; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value", ResourceController.GetString("Error_ParamNull"));
                if (!this.m_BulkCipher.ValidKeySize(value.Length*8))
                    throw new CryptographicException(ResourceController.GetString("Error_InvalidKeySize"));
                this.m_Key = (byte[]) value.Clone();
            }
        }

        /// <summary>
        ///     Gets or sets the initialization vector of the bulk cipher algorithm.
        /// </summary>
        /// <value>An array of bytes that contains the initialization vector of the bulk cipher algorithm.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
             "CA1819:PropertiesShouldNotReturnArrays")]
        public byte[] IV
        {
            get { return this.m_IV; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value", ResourceController.GetString("Error_ParamNull"));
                if (value.Length != this.m_BulkCipher.BlockSize/8)
                    throw new CryptographicException(ResourceController.GetString("Error_InvalidIVSize"));
                this.m_IV = (byte[]) value.Clone();
            }
        }

        private void Init(SymmetricAlgorithm bulkCipher, HashAlgorithm hash)
        {
            this.m_BulkCipher = bulkCipher;
            this.m_Hash = hash;
            this.m_Key = this.m_BulkCipher.Key;
            this.m_IV = this.m_BulkCipher.IV;
        }

        /// <summary>
        ///     Encrypts a given byte array.
        /// </summary>
        /// <param name="input">The array of bytes to encrypt.</param>
        /// <returns>A string representation of the encrypted data.</returns>
        /// <exception cref="ArgumentNullException"><i>input</i> is a null reference.</exception>
        public string Encrypt(byte[] input)
        {
            if (input == null)
                throw new ArgumentNullException("input", ResourceController.GetString("Error_ParamNull"));
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, this.m_BulkCipher.CreateEncryptor(this.m_Key, this.m_IV),
                CryptoStreamMode.Write);
            cs.Write(input, 0, input.Length);
            cs.Write(this.m_Hash.ComputeHash(input, 0, input.Length), 0, this.m_Hash.HashSize/8);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>
        ///     Encrypts a given string.
        /// </summary>
        /// <param name="input">The string to encrypt.</param>
        /// <returns>A string representation of the encrypted data.</returns>
        /// <remarks>The default encoding to convert the input string to an array of bytes is UTF-8.</remarks>
        /// <exception cref="ArgumentNullException"><i>input</i> is a null reference.</exception>
        public string Encrypt(string input)
        {
            return this.Encrypt(input, Encoding.UTF8);
        }

        /// <summary>
        ///     Encrypts a given string.
        /// </summary>
        /// <param name="input">The string to encrypt.</param>
        /// <param name="encoding">The encoding to use to convert the string to an array of bytes.</param>
        /// <returns>A string representation of the encrypted data.</returns>
        /// <exception cref="ArgumentNullException"><i>input</i> or <i>encoding</i> is a null reference.</exception>
        public string Encrypt(string input, Encoding encoding)
        {
            if (encoding == null)
                throw new ArgumentNullException("encoding", ResourceController.GetString("Error_ParamNull"));
            if (input == null)
                throw new ArgumentNullException("input", ResourceController.GetString("Error_ParamNull"));
            return this.Encrypt(encoding.GetBytes(input));
        }

        /// <summary>
        ///     Decrypts a given string.
        /// </summary>
        /// <param name="input">The string to decrypt.</param>
        /// <returns>An array of bytes, containing the unencrypted data.</returns>
        /// <exception cref="ArgumentNullException"><i>input</i> is a null reference.</exception>
        /// <exception cref="FormatException"><i>input</i> is an invalid Base64 string.</exception>
        /// <exception cref="ArgumentException">The length of <i>input</i> is invalid.</exception>
        /// <exception cref="CryptographicException">An error occurs during the decryption or integrity verification.</exception>
        public byte[] Decrypt(string input)
        {
            if (input == null)
                throw new ArgumentNullException("input", ResourceController.GetString("Error_ParamNull"));
            byte[] buffer = Convert.FromBase64String(input); // throws FormatException
            buffer = this.m_BulkCipher.CreateDecryptor(this.m_Key, this.m_IV)
                .TransformFinalBlock(buffer, 0, buffer.Length); // throws CryptographicException
            if (buffer.Length < this.m_Hash.HashSize/8)
                throw new ArgumentException(ResourceController.GetString("Error_ParamInvalid"), "input");
            byte[] hash = this.m_Hash.ComputeHash(buffer, 0, buffer.Length - this.m_Hash.HashSize/8);
            int offset = buffer.Length - this.m_Hash.HashSize/8;
            for (int i = 0; i < hash.Length; i++)
            {
                if (hash[i] != buffer[offset + i])
                    throw new CryptographicException(ResourceController.GetString("Error_InvalidHash"));
            }
            byte[] ret = new byte[buffer.Length - this.m_Hash.HashSize/8];
            Buffer.BlockCopy(buffer, 0, ret, 0, ret.Length);
            return ret;
        }

        /// <summary>
        ///     Decrypts a given string.
        /// </summary>
        /// <param name="input">The string to decrypt.</param>
        /// <param name="encoding">The encoding to use to convert the string to an array of bytes.</param>
        /// <returns>A string containing the unencrypted data.</returns>
        /// <exception cref="ArgumentNullException"><i>input</i> or <i>encoding</i> is a null reference.</exception>
        /// <exception cref="FormatException"><i>input</i> is an invalid Base64 string.</exception>
        /// <exception cref="ArgumentException">The length of <i>input</i> is invalid.</exception>
        /// <exception cref="CryptographicException">An error occurs during the decryption or integrity verification.</exception>
        public string DecryptString(string input, Encoding encoding)
        {
            if (encoding == null)
                throw new ArgumentNullException("encoding", ResourceController.GetString("Error_ParamNull"));
            if (input == null)
                throw new ArgumentNullException("input", ResourceController.GetString("Error_ParamNull"));
            return encoding.GetString(this.Decrypt(input));
        }

        /// <summary>
        ///     Decrypts a given string.
        /// </summary>
        /// <param name="input">The string to decrypt.</param>
        /// <returns>A string containing the unencrypted data.</returns>
        /// <remarks>The default encoding to convert the input string to an array of bytes is UTF-8.</remarks>
        /// <exception cref="ArgumentNullException"><i>input</i> is a null reference.</exception>
        /// <exception cref="FormatException"><i>input</i> is an invalid Base64 string.</exception>
        /// <exception cref="ArgumentException">The length of <i>input</i> is invalid.</exception>
        /// <exception cref="CryptographicException">An error occurs during the decryption or integrity verification.</exception>
        public string DecryptString(string input)
        {
            return this.DecryptString(input, Encoding.UTF8);
        }
    }
}