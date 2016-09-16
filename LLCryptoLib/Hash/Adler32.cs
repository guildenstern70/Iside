namespace LLCryptoLib.Hash
{
    /// <summary>Computes the Adler-32 hash for the input data using the managed library.</summary>
    public class Adler32 : System.Security.Cryptography.HashAlgorithm
    {
        private uint checksum;


        /// <summary>Initializes a new instance of the Adler32 class.</summary>
        public Adler32()
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
                this.checksum = 1;
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
                int n;
                uint s1 = this.checksum & 0xFFFF;
                uint s2 = this.checksum >> 16;

                while (cbSize > 0)
                {
                    n = 3800 > cbSize ? cbSize : 3800;
                    cbSize -= n;

                    while (--n >= 0)
                    {
                        s1 = s1 + (uint) (array[ibStart++] & 0xFF);
                        s2 = s2 + s1;
                    }

                    s1 %= 65521;
                    s2 %= 65521;
                }

                this.checksum = (s2 << 16) | s1;
            }
        }


        /// <summary>Performs any final activities required by the hash algorithm.</summary>
        /// <returns>The final hash value.</returns>
        protected override byte[] HashFinal()
        {
            lock (this)
            {
                return Utilities.UIntToByte(this.checksum, EndianType.BigEndian);
            }
        }
    }

    /// <summary>
    ///     LLCryptoLib.Hash library contains the functions to generate
    ///     hash codes (message digests) from any file.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
         "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class NamespaceDoc
    {
    }
}