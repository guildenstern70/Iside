using System;
using System.Text;

namespace LLCryptoLib.Crypto
{
    /// <summary>
    ///     TextPlayfair cipher.
    ///     The TextPlayfair is a primitively modern reckoning block cipher. Any new personal computer sold
    ///     today can break a message encoded with it in a matter of seconds. That is, with the proper
    ///     software, you could use such a computer to discover the original text without knowing the cipher key.
    ///     Some skilled cryptogrophists and puzzle experts can even break it with nothing more than pen and paper.
    ///     Nonetheless, it uses some principles common to modern computer block ciphers. Understanding the
    ///     TextPlayfair will give you a beginning insight into modern cryptographywithout all the complex mathematics
    ///     and number theory.
    ///     TextPlayfair Cipher uses a 5x5 or 9x9 square, in which the letters of an agreed key word or phrase are entered
    ///     (suppressing duplicates), followed by the rest of the alphabet in order (if 5x5 is used then an alphabet with
    ///     25 letters is used where I and J would usually be combined together, if 9x9 is used a broader range of symbols
    ///     is used. The more the symbols, the more the characters that can be encrypted).
    ///     The message to be enciphered is split into pairs of letters. If the two letters in the pair are in the same row,
    ///     the letters to the right of each are used. If they are in the same column, the letters below each are used.
    ///     Otherwise, the letters at the opposite corners of the rectangle are used.
    ///     Special treatment is required for identical pairs of letters and a single letter left over at the end.
    ///     Typically an obscure letter such as X would have been inserted to pad out the message.
    ///     LLCryptoLib implementations assumes, instead of the original 5x5 or 9x9 cipher, a combined 36x36 square.
    /// </summary>
    /// <example>
    ///     Playfair text encryption:
    ///     <code>
    /// TextAlgorithmParameters parms = null;
    /// TextCrypter textEncrypter = TextCrypterFactory.Create(SupportedTextAlgorithms.PLAYFAIR, parms);
    /// string encrypted = textEncrypter.TextEncryptDecrypt(origString, true);
    /// Console.WriteLine("Encrypted string: " + encrypted);
    /// string decrypted = textEncrypter.TextEncryptDecrypt(encrypted, false);
    /// Console.WriteLine("Decrypted string: " + decrypted);
    /// Console.WriteLine();
    /// </code>
    /// </example>
    public class TextPlayfair : TextAlgorithm
    {
        private class RowCol
        {
            public readonly short Col;
            public readonly short Row;

            public RowCol(short r, short c)
            {
                this.Row = r;
                this.Col = c;
            }
        }

        private const char SPECIAL = '^';

#if MONO


		private static readonly char[][] CIPHER1 =
		{ 
			new char[] {';','m','K','=','l','@','E','b','9'}, 
			new char[] {'%','1','p','-','i','5','o','U','.'}, 
			new char[] {'z','u','M','N','T','+','>','f','7'}, 
			new char[] {'c','$','R','W','!','G','O','F','3'}, 
			new char[] {'s','[','/','n','&','A','r','C','w'}, 
			new char[] {'<','2',')','J','Y','(','V','q','y'}, 
			new char[] {'a','B','g','d','4','k','v','D','Q'}, 
			new char[] {'?','e','6','"','S','h',':','t','#'}, 
			new char[] {'P','j','I','*','L','H',',','0','8'}
		};

		private static readonly char[][] CIPHER2 =
		{
			new char[] {'7','Q','-','6','#','v','!','P','a'}, 
			new char[] {'E','j','"','q','z','O','4','d','A'}, 
			new char[] {'U','h','N','Z','3','L','F','w','D'}, 
			new char[] {'=','t','T','/','f','K','s','m','8'}, 
			new char[] {'u','S','>','r','9','?','c','l','<'}, 
			new char[] {'e',',','b',')','*','.','Y','1','R'}, 
			new char[] {';','%','5',':','o','0','$','C','&'}, 
			new char[] {'@','B','M','g','I','W','X','k','+'}, 
			new char[] {'H','[','x','2','J','(','n','p','i'} 
		};

