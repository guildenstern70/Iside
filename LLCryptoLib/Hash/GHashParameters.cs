namespace LLCryptoLib.Hash
{
    /// <summary>A class that contains the parameters necessary to initialize a GHash algorithm.</summary>
    public class GHashParameters : HashAlgorithmParameters
    {
        /// <summary>Initializes a new instance of the GHashParamters class.</summary>
        /// <param name="shift">How many bits to shift.</param>
        public GHashParameters(int shift)
        {
            this.Shift = shift;
        }


        /// <summary>Gets or sets the shift value.</summary>
        public int Shift { get; set; }


        /// <summary>Retrieves a standard set of GHash parameters.</summary>
        /// <param name="standard">The name of the standard parameter set to retrieve.</param>
        /// <returns>The GHash Parameters for the given standard.</returns>
        public static GHashParameters GetParameters(GHashStandard standard)
        {
            GHashParameters temp;

            switch (standard)
            {
                case GHashStandard.GHash_3:
                    temp = new GHashParameters(3);
                    break;
                case GHashStandard.GHash_5:
                    temp = new GHashParameters(5);
                    break;
                default:
                    temp = new GHashParameters(5);
                    break;
            }

            return temp;
        }
    }
}