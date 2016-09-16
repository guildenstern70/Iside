using System;

namespace LLCryptoLib.Shred
{
    /// <summary>
    ///     HMG Infosec Standard 5 Enhanced shred method.
    ///     This shred method overwrites the file area with 0's, 1's and finally a random byte.
    /// </summary>
    public sealed class ShredHmgEnh : ShredBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ShredHmgEnh" /> class.
        /// </summary>
        public ShredHmgEnh()
        {
            this.bitSequence = new byte[3] {0x00, 0xFF, 0x22};
            Random rndSeed = new Random();
            this.bitSequence[2] = (byte) rndSeed.Next(255);
            this._passes = 3;
        }

        /// <summary>
        ///     Shredding method Name
        /// </summary>
        /// <value></value>
        public override string Name
        {
            get { return Strings.S00014; }
        }

        /// <summary>
        ///     Shredding method Available Shred Enum
        /// </summary>
        /// <value></value>
        public override AvailableShred Id
        {
            get { return AvailableShred.HMGIS5ENH; }
        }

        /// <summary>
        ///     Shredding method Detailed description
        /// </summary>
        /// <value></value>
        public override string Description
        {
            get { return Strings.S00015; }
        }
    }
}