using System.Windows.Input;

namespace Iside.Logic
{
    public class Commands
    {
        // FILE
        public static RoutedUICommand NewHash;
        public static RoutedUICommand OpenLeft;
        public static RoutedUICommand OpenRight;
        public static RoutedUICommand SaveAs;
        public static RoutedUICommand ExportReport;
        public static RoutedUICommand Options;
        public static RoutedUICommand Exit;

        // TOOLS
        public static RoutedUICommand HashNow;
        public static RoutedUICommand CompareNow;
        public static RoutedUICommand Folders;
        public static RoutedUICommand MultiFile;
        public static RoutedUICommand CdDvd;
        public static RoutedUICommand Md5Gen;
        public static RoutedUICommand Md5Verify;
        public static RoutedUICommand SfvGen;
        public static RoutedUICommand SfvVerify;

        // HELP
        public static RoutedUICommand Help;
        public static RoutedUICommand CheckForUpdates;
        public static RoutedUICommand IsideWeb;
        public static RoutedUICommand LittleLiteWeb;
        public static RoutedUICommand About;

        static Commands()
        {
            // UI Commands (Description, Name, TypeOfCommands)

            // FILE
            NewHash = new RoutedUICommand("Prepare for a new hash", "New", typeof(Commands));
            OpenLeft = new RoutedUICommand("Opens a file in the left pane", "Open", typeof(Commands));
            OpenRight = new RoutedUICommand("Opens a file in the right pane", "Open", typeof(Commands));
            SaveAs = new RoutedUICommand("Save the current comparison", "Save As...", typeof(Commands));
            ExportReport = new RoutedUICommand("Export the current comparison", "Export Report", typeof(Commands));
            Options = new RoutedUICommand("Set preferences", "Options", typeof(Commands));
            Exit = new RoutedUICommand("Exit Iside", "Exit", typeof(Commands));

            // TOOLS
            HashNow = new RoutedUICommand("Compute hash codes now", "Hash Now", typeof(Commands));
            CompareNow = new RoutedUICommand("Execute a new comparison", "Compare Now", typeof(Commands));
            Folders = new RoutedUICommand("Opens the folder comparison tool", "Iside Folders", typeof(Commands));
            MultiFile = new RoutedUICommand("Show hash code of more than one file", "Multifile Hash", typeof(Commands));
            CdDvd = new RoutedUICommand("Compute hash code of an entire CD/DVD", "CD/DVD Hash", typeof(Commands));
            Md5Gen = new RoutedUICommand("Generate the Md5Sum of a directory", "Generate Md5Sum", typeof(Commands));
            Md5Verify = new RoutedUICommand("Verify a previously saved Md5Sum against a directory", "Verify Md5Sum",
                typeof(Commands));
            SfvGen = new RoutedUICommand("Generate the SFV of a directory", "Generate SFV", typeof(Commands));
            SfvVerify = new RoutedUICommand("Verify a previously saved SFV against a directory", "Verify SFV",
                typeof(Commands));

            // HELP
            CheckForUpdates = new RoutedUICommand("Check for Iside updates", "Check For Updates", typeof(Commands));
            IsideWeb = new RoutedUICommand("Navigate to Iside Home Page", "Iside Home Page", typeof(Commands));
            LittleLiteWeb = new RoutedUICommand("Navigate to LittleLite Home Page", "LittleLite Home Page",
                typeof(Commands));
            About = new RoutedUICommand("Show Iside program details", "About", typeof(Commands));
        }
    }
}