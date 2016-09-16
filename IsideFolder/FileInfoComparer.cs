/**
    Iside - .NET WPF Version 
    Copyright (C) LittleLite Software
    Author Alessio Saltarin
**/

using System;
using System.IO;
using System.Collections;

using LLCryptoLib;

namespace IsideFolder
{
    public class FileInfoComparer : IComparer
    {
        #region IComparer Members

        public int Compare(object x, object y)
        {
            if ((x == null) && (y == null))
            {
                return 0; // equals
            }

            if ((x == null) && (y != null))
            {
                return 1; // x<y
            }

            if ((x != null) && (y == null))
            {
                return -1; // x>y
            }

            FileInfo f1 = (FileInfo)x;
            FileInfo f2 = (FileInfo)y;

            string fullNameFile1;
            string fullNameFile2;
            int returnCode;

            try
            {
                fullNameFile1 = f1.FullName;
                fullNameFile2 = f2.FullName;
                returnCode = fullNameFile1.CompareTo(fullNameFile2);
            }
            catch (InvalidOperationException)
            {
                returnCode = -1;
            }

            return returnCode;
        }

        #endregion
    }
}
