using System;

namespace LLCryptoLib.Hash
{
    /// <summary>Computes the GHash hash for the input data using the managed library.</summary>
    public class GHash : System.Security.Cryptography.HashAlgorithm
    {
        private uint hash;
        private readonly GHashParameters parameters;


        /// <summary>
        ///     Initializes a new instance of the <see cref="T:GHash" /> class.
        /// </summary>
        /// <param name="param">The Ghash parameters</param>
        public GHash(GHashParameters param)
        {
            lock (this)
            {
                if (param == null)
                {
                    throw new ArgumentNullException("param", "The GHashParameters cannot be null.");
                }
                this.parameters = param;
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
                    this.hash = (this.hash << this.parameters.Shift) + this.hash + array[i];
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