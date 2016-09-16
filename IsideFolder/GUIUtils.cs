using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace IsideFolder
{
    internal static class GUIUtils
    {
        public static void SetCursorWait(bool isWait)
        {
            if (isWait)
            {
                Mouse.OverrideCursor = Cursors.Wait;
            }
            else
            {
                Mouse.OverrideCursor = null;
            }
        }
    }
}
