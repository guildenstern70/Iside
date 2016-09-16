using System;

namespace LLCryptoLib.Hash
{
    /// <summary>
    ///     Enumeration for UbiTweak
    /// </summary>
    public enum UbiType : ulong
    {
        /// <summary>
        /// </summary>
        Key = 0,

        /// <summary>
        /// </summary>
        Config = 4,

        /// <summary>
        /// </summary>
        Personalization = 8,

        /// <summary>
        /// </summary>
        PublicKey = 12,

        /// <summary>
        /// </summary>
        KeyIdentifier = 16,

        /// <summary>
        /// </summary>
        Nonce = 20,

        /// <summary>
        /// </summary>
        Message = 48,

        /// <summary>
        /// </summary>
        Out = 63
    }

    /// <summary>
    ///     Tweak for Skein Hash algorithm
    /// </summary>
    internal class UbiTweak
    {
        private const ulong T1FlagFinal = unchecked((ulong) 1 << 63);
        private const ulong T1FlagFirst = unchecked((ulong) 1 << 62);

        /// <summary>
        ///     Initializes a new instance of the <see cref="UbiTweak" /> class.
        /// </summary>
        public UbiTweak()
        {
            this.Tweak = new ulong[2];
        }

        /// <summary>
        ///     Gets or sets the first block flag.
        /// </summary>
        public bool IsFirstBlock
        {
            get { return (this.Tweak[1] & T1FlagFirst) != 0; }
            set
            {
                long mask = value ? 1 : 0;
                this.Tweak[1] = (this.Tweak[1] & ~T1FlagFirst) | ((ulong) -mask & T1FlagFirst);
            }
        }

        /// <summary>
        ///     Gets or sets the final block flag.
        /// </summary>
        public bool IsFinalBlock
        {
            get { return (this.Tweak[1] & T1FlagFinal) != 0; }
            set
            {
                long mask = value ? 1 : 0;
                this.Tweak[1] = (this.Tweak[1] & ~T1FlagFinal) | ((ulong) -mask & T1FlagFinal);
            }
        }

        /// <summary>
        ///     Gets or sets the current tree level.
        /// </summary>
        public byte TreeLevel
        {
            get { return (byte) ((this.Tweak[1] >> 48) & 0x3f); }
            set
            {
                if (value > 63)
                    throw new Exception("Tree level must be between 0 and 63, inclusive.");

                this.Tweak[1] &= ~((ulong) 0x3f << 48);
                this.Tweak[1] |= (ulong) value << 48;
            }
        }

        /// <summary>
        ///     Gets or sets the number of bits processed so far, inclusive.
        /// </summary>
        public ulong BitsProcessed
        {
            get { return this.Tweak[0]; }
            set { this.Tweak[0] = value; }
        }

        /// <summary>
        ///     Gets or sets the current UBI block type.
        /// </summary>
        public UbiType BlockType
        {
            get { return (UbiType) (this.Tweak[1] >> 56); }
            set { this.Tweak[1] = (ulong) value << 56; }
        }

        /// <summary>
        ///     The current Threefish tweak value.
        /// </summary>
        public ulong[] Tweak { get; }

        /// <summary>
        ///     Starts a new UBI block type by setting BitsProcessed to zero, setting the first flag, and setting the block type.
        /// </summary>
        /// <param name="type">The UBI block type of the new block.</param>
        public void StartNewBlockType(UbiType type)
        {
            this.BitsProcessed = 0;
            this.BlockType = type;
            this.IsFirstBlock = true;
        }
    }
}