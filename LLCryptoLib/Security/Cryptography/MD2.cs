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
 * Copyright (C) 2003-2014 LittleLite Software
 * 
 */

using System;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using LLCryptoLib.Security.Resources;
using LLCryptoLib.Security.Win32;

namespace LLCryptoLib.Security.Cryptography {
	/// <summary>
	/// Represents the abstract class from which all implementations of the MD2 hash algorithm inherit.
	/// </summary>
	public abstract class MD2 : HashAlgorithm {
		/// <summary>
		/// Initializes a new instance of <see cref="MD2"/>.
		/// </summary>
		/// <remarks>You cannot create an instance of an abstract class. Application code will create a new instance of a derived class.</remarks>
        protected MD2() {
			this.HashSizeValue = 128;
		}
		/// <summary>
		/// Creates an instance of the default implementation of the <see cref="MD2"/> hash algorithm.
		/// </summary>
		/// <returns>A new instance of the MD2 hash algorithm.</returns>
		public static new MD2 Create () {
			return Create("MD2");
		}
		/// <summary>
		/// Creates an instance of the specified implementation of the <see cref="MD2"/> hash algorithm.
		/// </summary>
		/// <param name="hashName">The name of the specific implementation of MD2 to use.</param>
		/// <returns>A new instance of the specified implementation of MD2.</returns>
        /// <exception cref="CryptographicException">An error occurs while initializing the hash.</exception>
        /// <exception cref="ArgumentException"></exception>
        public static new MD2 Create(string hashName) {
            if (string.Equals(hashName, "MD2", StringComparison.InvariantCultureIgnoreCase) || string.Equals(hashName, "LLCryptoLib.Security.cryptography.md2cryptoserviceprovider", StringComparison.InvariantCultureIgnoreCase))
                return new MD2CryptoServiceProvider();
            throw new ArgumentException(ResourceController.GetString("Error_ParamInvalid"));
        }
	}
}