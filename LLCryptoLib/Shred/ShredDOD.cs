namespace LLCryptoLib.Shred
{
    /// <summary>
    ///     Rewrites the file area 7 times with 0xFF, 0x00, 0xFF, 0x00, 0xFF, 0x00, 0xF6 bytes.
    /// </summary>
    public sealed class ShredDOD : ShredBase
    {
        /// <summary>
        ///     Shred DOD (Department of Defense) constructor
        /// </summary>
        public ShredDOD()
        {
            this.bitSequence = new byte[7] {0xFF, 0x00, 0xFF, 0x00, 0xFF, 0x00, 0xF6};
            this._passes = 7;
        }

        #region IShredMethod Members

        /// <summary>
        ///     Shred Method Name
        /// </summary>
        public override string Name
        {
            get { return Strings.S00010; }
        }

        /// <summary>
        ///     Shred Enum ID
        /// </summary>
        public override AvailableShred Id
        {
            get { return AvailableShred.DOD; }
        }

        /// <summary>
        ///     Returns "Three iterations completely overwrite the file area six times. Each iteration makes two write-passes over
        ///     the entire drive: the first pass inscribes ONEs (1) and the next pass inscribes ZEROes (0). After the third
        ///     iteration, a seventh pass writes the government-designated code 246 across the drive (in hex 0xF6)"
        /// </summary>
        public override string Description
        {
            get { return Strings.S00011; }
        }

        #endregion
    }
}