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

using System;
using LLCryptoLib.Crypto;

namespace LLCryptoLib.Hash
{
    /// <summary>
    /// Configuration class for Skein hash
    /// </summary>
    public class SkeinConfig
    {
        private readonly int _stateSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkeinConfig"/> class.
        /// </summary>
        /// <param name="sourceHash">The source hash.</param>
        public SkeinConfig(Skein sourceHash)
        {
            _stateSize = sourceHash.StateSize;

            // Allocate config value
            ConfigValue = new ulong[sourceHash.StateSize / 8];

            // Set the state size for the configuration
            ConfigString = new ulong[ConfigValue.Length];
            ConfigString[1] = (ulong) sourceHash.HashSize;
        }

        /// <summary>
        /// Generates the configuration.
        /// </summary>
        public void GenerateConfiguration()
        {
            var cipher = ThreefishCipher.CreateCipher(_stateSize);
            var tweak = new UbiTweak();

            // Initialize the tweak value
            tweak.StartNewBlockType(UbiType.Config);
            tweak.IsFinalBlock = true;
            tweak.BitsProcessed = 32;

            cipher.SetTweak(tweak.Tweak);
            cipher.Encrypt(ConfigString, ConfigValue);

            ConfigValue[0] ^= ConfigString[0]; 
            ConfigValue[1] ^= ConfigString[1];
            ConfigValue[2] ^= ConfigString[2];
        }

        /// <summary>
        /// Generates the configuration.
        /// </summary>
        /// <param name="initialState">The initial state.</param>
        public void GenerateConfiguration(ulong[] initialState)
        {
            var cipher = ThreefishCipher.CreateCipher(_stateSize);
            var tweak = new UbiTweak();

            // Initialize the tweak value
            tweak.StartNewBlockType(UbiType.Config);
            tweak.IsFinalBlock = true;
            tweak.BitsProcessed = 32;

            cipher.SetKey(initialState);
            cipher.SetTweak(tweak.Tweak);
            cipher.Encrypt(ConfigString, ConfigValue);

            ConfigValue[0] ^= ConfigString[0];
            ConfigValue[1] ^= ConfigString[1];
            ConfigValue[2] ^= ConfigString[2];
        }

        /// <summary>
        /// Sets the schema.
        /// </summary>
        /// <param name="schema">The schema.</param>
        public void SetSchema(params byte[] schema)
        {
            if (schema.Length != 4) throw new Exception("Schema must be 4 bytes.");

            ulong n = ConfigString[0];

            // Clear the schema bytes
            n &= ~(ulong)0xfffffffful;
            // Set schema bytes
            n |= (ulong) schema[3] << 24;
            n |= (ulong) schema[2] << 16;
            n |= (ulong) schema[1] << 8;
            n |= (ulong) schema[0];

            ConfigString[0] = n;
        }

        /// <summary>
        /// Sets the version.
        /// </summary>
        /// <param name="version">The version.</param>
        public void SetVersion(int version)
        {
            if (version < 0 || version > 3)
                throw new Exception("Version must be between 0 and 3, inclusive.");

            ConfigString[0] &= ~((ulong)0x03 << 32);
            ConfigString[0] |= (ulong)version << 32;
        }

        /// <summary>
        /// Sets the size of the tree leaf.
        /// </summary>
        /// <param name="size">The size.</param>
        public void SetTreeLeafSize(byte size)
        {
            ConfigString[2] &= ~(ulong)0xff;
            ConfigString[2] |= (ulong)size;
        }

        /// <summary>
        /// Sets the size of the tree fan out.
        /// </summary>
        /// <param name="size">The size.</param>
        public void SetTreeFanOutSize(byte size)
        {
            ConfigString[2] &= ~((ulong)0xff << 8);
            ConfigString[2] |= (ulong)size << 8;
        }

        /// <summary>
        /// Sets the height of the max tree.
        /// </summary>
        /// <param name="height">The height.</param>
        public void SetMaxTreeHeight(byte height)
        {
            if (height == 1)
                throw new Exception("Tree height must be zero or greater than 1.");

            ConfigString[2] &= ~((ulong)0xff << 16);
            ConfigString[2] |= (ulong)height << 16;
        }

        /// <summary>
        /// Gets or sets the config value.
        /// </summary>
        /// <value>The config value.</value>
        public ulong[] ConfigValue { get; private set; }

        /// <summary>
        /// Gets or sets the config string.
        /// </summary>
        /// <value>The config string.</value>
        public ulong[] ConfigString { get; private set; }
    }
}
