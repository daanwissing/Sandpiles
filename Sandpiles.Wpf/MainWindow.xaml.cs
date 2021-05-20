using Sandpiles.Calc;
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
                Seed = 100_000
            };
            pile.SetSeed(settings);

            var renderThread = new Thread(new ThreadStart(Render));
            renderThread.Start();

            var calcThread = new Thread(new ThreadStart(Calculate));
            calcThread.Start();
        }

        private void Calculate()
        {
            isCalculating = true;
            while (pile.ToppleInPlace())
            {
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
                    Dispatcher.Invoke(() =>
                    {
                        DrawImg.Source = BmpImageFromBmp(bmpLast);
                        iterations.Content = frame;
                        time.Content = totalTime.Elapsed.ToString();
                    });
                }
                if (frame % 10_000 == 0)
                {
                    intervalTime.Restart();
                }
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