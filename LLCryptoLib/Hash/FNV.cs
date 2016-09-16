using System;

namespace LLCryptoLib.Hash
{
    /// <summary>Computes the FNV hash for the input data using the managed library.</summary>
    public class FNV : System.Security.Cryptography.HashAlgorithm
    {
        private ulong hash;
        private readonly FNVParameters parameters;


        /// <summary>Initializes a new instance of the FNV class.</summary>
        /// <param name="param">The parameters to utilize in the FNV calculation.</param>
        public FNV(FNVParameters param)
        {
            lock (this)
            {
                if (param == null)
                {
                    throw new ArgumentNullException("param", "The FNVParameters cannot be null.");
                }
                this.parameters = param;
                this.HashSizeValue = param.Order;
                this.Initialize();
            }
        }


        /// <summary>Initializes the algorithm.</summary>
        public override void Initialize()
        {
            lock (this)
            {
                this.State = 0;
                this.hash = (ulong) this.parameters.OffsetBasis;
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
                    if (this.parameters.Variation == FNVAlgorithmType.FNV1)
                    {
                        this.hash *= (ulong) this.parameters.Prime;
                        this.hash ^= array[i];
                    }
                    else if (this.parameters.Variation == FNVAlgorithmType.FNV1A)
                    {
                        this.hash ^= array[i];
                        this.hash = this.hash*(ulong) this.parameters.Prime;
                    }
                }
            }
        }


        /// <summary>Performs any final activities required by the hash algorithm.</summary>
        /// <returns>The final hash value.</returns>
        protected override byte[] HashFinal()
        {
            lock (this)
            {
                byte[] temp = Utilities.ULongToByte(this.hash, EndianType.BigEndian);
                byte[] final = new byte[this.parameters.Order/8];

                for (int i = final.Length - 1; i >= 0; i--)
                {
                    final[i] = temp[temp.Length - (this.parameters.Order/8 - i)];
                }

                return final;
            }
        }
    }
}