using Microsoft.Win32;
using Sandpiles.Calc;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sandpiles.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SandPileGrid Pile;

        private static WriteableBitmap Bitmap;

        private static bool isCalculating = false;

        private int Iteration = 0;

        private CancellationTokenSource CancelDrawToken;
        private CancellationTokenSource CancelCalcToken;

        private Color[] colors;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartButton.IsEnabled = false;
            StopButton.IsEnabled = true;
            SaveButton.IsEnabled = false;
            var dimension = Convert.ToInt32(size.Text);
            DrawCanvas.Height = dimension;
            DrawCanvas.Width = dimension;
            Bitmap = new WriteableBitmap(dimension, dimension, 96, 96, PixelFormats.Bgra32, null);
            DrawImg.Source = Bitmap;
            Pile = new SandPileGrid(dimension, dimension);
            var settings = new PileSettings
            {
                PrintConsole = false,
                Height = dimension,
                Width = dimension,
                Seed = Convert.ToInt32(seed.Text)
            };
            Pile.SetSeed(settings);
            SetColors();
            var calcThread = new Thread(new ThreadStart(Calculate));
            var renderThread = new Thread(new ThreadStart(Render));
            CancelCalcToken = new CancellationTokenSource();
            CancelDrawToken = new CancellationTokenSource();
            calcThread.Start();
            renderThread.Start();
        }

        private void SetColors()
        {
            colors = new[] {
                Color0.SelectedColor.GetValueOrDefault(),
                Color1.SelectedColor.GetValueOrDefault(),
                Color2.SelectedColor.GetValueOrDefault(),
                Color3.SelectedColor.GetValueOrDefault(),
                Color4.SelectedColor.GetValueOrDefault(),
                Color5.SelectedColor.GetValueOrDefault(),
            };
        }

        private void Calculate()
        {
            isCalculating = true;
            Iteration = 0;
            while (Pile.ToppleInPlace())
            {
                Iteration++;
                if (CancelCalcToken.IsCancellationRequested)
                    break;
            };
            isCalculating = false;
            Dispatcher.Invoke(() =>
            {
                StartButton.IsEnabled = true;
                SaveButton.IsEnabled = true;
                StopButton.IsEnabled = false;
            });
        }

        private void Render()
        {
            int frame = 0;
            int lastIteration = 0;
            var totalTime = Stopwatch.StartNew();
            var intervalTime = Stopwatch.StartNew();

            while (isCalculating)
            {
                if (CancelDrawToken.IsCancellationRequested)
                    return;

                lastIteration = Iteration;
                frame++;
                byte[] pixels = CalcPixelColors();
                lock (Bitmap)
                {
                    Dispatcher.Invoke(() =>
                    {
                        Bitmap.WritePixels(new Int32Rect(0, 0, Pile.Width - 1, Pile.Height - 1), pixels, Bitmap.BackBufferStride, 0, 0);
                        iterations.Content = Iteration;
                        time.Content = totalTime.Elapsed.ToString(@"hh\:mm\:ss\.fff");
                        var iterationsPerSecond = 1000 * (decimal)(Iteration - lastIteration) / (intervalTime.ElapsedMilliseconds);
                        var framespersecond = (decimal)1000 / (intervalTime.ElapsedMilliseconds);
                        ips.Content = iterationsPerSecond.ToString("0.000");
                        fps.Content = framespersecond.ToString("0.000");
                    });
                }

                intervalTime.Restart();
            }
        }

        private byte[] CalcPixelColors()
        {
            byte[] pixels = new byte[Pile.Width * Pile.Height * 4];
            for (var x = 0; x < Pile.Width; x++)
            {
                for (var y = 0; y < Pile.Height; y++)
                {
                    var color = colors[Math.Min(Pile.Grid[x][y], 5)];
                    var offset = (4 * y) + (4 * x * Pile.Width);
                    pixels[offset] = color.B;
                    pixels[offset + 1] = color.G;
                    pixels[offset + 2] = color.R;
                    pixels[offset + 3] = color.A;
                }
            }

            return pixels;
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            CancelCalcToken.Cancel();
            CancelDrawToken.Cancel();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                DefaultExt = ".png",
                Filter = "Portable Network Graphics|*.png|JPEG|*.jpeg",
                Title = "Save sandpile image",
                FileName = $"{DateTime.Now:yyyymmdd_hhMMss}_pile.png"
            };
            var result = dialog.ShowDialog();
            if (result == true)
            {
                using (var stream = File.OpenWrite(dialog.FileName))
                {
                    BitmapEncoder encoder;
                    if (dialog.FilterIndex == 1)
                    {
                        encoder = new PngBitmapEncoder();
                    }
                    else
                    {
                        encoder = new JpegBitmapEncoder();
                    }
                    encoder.Frames.Add(BitmapFrame.Create(Bitmap));
                    encoder.Save(stream);
                }
            }
        }
    }
}