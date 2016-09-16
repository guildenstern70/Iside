using System.Security.Cryptography;
using LLCryptoLib.Crypto;

namespace LLCryptoLib.Hash
{
    /// <summary>
    ///     Specifies the Skein initialization type.
    /// </summary>
    public enum SkeinInitializationType
    {
        /// <summary>
        ///     Identical to the standard Skein initialization.
        /// </summary>
        Normal,

        /// <summary>
        ///     Creates the initial state with zeros instead of the configuration block, then initializes the hash.
        ///     This does not start a new UBI block type, and must be done manually.
        /// </summary>
        ZeroedState,

        /// <summary>
        ///     Leaves the initial state set to its previous value, which is then chained with subsequent block transforms.
        ///     This does not start a new UBI block type, and must be done manually.
        /// </summary>
        ChainedState,

        /// <summary>
        ///     Creates the initial state by chaining the previous state value with the config block, then initializes the hash.
        ///     This starts a new UBI block type with the standard Payload type.
        /// </summary>
        ChainedConfig
    }

    /// <summary>
    ///     Implementation of Skein hash algorithm. Skein is a cryptographic hash function and one out of five finalists in
    ///     the NIST hash function competition to design what will become the SHA-3 standard, the intended successor of
    ///     SHA-1 and SHA-2. According to Stefan Lucks, the name Skein refers to how the Skein function intertwines the
    ///     input, similar to a coil of yarn, which is called a skein.
    /// </summary>
    public class Skein : HashAlgorithm
    {
        private readonly ThreefishCipher _cipher;

        private readonly ulong[] _cipherInput;

        private readonly int _cipherStateBytes;
        private readonly int _cipherStateWords;

        private readonly byte[] _inputBuffer;

        private readonly int _outputBytes;
        private readonly ulong[] _state;
        private int _bytesFilled;

        /// <summary>
        ///     Initializes the Skein hash instance.
        /// </summary>
        /// <param name="stateSize">
        ///     The internal state size of the hash in bits.
        ///     Supported values are 256, 512, and 1024.
        /// </param>
        /// <param name="outputSize">
        ///     The output size of the hash in bits.
        ///     Output size must be divisible by 8 and greater than zero.
        /// </param>
        public Skein(int stateSize, int outputSize)
        {
            // Make sure the output bit size > 0
            if (outputSize <= 0)
                throw new CryptographicException("Output bit size must be greater than zero.");

            // Make sure output size is divisible by 8
            if (outputSize%8 != 0)
                throw new CryptographicException("Output bit size must be divisible by 8.");

            this.StateSize = stateSize;
            this._cipherStateBytes = stateSize/8;
            this._cipherStateWords = stateSize/64;

            this.HashSizeValue = outputSize;
            this._outputBytes = (outputSize + 7)/8;

            // Figure out which cipher we need based on
            // the state size
            this._cipher = ThreefishCipher.CreateCipher(stateSize);
            if (this._cipher == null) throw new CryptographicException("Unsupported state size.");

            // Allocate buffers
            this._inputBuffer = new byte[this._cipherStateBytes];
            this._cipherInput = new ulong[this._cipherStateWords];
            this._state = new ulong[this._cipherStateWords];

            // Allocate tweak
            this.UbiParameters = new UbiTweak();

            // Generate the configuration string
            this.Configuration = new SkeinConfig(this);
            this.Configuration.SetSchema(83, 72, 65, 51); // "SHA3"
            this.Configuration.SetVersion(1);
            this.Configuration.GenerateConfiguration();
        }

        /// <summary>
        ///     Gets or sets the configuration for Skein
        /// </summary>
        /// <value>The configuration.</value>
        public SkeinConfig Configuration { get; }

        internal UbiTweak UbiParameters { get; }

        /// <summary>
        ///     Gets the size of the state.
        /// </summary>
        /// <value>The size of the state.</value>
        public int StateSize { get; }


        private void ProcessBlock(int bytes)
        {
            // Set the key to the current state
            this._cipher.SetKey(this._state);

            // Update tweak
            this.UbiParameters.BitsProcessed += (ulong) bytes;
            this._cipher.SetTweak(this.UbiParameters.Tweak);

            // Encrypt block
            this._cipher.Encrypt(this._cipherInput, this._state);

            // Feed-forward input with state
            for (int i = 0; i < this._cipherInput.Length; i++)
                this._state[i] ^= this._cipherInput[i];
        }

        /// <summary>
        ///     When overridden in a derived class, routes data written to the object into the hash algorithm for computing the
        ///     hash.
        /// </summary>
        /// <param name="array">The input to compute the hash code for.</param>
        /// <param name="ibStart">The offset into the byte array from which to begin using data.</param>
        /// <param name="cbSize">The number of bytes in the byte array to use as data.</param>
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            int bytesDone = 0;
            int offset = ibStart;

            // Fill input buffer
            while ((bytesDone < cbSize) && (offset < array.Length))
            {
                // Do a transform if the input buffer is filled
                if (this._bytesFilled == this._cipherStateBytes)
                {
                    // Copy input buffer to cipher input buffer
                    this.InputBufferToCipherInput();

                    // Process the block
                    this.ProcessBlock(this._cipherStateBytes);

                    // Clear first flag, which will be set
                    // by Initialize() if this is the first transform
                    this.UbiParameters.IsFirstBlock = false;

                    // Reset buffer fill count
                    this._bytesFilled = 0;
                }

                this._inputBuffer[this._bytesFilled++] = array[offset++];
                bytesDone++;
            }
        }

