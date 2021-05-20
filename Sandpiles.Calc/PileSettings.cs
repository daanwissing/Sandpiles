namespace Sandpiles.Calc
{
    public class PileSettings
    {
        public int Height { get; set; } = 100;
        public int Width { get; set; } = 100;
        public int Seed { get; set; } = 1000;
        public bool PrintConsole { get; set; } = true;
        public bool SaveIntermediateImage { get; set; } = false;
        public bool SaveFinalImage { get; set; } = true;

        public string Filename { get; set; }
    }
}