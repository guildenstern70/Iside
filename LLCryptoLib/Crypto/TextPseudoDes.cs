namespace LLCryptoLib.Crypto
{
    /// <summary>
    ///     PseudoDES text encoding class.
    ///     PseudoDES is a TextVigenere type of encoding, remade
    ///     for a number of times equal to TextAlgorithmParameters.Shift.
    ///     If TextAlgorithmParameters.Shift is zero, then a value of 7
    ///     is taken.
    /// </summary>
    public class TextPseudoDes : TextVigenere
    {
        private readonly int howManyTimes;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="p">Parametri must have a valid key</param>
        internal TextPseudoDes(TextAlgorithmParameters p) : base(p)
        {
            if (p.Shift == 0)
            {
                this.howManyTimes = 7;
            }
            else
            {
                this.howManyTimes = p.Shift;
            }
        }

        /// <summary>
        ///     Encoding algorithm
        /// </summary>
        /// <param name="txt">Plain text</param>
        /// <returns>Encoded text</returns>
        public override string Code(string txt)
        {
            string sequenceTmp = txt;

            for (short j = 0; j < (short) this.howManyTimes; j++)
            {
                sequenceTmp = this.Algo(sequenceTmp, true);
            }

            return sequenceTmp;
        }

        /// <summary>
        ///     Decoding algorithm
        /// </summary>
        /// <param name="txt">Encoded text</param>
        /// <returns>Decoded text</returns>
        public override string Decode(string txt)
        {
            string sequenceTmp = txt;

            for (short j = 0; j < (short) this.howManyTimes; j++)
            {
                sequenceTmp = this.Algo(sequenceTmp, false);
            }

            return sequenceTmp;
        }


        /// <summary>
        ///     Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return "Polyalphabetic with key =" + this.key + " and shift =" + this.shift;
        }
    }
}