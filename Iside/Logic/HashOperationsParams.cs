using System.Collections.Generic;
using System.Threading;
using System.Windows.Controls;
using IsideLogic;
using LLCryptoLib;
using LLCryptoLib.Hash;
using LLCryptoLib.Utils;

namespace Iside
{
    class HashOperationParams
    {
        private AutoResetEvent _resetDemand;
        private string _filePath;
        private HexEnum _style;
        private CallbackEntry _cbe;
        private List<HashBox> _hashes;

        /// <summary>
        /// Initializes a new instance of the <see cref="HashOperationParams"/> class.
        /// </summary>
        /// <param name="are">The are.</param>
        /// <param name="filepath">The filepath.</param>
        /// <param name="hstyle">The hstyle.</param>
        /// <param name="delCbe">The del cbe.</param>
        public HashOperationParams(AutoResetEvent are, string filepath, HexEnum hstyle, CallbackEntry delCbe)
        {
            this._resetDemand = are;
            this._filePath = filepath;
            this._style = hstyle;
            this._cbe = delCbe;
            this._hashes = new List<HashBox>();
        }

        /// <summary>
        /// Gets the Autoreset event
        /// </summary>
        /// <value>The reset demand.</value>
        public AutoResetEvent ResetDemand
        {
            get { return _resetDemand; }
        }

        /// <summary>
        /// Gets or sets the upper algorithm.
        /// </summary>
        /// <value>The upper algorithm.</value>
        public List<HashBox> Hashes
        {
            get { return this._hashes; }
            set { this._hashes = value; }
        }

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <value>The path.</value>
        public string FilePath
        {
            get { return _filePath; }
        }

        /// <summary>
        /// Gets the style.
        /// </summary>
        /// <value>The style.</value>
        public HexEnum Style
        {
            get { return _style; }
        }

        /// <summary>
        /// Gets the delegate callback.
        /// </summary>
        /// <value>The cbe.</value>
        public CallbackEntry Cbe
        {
            get { return _cbe; }
        }

    }
}
