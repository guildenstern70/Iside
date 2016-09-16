using System;
using System.Collections;

namespace LLCryptoLib.Hash
{
    /// <summary>Computes the CRC hash for the input data using the managed library.</summary>
    public class CRC : System.Security.Cryptography.HashAlgorithm
    {
        private static readonly Hashtable lookupTables;
        private long checksum;
        private readonly long[] lookup;

        private readonly CRCParameters parameters;
        private readonly long registerMask;


        // Pre-build the more popular lookup tables.
        static CRC()
        {
            lookupTables = new Hashtable();
            BuildLookup(CRCParameters.GetParameters(CRCStandard.CRC32));
        }


        /// <summary>Initializes a new instance of the CRC class.</summary>
        /// <param name="param">The parameters to utilize in the CRC calculation.</param>
        /// <exception cref="ArgumentNullException" />
        public CRC(CRCParameters param)
        {
            lock (this)
            {
                if (param == null)
                {
                    throw new ArgumentNullException("param", "The CRCParameters cannot be null.");
                }
                this.parameters = param;
                this.HashSizeValue = param.Order;

                BuildLookup(param);
                this.lookup = (long[]) lookupTables[param];
                if (param.Order == 64)
                {
                    this.registerMask = 0x00FFFFFFFFFFFFFF;
                }
                else
                {
                    this.registerMask = (long) (Math.Pow(2, param.Order - 8) - 1);
                }

                this.Initialize();
            }
        }


        /// <summary>Build the CRC lookup table for a given polynomial.</summary>
        private static void BuildLookup(CRCParameters param)
        {
            if (lookupTables.Contains(param))
            {
                // No sense in creating the table twice.
                return;
            }

            long[] table = new long[256];
            long topBit = (long) 1 << (param.Order - 1);
            long widthMask = (((1 << (param.Order - 1)) - 1) << 1) | 1;

            // Build the table.
            for (int i = 0; i < table.Length; i++)
            {
                table[i] = i;

                if (param.ReflectInput)
                {
                    table[i] = Reflect(i, 8);
                }

                table[i] = table[i] << (param.Order - 8);

                for (int j = 0; j < 8; j++)
                {
                    if ((table[i] & topBit) != 0)
                    {
                        table[i] = (table[i] << 1) ^ param.Polynomial;
                    }
                    else
                    {
                        table[i] <<= 1;
                    }
                }

                if (param.ReflectInput)
                {
                    table[i] = Reflect(table[i], param.Order);
                }

                table[i] &= widthMask;
            }

            // Add the new lookup table.
            lookupTables.Add(param, table);
        }


        /// <summary>Initializes the algorithm.</summary>
        public override void Initialize()
        {
            lock (this)
            {
                this.State = 0;
                this.checksum = this.parameters.InitialValue;
                if (this.parameters.ReflectInput)
                {
                    this.checksum = Reflect(this.checksum, this.parameters.Order);
                }
            }
        }


        /// <summary>Drives the hashing function.</summary>
        /// <param name="array">The array containing the data.</param>
        /// <param name="ibStart">The position in the array to begin reading from.</param>
        /// <param name="cbSize">How many bytes in the array to read.</param>
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            lock (this)
            {
                for (int i = ibStart; i < cbSize - ibStart; i++)
                {
                    if (this.parameters.ReflectInput)
                    {
                        this.checksum = ((this.checksum >> 8) & this.registerMask) ^
                                        this.lookup[(this.checksum ^ array[i]) & 0xFF];
                    }
                    else
                    {
                        this.checksum = (this.checksum << 8) ^
                                        this.lookup[((this.checksum >> (this.parameters.Order - 8)) ^ array[i]) & 0xFF];
                    }
                }
            }
        }


        /// <summary>Performs any final activities required by the hash algorithm.</summary>
        /// <returns>The final hash value.</returns>
        protected override byte[] HashFinal()
        {
            lock (this)
            {
                int i, shift, numBytes;
                byte[] temp;

                this.checksum ^= this.parameters.FinalXORValue;

                numBytes = this.parameters.Order/8;
                if (this.parameters.Order - numBytes*8 > 0)
                {
                    numBytes++;
                }
                temp = new byte[numBytes];
                for (i = numBytes - 1, shift = 0; i >= 0; i--, shift += 8)
                {
                    temp[i] = (byte) ((this.checksum >> shift) & 0xFF);
                }

                return temp;
            }
        }


        /// <summary>Reflects the lower bits of the value provided.</summary>
        /// <param name="data">The value to reflect.</param>
        /// <param name="numBits">The number of bits to reflect.</param>
        /// <returns>The reflected value.</returns>
        private static long Reflect(long data, int numBits)
        {
            long temp = data;

            for (int i = 0; i < numBits; i++)
            {
                long bitMask = (long) 1 << (numBits - 1 - i);

                if ((temp & 1) != 0)
                {
                    data |= bitMask;
                }
                else
                {
                    data &= ~bitMask;
                }

                temp >>= 1;
            }

            return data;
        }
    }
}