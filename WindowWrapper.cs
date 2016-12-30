using System;
using System.Windows.Forms;

namespace Dark_Souls_II_Save_Editor
{
    internal class WindowWrapper : IWin32Window
    {
        public WindowWrapper(IntPtr hwnd)
        {
            Handle = hwnd;
        }

        public IntPtr Handle { get; }
    }
}