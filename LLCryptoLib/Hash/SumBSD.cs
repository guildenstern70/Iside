namespace LLCryptoLib.Hash
{
    /// <summary>Computes the BSD-style checksum for the input data using the managed library.</summary>
    public class SumBSD : System.Security.Cryptography.HashAlgorithm
    {
        private ushort checksum;


        /// <summary>Initializes a new instance of the SumBSD class.</summary>
        public SumBSD()
        {
            lock (this)
            {
                this.HashSizeValue = 16;
                this.Initialize();
            }
        }


        /// <summary>Initializes the algorithm.</summary>
        public override void Initialize()
        {
            lock (this)
            {
                this.State = 0;
                this.checksum = 0;
            }
        }


        /// <summary>Performs the hash algorithm on the data provided.</summary>
        /// <param name="array">The array containing the data.</param>
        /// <param name="ibStart">The position in the array to begin reading from.</param>
        /// <param name="cbSize">How many bytes in the array to read.</param>
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            lock (this)
            {
                for (int i = ibStart; i < ibStart + cbSize; i++)
                {
                    this.checksum = (ushort) ((this.checksum >> 1) + ((this.checksum & 1) << 15));
                    this.checksum += (ushort) (array[i] & 0xFF);
                    this.checksum &= 0xFFFF;
                }
            }
        }


        /// <summary>Performs any final activities required by the hash algorithm.</summary>
        /// <returns>The final hash value.</returns>
        protected override byte[] HashFinal()
        {
            lock (this)
            {
                return Utilities.UShortToByte(this.checksum, EndianType.BigEndian);
            }
        }
    }
}