namespace LLCryptoLib.Crypto
{
    internal abstract class ThreefishCipher
    {
        protected const ulong KeyScheduleConst = 0x1BD11BDAA9FC1A22;
        protected const int ExpandedTweakSize = 3;

        protected ulong[] ExpandedKey;
        protected ulong[] ExpandedTweak;

        protected ThreefishCipher()
        {
            this.ExpandedTweak = new ulong[ExpandedTweakSize];
        }

        protected static ulong RotateLeft64(ulong v, int b)
        {
            return (v << b) | (v >> (64 - b));
        }

        protected static ulong RotateRight64(ulong v, int b)
        {
            return (v >> b) | (v << (64 - b));
        }

        protected static void Mix(ref ulong a, ref ulong b, int r)
        {
            a += b;
            b = RotateLeft64(b, r) ^ a;
        }

        protected static void Mix(ref ulong a, ref ulong b, int r, ulong k0, ulong k1)
        {
            b += k1;
            a += b + k0;
            b = RotateLeft64(b, r) ^ a;
        }

        protected static void UnMix(ref ulong a, ref ulong b, int r)
        {
            b = RotateRight64(b ^ a, r);
            a -= b;
        }

        protected static void UnMix(ref ulong a, ref ulong b, int r, ulong k0, ulong k1)
        {
            b = RotateRight64(b ^ a, r);
            a -= b + k0;
            b -= k1;
        }

        public void SetTweak(ulong[] tweak)
        {
            this.ExpandedTweak[0] = tweak[0];
            this.ExpandedTweak[1] = tweak[1];
            this.ExpandedTweak[2] = tweak[0] ^ tweak[1];
        }

        public void SetKey(ulong[] key)
        {
            int i;
            ulong parity = KeyScheduleConst;

            for (i = 0; i < this.ExpandedKey.Length - 1; i++)
            {
                this.ExpandedKey[i] = key[i];
                parity ^= key[i];
            }

            this.ExpandedKey[i] = parity;
        }

        public static ThreefishCipher CreateCipher(int stateSize)
        {
            switch (stateSize)
            {
                case 256:
                    return new Threefish256();
                case 512:
                    return new Threefish512();
                case 1024:
                    return new Threefish1024();
            }
            return null;
        }

        public abstract void Encrypt(ulong[] input, ulong[] output);
        public abstract void Decrypt(ulong[] input, ulong[] output);
    }
}