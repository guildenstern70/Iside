using IsideLogic;
using LLCryptoLib.Hash;

namespace Iside
{
    public class HashBox
    {
        public Quadrant HashQuadrant { get; set; }
        public string Message { get; set; }
        public string Hash { get; set; }
        public SupportedHashAlgo Algorithm { get; set; }
    }
}
