using System.Security.Cryptography;

namespace LLCryptoLib.Crypto
{
    /// <summary>
    ///     Threefish is a tweakable block cipher designed as part of the Skein hash function, an entry
    ///     in the NIST hash function competition. Threefish uses no S-boxes or other table lookups in order
    ///     to avoid cache timing attacks its nonlinearity comes from alternating additions with exclusive ORs.
    ///     In that respect, it's similar to Salsa20, TEA, and the SHA-3 candidates CubeHash and BLAKE.
    ///     Threefish and the Skein hash function were designed by Bruce Schneier, Niels Ferguson, Stefan Lucks,
    ///     Doug Whiting, Mihir Bellare, Tadayoshi Kohno, Jon Callas, and Jesse Walker.
    /// </summary>
    public class Threefish : SymmetricAlgorithm
    {
        private const int DefaultCipherSize = 256;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Threefish" /> class.
        /// </summary>
        /// <exception cref="T:System.Security.Cryptography.CryptographicException">
        ///     The implementation of the class derived from the symmetric algorithm is not valid.
        /// </exception>
        public Threefish()
        {
            // Set up supported key and block sizes for Threefish
            KeySizes[] supportedSizes =
            {
                new KeySizes(256, 512, 256), // Supported key sizes
                new KeySizes(1024, 1024, 0) // Supported block sizes
            };

            this.LegalBlockSizesValue = supportedSizes;
            this.LegalKeySizesValue = supportedSizes;

            // Set up default sizes
            this.KeySizeValue = DefaultCipherSize;
            this.BlockSizeValue = DefaultCipherSize;

            // ECB is the default for the other ciphers in
            // the standard library I think
            this.ModeValue = CipherMode.ECB;
        }

        /// <summary>
        ///     When overridden in a derived class, creates a symmetric decryptor object with the specified
        ///     <see cref="P:System.Security.Cryptography.SymmetricAlgorithm.Key" /> property and initialization vector (
        ///     <see cref="P:System.Security.Cryptography.SymmetricAlgorithm.IV" />).
        /// </summary>
        /// <param name="rgbKey">The secret key to use for the symmetric algorithm.</param>
        /// <param name="rgbIV">The initialization vector to use for the symmetric algorithm.</param>
        /// <returns>A symmetric decryptor object.</returns>
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            return new ThreefishTransform(rgbKey, rgbIV, ThreefishTransformType.Decrypt, this.ModeValue,
                this.PaddingValue);
        }

        /// <summary>
        ///     When overridden in a derived class, creates a symmetric encryptor object with the specified
        ///     <see cref="P:System.Security.Cryptography.SymmetricAlgorithm.Key" /> property and initialization vector (
        ///     <see cref="P:System.Security.Cryptography.SymmetricAlgorithm.IV" />).
        /// </summary>
        /// <param name="rgbKey">The secret key to use for the symmetric algorithm.</param>
        /// <param name="rgbIV">The initialization vector to use for the symmetric algorithm.</param>
        /// <returns>A symmetric encryptor object.</returns>
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            return new ThreefishTransform(rgbKey, rgbIV, ThreefishTransformType.Encrypt, this.ModeValue,
                this.PaddingValue);
        }

        /// <summary>
        ///     When overridden in a derived class, generates a random initialization vector (
        ///     <see cref="P:System.Security.Cryptography.SymmetricAlgorithm.IV" />) to use for the algorithm.
        /// </summary>
        public override void GenerateIV()
        {
            this.IVValue = GenerateRandomBytes(this.BlockSizeValue/8);
        }

        /// <summary>
        ///     When overridden in a derived class, generates a random key (
        ///     <see cref="P:System.Security.Cryptography.SymmetricAlgorithm.Key" />) to use for the algorithm.
        /// </summary>
        public override void GenerateKey()
        {
            this.KeyValue = GenerateRandomBytes(this.KeySizeValue/8);
        }

        /// <summary>
        ///     Generates the random bytes.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        private static byte[] GenerateRandomBytes(int amount)
        {
            var rngCrypto = new RNGCryptoServiceProvider();

            var bytes = new byte[amount];
            rngCrypto.GetBytes(bytes);

            return bytes;
        }
    }
}