/**
    Iside - .NET WPF Version 
    Copyright (C) LittleLite Software
    Author Alessio Saltarin
**/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace IsideFolder
{
    public static class Commands
    {
        public static RoutedUICommand Folder_New;
        public static RoutedUICommand Folder_OpenComparison;
        public static RoutedUICommand Folder_SetOriginal;
        public static RoutedUICommand Folder_SetComparison;
        public static RoutedUICommand Folder_Save;
        public static RoutedUICommand Folder_SaveAs;
        public static RoutedUICommand Folder_SaveCopyAs;
        public static RoutedUICommand Folder_ResultsList;
        public static RoutedUICommand Folder_ResultsList_Open;
        public static RoutedUICommand Folder_ResultsList_SaveAs;
        public static RoutedUICommand Folder_RecentComparisons;
        public static RoutedUICommand Folder_Exit;
        public static RoutedUICommand Folder_Run;
        public static RoutedUICommand Folder_CreateResultsList;
        public static RoutedUICommand Folder_Refresh;
        public static RoutedUICommand Folder_IsideFiles;
        public static RoutedUICommand Folder_ViewResults;
        public static RoutedUICommand Folder_ViewLog;
        public static RoutedUICommand Folder_Check_ExitOnFirst;
        public static RoutedUICommand Folder_Check_System;
        public static RoutedUICommand Folder_Check_Hidden;
        public static RoutedUICommand Folder_Check_Archive;

        // HELP
        public static RoutedUICommand Help;
        public static RoutedUICommand CheckForUpdates;
        public static RoutedUICommand IsideWeb;
        public static RoutedUICommand LittleLiteWeb;
        public static RoutedUICommand OrderingInfo;
        public static RoutedUICommand BuyNow;
        public static RoutedUICommand Activate;
        public static RoutedUICommand About;

        static Commands()
        {
            Folder_New = new RoutedUICommand("Reset for a new comparison", "New", typeof(Commands));
            Folder_OpenComparison = new RoutedUICommand("Open a saved comparison", "Open Comparison", typeof(Commands));
            Folder_SetOriginal = new RoutedUICommand("Set the original folder to compare", "Set Original Folder", typeof(Commands));
            Folder_SetComparison = new RoutedUICommand("Set the folder to be compared with", "Set Comparison Folder", typeof(Commands));
            Folder_Save = new RoutedUICommand("Save current comparison", "Save", typeof(Commands));
            Folder_SaveAs = new RoutedUICommand("Save current comparison as...", "Save As...", typeof(Commands));
            Folder_SaveCopyAs = new RoutedUICommand("Save a new copy of this comparison", "Save Copy As...", typeof(Commands));
            Folder_ResultsList = new RoutedUICommand("Show results list", "Results list", typeof(Commands));
            Folder_ResultsList_Open = new RoutedUICommand("Open an existing results list", "Open Results list", typeof(Commands));
            Folder_ResultsList_SaveAs = new RoutedUICommand("Save the current results as a Results list", "Save Results list", typeof(Commands));
            Folder_RecentComparisons = new RoutedUICommand("Show recent comparisons", "Recent Comparisons", typeof(Commands));
            Folder_Exit = new RoutedUICommand("Exit from Iside Folders", "Exit", typeof(Commands));
            Folder_Run = new RoutedUICommand("Run the comparison now", "Run comparison", typeof(Commands));
            Folder_Refresh = new RoutedUICommand("Re-run comparison", "Refresh", typeof(Commands));
            Folder_IsideFiles = new RoutedUICommand("Open Iside in files mode", "Iside Files", typeof(Commands));
            Folder_ViewResults = new RoutedUICommand("View comparison results", "View results", typeof(Commands));
            Folder_ViewLog = new RoutedUICommand("View operations log", "View log", typeof(Commands));
            Folder_Check_ExitOnFirst = new RoutedUICommand("Exit comparison if a difference is found", "Exit on first difference", typeof(Commands));
            Folder_Check_System = new RoutedUICommand("Include system files in comparison", "System files", typeof(Commands));
            Folder_Check_Hidden = new RoutedUICommand("Include hidden files in comparison", "Hidden files", typeof(Commands));
            Folder_Check_Archive = new RoutedUICommand("Include archive files in comparison", "Archive files", typeof(Commands));


            // HELP
            Help = new RoutedUICommand("Show Iside online manual", "Help", typeof(Commands));
            CheckForUpdates = new RoutedUICommand("Check for Iside updates", "Check For Updates", typeof(Commands));
            OrderingInfo = new RoutedUICommand("Learn about Iside licensing", "Ordering Info", typeof(Commands));
            Activate = new RoutedUICommand("Activate Iside", "Activate", typeof(Commands));
            BuyNow = new RoutedUICommand("Buy Iside license", "Buy Now", typeof(Commands));
            IsideWeb = new RoutedUICommand("Navigate to Iside Home Page", "Iside Home Page", typeof(Commands));
            LittleLiteWeb = new RoutedUICommand("Navigate to LittleLite Home Page", "LittleLite Home Page", typeof(Commands));
            About = new RoutedUICommand("Show Iside program details", "About", typeof(Commands));
        }
    }
}
