using System.Runtime.InteropServices;

namespace LLCryptoLib.Hash
{
    /// <summary>
    ///     Available Hash Algorithms enumeration
    /// </summary>
    [ComVisible(true)]
    public enum AvailableHash
    {
        /// <summary>
        ///     No Hash Algorithm
        /// </summary>
        FAKE = 0,

        /// <summary>
        ///     Adler 32 Hash Algorithm
        /// </summary>
        ADLER32 = 12,

        /// <summary>
        ///     CRC32 Hash Algorithm
        /// </summary>
        CRC32 = 6,

        /// <summary>
        ///     FCS16 Hash Algorithm
        /// </summary>
        FCS16 = 19,

        /// <summary>
        ///     FCS32 Hash Algorithm
        /// </summary>
        FCS32 = 20,

        /// <summary>
        ///     GOST Hash Algorithm
        /// </summary>
        GOST = 22,

        /// <summary>
        ///     MD2 Hash
        /// </summary>
        MD2 = 7,

        /// <summary>
        ///     MD4 Hash Algorithm
        /// </summary>
        MD4 = 8,

        /// <summary>
        ///     MD5 Hash Algorithm
        /// </summary>
        MD5 = 5,

        /// <summary>
        ///     SHA1 Hash Algorithm
        /// </summary>
        SHA1 = 2,

        /// <summary>
        ///     SHA224 Hash Algorithm
        /// </summary>
        SHA224 = 21,

        /// <summary>
        ///     SHA256 Hash Algorithm
        /// </summary>
        SHA256 = 3,

        /// <summary>
        ///     SHA384 Hash Algorithm
        /// </summary>
        SHA384 = 1,

        /// <summary>
        ///     SHA512 Hash Algorithm
        /// </summary>
        SHA512 = 4,

        /// <summary>
        ///     TIGER Hash Algorithm
        /// </summary>
        TIGER = 10,

        /// <summary>
        ///     RIPEMD160 Hash Algorithm
        /// </summary>
        RIPEMD160 = 9,

        /// <summary>
        ///     HAVAL 128 bits Hash Algorithm
        /// </summary>
        HAVAL128 = 13,

        /// <summary>
        ///     HAVAL 160 bits Hash Algorithm
        /// </summary>
        HAVAL160 = 14,

        /// <summary>
        ///     HAVAL 192 bits Hash Algorithm
        /// </summary>
        HAVAL192 = 15,

        /// <summary>
        ///     HAVAL 224 bits Hash Algorithm
        /// </summary>
        HAVAL224 = 16,

        /// <summary>
        ///     HAVAL 256 bits Hash Algorithm
        /// </summary>
        HAVAL256 = 17,

        /// <summary>
        ///     HMACSHA1 Keyed Hash Algorithm
        /// </summary>
        HMACSHA1 = 11,

        /// <summary>
        ///     Whirlpool Hash Algorithm
        /// </summary>
        WHIRLPOOL = 18,

        /// <summary>
        ///     GHash 32-3
        /// </summary>
        GHASH323 = 23,

        /// <summary>
        ///     GHash 32-5
        /// </summary>
        GHASH325 = 24,

        /// <summary>
        ///     Skein 224
        /// </summary>
        SKEIN224 = 25,

        /// <summary>
        ///     Skein 256
        /// </summary>
        SKEIN256 = 26,

        /// <summary>
        ///     Skein 384
        /// </summary>
        SKEIN384 = 27,

        /// <summary>
        ///     Skein 512
        /// </summary>
        SKEIN512 = 28,

        /// <summary>
        ///     FNV-1 32
        /// </summary>
        FNV32 = 29,

        /// <summary>
        ///     FNV-1 64
        /// </summary>
        FNV64 = 30,

        /// <summary>
        ///     FNV-1a 32
        /// </summary>
        FNV1A32 = 31,

        /// <summary>
        ///     FNV-1A 64
        /// </summary>
        FNV1A64 = 32
    }
}