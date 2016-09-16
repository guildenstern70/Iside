using System;

namespace LLCryptoLib.Hash
{
    /// <summary>A class that contains the parameters necessary to initialize a CRC algorithm.</summary>
    public class CRCParameters : HashAlgorithmParameters
    {
        private int order;


        /// <summary>Initializes a new instance of the CRCParamters class.</summary>
        /// <param name="order">The order of the CRC (e.g., how many bits).</param>
        /// <param name="polynomial">The polynomial to use in the calculations.</param>
        /// <param name="initial">The initial value of the CRC.</param>
        /// <param name="finalXOR">The final value to XOR with the CRC.</param>
        /// <param name="reflectIn">Whether or not to reflect the incoming data before calculating.</param>
        public CRCParameters(int order, long polynomial, long initial, long finalXOR, bool reflectIn)
        {
            this.Order = order;
            this.Polynomial = polynomial;
            this.InitialValue = initial;
            this.FinalXORValue = finalXOR;
            this.ReflectInput = reflectIn;
        }


        /// <summary>Gets or sets the order of the CRC (e.g., how many bits).</summary>
        public int Order
        {
            get { return this.order; }
            set
            {
                if ((value%8 != 0) || (value < 8) || (value > 64))
                {
                    throw new ArgumentOutOfRangeException("Order", value,
                        "CRC Order must represent full bytes and be between 8 and 64.");
                }
                this.order = value;
            }
        }

        /// <summary>Gets or sets the polynomial to use in the CRC calculations.</summary>
        public long Polynomial { get; set; }

        /// <summary>Gets or sets the initial value of the CRC.</summary>
        public long InitialValue { get; set; }

        /// <summary>Gets or sets the final value to XOR with the CRC.</summary>
        public long FinalXORValue { get; set; }

        /// <summary>Gets or sets the value dictating whether or not to reflect the incoming data before calculating. (UART)</summary>
        public bool ReflectInput { get; set; }


        /// <summary>
        ///     Serves as a hash function for a particular type, suitable for use in hashing algorithms and data structures
        ///     like a hash table.
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            string temp = this.Polynomial + this.Order.ToString() + this.ReflectInput;
            return temp.GetHashCode();
        }


        /// <summary>Retrieves a standard set of CRC parameters.</summary>
        /// <param name="standard">The name of the standard parameter set to retrieve.</param>
        /// <returns>The CRC Parameters for the given standard.</returns>
        public static CRCParameters GetParameters(CRCStandard standard)
        {
            CRCParameters param = null;

            switch (standard)
            {
                case CRCStandard.CRC8:
                    param = new CRCParameters(8, 0x07, 0, 0, false);
                    break;

                case CRCStandard.CRC16_IBM:
                    param = new CRCParameters(16, 0x8005, 0, 0, false);
                    break;

                case CRCStandard.CRC16_CCITT:
                    param = new CRCParameters(16, 0x1021, 0xFFFF, 0, false);
                    break;

                case CRCStandard.CRC16_ARC:
                    param = new CRCParameters(16, 0x8005, 0, 0, true);
                    break;

                case CRCStandard.CRC16_XMODEM:
                    param = new CRCParameters(16, 0x8408, 0, 0, true);
                    break;

                case CRCStandard.CRC16_ZMODEM:
                    param = new CRCParameters(16, 0x1021, 0, 0, false);
                    break;

                case CRCStandard.CRC24:
                    param = new CRCParameters(24, 0x1864CFB, 0xB704CE, 0, false);
                    break;

                case CRCStandard.CRC32:
                    param = new CRCParameters(32, 0x04C11DB7, 0xFFFFFFFF, 0xFFFFFFFF, true);
                    break;

                case CRCStandard.CRC32_JAMCRC:
                    param = new CRCParameters(32, 0x04C11DB7, 0xFFFFFFFF, 0, true);
                    break;

                case CRCStandard.CRC32_BZIP2:
                    param = new CRCParameters(32, 0x04C11DB7, 0xFFFFFFFF, 0xFFFFFFFF, false);
                    break;

                case CRCStandard.CRC64_ISO:
                    param = new CRCParameters(64, 0x000000000000001B, 0, 0, true);
                    break;

                case CRCStandard.CRC64_ECMA:
                    param = new CRCParameters(64, 0x42F0E1EBA9EA3693, -1, -1, false);
                    break;
            }

            return param;
        }
    }
}