using System.Reflection;
using System.Resources;

namespace LLCryptoLib.Security.Resources
{
    internal static class ResourceController
    {
        private static readonly ResourceManager m_Manager = new ResourceManager("Resources.SecurityServicesMessages",
            Assembly.GetExecutingAssembly());

        public static string GetString(string key)
        {
            return m_Manager.GetString(key);
        }
    }

    /// <summary>
    ///     LLCryptoLib.Security.Resources is the place for digital
    ///     certificates related resources.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
         "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class NamespaceDoc
    {
    }
}