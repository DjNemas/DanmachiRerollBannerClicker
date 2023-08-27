using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DanmachiRerollBannerClicker
{
    public enum MouseButtons
    {
        Left,
        Right,
        Middle
    }

    public class Win32Mouse
    {
        private Vector2Int mousePos;
        public event EventHandler<StatusCodeArgs>? ExternalMouseFunctionStatusCode;

        public Win32Mouse()
        {
            mousePos = Vector2Int.Zero();
        }

        public Win32Mouse Wait(TimeSpan ts)
        {
            Task.Delay(ts).Wait();
            return this;
        }

        public Win32Mouse SetMousePos(Vector2Int mousePos)
        {
            SetMousePos(mousePos.X, mousePos.Y);
            return this;
        }

        public Win32Mouse SetMousePos(int x, int y)
        {
            mousePos = new Vector2Int(x, y);
            return this;
        }

        public Win32Mouse MoveMouseMonitor()
        {
            int statusCode = Win32.SetCursorPos(mousePos.X, mousePos.Y);

            StatusCodeArgs args = new("MoveMouse()", "SetCursorPos(int x, int y)", statusCode.ToString());
            OnExternelMouseFunctionStatusCode(args);

            return this;
        }

        public Vector2Int GetMouseMonitorPos()
        {
            int statusCode = Win32.GetCursorPos(out Point point);

            StatusCodeArgs args = new("GetMousePos()", "GetCursorPos(out POINT point)", statusCode.ToString());
            OnExternelMouseFunctionStatusCode(args);

            return new Vector2Int(point.X, point.Y);
        }

        public CursorInfo GetMouseInfo()
        {
            CursorInfo cursorInfo = new CursorInfo();
            cursorInfo.StrucSize = (uint)Marshal.SizeOf(typeof(CursorInfo));
            int statusCode = Win32.GetCursorInfo(ref cursorInfo);

            StatusCodeArgs args = new("GetCursorInfo()", "GetCursorInfo(ref cursorInfo)", statusCode.ToString());
            OnExternelMouseFunctionStatusCode(args);

            return cursorInfo;
        }

        public UIntPtr GetMouseHandler()
        {
            UIntPtr pointer = Win32.GetCursor();

            StatusCodeArgs args = new("GetCourserHandler()", "GetCursor()", "\"No Status Code Available\"");
            OnExternelMouseFunctionStatusCode(args);

            return pointer;
        }

        public Win32Mouse ClickMouseButton(MouseButtons button)
        {
            Input[] inputs = GetMouseInputs(button);
            SendInput(inputs);
            return this;
        }

        private Input[] GetMouseInputs(MouseButtons buttons)
        {
            Input inputDown = new();
            inputDown.type = InputType.Input_Mouse;
            Input inputUp = new();
            inputUp.type = InputType.Input_Mouse;

            switch (buttons)
            {
                case MouseButtons.Left:
                    {
                        inputDown.Union.MouseInput = GetMouseInput(MouseEventFlags.LeftDown);
                        inputUp.Union.MouseInput = GetMouseInput(MouseEventFlags.LeftUp);
                        break;
                    }
                case MouseButtons.Right:
                    {
                        inputDown.Union.MouseInput = GetMouseInput(MouseEventFlags.RightDown);
                        inputUp.Union.MouseInput = GetMouseInput(MouseEventFlags.RightUp);
                        break;
                    }
                case MouseButtons.Middle:
                    {
                        inputDown.Union.MouseInput = GetMouseInput(MouseEventFlags.MiddleDown);
                        inputUp.Union.MouseInput = GetMouseInput(MouseEventFlags.MiddleUp);
                        break;
                    }
            }
            return new Input[] { inputDown, inputUp };
        }

        private MouseInput GetMouseInput(MouseEventFlags flags)
        {
            MouseInput input = new();
            input.Flags = flags;
            return input;
        }

        private void SendInput(Input[] input)
        {
            uint statusCode = Win32.SendInput(Convert.ToUInt32(input.Count()), input, Marshal.SizeOf(typeof(Input)));
            StatusCodeArgs args = new("ClickMouseButton()",
                "SendInput(in uint inputNumber, in Input[] inputArray, int sizeOfInputStructInBytes)",
                "Success with " + statusCode.ToString() + " Inputs.");
            OnExternelMouseFunctionStatusCode(args);
        }

        private void OnExternelMouseFunctionStatusCode(StatusCodeArgs args)
        {
            ExternalMouseFunctionStatusCode?.Invoke(this, args);
        }
    }
}
