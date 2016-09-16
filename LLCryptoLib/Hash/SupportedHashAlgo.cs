using System.Security.Cryptography;

namespace LLCryptoLib.Hash
{
    /// <summary>
    ///     SupportedHashAlgo.
    ///     Container for any supported Hash Algorithm.
    /// </summary>
    public class SupportedHashAlgo
    {
        private string desc;

        internal SupportedHashAlgo(AvailableHash id, string sName, bool needKey, bool isFast, HashAlgorithm ha)
        {
            this.Id = id;
            this.Name = sName;
            this.IsKeyed = needKey;
            this.IsFast = isFast;
            this.Algorithm = ha;
        }

        /// <summary>
        ///     Hash Algorithm ID
        /// </summary>
        public AvailableHash Id { get; }

        /// <summary>
        ///     Hash Algorithm Name
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Hash Algorithm Description
        /// </summary>
        public string Description
        {
            set { this.desc = value; }
            get
            {
                if (this.desc != null)
                {
                    return this.desc;
                }

                return "Unavailable";
            }
        }

        /// <summary>
        ///     If the Algorithm is keyed
        /// </summary>
        public bool IsKeyed { get; }

        /// <summary>
        ///     If the Algorithm is supposed to be fast
        /// </summary>
        public bool IsFast { get; }

        /// <summary>
        ///     The HashAlgorithm
        /// </summary>
        public HashAlgorithm Algorithm { get; }

        /// <summary>
        ///     A hash code for HashTable calculus.
        /// </summary>
        /// <returns>A hash code for HashTable calculus</returns>
        public override int GetHashCode()
        {
            int hidx = (int) this.Id;
            return hidx*this.Name.Length;
        }

        /// <summary>
        ///     The name of the hash algorithm
        /// </summary>
        /// <returns>The name of the hash algorithm</returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        ///     If two SupportedHashAlgo are the same
        /// </summary>
        /// <param name="obj">Another SupportedHashAlgo</param>
        /// <returns>True if the name of the two algorithms are the same</returns>
        public override bool Equals(object obj)
        {
            SupportedHashAlgo rr = obj as SupportedHashAlgo;

            if (obj == null)
            {
                return false;
            }

            bool eq = false;

            if ((this.Id == rr.Id) && this.Name.Equals(rr.Name))
            {
                eq = true;
            }

            return eq;
        }
    }
}