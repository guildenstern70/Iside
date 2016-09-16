using System;
using LLCryptoLib.Crypto;

namespace LLCryptoLib.Hash
{
    /// <summary>
    ///     Configuration class for Skein hash
    /// </summary>
    public class SkeinConfig
    {
        private readonly int _stateSize;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SkeinConfig" /> class.
        /// </summary>
        /// <param name="sourceHash">The source hash.</param>
        public SkeinConfig(Skein sourceHash)
        {
            this._stateSize = sourceHash.StateSize;

            // Allocate config value
            this.ConfigValue = new ulong[sourceHash.StateSize/8];

            // Set the state size for the configuration
            this.ConfigString = new ulong[this.ConfigValue.Length];
            this.ConfigString[1] = (ulong) sourceHash.HashSize;
        }

        /// <summary>
        ///     Gets or sets the config value.
        /// </summary>
        /// <value>The config value.</value>
        public ulong[] ConfigValue { get; }

        /// <summary>
        ///     Gets or sets the config string.
        /// </summary>
        /// <value>The config string.</value>
        public ulong[] ConfigString { get; }

        /// <summary>
        ///     Generates the configuration.
        /// </summary>
        public void GenerateConfiguration()
        {
            var cipher = ThreefishCipher.CreateCipher(this._stateSize);
            var tweak = new UbiTweak();

            // Initialize the tweak value
            tweak.StartNewBlockType(UbiType.Config);
            tweak.IsFinalBlock = true;
            tweak.BitsProcessed = 32;

            cipher.SetTweak(tweak.Tweak);
            cipher.Encrypt(this.ConfigString, this.ConfigValue);

            this.ConfigValue[0] ^= this.ConfigString[0];
            this.ConfigValue[1] ^= this.ConfigString[1];
            this.ConfigValue[2] ^= this.ConfigString[2];
        }

        /// <summary>
        ///     Generates the configuration.
        /// </summary>
        /// <param name="initialState">The initial state.</param>
        public void GenerateConfiguration(ulong[] initialState)
        {
            var cipher = ThreefishCipher.CreateCipher(this._stateSize);
            var tweak = new UbiTweak();

            // Initialize the tweak value
            tweak.StartNewBlockType(UbiType.Config);
            tweak.IsFinalBlock = true;
            tweak.BitsProcessed = 32;

            cipher.SetKey(initialState);
            cipher.SetTweak(tweak.Tweak);
            cipher.Encrypt(this.ConfigString, this.ConfigValue);

            this.ConfigValue[0] ^= this.ConfigString[0];
            this.ConfigValue[1] ^= this.ConfigString[1];
            this.ConfigValue[2] ^= this.ConfigString[2];
        }

        /// <summary>
        ///     Sets the schema.
        /// </summary>
        /// <param name="schema">The schema.</param>
        public void SetSchema(params byte[] schema)
        {
            if (schema.Length != 4) throw new Exception("Schema must be 4 bytes.");

            ulong n = this.ConfigString[0];

            // Clear the schema bytes
            n &= ~0xfffffffful;
            // Set schema bytes
            n |= (ulong) schema[3] << 24;
            n |= (ulong) schema[2] << 16;
            n |= (ulong) schema[1] << 8;
            n |= schema[0];

            this.ConfigString[0] = n;
        }

        /// <summary>
        ///     Sets the version.
        /// </summary>
        /// <param name="version">The version.</param>
        public void SetVersion(int version)
        {
            if ((version < 0) || (version > 3))
                throw new Exception("Version must be between 0 and 3, inclusive.");

            this.ConfigString[0] &= ~((ulong) 0x03 << 32);
            this.ConfigString[0] |= (ulong) version << 32;
        }

        /// <summary>
        ///     Sets the size of the tree leaf.
        /// </summary>
        /// <param name="size">The size.</param>
        public void SetTreeLeafSize(byte size)
        {
            this.ConfigString[2] &= ~(ulong) 0xff;
            this.ConfigString[2] |= size;
        }

        /// <summary>
        ///     Sets the size of the tree fan out.
        /// </summary>
        /// <param name="size">The size.</param>
        public void SetTreeFanOutSize(byte size)
        {
            this.ConfigString[2] &= ~((ulong) 0xff << 8);
            this.ConfigString[2] |= (ulong) size << 8;
        }

        /// <summary>
        ///     Sets the height of the max tree.
        /// </summary>
        /// <param name="height">The height.</param>
        public void SetMaxTreeHeight(byte height)
        {
            if (height == 1)
                throw new Exception("Tree height must be zero or greater than 1.");

            this.ConfigString[2] &= ~((ulong) 0xff << 16);
            this.ConfigString[2] |= (ulong) height << 16;
        }
    }
}