using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace DanmachiRerollBannerClicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        private CancellationTokenSource _tokenRunningCapture;

        private EventHandler<WindowDataEventArgs> _newCaptureImage;

        private ObservableCollection<string> _listProcesses = new();

        private Detection _detection;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            AllocConsole();
#endif
            _tokenRunningCapture = new CancellationTokenSource();
            _newCaptureImage += OnNewCaptureImage;

            CB_Processes.ItemsSource = _listProcesses;
            GetDanchroProcess();

            _detection = new Detection(State.NotRunning, Convert.ToInt32(TB_MinUR.Text));  
            LBL_CurrentState.Content = State.NotRunning.ToString();

            _detection.StatusChanged += OnStateChanged;
            _detection.FoundGachaResult += OnFoundGachaResult;

        }

        #region Button Click Events
        private void Btn_Start_Click(object sender, RoutedEventArgs e)
        {
            if ((string)CB_Processes.SelectedItem != "danchro")
            {
                MessageBox.Show("Please start the game and hit Search Processes again.", "Process danchro not found!", MessageBoxButton.OK);
                return;
            }

            TB_FPS.IsEnabled = false;
            Btn_Start.IsEnabled = false;
            Btn_Stop.IsEnabled = true;

            _detection.SetState(State.Redraw);

            _tokenRunningCapture = new CancellationTokenSource();
            _ = Task.Run(() => RunCapture(_tokenRunningCapture.Token));
        }

        private void Btn_Stop_Click(object sender, RoutedEventArgs e)
        {
            TB_FPS.IsEnabled = true;
            Btn_Start.IsEnabled = true;
            Btn_Stop.IsEnabled = false;

            _detection.SetState(State.NotRunning);

            _tokenRunningCapture.Cancel();
        }

        private void Btn_SearchProcesses_Click(object sender, RoutedEventArgs e)
        {
            GetDanchroProcess();
            CB_Processes.SelectedIndex = 0;
        }
        #endregion

        #region Capture
        private async Task RunCapture(CancellationToken token)
        {
            try
            {
                nint processWindowsPtr = await GetProcessPointer();
                if (processWindowsPtr == nint.Zero) return;

                while (true)
                {
                    if (token.IsCancellationRequested) return;

                    CaptureProcesseWindowOnMonitor(processWindowsPtr);

                    string speed = "0";
                    await Application.Current.Dispatcher.InvokeAsync(() => speed = TB_FPS.Text);
                    await Task.Delay((int)(1 / float.Parse(speed) * 1000));
                }
            } 
            catch (Exception ex) 
            {
                await Console.Out.WriteLineAsync(ex.ToString());
            }
        }

        private void CaptureProcesseWindowOnMonitor(nint processWindowsPtr)
        {
            Rectangle rect = Rectangle.Empty;
            Win32.GetWindowRect(processWindowsPtr, ref rect);

            var bitmap = new Bitmap(rect.Width - rect.Left, rect.Height - rect.Top, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            var g = Graphics.FromImage(bitmap);

            var topLeft = new System.Drawing.Point(rect.Left, rect.Top);
            var windowSize = new System.Drawing.Size(rect.Width - rect.Left, rect.Height - rect.Top);

            g.CopyFromScreen(topLeft, System.Drawing.Point.Empty, windowSize);

            _newCaptureImage.Invoke(this, new WindowDataEventArgs(bitmap, rect));
        }

        private async Task<nint> GetProcessPointer()
        {
            Process[] processes = new Process[0];
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                processes = Process.GetProcessesByName(CB_Processes.SelectedItem.ToString());
            });

            return processes.FirstOrDefault()?.MainWindowHandle ?? nint.Zero;
        }
        
        #endregion

        #region Events
        private async void OnNewCaptureImage(object? sender, WindowDataEventArgs windowsData)
        {
            Bitmap bitmap = (Bitmap)windowsData.Bitmap.Clone();
            try
            {
                if (_detection.IsDetecting) return;
                _detection.Detect(ref bitmap, windowsData.WindowsPosition);

                DisplayImage(bitmap);

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
            }
            
        }

        private async void OnStateChanged (object? sender, State state)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                LBL_CurrentState.Content = state.ToString();
            });
        }

        private async void OnFoundGachaResult (object sender, RoutedEventArgs arg)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Btn_Stop_Click(sender, arg);
            });
        }

        #endregion

        #region Helper
        private void TB_FPS_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb is null) return;

            int number;
            var success = int.TryParse(tb.Text, out number);

            if (!success) number = 0;

            if (number < 0)
                number = 0;
            if (number > 120)
                number = 120;

            tb.Text = number.ToString();
        }

        private void TB_MinUR_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb is null) return;

            int number;
            var success = int.TryParse(tb.Text, out number);

            if (!success) number = 1;

            if (number < 1)
                number = 1;
            if (number > 10)
                number = 10;

            tb.Text = number.ToString();

            if (_detection is not null)
                _detection.SetNumberOfUrToFind(number);
        }

        private void GetDanchroProcess()
        {
            _listProcesses.Clear();
            Process[] processes = Process.GetProcessesByName("danchro");
            if (processes.Length >= 1)
                _listProcesses.Add(processes.FirstOrDefault().ProcessName);
            else
                _listProcesses.Add("Game Process Not Found");
        }

        private void SearchProcesses()
        {
            List<string> newProcesses = new();

            Process[] processes = Process.GetProcesses();
            foreach (Process process in processes)
            {
                newProcesses.Add(process.ProcessName);
            }

            // And Order
            var orderdList = newProcesses.Order();
            
            _listProcesses.Clear();
            foreach (var item in orderdList)
            {
                _listProcesses.Add(item);
            }
        }

        public async void DisplayImage(Bitmap bitmap)
        {
            var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            ms.Position = 0;

            var captureImage = new BitmapImage();
            captureImage.BeginInit();
            captureImage.StreamSource = ms;
            captureImage.EndInit();
            captureImage.Freeze();

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Img_Captured.Source = captureImage;
            });
        }

        #endregion
    }
}
