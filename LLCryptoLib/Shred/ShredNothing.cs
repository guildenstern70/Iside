namespace LLCryptoLib.Shred
{
    /// <summary>
    ///     No shred option. It deletes the file with OS delete.
    /// </summary>
    public sealed class ShredNothing : ShredBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ShredNothing" /> class.
        /// </summary>
        public ShredNothing()
        {
            this.bitSequence = null;
            this._passes = 0;
        }

        #region IShredMethod Members

        /// <summary>
        ///     Shred Method Name
        /// </summary>
        public override string Name
        {
            get { return Strings.S00001; }
        }

        /// <summary>
        ///     Shred Enum ID
        /// </summary>
        public override AvailableShred Id
        {
            get { return AvailableShred.NOTHING; }
        }

        /// <summary>
        ///     Shredding description
        /// </summary>
        public override string Description
        {
            get { return Strings.S00002; }
        }

        #endregion
    }
}