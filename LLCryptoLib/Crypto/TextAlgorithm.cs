using System.Security.Cryptography;

namespace LLCryptoLib.Crypto
{
    /// <summary>
    ///     TextAlgorithm
    ///     The 'TextAlgorithm' perform these conversions in the crypting phase:
    ///     - Text in clear is converted into array of bytes with 'StringToBytes'
    ///     - Bytes are passed to encryption algorithm
    ///     - Encryption algorithm returns a MemoryStream
    ///     - MemoryStream is turned into a Base64 string with 'MemoryToBase64String'
    ///     And these when decrypting:
    ///     - Text must be in Base64 in order to be decrypted
    ///     - Base64 string is turned into bytes with 'Base64StringToBytes'
    ///     - Bytes are passed to decryption algorithm
    ///     - Decryption returns a MemoryStream
    ///     - MemoryStream is turned into clear text with 'MemoryToString'
    ///     To create a TextAlgorithm see, for instance, TextROT13 <see cref="TextROT13" />
    /// </summary>
    public abstract class TextAlgorithm
    {
        /// <summary>
        ///     Text Algorithm inizialization vector
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")
        ] protected byte[] maIV;

        /// <summary>
        ///     Text Algorithm byte key
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")
        ] protected byte[] maKey;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="type">If it's a binary or text algorithm</param>
        protected TextAlgorithm(TextAlgorithmType type)
        {
            this.AlgorithmType = type;
        }

        /// <summary>
        ///     Text algorithm type. If it's a normal text algorithm (ie: TextROT13)
        ///     then this will be TextAlgorithm.Text. If it's a binary algorithm
        ///     turned into a text algorithm via Base64 conversions, this will be
        ///     TextAlgorithm.Binary
        /// </summary>
        public TextAlgorithmType AlgorithmType { get; }

        /// <summary>
        ///     Encryption text algorithm
        /// </summary>
        /// <param name="txt">String to be encrypted</param>
        /// <returns>Encrypted string</returns>
        public abstract string Code(string txt);

        /// <summary>
        ///     Decryption text algorithm
        /// </summary>
        /// <param name="txt">String to decrypted</param>
        /// <returns>Decrypted string</returns>
        public abstract string Decode(string txt);

        /// <summary>
        ///     Generates a key and an initialization value.
        ///     Max size is 32 = keySize+blockSize
        /// </summary>
        /// <param name="secretPhrase">Passphrase</param>
        /// <param name="keySize">Algorithm TextVigenere Size in bytes</param>
        /// <param name="blockSize">Algorithm Block (and IV) Size in bytes</param>
        /// <exception cref="LLCryptoLibException" />
        protected void GenerateKey(string secretPhrase, short keySize, short blockSize)
        {
            // Initialize internal values
            this.maKey = new byte[keySize];
            this.maIV = new byte[blockSize];

            // Perform a hash operation using the phrase. 
            int bytesNeeded = keySize + blockSize;
            byte[] bytePhrase = Config.TextEncoding.GetBytes(secretPhrase);
            byte[] result;

            if (bytesNeeded <= 48) // 384 bit are enough
            {
                System.Diagnostics.Debug.WriteLine("Applying SHA384 hash to initialize key and vector");
                SHA384Managed sha384 = new SHA384Managed();
                sha384.ComputeHash(bytePhrase);
                result = sha384.Hash;
            }
            else if (bytesNeeded <= 64) // 512 bits are enough
            {
                System.Diagnostics.Debug.WriteLine("Applying SHA512 hash to initialize key and vector");
                SHA512Managed sha512 = new SHA512Managed();
                sha512.ComputeHash(bytePhrase);
                result = sha512.Hash;
            }
            else if (bytesNeeded <= 320)
            {
                // We copy 5 times the hash over result
                System.Diagnostics.Debug.WriteLine("Applying SHA512*4 hash to initialize key and vector");
                SHA512Managed sha512 = new SHA512Managed();
                sha512.ComputeHash(bytePhrase);
                byte[] sha = sha512.Hash;
                int shaLen = sha.Length;
                result = new byte[320];
                for (int k = 0; k < 4; k++)
                {
                    for (int j = 0; j < shaLen; j++)
                    {
                        result[k*shaLen + j] = sha[j];
                    }
                }
            }
            else
            {
                throw new LLCryptoLibException("LLCryptoLib could not handle keys+block > 320 bytes");
            }

            // Transfer the first keyBytes characters of the hashed value to the key
            // and the remaining characters to the intialization vector.
            for (short loop = 0; loop < keySize; loop++)
            {
                this.maKey[loop] = result[loop];
            }

            for (short loop = keySize; loop < keySize + blockSize; loop++)
            {
                this.maIV[loop - keySize] = result[loop];
            }
        }
    }
}