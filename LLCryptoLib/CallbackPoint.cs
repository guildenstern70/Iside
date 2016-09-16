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


namespace LLCryptoLib
{
	/// <summary>
	/// Delegate for displaying advancement. This is a delegate to 
	/// call a piece of code whenever a certain event happens on
	/// the main computing loop.
	/// </summary>
	/// <example>
	/// <code>
	///	//0. Update counter is a method with signature FeedbackExample(int i, string message)
	/// CallbackPoint cbp = new CallbackPoint(FeedbackExample.UpdateCounter);
	/// 
    /// //1. Encrypt with Rijndael
	/// StreamARC4512 cryptAlgorithm = LLCryptoLib.Crypto.StreamAlgorithmFactory.ArcFour512;
	/// LLCryptoLib.Crypto.StreamCrypter crypter = new StreamCrypter(cryptAlgorithm);
	/// crypter.GenerateKeys("password");
	/// string encryptedFile = rndFile.FullName + ".enc";
    /// // here we use the CallbackPoint
	/// crypter.EncryptDecrypt(rndFile.FullName, encryptedFile, true, cbp);
	/// Console.WriteLine("File encrypted into " + encryptedFile);
	///	</code>
	/// </example>
	public delegate void CallbackPoint(int p, string message);
}
