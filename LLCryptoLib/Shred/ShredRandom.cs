using System;

namespace LLCryptoLib.Shred
{
    /// <summary>
    ///     Rewrites the file area 5 times with random bytes
    /// </summary>
    public sealed class ShredRandom : ShredBase
    {
        /// <summary>
        ///     ShredRandom constructor
        /// </summary>
        public ShredRandom()
        {
            Random rnd = new Random();
            this.bitSequence = new byte[5];
            this._passes = 5;
            rnd.NextBytes(this.bitSequence);
        }

        #region IShredMethod Members

        /// <summary>
        ///     Shred Method Name
        /// </summary>
        public override string Name
        {
            get { return Strings.S00008; }
        }

        /// <summary>
        ///     Shred Enum ID
        /// </summary>
        public override AvailableShred Id
        {
            get { return AvailableShred.RANDOM; }
        }

        /// <summary>
        ///     Returns "Five times overwrite the file area with a sequence of pseudorandom bits"
        /// </summary>
        public override string Description
        {
            get { return Strings.S00009; }
        }

        #endregion
    }
}