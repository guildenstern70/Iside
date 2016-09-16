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
 * 
 * Copyright (C) 2003-2014 LittleLite Software
 * 
 */

namespace LLCryptoLib.Crypto
{
    internal class Threefish256 : ThreefishCipher
    {
        const int CipherSize = 256;
        const int CipherQwords = CipherSize / 64;
        const int ExpandedKeySize = CipherQwords + 1;

        public Threefish256()
        {
            // Create the expanded key array
            ExpandedKey = new ulong[ExpandedKeySize];
            ExpandedKey[ExpandedKeySize - 1] = KeyScheduleConst;
        }

        public override void Encrypt(ulong[] input, ulong[] output)
        {
            // Cache the block, key, and tweak
            ulong b0 = input[0], b1 = input[1],
                  b2 = input[2], b3 = input[3];
            ulong k0 = ExpandedKey[0], k1 = ExpandedKey[1],
                  k2 = ExpandedKey[2], k3 = ExpandedKey[3],
                  k4 = ExpandedKey[4];
            ulong t0 = ExpandedTweak[0], t1 = ExpandedTweak[1],
                  t2 = ExpandedTweak[2];

            Mix(ref b0, ref b1, 14, k0, k1 + t0);
            Mix(ref b2, ref b3, 16, k2 + t1, k3);
            Mix(ref b0, ref b3, 52);
            Mix(ref b2, ref b1, 57);
            Mix(ref b0, ref b1, 23);
            Mix(ref b2, ref b3, 40);
            Mix(ref b0, ref b3, 5);
            Mix(ref b2, ref b1, 37);
            Mix(ref b0, ref b1, 25, k1, k2 + t1);
            Mix(ref b2, ref b3, 33, k3 + t2, k4 + 1);
            Mix(ref b0, ref b3, 46);
            Mix(ref b2, ref b1, 12);
            Mix(ref b0, ref b1, 58);
            Mix(ref b2, ref b3, 22);
            Mix(ref b0, ref b3, 32);
            Mix(ref b2, ref b1, 32);
            Mix(ref b0, ref b1, 14, k2, k3 + t2);
            Mix(ref b2, ref b3, 16, k4 + t0, k0 + 2);
            Mix(ref b0, ref b3, 52);
            Mix(ref b2, ref b1, 57);
            Mix(ref b0, ref b1, 23);
            Mix(ref b2, ref b3, 40);
            Mix(ref b0, ref b3, 5);
            Mix(ref b2, ref b1, 37);
            Mix(ref b0, ref b1, 25, k3, k4 + t0);
            Mix(ref b2, ref b3, 33, k0 + t1, k1 + 3);
            Mix(ref b0, ref b3, 46);
            Mix(ref b2, ref b1, 12);
            Mix(ref b0, ref b1, 58);
            Mix(ref b2, ref b3, 22);
            Mix(ref b0, ref b3, 32);
            Mix(ref b2, ref b1, 32);
            Mix(ref b0, ref b1, 14, k4, k0 + t1);
            Mix(ref b2, ref b3, 16, k1 + t2, k2 + 4);
            Mix(ref b0, ref b3, 52);
            Mix(ref b2, ref b1, 57);
            Mix(ref b0, ref b1, 23);
            Mix(ref b2, ref b3, 40);
            Mix(ref b0, ref b3, 5);
            Mix(ref b2, ref b1, 37);
            Mix(ref b0, ref b1, 25, k0, k1 + t2);
            Mix(ref b2, ref b3, 33, k2 + t0, k3 + 5);
            Mix(ref b0, ref b3, 46);
            Mix(ref b2, ref b1, 12);
            Mix(ref b0, ref b1, 58);
            Mix(ref b2, ref b3, 22);
            Mix(ref b0, ref b3, 32);
            Mix(ref b2, ref b1, 32);
            Mix(ref b0, ref b1, 14, k1, k2 + t0);
            Mix(ref b2, ref b3, 16, k3 + t1, k4 + 6);
            Mix(ref b0, ref b3, 52);
            Mix(ref b2, ref b1, 57);
            Mix(ref b0, ref b1, 23);
            Mix(ref b2, ref b3, 40);
            Mix(ref b0, ref b3, 5);
            Mix(ref b2, ref b1, 37);
            Mix(ref b0, ref b1, 25, k2, k3 + t1);
            Mix(ref b2, ref b3, 33, k4 + t2, k0 + 7);
            Mix(ref b0, ref b3, 46);
            Mix(ref b2, ref b1, 12);
            Mix(ref b0, ref b1, 58);
            Mix(ref b2, ref b3, 22);
            Mix(ref b0, ref b3, 32);
            Mix(ref b2, ref b1, 32);
            Mix(ref b0, ref b1, 14, k3, k4 + t2);
            Mix(ref b2, ref b3, 16, k0 + t0, k1 + 8);
            Mix(ref b0, ref b3, 52);
            Mix(ref b2, ref b1, 57);
            Mix(ref b0, ref b1, 23);
            Mix(ref b2, ref b3, 40);
            Mix(ref b0, ref b3, 5);
            Mix(ref b2, ref b1, 37);
            Mix(ref b0, ref b1, 25, k4, k0 + t0);
            Mix(ref b2, ref b3, 33, k1 + t1, k2 + 9);
            Mix(ref b0, ref b3, 46);
            Mix(ref b2, ref b1, 12);
            Mix(ref b0, ref b1, 58);
            Mix(ref b2, ref b3, 22);
            Mix(ref b0, ref b3, 32);
            Mix(ref b2, ref b1, 32);
            Mix(ref b0, ref b1, 14, k0, k1 + t1);
            Mix(ref b2, ref b3, 16, k2 + t2, k3 + 10);
            Mix(ref b0, ref b3, 52);
            Mix(ref b2, ref b1, 57);
            Mix(ref b0, ref b1, 23);
            Mix(ref b2, ref b3, 40);
            Mix(ref b0, ref b3, 5);
            Mix(ref b2, ref b1, 37);
            Mix(ref b0, ref b1, 25, k1, k2 + t2);
            Mix(ref b2, ref b3, 33, k3 + t0, k4 + 11);
            Mix(ref b0, ref b3, 46);
            Mix(ref b2, ref b1, 12);
            Mix(ref b0, ref b1, 58);
            Mix(ref b2, ref b3, 22);
            Mix(ref b0, ref b3, 32);
            Mix(ref b2, ref b1, 32);
            Mix(ref b0, ref b1, 14, k2, k3 + t0);
            Mix(ref b2, ref b3, 16, k4 + t1, k0 + 12);
            Mix(ref b0, ref b3, 52);
            Mix(ref b2, ref b1, 57);
            Mix(ref b0, ref b1, 23);
            Mix(ref b2, ref b3, 40);
            Mix(ref b0, ref b3, 5);
            Mix(ref b2, ref b1, 37);
            Mix(ref b0, ref b1, 25, k3, k4 + t1);
            Mix(ref b2, ref b3, 33, k0 + t2, k1 + 13);
            Mix(ref b0, ref b3, 46);
            Mix(ref b2, ref b1, 12);
            Mix(ref b0, ref b1, 58);
            Mix(ref b2, ref b3, 22);
            Mix(ref b0, ref b3, 32);
            Mix(ref b2, ref b1, 32);
            Mix(ref b0, ref b1, 14, k4, k0 + t2);
            Mix(ref b2, ref b3, 16, k1 + t0, k2 + 14);
            Mix(ref b0, ref b3, 52);
            Mix(ref b2, ref b1, 57);
            Mix(ref b0, ref b1, 23);
            Mix(ref b2, ref b3, 40);
            Mix(ref b0, ref b3, 5);
            Mix(ref b2, ref b1, 37);
            Mix(ref b0, ref b1, 25, k0, k1 + t0);
            Mix(ref b2, ref b3, 33, k2 + t1, k3 + 15);
            Mix(ref b0, ref b3, 46);
            Mix(ref b2, ref b1, 12);
            Mix(ref b0, ref b1, 58);
            Mix(ref b2, ref b3, 22);
            Mix(ref b0, ref b3, 32);
            Mix(ref b2, ref b1, 32);
            Mix(ref b0, ref b1, 14, k1, k2 + t1);
            Mix(ref b2, ref b3, 16, k3 + t2, k4 + 16);
            Mix(ref b0, ref b3, 52);
            Mix(ref b2, ref b1, 57);
            Mix(ref b0, ref b1, 23);
            Mix(ref b2, ref b3, 40);
            Mix(ref b0, ref b3, 5);
            Mix(ref b2, ref b1, 37);
            Mix(ref b0, ref b1, 25, k2, k3 + t2);
            Mix(ref b2, ref b3, 33, k4 + t0, k0 + 17);
            Mix(ref b0, ref b3, 46);
            Mix(ref b2, ref b1, 12);
            Mix(ref b0, ref b1, 58);
            Mix(ref b2, ref b3, 22);
            Mix(ref b0, ref b3, 32);
            Mix(ref b2, ref b1, 32);

            output[0] = b0 + k3;
            output[1] = b1 + k4 + t0;
            output[2] = b2 + k0 + t1;
            output[3] = b3 + k1 + 18;
        }

