using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanmachiRerollBannerClicker
{
    public struct MouseInput
    {
        /// <summary>
        /// <para>The absolute position of the mouse, or the amount of motion since the last mouse event was generated, depending on the value of the Flags member.</para>
        /// <para>Absolute data is specified as the x coordinate of the mouse; relative data is specified as the number of pixels moved.</para>
        /// </summary>
        public int XPos;
        /// <summary>
        /// <para>The absolute position of the mouse, or the amount of motion since the last mouse event was generated, depending on the value of the Flags member.</para>
        /// <para>Absolute data is specified as the y coordinate of the mouse; relative data is specified as the number of pixels moved.</para>
        /// </summary>
        public int YPos;
        /// <summary>
        /// <para>If Flags contains MOUSEEVENTF_WHEEL, then mouseData specifies the amount of wheel movement. A positive value indicates that the wheel was rotated forward, away from the user; a negative value indicates that the wheel was rotated backward, toward the user. One wheel click is defined as WHEEL_DELTA, which is 120.</para>
        /// 
        /// <para>Windows Vista: If Flags contains MOUSEEVENTF_HWHEEL, then dwData specifies the amount of wheel movement.A positive value indicates that the wheel was rotated to the right; a negative value indicates that the wheel was rotated to the left.One wheel click is defined as WHEEL_DELTA, which is 120.</para>
        ///
        /// <para>If Flags does not contain MOUSEEVENTF_WHEEL, MOUSEEVENTF_XDOWN, or MOUSEEVENTF_XUP, then mouseData should be zero.</para>
        ///
        /// <para>If Flags contains MOUSEEVENTF_XDOWN or MOUSEEVENTF_XUP, then mouseData specifies which X buttons were pressed or released. This value may be any combination of the following flags.</para>
        /// </summary>
        public uint MouseData;
        /// <summary>
        /// <para>A set of bit flags that specify various aspects of mouse motion and button clicks. The bits in this member can be any reasonable combination of the following values.</para>
        ///
        /// <para>The bit flags that specify mouse button status are set to indicate changes in status, not ongoing conditions.For example, if the left mouse button is pressed and held down, MOUSEEVENTF_LEFTDOWN is set when the left button is first pressed, but not for subsequent motions.Similarly MOUSEEVENTF_LEFTUP is set only when the button is first released.</para>
        ///
        /// <para>You cannot specify both the MOUSEEVENTF_WHEEL flag and either MOUSEEVENTF_XDOWN or MOUSEEVENTF_XUP flags simultaneously in the dwFlags parameter, because they both require use of the mouseData field.</para>
        /// </summary>
        public MouseEventFlags Flags;
        /// <summary>
        /// The time stamp for the event, in milliseconds. If this parameter is 0, the system will provide its own time stamp.
        /// </summary>
        public uint Time;
        /// <summary>
        /// An additional value associated with the mouse event. An application calls GetMessageExtraInfo to obtain this extra information.
        /// </summary>
        public UIntPtr Extrainfo;
    }

    internal struct MouseButton
    {
        public static readonly uint XButton1 = 0x0001;
        public static readonly uint XButton2 = 0x0002;
    }

    [Flags]
    public enum MouseEventFlags : uint
    {
        /// <summary>
        /// Movement occurred.
        /// </summary>
        Move = 0x0001,
        /// <summary>
        /// The left button was pressed.
        /// </summary>
        LeftDown = 0x0002,
        /// <summary>
        /// The left button was released.
        /// </summary>
        LeftUp = 0x0004,
        /// <summary>
        /// The right button was pressed.
        /// </summary>
        RightDown = 0x0008,
        /// <summary>
        /// The right button was released.
        /// </summary>
        RightUp = 0x00010,
        /// <summary>
        /// The middle button was pressed.
        /// </summary>
        MiddleDown = 0x0020,
        /// <summary>
        /// The middle button was released.
        /// </summary>
        MiddleUp = 0x0040,
        /// <summary>
        /// An X button was pressed.
        /// </summary>
        XDown = 0x0080,
        /// <summary>
        /// An Y button was released.
        /// </summary>
        XUp = 0x0100,
        /// The wheel was moved, if the mouse has a wheel. The amount of movement is specified in mouseData.
        /// </summary>
        Wheel = 0x0800,
        /// <summary>
        /// The wheel was moved horizontally, if the mouse has a wheel. The amount of movement is specified in mouseData.
        /// <para>Windows XP/2000: This value is not supported.</para>
        /// </summary>
        HWheel = 0x1000,
        /// <summary>
        /// The WM_MOUSEMOVE messages will not be coalesced. The default behavior is to coalesce WM_MOUSEMOVE messages.
        /// <para>Windows XP/2000: This value is not supported.</para>
        /// </summary>
        Move_NoCoalesce = 0x2000,
        /// <summary>
        /// Maps coordinates to the entire desktop. Must be used with MOUSEEVENTF_ABSOLUTE.
        /// </summary>
        VirtualDesk = 0x4000,
        /// <summary>
        /// The dx and dy members contain normalized absolute coordinates.
        /// If the flag is not set, dxand dy contain relative data (the change in position since the last reported position).
        /// This flag can be set, or not set, regardless of what kind of mouse or other pointing device, if any, is connected to the system.
        /// For further information about relative mouse motion, see the following Remarks section.
        /// </summary>
        Absolute = 0x8000
    }
}
