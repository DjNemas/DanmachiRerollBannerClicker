using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DanmachiRerollBannerClicker
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Input
    {
        public InputType type;
        public InputUnion Union;
    }

    public enum InputType : uint
    {
        Input_Mouse = 0,
        Input_Keyboard = 1,
        Input_Hardware = 2
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct InputUnion
    {
        [FieldOffset(0)]
        public MouseInput MouseInput;
        //[FieldOffset(0)]
        //public KeyboardInput KeyboardInput;
        //[FieldOffset(0)]
        //public HardwareInput HardwareInput;
    }
}