		private static readonly char[][] CIPHER3 =
		{
			new char[] {'2','D','(','r','B','y','V','b','n'}, 
			new char[] {'Z','F','6','9','k','3','P','m','p'}, 
			new char[] {'f',')','J','/','"','i','s','[','?'}, 
			new char[] {'&','q','h','#','5','@','T','S','I'}, 
			new char[] {';','R','=','a','X','u','O','G','.'}, 
			new char[] {'e','z','N','>','x','4','o','w','H'}, 
			new char[] {',','W','Q','0','A','$','*','+','8'}, 
			new char[] {'K','t','d','l','j',':','v','E','g'}, 
			new char[] {'U','%','-','L','M','C','7','c','Y'} 
		};

		private static readonly char[][] CIPHER4 =	
		{
			new char[] {'6','X','F','4','b','k','Q','2','<'}, 
			new char[] {'l','u','"','r','g','S','c','A','N'}, 
			new char[] {'*','o','v','1',':','@','#','/','?'}, 
			new char[] {'T','V','W','3','G','.','t','Z','d'}, 
			new char[] {'%','M','Y','7','m','h','J','L','O'}, 
			new char[] {'w',',','[','0','&','>','e','q',';'}, 
			new char[] {'E','B','i','n','j','K','a','8','+'}, 
			new char[] {'$','x','f','-','H','y','U','s','!'}, 
			new char[] {'I','p','D','9','(','C',')','P','R'} 
		};

#else

        private static readonly char[][] CIPHER1 =
        {
            new[] {'q', 'b', 'c', 'd', 'S', 'A', 'B', 'C', '*'},
            new[] {'f', 'g', 'Y', 'i', 'j', 'D', 'E', 'F', 'Q'},
            new[] {'p', 'a', 'r', '(', 't', '7', 'K', '4', '/'},
            new[] {'u', 'w', 'v', 'x', 'z', 'M', 'N', 'O', '"'},
            new[] {'P', '+', 'y', 'R', 'e', 'T', 'U', 'V', 'ò'},
            new[] {'W', '=', 'h', 'Z', ' ', '.', ',', ';', 'à'},
            new[] {'k', 'è', 'm', 'n', 'o', 'G', 'H', 'I', 'X'},
            new[] {'1', '2', '3', 'L', '5', '6', 'J', '8', 'ì'},
            new[] {'9', '0', '!', '@', '$', '%', 's', ')', 'l'}
        };

        private static readonly char[][] CIPHER2 =
        {
            new[] {'P', '@', 'y', 'R', 'S', 'T', 'U', 'V', 'ò'},
            new[] {'u', 'w', 'v', 'e', 'z', 'M', 'N', 'O', '"'},
            new[] {'a', 'b', 'c', 'd', 'x', 'A', '5', 'C', '*'},
            new[] {'f', 'g', '%', 'i', 'j', 'D', 'E', '9', '+'},
            new[] {'p', 'q', 'r', 's', 't', 'J', 'K', 'L', '/'},
            new[] {'k', 'l', 'm', 'n', 'o', 'G', 'H', 'I', '='},
            new[] {'W', 'X', 'Y', 'Z', ' ', '.', ',', ';', 'à'},
            new[] {'1', '2', '3', '4', 'B', '6', '7', '8', 'ì'},
            new[] {'F', '0', '!', 'Q', '$', 'h', '(', ')', 'è'}
        };

        private static readonly char[][] CIPHER3 =
        {
            new[] {'f', 'g', 'w', 'i', 'j', 'D', 'ò', 'F', '+'},
            new[] {'k', 'l', '$', 'n', 'o', 'G', 'H', 'I', '='},
            new[] {'p', '8', 'r', 's', 't', 'J', 'K', 'L', '/'},
            new[] {'a', '!', 'c', 'd', 'e', 'A', 'T', 'C', '*'},
            new[] {'u', 'h', 'v', 'x', 'z', 'M', 'N', 'O', '"'},
            new[] {'P', 'Q', 'y', 'R', '3', 'B', 'U', 'V', 'E'},
            new[] {'W', 'X', 'Y', 'Z', ' ', '.', ',', ';', 'à'},
            new[] {'1', '5', 'S', '4', '2', '6', '7', 'q', 'ì'},
            new[] {'9', '0', 'b', '@', 'm', '%', '(', ')', 'è'}
        };

