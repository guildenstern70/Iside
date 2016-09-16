using System.IO;
using LLCryptoLib;
using LLCryptoLib.Hash;
using LLCryptoLib.Utils;

namespace Iside.Logic
{
    class MultiHashParameters
    {
        private FileInfo[] _files;
        private HexEnum _hs;
        private CallbackEntry _cbe;
        private Hash _hash;

        public MultiHashParameters(FileInfo[] files, Hash hash, HexEnum hs, CallbackEntry cbe)
        {
            this._files = files;
            this._hs = hs;
            this._cbe = cbe;
            this._hash = hash;
        }

        public CallbackEntry Callback
        {
            get
            {
                return _cbe;
            }
        }

        public FileInfo[] Files
        {
            get
            {
                return _files;
            }
        }

        public Hash Hash
        {
            get
            {
                return _hash;
            }
        }

        public HexEnum HexStyle
        {
            get
            {
                return _hs;
            }
        }

    }
}
