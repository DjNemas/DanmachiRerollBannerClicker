using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DanmachiRerollBannerClicker
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CursorInfo
    {
        public uint StrucSize;
        public CursorStates Flags;
        public UIntPtr CursorHandler;
        public Point MouseMonitorPosition;
    }

    [Flags]
    public enum CursorStates : uint
    {
        Cursor_Hidden = 0,
        Cursor_Showing = 0x00000001,
        Cursor_Suppressed = 0x00000002
    }
}
