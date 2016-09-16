using System;

namespace LLCryptoLib.Hash
{
    /// <summary>A class that contains the parameters necessary to initialize a Snefru2 algorithm.</summary>
    public class Snefru2Parameters : HashAlgorithmParameters
    {
        private short length;
        private short passes;


        /// <summary>Initializes a new instance of the Snefru2Paramters class.</summary>
        /// <param name="passes">How many transformation passes to do.</param>
        /// <param name="length">The bit length of the final hash.</param>
        public Snefru2Parameters(short passes, short length)
        {
            this.Passes = passes;
            this.Length = length;
        }


        /// <summary>Gets or sets the number of passes.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
             "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public short Passes
        {
            get { return this.passes; }
            set
            {
                if ((value != 4) && (value != 8))
                {
                    throw new ArgumentException("The number of passes can only be 4 or 8.", "Passes");
                }
                this.passes = value;
            }
        }

        /// <summary>Gets or sets the bit length.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
             "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public short Length
        {
            get { return this.length; }
            set
            {
                if ((value != 128) && (value != 256))
                {
                    throw new ArgumentException("The Snefru2 bit length can only be 128 or 256 bits long.", "Length");
                }
                this.length = value;
            }
        }


        /// <summary>Retrieves a standard set of Snefru2 parameters.</summary>
        /// <param name="standard">The name of the standard parameter set to retrieve.</param>
        /// <returns>The Snefru2 Parameters for the given standard.</returns>
        public static Snefru2Parameters GetParameters(Snefru2Standard standard)
        {
            Snefru2Parameters temp;

            switch (standard)
            {
                case Snefru2Standard.Snefru2_4_128:
                    temp = new Snefru2Parameters(4, 128);
                    break;
                case Snefru2Standard.Snefru2_4_256:
                    temp = new Snefru2Parameters(4, 256);
                    break;
                case Snefru2Standard.Snefru2_8_128:
                    temp = new Snefru2Parameters(8, 128);
                    break;
                case Snefru2Standard.Snefru2_8_256:
                    temp = new Snefru2Parameters(8, 256);
                    break;
                default:
                    temp = new Snefru2Parameters(8, 256);
                    break;
            }

            return temp;
        }
    }
}