namespace LLCryptoLib.Hash
{
    /// <summary>Computes the SysV-style checksum for the input data using the managed library.</summary>
    public class SumSysV : System.Security.Cryptography.HashAlgorithm
    {
        private ushort checksum;


        /// <summary>Initializes a new instance of the SumSysV class.</summary>
        public SumSysV()
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
                    this.checksum += (ushort) (array[i] & 0xFF);
                }
            }
        }


        /// <summary>Performs any final activities required by the hash algorithm.</summary>
        /// <returns>The final hash value.</returns>
        protected override byte[] HashFinal()
        {
            lock (this)
            {
                long r = (this.checksum & 0xFFFF) + (((this.checksum & 0xffffffff) >> 16) & 0xFFFF);
                this.checksum = (ushort) ((r & 0xFFFF) + (r >> 16));
                return Utilities.UShortToByte(this.checksum, EndianType.BigEndian);
            }
        }
    }
}