using System;
using System.IO;
using System.Security.Cryptography;

namespace LLCryptoLib.Crypto
{
    /// <summary>
    ///     DES algorithm for text.
    ///     When encrypting, the algorithm is applied to the byte encoding of the input text, see
    ///     <see cref="TextEncryptionUtils.StringToBytes" />,
    ///     then, after encryption, rendered to a Base64 string with <see cref="TextEncryptionUtils.MemoryToBase64String" />.
    ///     When decrypting, the text is first trasformed into a byte sequence with
    ///     <see cref="TextEncryptionUtils.Base64StringToBytes" />,
    ///     then decrypted, then the resulting bytes are transformed to a string with
    ///     <see cref="TextEncryptionUtils.MemoryToString" />
    /// </summary>
    /// <example>
    ///     <code>
    /// TextAlgorithmParameters parms = new TextAlgorithmParameters("llcryptopassword");
    /// TextCrypter textEncrypter = new TextCrypter(new TextDES(parms));
    /// encrypted = textEncrypter.Base64EncryptDecrypt(origString, true);
    /// Console.WriteLine("Encrypted string: " + encrypted);
    /// decrypted = textEncrypter.Base64EncryptDecrypt(encrypted, false);
    /// Console.WriteLine("Decrypted string: " + decrypted);
    /// </code>
    /// </example>
    internal class TextDES : TextAlgorithm
    {
        /// <summary>
        ///     DES class
        /// </summary>
        /// <param name="p">Parametri (key and shift)</param>
        internal TextDES(TextAlgorithmParameters p)
            : base(TextAlgorithmType.BINARY)
        {
            string key = p.Key + Convert.ToString(p.Shift);
            System.Diagnostics.Debug.WriteLine("Using DES with key = " + key);
            this.GenerateKey(key, 8, 8);
            // TextVigenere size = 8*8 bytes
            // Block size = 8*8 bytes
        }

        /// <summary>
        ///     Code using DES algoritml
        /// </summary>
        /// <param name="txt">String text to code (text must be UTF8 compatible)</param>
        /// <returns>Base64 representation of text</returns>
        public override string Code(string txt)
        {
            string output = string.Empty;

            try
            {
                byte[] intxt = TextEncryptionUtils.StringToBytes(txt);
                System.Diagnostics.Debug.WriteLine(string.Format(Config.NUMBERFORMAT,
                    "DES Encoding with key={0} and block={1}", TextEncryptionUtils.BytesToBase64String(this.maKey),
                    TextEncryptionUtils.BytesToBase64String(this.maIV)));
                MemoryStream ms = this.EncryptData(intxt, true);
                output = TextEncryptionUtils.MemoryToBase64String(ms);
                System.Diagnostics.Debug.WriteLine(string.Format(Config.NUMBERFORMAT, "Final bytes count: {0}",
                    ms.ToArray().Length));
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("DES Code error: " + e.Message);
                output = "DES encryption failed: wrong password or invalid input data.";
            }

            return output;
        }

        /// <summary>
        ///     Decode using DES algorithm
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public override string Decode(string txt)
        {
            string output = "";

            try
            {
                byte[] intxt = TextEncryptionUtils.Base64StringToBytes(txt);
#if DEBUG
                string msg = string.Format(Config.NUMBERFORMAT, "DES Decoding with key={0} and block={1}",
                    TextEncryptionUtils.BytesToBase64String(this.maKey),
                    TextEncryptionUtils.BytesToBase64String(this.maIV));
                System.Diagnostics.Debug.WriteLine(msg);
#endif
                MemoryStream ms = this.EncryptData(intxt, false);
                output = TextEncryptionUtils.MemoryToString(ms);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("DES Decode error: " + e.Message);
                output = "DES decryption failed: wrong password or invalid input data.";
            }

            return output;
        }

        /// <summary>
        ///     Encrypts/Decrypts using DES algorithm
        /// </summary>
        /// <param name="txt">Bytes to crypt/decrypt in bytes</param>
        /// <param name="isCrypting">If true, crypt, else decrypt</param>
        /// <returns></returns>
        private MemoryStream EncryptData(byte[] txt, bool isCrypting)
        {
            //Create the memory streams 
            MemoryStream min = new MemoryStream(txt);
            MemoryStream mout = new MemoryStream();
            mout.SetLength(0);

            //Create variables to help with read and write.
            byte[] bin = new byte[100]; //This is intermediate storage for the encryption.
            long rdlen = 0; //This is the total number of bytes written.
            int len = 1; //This is the number of bytes to be written at a time.

            DES des = this.createDES();
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

        private DES createDES()
        {
            bool sizeOk = false;
            bool blockOk = false;

            short keyLen = (short) (this.maKey.Length*8);
            short ivLen = (short) (this.maIV.Length*8);

            DES des = new DESCryptoServiceProvider();

            // Check key size
            KeySizes[] ks = des.LegalKeySizes;
            foreach (KeySizes k in ks)
            {
                if ((keyLen >= k.MinSize) && (keyLen <= k.MaxSize))
                {
                    sizeOk = true;
                    break;
                }
            }

            // Check block size
            KeySizes[] bs = des.LegalBlockSizes;
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
                System.Diagnostics.Debug.WriteLine("Key Size DES Algorithm not correct!");
                throw new LLCryptoLibException("Key Size DES Algorithm not correct!");
            }

            if (!blockOk)
            {
                System.Diagnostics.Debug.WriteLine("Block Size DES Algorithm not correct!");
                throw new LLCryptoLibException("Block Size DES Algorithm not correct!");
            }

            des.Key = this.maKey;
            des.IV = this.maIV;

            return des;
        }
    }
}