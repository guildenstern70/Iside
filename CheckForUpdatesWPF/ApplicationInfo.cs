/**
    Check For Updates Utility - .NET WPF Version 
    Copyright (C) LittleLite Software
    Author Alessio Saltarin
**/

namespace CheckForUpdatesWPF
{
    public class ApplicationInfo
    {
        public string Name { get; set; }
        public string XmlTag { get; set; }
        public string RegistryKey { get; set; }
        public string Version { get; set; }
        public string Build { get; set; }
        public string Url { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationInfo"/> struct.
        /// </summary>
        /// <param name="aName">A name.</param>
        /// <param name="aTag">The name of the application in the XML file in LittleLite.net</param>
        /// <param name="aReg">A reg.</param>
        /// <param name="aVersion">A version.</param>
        /// <param name="aBuild">A build.</param>
        /// <param name="aUrl">A URL.</param>
        public ApplicationInfo(string aName, string aTag, string aReg, string aVersion, string aBuild, string aUrl)
        {
            this.Name = aName;
            this.XmlTag = aTag;
            this.RegistryKey = aReg;
            this.Version = aVersion;
            this.Build = aBuild;
            this.Url = aUrl;
        }
    }
}
