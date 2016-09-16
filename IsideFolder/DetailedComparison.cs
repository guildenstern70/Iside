using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsideFolder
{
    /// <summary>
    /// A DetailedComparison is a comparison plus a number
    /// of ancillary information.
    /// </summary>
    public class DetailedComparison : Comparison
    {

        public DetailedComparison(string left, string right) : base(left, right) {}
        public DetailedComparison(DirectoryInfo left, FileSystemInfo right) : base(left, right) {}  

        public bool IncludeHidden { get; set; }
        public bool IncludeSystem { get; set; }
        public bool IncludeArchive { get; set; }
        public int NrOfFolders { get; set; }
        public int NrOfFiles { get; set; }
    }
}
