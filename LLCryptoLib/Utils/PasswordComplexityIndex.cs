using System;

namespace LLCryptoLib.Utils
{
    /// <summary>
    ///     PasswordComplexityIndex class can evaluate a string to return an index
    ///     between 0 (minimum) and 40 (maximum) that indicates
    ///     the password strength.
    /// </summary>
    public class PasswordComplexityIndex
    {
        private const int ALPHASET = 26;
        private const int NUMBERSET = 10;
        private const int SPECIALSET = 32;
        private const ulong HIDESKTOPTRIALRATE = 17179869184; // 2*2^33

        private string _cryptoPassword;
        private int _space;
        private int _totalChars;

        /// <summary>
        ///     The password complexity index.
        /// </summary>
        /// <see href="http://www.mandylionlabs.com/index15.htm" />
        public PasswordComplexityIndex()
        {
            this._cryptoPassword = string.Empty;
        }

        /// <summary>
        ///     The complexity of this password as total requested workload in floating points
        /// </summary>
        public ulong Complexity { get; private set; }

        /// <summary>
        ///     The estimated hours to crack the password on a computer capable of 2*2^33 trials per hour
        /// </summary>
        public float HoursToCrack
        {
            get
            {
                float hoursToCrack = 0.0F;
                hoursToCrack = this.Complexity/HIDESKTOPTRIALRATE;
                return hoursToCrack;
            }
        }

        /// <summary>
        ///     It is ln(Complexity), with a value between 0 (minimum) and 40 (maximum)
        /// </summary>
        public uint ComplexityProxy
        {
            get
            {
                uint percentage = (uint) Math.Log(this.Complexity);
                return Math.Min(40, percentage);
            }
        }

        /// <summary>
        ///     Evaluate the complexity index of the given password/passphrase
        /// </summary>
        /// <param name="passphrase">a given password string</param>
        public void Evaluate(string passphrase)
        {
            this._cryptoPassword = passphrase;
            this.ComputeComplexity();
        }

        private void ComputeComplexity()
        {
            ulong complexity;
            this.ScanCharacters();


            complexity = Lambda(this._space, this._totalChars);


            this.Complexity = complexity/2;
        }

        private static ulong Lambda(int setNumber, int number)
        {
            double nPow = Math.Pow(setNumber, number);
            if (nPow > long.MaxValue)
            {
                nPow = long.MaxValue;
            }
            ulong lambda = number > 0 ? (ulong) nPow : 1;
            return lambda;
        }

        private void ScanSpace(int lw, int up, int nb, int sp)
        {
            int setSpace = 0;

            if (lw > 0)
            {
                setSpace += ALPHASET;
            }

            if (up > 0)
            {
                setSpace += ALPHASET;
            }

            if (nb > 0)
            {
                setSpace += NUMBERSET;
            }

            if (sp > 0)
            {
                setSpace += SPECIALSET;
            }

            this._space = setSpace;
            this._totalChars = lw + up + nb + sp;
        }

        private void ScanCharacters()
        {
            int lowerAlfaChars = 0;
            int upperAlfaChars = 0;
            int numberChars = 0;
            int specialChars = 0;

            foreach (char character in this._cryptoPassword)
            {
                if (char.IsDigit(character))
                {
                    ++numberChars;
                }
                else if (char.IsLower(character))
                {
                    ++lowerAlfaChars;
                }
                else if (char.IsUpper(character))
                {
                    ++upperAlfaChars;
                }
                else
                {
                    ++specialChars;
                }
            }

            this.ScanSpace(lowerAlfaChars, upperAlfaChars, numberChars, specialChars);
        }
    }
}