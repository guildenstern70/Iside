/*
 * LLCryptoLib - Advanced .NET Encryption and Hashing Library
 * v.$id$
 * 
 * The contents of this file are subject to the license distributed with
 * the package (the License). This file cannot be distributed without the 
 * original LittleLite Software license file. The distribution of this
 * file is subject to the agreement between the licensee and LittleLite
 * Software.
 * 
 * Customer that has purchased Source Code License may alter this
 * file and distribute the modified binary redistributables with applications. 
 * Except as expressly authorized in the License, customer shall not rent,
 * lease, distribute, sell, make available for download of this file. 
 * 
 * This software is not Open Source, nor Free. Its usage must adhere
 * with the License obtained from LittleLite Software.
 * 
 * The source code in this file may be derived, all or in part, from existing
 * other source code, where the original license permit to do so.
 * 
 * (C) 2003-2008 by LittleLite Software
 * 
 */


using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace LLCryptoLib.Crypto
{
    /// <summary>
    /// Twofish Stream class.
    /// The Twofish implementation allows only PaddingMode.Zeroes.
    /// Before using this algorithm, you should save the input file length somewhere.
    /// TODO: An implementation that saves this value somewhere.
    /// </summary>
    [Serializable]
    [ComVisible(false)]
    public class StreamTwofish : StreamAlgorithm
    {

        /// <summary>
        /// StreamBlowfish constructor
        /// </summary>
        public StreamTwofish()
            : base(16, 16, "Twofish (128 bit)")
        {
        }

        /// <summary>
        /// Return SupportedStreamAlgorithms.BLOWFISH
        /// </summary>
        public override SupportedStreamAlgorithms SupportedAlgorithmID
        {
            get
            {
                return SupportedStreamAlgorithms.TWOFISH;
            }
        }

        /// <summary>
        /// Return a newly constructed SymmetricAlgorithm of type BlowfishManaged
        /// </summary>
        public override SymmetricAlgorithm Algorithm
        {
            get
            {
                SymmetricAlgorithm sa = new TwofishManaged();
                sa.BlockSize = (this.BlockLen * 8);
                sa.KeySize = (this.KeyLen * 8);
                //sa.Padding = PaddingMode.PKCS7;
                //sa.Padding = PaddingMode.None;
                return sa;
            }
        }
    }
}