        private static readonly char[][] CIPHER4 =
        {
            new[] {'$', 'l', 'B', 'n', 'o', 'G', 'H', 'I', '='},
            new[] {'p', 'q', 'r', 's', 't', 'J', 'K', 'L', '/'},
            new[] {'9', '0', '!', 'e', 'k', '%', '(', ')', 'è'},
            new[] {'P', 'X', 'y', 'R', 'S', '4', '6', 'V', 'ò'},
            new[] {'f', 'g', 'h', 'N', 'j', 'D', '8', 'F', '+'},
            new[] {'W', 'Q', 'Y', 'Z', ' ', '.', ',', ';', 'à'},
            new[] {'a', 'ì', 'c', 'd', '@', 'A', 'm', 'C', '*'},
            new[] {'1', 'u', '3', 'T', '5', 'U', '7', 'E', 'b'},
            new[] {'2', 'w', 'v', 'x', 'z', 'M', 'i', 'O', '"'}
        };

#endif

        internal TextPlayfair() : base(TextAlgorithmType.TEXT)
        {
        }

        /// <summary>
        ///     Encoding algorithm for TextPlayfair.
        /// </summary>
        /// <param name="txt">String to encode</param>
        /// <returns>Encoded string</returns>
        public override string Code(string txt)
        {
            StringBuilder sb = new StringBuilder();
            string workTxt = txt;

            if (workTxt.Length%2 > 0) // if len is not pair
            {
                workTxt += " ";
            }

            int len = workTxt.Length;

            char clear0, clear1;
            char coded0, coded1;

            for (int j = 0; j < len - 1; j += 2)
            {
                clear0 = workTxt[j];
                clear1 = workTxt[j + 1];

                RowCol rc_clear0 = isIn(CIPHER1, clear0);
                RowCol rc_clear1 = isIn(CIPHER4, clear1);

                if ((rc_clear0 != null) && (rc_clear1 != null))
                {
                    coded0 = CIPHER3[rc_clear1.Row][rc_clear0.Col];
                    coded1 = CIPHER2[rc_clear0.Row][rc_clear1.Col];
                }
                else
                {
                    coded0 = clear0;
                    coded1 = clear1;
                }

                sb.Append(coded0);
                sb.Append(coded1);
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Decoding algorithm for TextPlayfair.
        /// </summary>
        /// <param name="txt">Encoded string</param>
        /// <returns>Decoded string</returns>
        /// <exception cref="ArgumentNullException" />
        public override string Decode(string txt)
        {
            if (txt == null)
            {
                throw new ArgumentNullException("txt");
            }

            StringBuilder sb = new StringBuilder();

            if (txt.Length%2 > 0) // if len is not pair
            {
                txt += " ";
            }

            int len = txt.Length;

            char clear0, clear1;
            char coded0, coded1;

            for (int j = 0; j < len - 1; j += 2)
            {
                coded0 = txt[j];
                coded1 = txt[j + 1];

                RowCol rc_coded0 = isIn(CIPHER3, coded0);
                RowCol rc_coded1 = isIn(CIPHER2, coded1);

                if ((rc_coded0 != null) && (rc_coded1 != null))
                {
                    clear0 = CIPHER1[rc_coded1.Row][rc_coded0.Col];
                    clear1 = CIPHER4[rc_coded0.Row][rc_coded1.Col];
                }
                else
                {
                    clear0 = coded0;
                    clear1 = coded1;
                }

                sb.Append(clear0);
                sb.Append(clear1);
            }

            return sb.ToString();
        }


        /// <summary>
        ///     Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return "Playfair cipher";
        }


        private static RowCol isIn(char[][] cipher, char toBeFound)
        {
            RowCol ret = null;

            for (short rr = 0; rr < cipher.Length; ++rr)
            {
                for (short cc = 0; cc < cipher[rr].Length; ++cc)
                {
                    if (cipher[rr][cc] == toBeFound)
                    {
                        ret = new RowCol(rr, cc);
                    }
                }
            }

            return ret;
        }
    }
}