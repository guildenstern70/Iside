namespace LLCryptoLib.Shred
{
    /// <summary>
    ///     Base class for shred methods
    /// </summary>
    public abstract class ShredBase : IShredMethod
    {
        /// <summary>
        ///     Shred method passes. IE: Complex method has 3 passes.
        /// </summary>
        protected int _passes;

        /// <summary>
        ///     Shred method bit sequence
        /// </summary>
        protected byte[] bitSequence;

        /// <summary>
        ///     Shredding method Name
        /// </summary>
        /// <value></value>
        public abstract string Name { get; }

        /// <summary>
        ///     Shredding method Available Shred Enum
        /// </summary>
        /// <value></value>
        public abstract AvailableShred Id { get; }

        /// <summary>
        ///     Shredding method Detailed description
        /// </summary>
        /// <value></value>
        public abstract string Description { get; }

        /// <summary>
        ///     Returns method passes
        /// </summary>
        public int Passes
        {
            get { return this._passes; }
        }

        /// <summary>
        ///     Returns the DOD byte sequence: 0xFF, 0x00, 0xFF, 0x00, 0xFF, 0x00, 0xF6
        /// </summary>
        public byte[] GetSequence()
        {
            return this.bitSequence;
        }

        /// <summary>
        ///     Returns the method description string.
        /// </summary>
        /// <returns>Returns the method description string, ie:"DoD 5220-22M"</returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}