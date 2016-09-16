namespace LLCryptoLib.Hash
{
    /// <summary>Computes the ElfHash hash for the input data using the managed library.</summary>
    public class ElfHash : System.Security.Cryptography.HashAlgorithm
    {
        private uint ghash;
        private uint hash;


        /// <summary>Initializes a new instance of the ElfHash class.</summary>
        public ElfHash()
        {
            lock (this)
            {
                this.HashSizeValue = 32;
                this.Initialize();
            }
        }


        /// <summary>Initializes the algorithm.</summary>
        public override void Initialize()
        {
            lock (this)
            {
                this.State = 0;
                this.hash = 0;
                this.ghash = 0;
            }
        }


        /// <summary>Drives the hashing function.</summary>
        /// <param name="array">The array containing the data.</param>
        /// <param name="ibStart">The position in the array to begin reading from.</param>
        /// <param name="cbSize">How many bytes in the array to read.</param>
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            lock (this)
            {
                for (int i = ibStart; i < ibStart + cbSize; i++)
                {
                    this.hash = (this.hash << 4) + array[i];
                    this.ghash = this.hash & 0xF0000000;
                    if (this.ghash != 0)
                    {
                        this.hash ^= this.ghash >> 24;
                    }
                    this.hash &= ~this.ghash;
                }
            }
        }


        /// <summary>Performs any final activities required by the hash algorithm.</summary>
        /// <returns>The final hash value.</returns>
        protected override byte[] HashFinal()
        {
            lock (this)
            {
                return Utilities.UIntToByte(this.hash, EndianType.BigEndian);
            }
        }
    }
}