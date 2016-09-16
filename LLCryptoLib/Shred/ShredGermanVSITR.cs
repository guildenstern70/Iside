namespace LLCryptoLib.Shred
{
    /// <summary>
    ///     German VSITR shred method.
    ///     This shred method overwrites the file area with 0's, 1's and finally a random byte.
    /// </summary>
    public sealed class ShredGermanVSITR : ShredBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ShredGermanVSITR" /> class.
        /// </summary>
        public ShredGermanVSITR()
        {
            this.bitSequence = new byte[7] {0x00, 0xFF, 0x00, 0xFF, 0x00, 0xFF, 0x41};
            this._passes = 7;
        }

        /// <summary>
        ///     Shredding method Name
        /// </summary>
        /// <value></value>
        public override string Name
        {
            get { return Strings.S00016; }
        }

        /// <summary>
        ///     Shredding method Available Shred Enum
        /// </summary>
        /// <value></value>
        public override AvailableShred Id
        {
            get { return AvailableShred.GERMAN; }
        }

        /// <summary>
        ///     Shredding method Detailed description
        /// </summary>
        /// <value></value>
        public override string Description
        {
            get { return Strings.S00017; }
        }
    }
}