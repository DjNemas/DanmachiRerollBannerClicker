using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanmachiRerollBannerClicker
{
    internal class DetectionImages
    {
        private static readonly string folderPath = "./Ressource/Images/";

        private static readonly string RedrawString = folderPath + "redraw.jpg";
        private static readonly string YesString = folderPath + "yes.jpg";
        private static readonly string SkipString = folderPath + "skip.jpg";
        private static readonly string URString = folderPath + "ur.jpg";

        public static DetectionImages Redraw = new DetectionImages(RedrawString);
        public static DetectionImages Yes = new DetectionImages(YesString);
        public static DetectionImages Skip = new DetectionImages(SkipString);
        public static DetectionImages UR = new DetectionImages(URString);

        public readonly string SelectedImage;

        private DetectionImages(string currentImage)
        {
            SelectedImage = currentImage;
        }
    }
}
