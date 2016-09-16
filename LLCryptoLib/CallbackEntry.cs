namespace LLCryptoLib
{
    /// <summary>
    ///     Delegate to be used in long computing opertions
    ///     to get a counter on operation status.
    ///     p is the value returned.
    ///     <seealso cref="CallbackPoint" />
    /// </summary>
    /// <code>
    /// 	//0. UpdateCounter is a method with signature FeedbackExample(int p)
    /// 	CallbackEntry cbp = new CallbackEntry(FeedbackExample.UpdateCounter);
    /// 	</code>
    public delegate void CallbackEntry(int p);
}