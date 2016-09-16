namespace AxsUtils
{
    /// <summary>
    ///     Template for Singleton classes
    /// </summary>
    internal class Singleton
    {
        private static Singleton s;

        private Singleton()
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static Singleton Reference
        {
            get
            {
                if (s == null)
                {
                    s = new Singleton();
                }

                return s;
            }
        }
    }
}