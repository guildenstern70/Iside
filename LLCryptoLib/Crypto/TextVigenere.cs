using System;
using System.Text;
using LLCryptoLib.Utils;

namespace LLCryptoLib.Crypto
{
    /// <summary>
    ///     Implements Vigenere/Polyalphabetical Text Crypto Algorithm.
    ///     The Vigenere encryption was the creation of the French diplomat, Blaise de Vigenere, 1523-1596.
    ///     Like Caesar and all the cryptographers that followed, he did not visualize the cipher in modular
    ///     arithmetical terms. Rather he viewed the cypher as a substitution cipher where a different alphabet
    ///     was used for the next letter of the message, with the alphabets repeating periodically --- according
    ///     to some key. Rather than setting several different alphabets, the cryptographer would use the Vigenere
    ///     square.
    ///     Here's the idea. For the given key word "FIRST", encrypt each letter of the message taken in the
    ///     left-most column to the letter in the keyword-letter column. Thus, the first five letters of the
    ///     message use the alphabets corresponding the the "F", "I", "R", "S", and "T" columns. So, the
    ///     Vigenere code with this keyword is really five Caesar shifts used in a cyclical fashion. Decription
    ///     is carried out working backwards from the keyword-columns to the left-most column. Because we are
    ///     really using five alphabets, the Vigenere encryption is sometimes called a polyalphabetic
    ///     (many + alphbets) code.
    /// </summary>
    /// <example>
    ///     Polyalphabetical text encryption:
    ///     <code>
    /// TextAlgorithmParameters parms = new TextAlgorithmParameters(3);
    /// TextCrypter textEncrypter = TextCrypterFactory.Create(SupportedTextAlgorithms.POLYALPHABETIC,parms);
    /// string encrypted = textEncrypter.TextEncryptDecrypt(origString, true);
    /// Console.WriteLine("Encrypted string: " + encrypted);
    /// string decrypted = textEncrypter.TextEncryptDecrypt(encrypted, false);
    /// Console.WriteLine("Decrypted string: " + decrypted);
    /// Console.WriteLine();
    /// </code>
    /// </example>
    public class TextVigenere : TextAlgorithm
    {
        /// <summary>
        ///     Algorithm key string
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")
        ] protected string key;

        internal TextAlgorithmParameters prvparam;

        /// <summary>
        ///     Algorithm shift number
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")
        ] protected int shift;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="p">Parametri must have a valid key</param>
        internal TextVigenere(TextAlgorithmParameters p) : base(TextAlgorithmType.TEXT)
        {
            this.shift = p.Shift;
            this.key = p.Key;
            this.prvparam = p;
        }

        /// <summary>
        ///     Return coded string
        /// </summary>
        /// <param name="txt">input clear string</param>
        /// <returns>coded string</returns>
        public override string Code(string txt)
        {
            return this.Algo(txt, true);
        }

        /// <summary>
        ///     Decode
        /// </summary>
        /// <param name="txt">coded string</param>
        /// <returns>clear string</returns>
        public override string Decode(string txt)
        {
            return this.Algo(txt, false);
        }

        /// <summary>
        ///     Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return "Vigenere with key=" + this.key + " and shift= " + this.shift;
        }


        /// <summary>
        ///     Encoding algorithm
        /// </summary>
        /// <param name="text">Encoded or decoded text</param>
        /// <param name="coding">If true coding, else decoding</param>
        /// <returns>An encoded/decoded string</returns>
        /// <exception cref="ArgumentNullException" />
        protected string Algo(string text, bool coding)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            StringBuilder tmp = new StringBuilder();
            string output;
            //Alfa alfa = Alfa.GetAlpha();
            char curchar;
            int npos;
            int carpos;

            for (int j = 0; j < text.Length; j++)
            {
                curchar = text[j];
                carpos = Alpha.GetPosOf(curchar);
                if (carpos >= 0)
                {
                    if (coding)
                    {
                        npos = carpos + this.prvparam.GetShiftAt(j);
                    }
                    else
                    {
                        npos = carpos - this.prvparam.GetShiftAt(j);
                    }
                    tmp.Append(Alpha.GetCharAt(npos));
                }
                else
                {
                    tmp.Append(curchar);
                }
            }

            output = tmp.ToString();

            if (this.shift != 0) // add TextROT13
            {
                int realshift = this.shift;
                if (!coding)
                {
                    realshift = -this.shift;
                }

                StringBuilder newtmp = new StringBuilder();

                for (int j = 0; j < tmp.Length; j++)
                {
                    curchar = output[j];
                    carpos = Alpha.GetPosOf(curchar);
                    if (carpos >= 0)
                    {
                        npos = carpos + realshift;
                        newtmp.Append(Alpha.GetCharAt(npos));
                    }
                    else
                    {
                        newtmp.Append(curchar);
                    }
                }

                output = newtmp.ToString();
            }

            return output;
        }
    }
}