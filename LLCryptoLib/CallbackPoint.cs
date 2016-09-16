namespace LLCryptoLib
{
    /// <summary>
    ///     Delegate for displaying advancement. This is a delegate to
    ///     call a piece of code whenever a certain event happens on
    ///     the main computing loop.
    /// </summary>
    /// <example>
    ///     <code>
    /// 	//0. Update counter is a method with signature FeedbackExample(int i, string message)
    ///  CallbackPoint cbp = new CallbackPoint(FeedbackExample.UpdateCounter);
    ///  
    ///  //1. Encrypt with Rijndael
    ///  StreamARC4512 cryptAlgorithm = LLCryptoLib.Crypto.StreamAlgorithmFactory.ArcFour512;
    ///  LLCryptoLib.Crypto.StreamCrypter crypter = new StreamCrypter(cryptAlgorithm);
    ///  crypter.GenerateKeys("password");
    ///  string encryptedFile = rndFile.FullName + ".enc";
    ///  // here we use the CallbackPoint
    ///  crypter.EncryptDecrypt(rndFile.FullName, encryptedFile, true, cbp);
    ///  Console.WriteLine("File encrypted into " + encryptedFile);
    /// 	</code>
    /// </example>
    public delegate void CallbackPoint(int p, string message);
}