        public override void Decrypt(ulong[] input, ulong[] output)
        {
            // Cache the block, key, and tweak
            ulong b0 = input[0], b1 = input[1],
                  b2 = input[2], b3 = input[3];
            ulong k0 = ExpandedKey[0], k1 = ExpandedKey[1],
                  k2 = ExpandedKey[2], k3 = ExpandedKey[3],
                  k4 = ExpandedKey[4];
            ulong t0 = ExpandedTweak[0], t1 = ExpandedTweak[1],
                  t2 = ExpandedTweak[2];

            b0 -= k3;
            b1 -= k4 + t0;
            b2 -= k0 + t1;
            b3 -= k1 + 18;
            UnMix(ref b0, ref b3, 32);
            UnMix(ref b2, ref b1, 32);
            UnMix(ref b0, ref b1, 58);
            UnMix(ref b2, ref b3, 22);
            UnMix(ref b0, ref b3, 46);
            UnMix(ref b2, ref b1, 12);
            UnMix(ref b0, ref b1, 25, k2, k3 + t2);
            UnMix(ref b2, ref b3, 33, k4 + t0, k0 + 17);
            UnMix(ref b0, ref b3, 5);
            UnMix(ref b2, ref b1, 37);
            UnMix(ref b0, ref b1, 23);
            UnMix(ref b2, ref b3, 40);
            UnMix(ref b0, ref b3, 52);
            UnMix(ref b2, ref b1, 57);
            UnMix(ref b0, ref b1, 14, k1, k2 + t1);
            UnMix(ref b2, ref b3, 16, k3 + t2, k4 + 16);
            UnMix(ref b0, ref b3, 32);
            UnMix(ref b2, ref b1, 32);
            UnMix(ref b0, ref b1, 58);
            UnMix(ref b2, ref b3, 22);
            UnMix(ref b0, ref b3, 46);
            UnMix(ref b2, ref b1, 12);
            UnMix(ref b0, ref b1, 25, k0, k1 + t0);
            UnMix(ref b2, ref b3, 33, k2 + t1, k3 + 15);
            UnMix(ref b0, ref b3, 5);
            UnMix(ref b2, ref b1, 37);
            UnMix(ref b0, ref b1, 23);
            UnMix(ref b2, ref b3, 40);
            UnMix(ref b0, ref b3, 52);
            UnMix(ref b2, ref b1, 57);
            UnMix(ref b0, ref b1, 14, k4, k0 + t2);
            UnMix(ref b2, ref b3, 16, k1 + t0, k2 + 14);
            UnMix(ref b0, ref b3, 32);
            UnMix(ref b2, ref b1, 32);
            UnMix(ref b0, ref b1, 58);
            UnMix(ref b2, ref b3, 22);
            UnMix(ref b0, ref b3, 46);
            UnMix(ref b2, ref b1, 12);
            UnMix(ref b0, ref b1, 25, k3, k4 + t1);
            UnMix(ref b2, ref b3, 33, k0 + t2, k1 + 13);
            UnMix(ref b0, ref b3, 5);
            UnMix(ref b2, ref b1, 37);
            UnMix(ref b0, ref b1, 23);
            UnMix(ref b2, ref b3, 40);
            UnMix(ref b0, ref b3, 52);
            UnMix(ref b2, ref b1, 57);
            UnMix(ref b0, ref b1, 14, k2, k3 + t0);
            UnMix(ref b2, ref b3, 16, k4 + t1, k0 + 12);
            UnMix(ref b0, ref b3, 32);
            UnMix(ref b2, ref b1, 32);
            UnMix(ref b0, ref b1, 58);
            UnMix(ref b2, ref b3, 22);
            UnMix(ref b0, ref b3, 46);
            UnMix(ref b2, ref b1, 12);
            UnMix(ref b0, ref b1, 25, k1, k2 + t2);
            UnMix(ref b2, ref b3, 33, k3 + t0, k4 + 11);
            UnMix(ref b0, ref b3, 5);
            UnMix(ref b2, ref b1, 37);
            UnMix(ref b0, ref b1, 23);
            UnMix(ref b2, ref b3, 40);
            UnMix(ref b0, ref b3, 52);
            UnMix(ref b2, ref b1, 57);
            UnMix(ref b0, ref b1, 14, k0, k1 + t1);
            UnMix(ref b2, ref b3, 16, k2 + t2, k3 + 10);
            UnMix(ref b0, ref b3, 32);
            UnMix(ref b2, ref b1, 32);
            UnMix(ref b0, ref b1, 58);
            UnMix(ref b2, ref b3, 22);
            UnMix(ref b0, ref b3, 46);
            UnMix(ref b2, ref b1, 12);
            UnMix(ref b0, ref b1, 25, k4, k0 + t0);
            UnMix(ref b2, ref b3, 33, k1 + t1, k2 + 9);
            UnMix(ref b0, ref b3, 5);
            UnMix(ref b2, ref b1, 37);
            UnMix(ref b0, ref b1, 23);
            UnMix(ref b2, ref b3, 40);
            UnMix(ref b0, ref b3, 52);
            UnMix(ref b2, ref b1, 57);
            UnMix(ref b0, ref b1, 14, k3, k4 + t2);
            UnMix(ref b2, ref b3, 16, k0 + t0, k1 + 8);
            UnMix(ref b0, ref b3, 32);
            UnMix(ref b2, ref b1, 32);
            UnMix(ref b0, ref b1, 58);
            UnMix(ref b2, ref b3, 22);
            UnMix(ref b0, ref b3, 46);
            UnMix(ref b2, ref b1, 12);
            UnMix(ref b0, ref b1, 25, k2, k3 + t1);
            UnMix(ref b2, ref b3, 33, k4 + t2, k0 + 7);
            UnMix(ref b0, ref b3, 5);
            UnMix(ref b2, ref b1, 37);
            UnMix(ref b0, ref b1, 23);
            UnMix(ref b2, ref b3, 40);
            UnMix(ref b0, ref b3, 52);
            UnMix(ref b2, ref b1, 57);
            UnMix(ref b0, ref b1, 14, k1, k2 + t0);
            UnMix(ref b2, ref b3, 16, k3 + t1, k4 + 6);
            UnMix(ref b0, ref b3, 32);
            UnMix(ref b2, ref b1, 32);
            UnMix(ref b0, ref b1, 58);
            UnMix(ref b2, ref b3, 22);
            UnMix(ref b0, ref b3, 46);
            UnMix(ref b2, ref b1, 12);
            UnMix(ref b0, ref b1, 25, k0, k1 + t2);
            UnMix(ref b2, ref b3, 33, k2 + t0, k3 + 5);
            UnMix(ref b0, ref b3, 5);
            UnMix(ref b2, ref b1, 37);
            UnMix(ref b0, ref b1, 23);
            UnMix(ref b2, ref b3, 40);
            UnMix(ref b0, ref b3, 52);
            UnMix(ref b2, ref b1, 57);
            UnMix(ref b0, ref b1, 14, k4, k0 + t1);
            UnMix(ref b2, ref b3, 16, k1 + t2, k2 + 4);
            UnMix(ref b0, ref b3, 32);
            UnMix(ref b2, ref b1, 32);
            UnMix(ref b0, ref b1, 58);
            UnMix(ref b2, ref b3, 22);
            UnMix(ref b0, ref b3, 46);
            UnMix(ref b2, ref b1, 12);
            UnMix(ref b0, ref b1, 25, k3, k4 + t0);
            UnMix(ref b2, ref b3, 33, k0 + t1, k1 + 3);
            UnMix(ref b0, ref b3, 5);
            UnMix(ref b2, ref b1, 37);
            UnMix(ref b0, ref b1, 23);
            UnMix(ref b2, ref b3, 40);
            UnMix(ref b0, ref b3, 52);
            UnMix(ref b2, ref b1, 57);
            UnMix(ref b0, ref b1, 14, k2, k3 + t2);
            UnMix(ref b2, ref b3, 16, k4 + t0, k0 + 2);
            UnMix(ref b0, ref b3, 32);
            UnMix(ref b2, ref b1, 32);
            UnMix(ref b0, ref b1, 58);
            UnMix(ref b2, ref b3, 22);
            UnMix(ref b0, ref b3, 46);
            UnMix(ref b2, ref b1, 12);
            UnMix(ref b0, ref b1, 25, k1, k2 + t1);
            UnMix(ref b2, ref b3, 33, k3 + t2, k4 + 1);
            UnMix(ref b0, ref b3, 5);
            UnMix(ref b2, ref b1, 37);
            UnMix(ref b0, ref b1, 23);
            UnMix(ref b2, ref b3, 40);
            UnMix(ref b0, ref b3, 52);
            UnMix(ref b2, ref b1, 57);
            UnMix(ref b0, ref b1, 14, k0, k1 + t0);
            UnMix(ref b2, ref b3, 16, k2 + t1, k3);

            output[0] = b0;
            output[1] = b1;
            output[2] = b2;
            output[3] = b3;
        }
    }
}
