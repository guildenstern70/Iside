namespace LLCryptoLib.Shred
{
    /// <summary>
    ///     Rewrites file area one time with 0x00 byte
    /// </summary>
    public sealed class ShredSimple : ShredBase
    {
        /// <summary>
        ///     ShredSimple constructor
        /// </summary>
        public ShredSimple()
        {
            this.bitSequence = new byte[1] {0x00};
            this._passes = 1;
        }

        #region IShredMethod Members

        /// <summary>
        ///     Shred Method Name
        /// </summary>
        public override string Name
        {
            get { return Strings.S00004; }
        }

        /// <summary>
        ///     Shred Enum ID
        /// </summary>
        public override AvailableShred Id
        {
            get { return AvailableShred.SIMPLE; }
        }

        /// <summary>
        ///     Return "Overwrite the file area with a series of 0 (0x00) bits"
        /// </summary>
        public override string Description
        {
            get { return Strings.S00005; }
        }

        #endregion
    }
}