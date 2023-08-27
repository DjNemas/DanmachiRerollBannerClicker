using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DanmachiRerollBannerClicker
{
    internal class Win32
    {
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(nint windowPointer, ref Rectangle rec);

        [DllImport("user32.dll")]
        public static extern bool PrintWindow(nint windowPointer, nint devicePointer, PrintWindowArgs flags);

        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(nint hWnd, ShowWindowArgs nCmdShow);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsIconic(nint hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(nint hWnd);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(nint hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int SetCursorPos(int x, int y);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetCursorPos(out Point point);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetCursorInfo(ref CursorInfo point);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern UIntPtr GetCursor();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint SendInput(uint inputNumber, Input[] inputArray, int sizeOfInputStructInBytes);
    }

    public enum PrintWindowArgs : uint
    {
        PW_FULLWINDOW = 0x0,
        PW_CLIENTONLY = 0x1
    }

    public enum ShowWindowArgs : int
    {
        SHOWNORMAL = 1,
        SHOWMINIMIZED = 2,
        SHOWMAXIMIZED = 3
    }
}
