using System.IO;
using LLCryptoLib;
using LLCryptoLib.Hash;
using LLCryptoLib.Utils;

namespace Iside.Logic
{
    internal class MultiHashParameters
    {
        public MultiHashParameters(FileInfo[] files, Hash hash, HexEnum hs, CallbackEntry cbe)
        {
            this.Files = files;
            this.HexStyle = hs;
            this.Callback = cbe;
            this.Hash = hash;
        }

        public CallbackEntry Callback { get; }

        public FileInfo[] Files { get; }

        public Hash Hash { get; }

        public HexEnum HexStyle { get; }
    }
}