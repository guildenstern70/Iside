using System;

namespace LLCryptoLib.Shred
{
    /// <summary>
    ///     Rewrites the file area 3 times with 0xFF, 0x00, random bytes
    /// </summary>
    public sealed class ShredComplex : ShredBase
    {
        /// <summary>
        ///     Shred complex constructor
        /// </summary>
        public ShredComplex()
        {
            this.bitSequence = new byte[3] {0xFF, 0x00, 0xFF};
            Random rndSeed = new Random();
            this.bitSequence[2] = (byte) rndSeed.Next(255);
            this._passes = 3;
        }

        #region IShredMethod Members

        /// <summary>
        ///     Shred Method Name
        /// </summary>
        public override string Name
        {
            get { return Strings.S00006; }
        }

        /// <summary>
        ///     Shred Enum ID
        /// </summary>
        public override AvailableShred Id
        {
            get { return AvailableShred.COMPLEX; }
        }

        /// <summary>
        ///     Shred complex description
        /// </summary>
        public override string Description
        {
            get { return Strings.S00007; }
        }

        #endregion
    }
}