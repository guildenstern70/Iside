using System.Collections.Generic;
using System.Threading;
using LLCryptoLib;
using LLCryptoLib.Utils;

namespace Iside
{
    internal class HashOperationParams
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="HashOperationParams" /> class.
        /// </summary>
        /// <param name="are">The are.</param>
        /// <param name="filepath">The filepath.</param>
        /// <param name="hstyle">The hstyle.</param>
        /// <param name="delCbe">The del cbe.</param>
        public HashOperationParams(AutoResetEvent are, string filepath, HexEnum hstyle, CallbackEntry delCbe)
        {
            this.ResetDemand = are;
            this.FilePath = filepath;
            this.Style = hstyle;
            this.Cbe = delCbe;
            this.Hashes = new List<HashBox>();
        }

        /// <summary>
        ///     Gets the Autoreset event
        /// </summary>
        /// <value>The reset demand.</value>
        public AutoResetEvent ResetDemand { get; }

        /// <summary>
        ///     Gets or sets the upper algorithm.
        /// </summary>
        /// <value>The upper algorithm.</value>
        public List<HashBox> Hashes { get; set; }

        /// <summary>
        ///     Gets the path.
        /// </summary>
        /// <value>The path.</value>
        public string FilePath { get; }

        /// <summary>
        ///     Gets the style.
        /// </summary>
        /// <value>The style.</value>
        public HexEnum Style { get; }

        /// <summary>
        ///     Gets the delegate callback.
        /// </summary>
        /// <value>The cbe.</value>
        public CallbackEntry Cbe { get; }
    }
}