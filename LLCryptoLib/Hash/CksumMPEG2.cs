// $Id: CksumMPEG2.cs,v 1.1 2006/09/13 09:21:14 asaltar Exp $

#region License
/* ***** BEGIN LICENSE BLOCK *****
 * Version: MPL 1.1
 *
 * The contents of this file are subject to the Mozilla Public License Version
 * 1.1 (the "License"); you may not use this file except in compliance with
 * the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 *
 * Software distributed under the License is distributed on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
 * for the specific language governing rights and limitations under the
 * License.
 *
 * The Original Code is Classless.Hasher - C#/.NET Hash and Checksum Algorithm Library.
 *
 * The Initial Developer of the Original Code is Classless.net.
 * Portions created by the Initial Developer are Copyright (C) 2006 the Initial
 * Developer. All Rights Reserved.
 *
 * Contributor(s):
 *		Jason Simeone (jay@classless.net)
 * 
 * ***** END LICENSE BLOCK ***** */
#endregion

using System;
using LLCryptoLib.Hash;

namespace LLCryptoLib.Hash {
	/// <summary>Computes the MPEG2 varient of the Cksum hash for the input data using the managed library.</summary>
	public class CksumMPEG2 : Cksum {
		/// <summary>Initializes the algorithm.</summary>
		override public void Initialize() {
			lock (this) {
				base.Initialize();
				checksum = 0xFFFFFFFF;
			}
		}


		/// <summary>Performs any final activities required by the hash algorithm.</summary>
		/// <returns>The final hash value.</returns>
		override protected byte[] HashFinal() {
			lock (this) {
				return Utilities.UIntToByte((checksum & 0xFFFFFFFF), EndianType.BigEndian);
			}
		}
	}
}
