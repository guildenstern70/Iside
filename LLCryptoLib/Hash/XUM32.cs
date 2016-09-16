using System;

namespace LLCryptoLib.Hash
{
    /// <summary>Computes the XUM32 hash for the input data using the managed library.</summary>
    public class XUM32 : System.Security.Cryptography.HashAlgorithm
    {
        private readonly System.Security.Cryptography.HashAlgorithm crcHash;
        private readonly System.Security.Cryptography.HashAlgorithm elfHash;
        private uint length;


        /// <summary>Initializes a new instance of the XUMHash class.</summary>
        public XUM32()
        {
            lock (this)
            {
                this.HashSizeValue = 32;
                this.crcHash = new CRC(CRCParameters.GetParameters(CRCStandard.CRC32));
                this.elfHash = new ElfHash();
                this.Initialize();
            }
        }


        /// <summary>Initializes the algorithm.</summary>
        public override void Initialize()
        {
            lock (this)
            {
                this.State = 0;
                this.length = 0;
                this.crcHash.Initialize();
                this.elfHash.Initialize();
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
                byte[] temp = new byte[array.Length];
                Array.Copy(array, temp, array.Length);

                this.length += (uint) cbSize;

                this.elfHash.TransformBlock(array, ibStart, cbSize, temp, 0);
                this.crcHash.TransformBlock(array, ibStart, cbSize, temp, 0);
            }
        }


        /// <summary>Performs any final activities required by the hash algorithm.</summary>
        /// <returns>The final hash value.</returns>
        protected override byte[] HashFinal()
        {
            lock (this)
            {
                uint hash = Utilities.RotateLeft(this.length, 16);
                uint[] temp;

                this.crcHash.TransformFinalBlock(new byte[1], 0, 0);
                temp = Utilities.ByteToUInt(this.crcHash.Hash, EndianType.BigEndian);
                hash ^= temp[0];
                this.elfHash.TransformFinalBlock(new byte[1], 0, 0);
                temp = Utilities.ByteToUInt(this.elfHash.Hash, EndianType.BigEndian);
                hash ^= temp[0]%0x03E5;

                return Utilities.UIntToByte(hash, EndianType.BigEndian);
            }
        }
    }
}