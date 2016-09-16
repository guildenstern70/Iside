namespace LLCryptoLib.Crypto
{
    /// <summary>
    ///     Available symmetric encryption algorithms.
    /// </summary>
    /// <example>
    ///     <code>
    ///  
    ///  // 1. Set algorithm
    ///  IStreamAlgorithm cryptoAlgo = StreamAlgorithmFactory.Create(SupportedStreamAlgorithms.BLOWFISH);
    /// 
    ///  // 2. Encrypt 
    ///  StreamCrypter crypter = new StreamCrypter(cryptoAlgo);
    ///  crypter.GenerateKeys("littlelitesoftware");
    ///  crypter.EncryptDecrypt(rndFile.FullName, encryptedFile, true, null);
    ///  Console.WriteLine("File encrypted into " + encryptedFile);
    /// 
    ///  // 3. Decrypt
    ///  StreamCrypter decrypter = new StreamCrypter(cryptoAlgo);
    ///  crypter.GenerateKeys("littlelitesoftware");
    ///  crypter.EncryptDecrypt(encryptedFile, decryptedFile, false, null);
    ///  Console.WriteLine("File decrypted into " + decryptedFile);
    ///  
    ///  </code>
    /// </example>
    public enum SupportedStreamAlgorithms
    {
        /// <summary>
        ///     DES
        /// </summary>
        DES,

        /// <summary>
        ///     Triple DES or 3DES
        /// </summary>
        TRIPLEDES,

        /// <summary>
        ///     Rijndael or AES 128 bit
        /// </summary>
        RIJNDAEL,

        /// <summary>
        ///     AES or Rijndael 128 bit
        /// </summary>
        AES128,

        /// <summary>
        ///     AES or Rijndael 192 bit
        /// </summary>
        AES192,

        /// <summary>
        ///     AES or Rijndael 256 bit
        /// </summary>
        AES256,

        /// <summary>
        ///     ARC4 128 bit
        /// </summary>
        ARC4,

        /// <summary>
        ///     ARC4 512 bit
        /// </summary>
        ARC4512,

        /// <summary>
        ///     ARC4 1024 bit
        /// </summary>
        ARC41024,

        /// <summary>
        ///     ARC4 2048 bit
        /// </summary>
        ARC42048,

        /// <summary>
        ///     Twofish 128 bit
        /// </summary>
        TWOFISH,

        /// <summary>
        ///     Twofish 256 bit
        /// </summary>
        TWOFISH256,

        /// <summary>
        ///     Blowfish 128 bit
        /// </summary>
        BLOWFISH,

        /// <summary>
        ///     Blowfish 256 bit
        /// </summary>
        BLOWFISH256,

        /// <summary>
        ///     Blowfish 448 bit
        /// </summary>
        BLOWFISH448,

        /// <summary>
        ///     XOR
        /// </summary>
        XOR,

        /// <summary>
        ///     CAST5 128 bit
        /// </summary>
        CAST5,

        /// <summary>
        ///     THREEFISH 256 bit
        /// </summary>
        THREEFISH,

        /// <summary>
        ///     THREEFISH 512 bit
        /// </summary>
        THREEFISH512,

        /// <summary>
        ///     THREEFISH 1024 bit
        /// </summary>
        THREEFISH1024
    }
}