using Sandpiles.Calc;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Sandpiles.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SandPileGrid pile;

        private static Bitmap bmpLive;
        private static Bitmap bmpLast;

        private static bool isCalculating = false;

        private int iteration = 0;

        public MainWindow()
        {
            InitializeComponent();
            bmpLive = new Bitmap((int)DrawCanvas.Width, (int)DrawCanvas.Height);
            bmpLast = (Bitmap)bmpLive.Clone();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StartButton.IsEnabled = false;
            pile = new SandPileGrid((int)DrawCanvas.Height, (int)DrawCanvas.Width);
            var settings = new PileSettings
            {
                PrintConsole = false,
                Height = (int)DrawCanvas.Height,
                Width = (int)DrawCanvas.Width,
                Seed = Convert.ToInt32(seed.Text)
            };
            pile.SetSeed(settings);

            var calcThread = new Thread(new ThreadStart(Calculate));
            var renderThread = new Thread(new ThreadStart(Render));

            calcThread.Start();
            renderThread.Start();
        }

        private void Calculate()
        {
            isCalculating = true;
            iteration = 0;
            while (pile.ToppleInPlace())
            {
                iteration++;
            };
            isCalculating = false;
            Dispatcher.Invoke(() =>
            {
                StartButton.IsEnabled = true;
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
                frame++;
                for (var x = 0; x < pile.Width; x++)
                {
                    for (var y = 0; y < pile.Height; y++)
                    {
                        bmpLive.SetPixel(x, y, GetColor(pile.Grid[x][y]));
                    }
                }

                lock (bmpLast)
                {
                    bmpLast.Dispose();
                    bmpLast = (Bitmap)bmpLive.Clone();
                    var iterationsPerSecond = 1000 * (decimal)(iteration - lastIteration) / intervalTime.ElapsedMilliseconds;
                    var framespersecond = (decimal)1000 / intervalTime.ElapsedMilliseconds;
                    Dispatcher.Invoke(() =>
                    {
                        DrawImg.Source = BmpImageFromBmp(bmpLast);
                        iterations.Content = iteration;
                        time.Content = totalTime.Elapsed.ToString(@"hh\:mm\:ss\.fff");
                        ips.Content = iterationsPerSecond.ToString("#.###");
                        fps.Content = framespersecond.ToString("#.###");
                    });
                }
                lastIteration = iteration;
                intervalTime.Restart();
            }
        }

        private static Color GetColor(int grains)
        {
            return grains switch
            {
                0 => Color.Black,
                1 => Color.Red,
                2 => Color.Orange,
                3 => Color.Yellow,
                4 => Color.LightYellow,
                _ => Color.White,
            };
        }

        private static BitmapImage BmpImageFromBmp(Bitmap bmp)
        {
            using (var memory = new System.IO.MemoryStream())
            {
                bmp.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }
    }
}