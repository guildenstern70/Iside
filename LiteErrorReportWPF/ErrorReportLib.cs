using System;
using System.Security.Principal;
using System.Text;
using System.Threading;
using LLCryptoLib.Hash;
using LLCryptoLib.Utils;

namespace LiteErrorReportWPF
{
    public static class ErrorReportLib
    {
        /// <summary>
        /// Errors the code number.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static string ErrorCodeNumber(string message)
        {
            Hash hashFunc = new Hash();
            hashFunc.SetAlgorithm(AvailableHash.FCS32);
            string errorCode = "0x";
            errorCode += hashFunc.ComputeHashStyle(message, HexEnum.CLASSIC, Encoding.ASCII);
            return errorCode;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is user admin.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is user admin; otherwise, <c>false</c>.
        /// </value>
        public static bool IsUserAdmin
        {
            get
            {
                bool isAdmin = false;
                AppDomain ad = Thread.GetDomain();
                ad.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
                WindowsPrincipal user = (WindowsPrincipal)Thread.CurrentPrincipal;
                if (user.IsInRole(WindowsBuiltInRole.Administrator))
                {
                    isAdmin = true;
                }
                return isAdmin;
            }
        }
    }
}
