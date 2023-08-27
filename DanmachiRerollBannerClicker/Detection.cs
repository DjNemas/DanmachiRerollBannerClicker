using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace DanmachiRerollBannerClicker
{
    internal class Detection
    {
        private State _currentState;
        private Win32Mouse _mouse;
        private bool _canClickOnRoll = true;
        private const int _checkFramesAmount = 30;
        private int _checkFrames = _checkFramesAmount;
        private bool _foundEnoughMatches = false;
        private int _numberOfUrToFind;
        private TimeSpan _timeToWaitOnRoll = TimeSpan.FromSeconds(1);

        public bool IsDetecting = false;
        public EventHandler<State> StatusChanged;
        public EventHandler<System.Windows.RoutedEventArgs> FoundGachaResult;

        public Detection(State currentState, int numberOfUrToFind)
        {
            _currentState = currentState;
            _mouse = new Win32Mouse();
            _numberOfUrToFind = numberOfUrToFind;
        }

        public void SetNumberOfUrToFind(int number)
        {
            _numberOfUrToFind = number;
        }

        public void SetState(State state)
        {
            _currentState = state;
            StatusChanged.Invoke(this, state);
        }

        public void Detect(ref Bitmap bitmap, Rectangle windowsCoordinates)
        {
            IsDetecting = true;
            switch (_currentState)
            {
                case State.Redraw:
                {
                    int matchesFound;
                    Point clickPosition;
                    DetectRedrawObject(ref bitmap, out matchesFound, out clickPosition, windowsCoordinates, 0.9, DetectionImages.Redraw);
                    Console.WriteLine(matchesFound);
                    if (matchesFound == 1)
                    {
                        ClickMouse(clickPosition);
                        ChangeState(State.RedrawYesButton);
                    }
                    break;
                }
                case State.RedrawYesButton:
                    {
                        int matchesFound;
                        Point clickPosition;
                        DetectObjects(ref bitmap, out matchesFound, out clickPosition, windowsCoordinates, 0.895, DetectionImages.Yes);
                        Console.WriteLine(matchesFound);
                        if (matchesFound == 1)
                        {
                            ClickMouse(clickPosition);
                            ChangeState(State.Skip);
                        }
                        break;
                    }
                case State.Skip:
                    {
                        int matchesFound;
                        Point clickPosition;
                        DetectObjects(ref bitmap, out matchesFound, out clickPosition, windowsCoordinates, 0.95, DetectionImages.Skip);
                        Console.WriteLine(matchesFound);
                        if (matchesFound == 1)
                        {
                            ClickMouse(clickPosition);
                            ChangeState(State.Click);
                        }
                        break;
                    }
                case State.Click:
                    {
                        int matchesFound;
                        Point clickPosition;
                        DetectRedrawObject(ref bitmap, out matchesFound, out clickPosition, windowsCoordinates, 0.8, DetectionImages.Redraw);
                        if (matchesFound == 1)
                        {
                            ChangeState(State.GachaResult);
                        }
                        else
                        {
                            if (_canClickOnRoll)
                                _ = Task.Run(async () => await ClickOnRoll(windowsCoordinates));
                        }                        
                        break;
                    }
                case State.GachaResult:
                    {
                        int matchesFound;
                        Point clickPosition;
                        DetectResultObjects(ref bitmap, out matchesFound, out clickPosition, windowsCoordinates, 0.8, DetectionImages.UR);
                        Console.WriteLine(matchesFound);

                        if (_checkFrames > 0)
                        {
                            if(matchesFound >= _numberOfUrToFind)
                                _foundEnoughMatches = true;
                            _checkFrames--;
                            break;
                        }

                        if (!_foundEnoughMatches)
                        {
                            ChangeState(State.Redraw);
                        }
                        else
                        {
                            ChangeState(State.NotRunning);
                        }
                        _checkFrames = _checkFramesAmount;
                        break;
                    }
                case State.NotRunning:
                default:
                    {

                        FoundGachaResult.Invoke(this, new System.Windows.RoutedEventArgs());
                    }
                    break;
            }
            IsDetecting = false;
        }

        private void ChangeState(State state)
        {
            _currentState = state;
            StatusChanged.Invoke(this, state);
        }

        private async Task ClickMouse(Point position)
        {
            var oldPosition = _mouse.GetMouseMonitorPos();

            _mouse.SetMousePos(position.X, position.Y);
            _mouse.MoveMouseMonitor();
            _mouse.ClickMouseButton(MouseButtons.Left);
            _mouse.Wait(TimeSpan.FromMilliseconds(50));
            _mouse.SetMousePos(oldPosition);
            _mouse.MoveMouseMonitor();
        }

        private async Task ClickOnRoll(Rectangle windowsCoordinates)
        {
            var oldPosition = _mouse.GetMouseMonitorPos();

            _canClickOnRoll = false;
            _mouse.SetMousePos(windowsCoordinates.X + 50, windowsCoordinates.Y + 50);
            _mouse.MoveMouseMonitor();
            _mouse.ClickMouseButton(MouseButtons.Left);
            _mouse.Wait(TimeSpan.FromMilliseconds(50));
            _mouse.SetMousePos(oldPosition);
            _mouse.MoveMouseMonitor();
            _mouse.Wait(_timeToWaitOnRoll);
            _canClickOnRoll = true;
        }

        private void DetectResultObjects(ref Bitmap bitmap, out int matchesFound, out Point clickPosition, Rectangle windowsCoordintes, double threshold, DetectionImages imageToDetect)
        {
            Mat baseImage = bitmap.ToMat();
            Mat searchImage = CvInvoke.Imread(imageToDetect.SelectedImage);

            List<UnitMat> listToCheck = new List<UnitMat>();

            Size urSize = new Size(65, 45);

            Point position = new Point(250, 230);
            listToCheck.Add(new UnitMat(new Mat(baseImage, new Rectangle(position, urSize)), position));
            position = new Point(475, 230);
            listToCheck.Add(new UnitMat(new Mat(baseImage, new Rectangle(position, urSize)), position));
            position = new Point(705, 230);
            listToCheck.Add(new UnitMat(new Mat(baseImage, new Rectangle(position, urSize)), position));
            position = new Point(930, 230);
            listToCheck.Add(new UnitMat(new Mat(baseImage, new Rectangle(position, urSize)), position));
            position = new Point(1160, 230);
            listToCheck.Add(new UnitMat(new Mat(baseImage, new Rectangle(position, urSize)), position));
            position = new Point(250, 460);
            listToCheck.Add(new UnitMat(new Mat(baseImage, new Rectangle(position, urSize)), position));
            position = new Point(475, 460);
            listToCheck.Add(new UnitMat(new Mat(baseImage, new Rectangle(position, urSize)), position));
            position = new Point(705, 460);
            listToCheck.Add(new UnitMat(new Mat(baseImage, new Rectangle(position, urSize)), position));
            position = new Point(930, 460);
            listToCheck.Add(new UnitMat(new Mat(baseImage, new Rectangle(position, urSize)), position));
            position = new Point(1160, 460);
            listToCheck.Add(new UnitMat(new Mat(baseImage, new Rectangle(position, urSize)), position));


            GetAndDrawMatchesForList(listToCheck, baseImage, searchImage, out matchesFound, out clickPosition, windowsCoordintes, threshold);
            bitmap = baseImage.ToBitmap();

        }        

        private void DetectRedrawObject(ref Bitmap bitmap, out int matchesFound, out Point clickPosition, Rectangle windowsCoordintes, double threshold, DetectionImages imageToDetect)
        {
            Mat baseImage = bitmap.ToMat();
            Mat searchImage = CvInvoke.Imread(imageToDetect.SelectedImage);

            List<UnitMat> listToCheck = new List<UnitMat>();

            Point position = new Point(470, 800);
            Size urSize = new Size(265, 80);
            
            listToCheck.Add(new UnitMat(new Mat(baseImage, new Rectangle(position, urSize)), position));

            GetAndDrawMatchesForList(listToCheck, baseImage, searchImage, out matchesFound, out clickPosition, windowsCoordintes, threshold);
            bitmap = baseImage.ToBitmap();
        }

        private void DetectObjects(ref Bitmap bitmap, out int matchesFound, out Point clickPosition, Rectangle windowsCoordintes, double threshold, DetectionImages imageToDetect)
        {
            matchesFound = 0;
            clickPosition = new(0, 0);

            Mat baseImage = bitmap.ToMat();
            Mat searchImage = CvInvoke.Imread(imageToDetect.SelectedImage);

            Mat outputImage = new Mat();

            CvInvoke.MatchTemplate(baseImage, searchImage, outputImage, TemplateMatchingType.CcoeffNormed);

            double minVal = 0.0d;
            double maxVal = 0.0d;
            Point minLoc = new Point();
            Point maxLoc = new Point();

            CvInvoke.MinMaxLoc(outputImage, ref minVal, ref maxVal, ref minLoc, ref maxLoc);
            CvInvoke.Threshold(outputImage, outputImage, threshold, 1, ThresholdType.ToZero);

            var matches = outputImage.ToImage<Gray, byte>();

            for (int i = 0; i < matches.Rows; i++)
            {
                for (int j = 0; j < matches.Cols; j++)
                {
                    if (matches[i, j].Intensity > threshold)
                    {
                        
                        Point loc = new Point(j, i);
                        Rectangle box = new Rectangle(loc, searchImage.Size);
                        CvInvoke.Rectangle(baseImage, box, new MCvScalar(255, 242, 7), 2); // Object Detection
                        
                        clickPosition = new Point(searchImage.Size.Width / 2 + windowsCoordintes.X + loc.X, searchImage.Size.Height / 2 + windowsCoordintes.Y + loc.Y);
                        Point clickDrawPosition = new Point(searchImage.Size.Width / 2 + loc.X, searchImage.Size.Height / 2 + loc.Y);

                        box = new Rectangle(new Point(clickDrawPosition.X - 5, clickDrawPosition.Y - 5), new Size(10,10));                        
                        CvInvoke.Rectangle(baseImage, box, new MCvScalar(0, 0, 255), 2); // Click Position

                        matchesFound++;
                    }
                }
            }

            bitmap = baseImage.ToBitmap();
        }

        private void GetAndDrawMatchesForList(List<UnitMat> listToCheck, Mat baseImage, Mat searchImage, out int matchesFound, out Point clickPosition, in Rectangle windowsCoordintes, double threshold)
        {
            matchesFound = 0;
            clickPosition = new(0, 0);

            foreach (UnitMat unit in listToCheck)
            {
                Mat outputImage = new Mat();

                CvInvoke.MatchTemplate(unit.Mat, searchImage, outputImage, TemplateMatchingType.CcoeffNormed);

                double minVal = 0.0d;
                double maxVal = 0.0d;
                Point minLoc = new Point();
                Point maxLoc = new Point();

                CvInvoke.MinMaxLoc(outputImage, ref minVal, ref maxVal, ref minLoc, ref maxLoc);
                CvInvoke.Threshold(outputImage, outputImage, threshold, 1, ThresholdType.ToZero);

                var matches = outputImage.ToImage<Gray, byte>();
                bool matchFoundInLoop = false;

                for (int i = 0; i < matches.Rows; i++)
                {
                    for (int j = 0; j < matches.Cols; j++)
                    {
                        if (matches[i, j].Intensity > threshold)
                        {

                            Point loc = new Point(unit.Point.X + j, unit.Point.Y + i);
                            Rectangle box = new Rectangle(loc, searchImage.Size);
                            CvInvoke.Rectangle(baseImage, box, new MCvScalar(255, 242, 7), 2); // Object Detection

                            clickPosition = new Point(searchImage.Size.Width / 2 + windowsCoordintes.X + loc.X, searchImage.Size.Height / 2 + windowsCoordintes.Y + loc.Y);
                            Point clickDrawPosition = new Point(searchImage.Size.Width / 2 + loc.X, searchImage.Size.Height / 2 + loc.Y);

                            box = new Rectangle(new Point(clickDrawPosition.X - 5, clickDrawPosition.Y - 5), new Size(10, 10));
                            CvInvoke.Rectangle(baseImage, box, new MCvScalar(0, 0, 255), 2); // Click Position

                            matchFoundInLoop = true;
                        }
                    }
                }

                if (matchFoundInLoop)
                    matchesFound++;
            }
        }
        private class UnitMat
        {
            public Mat Mat { get; private set; }
            public Point Point { get; private set; }
            public UnitMat(Mat mat, Point point)
            {
                Point = point;
                Mat = mat;
            }
        }
    }


    internal enum State
    {
        GachaResult,
        Redraw,
        RedrawYesButton,
        Skip,
        Click,
        NotRunning
    }
}