        /// <summary>
        ///     When overridden in a derived class, finalizes the hash computation after the last data is processed by the
        ///     cryptographic stream object.
        /// </summary>
        /// <returns>The computed hash code.</returns>
        protected override byte[] HashFinal()
        {
            int i;

            // Pad left over space in input buffer with zeros
            // and copy to cipher input buffer
            for (i = this._bytesFilled; i < this._inputBuffer.Length; i++)
                this._inputBuffer[i] = 0;

            this.InputBufferToCipherInput();

            // Do final message block
            this.UbiParameters.IsFinalBlock = true;
            this.ProcessBlock(this._bytesFilled);

            // Clear cipher input
            for (i = 0; i < this._cipherInput.Length; i++)
                this._cipherInput[i] = 0;

            // Do output block counter mode output
            int j;

            var hash = new byte[this._outputBytes];
            var oldState = new ulong[this._cipherStateWords];

            // Save old state
            for (j = 0; j < this._state.Length; j++)
                oldState[j] = this._state[j];

            for (i = 0; i < this._outputBytes; i += this._cipherStateBytes)
            {
                this.UbiParameters.StartNewBlockType(UbiType.Out);
                this.UbiParameters.IsFinalBlock = true;
                this.ProcessBlock(8);

                // Output a chunk of the hash
                int outputSize = this._outputBytes - i;
                if (outputSize > this._cipherStateBytes)
                    outputSize = this._cipherStateBytes;

                PutBytes(this._state, hash, i, outputSize);

                // Restore old state
                for (j = 0; j < this._state.Length; j++)
                    this._state[j] = oldState[j];

                // Increment counter
                this._cipherInput[0]++;
            }

            return hash;
        }

        /// <summary>
        ///     Creates the initial state with zeros instead of the configuration block, then initializes the hash.
        ///     This does not start a new UBI block type, and must be done manually.
        /// </summary>
        public void Initialize(SkeinInitializationType initializationType)
        {
            switch (initializationType)
            {
                case SkeinInitializationType.Normal:
                    // Normal initialization
                    this.Initialize();
                    return;

                case SkeinInitializationType.ZeroedState:
                    // Copy the configuration value to the state
                    for (int i = 0; i < this._state.Length; i++)
                        this._state[i] = 0;
                    break;

                case SkeinInitializationType.ChainedState:
                    // Keep the state as it is and do nothing
                    break;

                case SkeinInitializationType.ChainedConfig:
                    // Generate a chained configuration
                    this.Configuration.GenerateConfiguration(this._state);
                    // Continue initialization
                    this.Initialize();
                    return;
            }

            // Reset bytes filled
            this._bytesFilled = 0;
        }

        /// <summary>
        ///     Initializes an implementation of the <see cref="T:System.Security.Cryptography.HashAlgorithm" /> class.
        /// </summary>
        public sealed override void Initialize()
        {
            // Copy the configuration value to the state
            for (int i = 0; i < this._state.Length; i++)
                this._state[i] = this.Configuration.ConfigValue[i];

            // Set up tweak for message block
            this.UbiParameters.StartNewBlockType(UbiType.Message);

            // Reset bytes filled
            this._bytesFilled = 0;
        }

        // Moves the byte input buffer to the ulong cipher input
        private void InputBufferToCipherInput()
        {
            for (int i = 0; i < this._cipherStateWords; i++)
                this._cipherInput[i] = GetUInt64(this._inputBuffer, i*8);
        }

        #region Utils

        private static ulong GetUInt64(byte[] buf, int offset)
        {
            ulong v = buf[offset];
            v |= (ulong) buf[offset + 1] << 8;
            v |= (ulong) buf[offset + 2] << 16;
            v |= (ulong) buf[offset + 3] << 24;
            v |= (ulong) buf[offset + 4] << 32;
            v |= (ulong) buf[offset + 5] << 40;
            v |= (ulong) buf[offset + 6] << 48;
            v |= (ulong) buf[offset + 7] << 56;
            return v;
        }

        private static void PutBytes(ulong[] input, byte[] output, int offset, int byteCount)
        {
            int j = 0;
            for (int i = 0; i < byteCount; i++)
            {
                //PutUInt64(output, i + offset, input[i / 8]);
                output[offset + i] = (byte) ((input[i/8] >> j) & 0xff);
                j = (j + 8)%64;
            }
        }

        #endregion
    }

    /// <summary>
    ///     Skein 224 bit
    /// </summary>
    public class Skein224 : Skein
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Skein224" /> class.
        /// </summary>
        public Skein224() : base(256, 224)
        {
        }
    }

    /// <summary>
    ///     Skein 256 bit
    /// </summary>
    public class Skein256 : Skein
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Skein256" /> class.
        /// </summary>
        public Skein256() : base(256, 256)
        {
        }
    }

    /// <summary>
    ///     Skein 384 bit
    /// </summary>
    public class Skein384 : Skein
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Skein384" /> class.
        /// </summary>
        public Skein384() : base(512, 384)
        {
        }
    }

    /// <summary>
    ///     Skein 512 bit
    /// </summary>
    public class Skein512 : Skein
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Skein512" /> class.
        /// </summary>
        public Skein512() : base(512, 512)
        {
        }
    }

    /// <summary>
    ///     Skein 1024 bit
    /// </summary>
    public class Skein1024 : Skein
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Skein1024" /> class.
        /// </summary>
        public Skein1024() : base(1024, 1024)
        {
        }
    }
}