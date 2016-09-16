using System;

namespace LLCryptoLib.Shred
{
    /// <summary>
    ///     Rewrites the file area 35 times with 0x34, 0x12, 0x1B, 0x00, 0x55, 0xAA, 0x24, 0x92,
    ///     0x49, 0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66,
    ///     0x77, 0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE,
    ///     0xFF, 0x24, 0x92, 0x49, 0xDB, 0x6D, 0xB6, 0x12,
    ///     0xFF, 0x82, 0x9A
    /// </summary>
    public sealed class ShredGutmann : ShredBase
    {
        /// <summary>
        ///     Gutmann constructor
        /// </summary>
        public ShredGutmann()
        {
            Random rnd = new Random();

            this.bitSequence = new byte[35]
            {
                0x34, 0x12, 0x1B, 0x00, 0x55, 0xAA, 0x24, 0x92,
                0x49, 0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66,
                0x77, 0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE,
                0xFF, 0x24, 0x92, 0x49, 0xDB, 0x6D, 0xB6, 0x12,
                0xFF, 0x82, 0x9A
            };

            byte[] initialSequence = new byte[4];
            byte[] endingSequence = new byte[4];

            rnd.NextBytes(initialSequence);
            rnd.NextBytes(endingSequence);

            this._passes = 35;

            for (int j = 0; j < 4; j++)
            {
                this.bitSequence[0 + j] = initialSequence[j];
                this.bitSequence[this._passes - 4 + j] = endingSequence[j];
            }
        }

        #region IShredMethod Members

        /// <summary>
        ///     Shred Method Name
        /// </summary>
        public override string Name
        {
            get { return Strings.S00012; }
        }

        /// <summary>
        ///     Shred Enum ID
        /// </summary>
        public override AvailableShred Id
        {
            get { return AvailableShred.GUTMANN; }
        }

        /// <summary>
        ///     Return "Overwrite the file area following the Gutmann standard recommendations."
        /// </summary>
        public override string Description
        {
            get { return Strings.S00013; }
        }

        #endregion
    }
}