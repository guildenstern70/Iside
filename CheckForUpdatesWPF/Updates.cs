/**
    Check For Updates Utility - .NET WPF Version 
    Copyright (C) LittleLite Software
    Author Alessio Saltarin
**/

using System.Windows;

namespace CheckForUpdatesWPF
{
    public static class Updates
    {
        public static void Check(Window parent, ApplicationInfo appInfo)
        {
            CheckForUpdates uf = new CheckForUpdates(appInfo);
            uf.Owner = parent;
            uf.ShowDialog();
        }
    }
}
