using System;
using System.IO;
using System.Security.Cryptography;

namespace LLCryptoLib.Crypto
{
    /// <summary>
    ///     Rijndael encryption class for text
    /// </summary>
    /// <example>
    ///     <code>
    /// TextAlgorithmParameters parms = new TextAlgorithmParameters("llcryptopassword");
    /// TextCrypter textEncrypter = new TextCrypter(new TextRijndael(parms));
    /// encrypted = textEncrypter.Base64EncryptDecrypt(origString, true);
    /// Console.WriteLine("Encrypted string: " + encrypted);
    /// decrypted = textEncrypter.Base64EncryptDecrypt(encrypted, false);
    /// Console.WriteLine("Decrypted string: " + decrypted);
    /// </code>
    /// </example>
    internal class TextRijndael : TextAlgorithm
    {
        private readonly short sizeIV;

        private readonly short sizeKey;

        /// <summary>
        ///     Rijndael encryption class constructor
        /// </summary>
        /// <param name="p">Parametri (key and shift)</param>
        internal TextRijndael(TextAlgorithmParameters p)
            : base(TextAlgorithmType.BINARY)
        {
            if ((p.BlockSize == 0) || (p.KeySize == 0))
            {
                this.sizeKey = 16;
                this.sizeIV = 16;
            }
            else
            {
                this.sizeKey = p.KeySize;
                this.sizeIV = p.BlockSize;
            }
            this.GenerateKey(p.Key + string.Format(Config.NUMBERFORMAT, "{0}", p.Shift), this.sizeKey, this.sizeIV);
        }

        /// <summary>
        ///     Length of the key in bits
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal short KeySize
        {
            get { return (short) (this.sizeKey*8); }
        }

        /// <summary>
        ///     Length of the block in bits
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal short BlockSize
        {
            get { return (short) (this.sizeIV*8); }
        }

        /// <summary>
        ///     Code using Rijndael algoritml
        /// </summary>
        /// <param name="txt">String text to code (text must be UTF8 compatible)</param>
        /// <returns>Base64 representation of text</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public override string Code(string txt)
        {
            string output = "";

            try
            {
                byte[] intxt = TextEncryptionUtils.StringToBytes(txt);
                System.Diagnostics.Debug.WriteLine(string.Format(Config.NUMBERFORMAT,
                    "Rijndael Encoding with key={0} and block={1}", TextEncryptionUtils.BytesToBase64String(this.maKey),
                    TextEncryptionUtils.BytesToBase64String(this.maIV)));
                MemoryStream ms = this.EncryptData(intxt, true);
                output = TextEncryptionUtils.MemoryToBase64String(ms);
                System.Diagnostics.Debug.WriteLine(string.Format(Config.NUMBERFORMAT, "Final bytes count: {0}",
                    ms.ToArray().Length));
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Rijndael Encode error: " + e.Message);
                output = "Rijndael encryption failed: wrong password or invalid input data.";
            }

            return output;
        }

        /// <summary>
        ///     Decode using Rijndael algorithm
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public override string Decode(string txt)
        {
            string output = "";

            try
            {
                byte[] intxt = TextEncryptionUtils.Base64StringToBytes(txt);
                System.Diagnostics.Debug.WriteLine(string.Format(Config.NUMBERFORMAT, "Initial bytes count: {0}",
                    intxt.Length));
                System.Diagnostics.Debug.WriteLine(string.Format(Config.NUMBERFORMAT,
                    "Rijndael Decoding with key={0} and block={1}", TextEncryptionUtils.BytesToBase64String(this.maKey),
                    TextEncryptionUtils.BytesToBase64String(this.maIV)));
                MemoryStream ms = this.EncryptData(intxt, false);
                output = TextEncryptionUtils.MemoryToString(ms);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Rijndael Decode error: " + e.Message);
                output = "Rijndael decryption failed: wrong password or invalid input data.";
            }

            return output;
        }

        /// <summary>
        ///     Encrypts/Decrypts using Rijndael algorithm
        /// </summary>
        /// <param name="txt">Bytes to crypt/decrypt in bytes</param>
        /// <param name="isCrypting">If true, crypt, else decrypt</param>
        /// <returns></returns>
        internal MemoryStream EncryptData(byte[] txt, bool isCrypting)
        {
            //Create the memory streams 
            MemoryStream min = new MemoryStream(txt);
            MemoryStream mout = new MemoryStream();
            mout.SetLength(0);

            //Create variables to help with read and write.
            byte[] bin = new byte[100]; //This is intermediate storage for the encryption.
            long rdlen = 0; //This is the total number of bytes written.
            int len = 1; //This is the number of bytes to be written at a time.


            Rijndael des = this.createRijndael();
            CryptoStream encStream = null;

            if (isCrypting)
            {
                encStream = new CryptoStream(mout, des.CreateEncryptor(), CryptoStreamMode.Write);
                while (rdlen < min.Length)
                {
                    len = min.Read(bin, 0, 100);
                    encStream.Write(bin, 0, len);
                    rdlen += len;
                }
            }
            else
            {
                encStream = new CryptoStream(min, des.CreateDecryptor(), CryptoStreamMode.Read);
                while (len > 0)
                {
                    len = encStream.Read(bin, 0, 100);
                    mout.Write(bin, 0, len);
                }
            }


            encStream.Close();
            mout.Close();
            min.Close();


            return mout;
        }

        private Rijndael createRijndael()
        {
            bool sizeOk = false;
            bool blockOk = false;

            short keyLen = (short) (this.maKey.Length*8);
            short ivLen = (short) (this.maIV.Length*8);

            Rijndael rj = new RijndaelManaged();

            // Check key size
            KeySizes[] ks = rj.LegalKeySizes;
            foreach (KeySizes k in ks)
            {
                if ((keyLen >= k.MinSize) && (keyLen <= k.MaxSize))
                {
                    sizeOk = true;
                    break;
                }
            }

            // Check block size
            KeySizes[] bs = rj.LegalBlockSizes;
            foreach (KeySizes b in bs)
            {
                if ((ivLen >= b.MinSize) && (ivLen <= b.MaxSize))
                {
                    blockOk = true;
                    break;
                }
            }

            if (!sizeOk)
            {
                throw new LLCryptoLibException(string.Format(Config.NUMBERFORMAT,
                    "Key Size Rijndael Algorithm ({0}) not correct!", keyLen));
            }

            if (!blockOk)
            {
                throw new LLCryptoLibException(string.Format(Config.NUMBERFORMAT,
                    "Block Size Rijndael Algorithm ({0}) not correct!", ivLen));
            }

            rj.Key = this.maKey;
            rj.BlockSize = ivLen;
            rj.IV = this.maIV;

            return rj;
        }
    }
}