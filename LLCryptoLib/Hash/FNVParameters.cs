using System;

namespace LLCryptoLib.Hash
{
    /// <summary>A class that contains the parameters necessary to initialize a FNV algorithm.</summary>
    public class FNVParameters : HashAlgorithmParameters
    {
        private int order;


        /// <summary>Initializes a new instance of the FNVParamters class.</summary>
        /// <param name="order">The order of the FNV (e.g., how many bits).</param>
        /// <param name="prime">The prime number to use in the FNV calculations.</param>
        /// <param name="offsetBasis">The offset basis of the FNV.</param>
        /// <param name="type">The FNV algorithm variation.</param>
        public FNVParameters(int order, long prime, long offsetBasis, FNVAlgorithmType type)
        {
            this.Order = order;
            this.Prime = prime;
            this.OffsetBasis = offsetBasis;
            this.Variation = type;
        }


        /// <summary>Gets or sets the order of the FNV (e.g., how many bits).</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
             "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public int Order
        {
            get { return this.order; }
            set
            {
                if ((value != 32) && (value != 64))
                {
                    throw new ArgumentException("The FNV order can only be 32 or 64 bits long.", "Order");
                }
                this.order = value;
            }
        }

        /// <summary>Gets or sets the prime number to use in the FNV calculations.</summary>
        public long Prime { get; set; }

        /// <summary>Gets or sets the offset basis of the FNV.</summary>
        public long OffsetBasis { get; set; }

        /// <summary>Gets or sets the FNV algorithm variation.</summary>
        public FNVAlgorithmType Variation { get; set; }


        /// <summary>Retrieves a standard set of FNV parameters.</summary>
        /// <param name="standard">The name of the standard parameter set to retrieve.</param>
        /// <returns>The FNV Parameters for the given standard.</returns>
        public static FNVParameters GetParameters(FNVStandard standard)
        {
            FNVParameters temp;

            switch (standard)
            {
                case FNVStandard.FNV0_32:
                    temp = new FNVParameters(32, 0x01000193, 0x00000000, FNVAlgorithmType.FNV1);
                    break;
                case FNVStandard.FNV0_64:
                    temp = new FNVParameters(64, 0x0100000001B3, 0x00000000, FNVAlgorithmType.FNV1);
                    break;
                case FNVStandard.FNV1_32:
                    temp = new FNVParameters(32, 0x01000193, 0x811C9DC5, FNVAlgorithmType.FNV1);
                    break;
                case FNVStandard.FNV1_64:
                    temp = new FNVParameters(64, 0x0100000001B3, unchecked((long) 0xCBF29CE484222325),
                        FNVAlgorithmType.FNV1);
                    break;
                case FNVStandard.FNV1A_32:
                    temp = new FNVParameters(32, 0x01000193, 0x811C9DC5, FNVAlgorithmType.FNV1A);
                    break;
                case FNVStandard.FNV1A_64:
                    temp = new FNVParameters(64, 0x0100000001B3, unchecked((long) 0xCBF29CE484222325),
                        FNVAlgorithmType.FNV1A);
                    break;
                default:
                    temp = new FNVParameters(32, 0x01000193, 0x811C9DC5, FNVAlgorithmType.FNV1);
                    break;
            }

            return temp;
        }
    }
}