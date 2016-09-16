using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace LLCryptoLib.Hash
{
    /// <summary>Computes Parallel hashes for the input data using the managed libraries.</summary>
    public class ParallelHash : HashAlgorithm
    {
        private readonly List<HashAlgorithm> hashers;

        /// <summary>Initializes an instance of ParellelHash.</summary>
        /// <param name="hashers">The list of HashAlgorithms to use in the calculations.</param>
        public ParallelHash(params HashAlgorithm[] hashers)
        {
            this.hashers = new List<HashAlgorithm>();
            for (int i = 0; i < hashers.Length; i++)
            {
                this.hashers.Add(hashers[i]);
                this.HashSizeValue += hashers[i].HashSize;
            }
        }


        /// <summary>Initializes the algorithm.</summary>
        public override void Initialize()
        {
            lock (this)
            {
                this.State = 0;
                for (int i = 0; i < this.hashers.Count; i++)
                {
                    this.hashers[i].Initialize();
                }
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
                byte[] temp = new byte[array.Length];

                for (int i = 0; i < this.hashers.Count; i++)
                {
                    this.hashers[i].TransformBlock(array, ibStart, cbSize, temp, 0);
                }
            }
        }

        /// <summary>Performs any final activities required by the hash algorithm.</summary>
        /// <returns>The final hash value.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
             "CA1817:DoNotCallPropertiesThatCloneValuesInLoops")]
        protected override byte[] HashFinal()
        {
            lock (this)
            {
                byte[] hash = new byte[this.HashSize/8];
                byte[] dummy = new byte[1];
                byte[] temp;
                int position = 0;

                for (int i = 0; i < this.hashers.Count; i++)
                {
                    this.hashers[i].TransformFinalBlock(dummy, 0, 0);
                    temp = this.hashers[i].Hash;
                    Array.Copy(temp, 0, hash, position, temp.Length);
                    position += temp.Length;
                }

                return hash;
            }
        }
    }
}