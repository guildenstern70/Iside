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
using System.Collections.Generic;
using System.Text;
using LLCryptoLib.Security.Certificates;

namespace LLCryptoLib.Security.Certificates
{
    /// <summary>
    /// Wrapper around <see cref="Certificate"/> class.
    /// Holds information about certificate path and format
    /// </summary>
    public class LLCertificate : Certificate
    {
        private CertificateFormat format;
        private string certPath;

        /// <summary>
        /// Creates a new certificate from a PEM/Base64 string.
        /// </summary>
        /// <param name="pemContents">The PEM contents.</param>
        /// <returns>A newly created LLCertificate object</returns>
        public static LLCertificate CreateFromPemString(string pemContents)
        {
#if MONO
			throw new LLCryptoLibException("This function is not available on Mono.");
#else
            byte[] certBuffer = Convert.FromBase64String(pemContents);
            Certificate certX = Certificate.CreateFromCerFile(certBuffer);
            return new LLCertificate(certX);
#endif
        }

        /// <summary>
        /// Creates the certificate from a DER/PEM file
        /// </summary>
        /// <param name="path">The path of DER/PER file.</param>
        /// <param name="format">The certificate format as in <see cref="CertificateFormat"/></param>
        /// <returns>A newly created LLCertificate object</returns> 
        public static LLCertificate CreateCertificate(string path, CertificateFormat format)
        {

            LLCertificate lcert = null;

            switch (format)
            {
                case CertificateFormat.BASE64:
                case CertificateFormat.DER:
                default:
                    lcert = (LLCertificate)Certificate.CreateFromCerFile(path);
                    break;
                case CertificateFormat.PEM:
                    lcert = (LLCertificate)Certificate.CreateFromPemFile(path);
                    break;
            }

            lcert.Path = path;
            lcert.FileFormat = format;

            return lcert;
        }

        /// <summary>
        /// Creates the certificate from a PFX file
        /// </summary>
        /// <param name="path">The certificate PFX path.</param>
        /// <param name="password">The certificate password (usually for a PFX file).</param>
        /// <returns>A newly created LLCertificate object</returns>
        public static LLCertificate CreateCertificate(string path, string password)
        {

            Certificate cert = Certificate.CreateFromPfxFile(path, password);
            LLCertificate lcert = new LLCertificate(cert); 

            lcert.Path = path;
            lcert.FileFormat = CertificateFormat.UNKNOWN;

            return lcert;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LLCertificate"/> class.
        /// </summary>
        /// <param name="cert">The cert.</param>
        public LLCertificate(Certificate cert) : base(cert) 
        {
            this.format = CertificateFormat.UNKNOWN;
        }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path
        {
            get
            {
                return this.certPath;
            }

            set
            {
                this.certPath = value;
            }
        }

        /// <summary>
        /// Gets or sets the file format.
        /// </summary>
        /// <value>The file format.</value>
        public CertificateFormat FileFormat
        {
            get
            {
                return this.format;
            }
            set
            {
                this.format = value;
            }
        }

        /// <summary>
        /// Gets the effective date.
        /// </summary>
        /// <value>The effective date.</value>
        public DateTime EffectiveDate
        {
            get
            {
                return this.GetEffectiveDate();
            }
        }

        /// <summary>
        /// Gets to date.
        /// </summary>
        /// <value>To date.</value>
        public DateTime ToDate
        {
            get
            {
                return this.GetExpirationDate();
            }
        }

        /// <summary>
        /// Gets the format.
        /// </summary>
        /// <value>The format.</value>
        public string Format
        {
            get
            {
                return this.GetFormat();
            }
        }

        /// <summary>
        /// Gets the hash.
        /// </summary>
        /// <value>The hash.</value>
        public string Hash
        {
            get
            {
                return this.GetCertHashString();
            }
        }

        /// <summary>
        /// Gets the issuer.
        /// </summary>
        /// <value>The issuer.</value>
        public string Issuer
        {
            get
            {
                return this.GetIssuerName();
            }
        }

        /// <summary>
        /// Gets the key parameters.
        /// </summary>
        /// <value>The key parameters.</value>
        public string KeyParameters
        {
            get
            {
                return this.GetKeyAlgorithmParametersString();
            }
        }

        /// <summary>
        /// Gets the public key as a hexadecimal string.
        /// </summary>
        /// <value>The public key.</value>
        public string PublicKeyStr
        {
            get
            {
                return this.GetPublicKeyString();
            }
        }

        /// <summary>
        /// Gets the private key as an XML string.
        /// </summary>
        /// <value>If this certificate contains a private key, return the private key as XML. Else return null.</value>
        public string PrivateKeyXml
        {
            get
            {
                string xmlStr = null;
                if (this.HasPrivateKey())
                {
                    xmlStr = this.PrivateKey.ToXmlString(true);
                }
                return xmlStr;
            }
        }

        /// <summary>
        /// Gets the public key as an XML string.
        /// </summary>
        /// <value>The public key.</value>
        public string PublicKeyXml
        {
            get
            {
                return this.PublicKey.ToXmlString(false);
            }
        }

        /// <summary>
        /// Gets the chain.
        /// </summary>
        /// <returns>The certificate chain of this certificate</returns>
        public Certificate[] GetChain()
        {
            return this.GetCertificateChain().GetCertificates();
        }
    }
}