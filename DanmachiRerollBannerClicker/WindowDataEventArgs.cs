using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanmachiRerollBannerClicker
{
    internal class WindowDataEventArgs : EventArgs
    {
        public Bitmap Bitmap { get; private set; }
        public Rectangle WindowsPosition { get; private set; }

        public WindowDataEventArgs(Bitmap bitmap, Rectangle windowsPosition)
        {
            Bitmap = bitmap;
            WindowsPosition = windowsPosition;
        }
    }
}
