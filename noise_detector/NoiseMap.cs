using System.Drawing;

namespace noise_detector
{
    public class NoiseMap
    {
        public int Width { get; protected set; }
        public int Height { get; protected set; }

        public Bitmap InitialImage { get; protected set; }
        public int[,] AbsoluteNoises { get; protected set; }
        
        public NoiseMap(Bitmap initialImage)
        {
            InitialImage = initialImage;
        
            Width = initialImage.Width;
            Height = initialImage.Height;

            AbsoluteNoises = new int[Width, Height];

            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                    AbsoluteNoises[i, j] = -1;
        }

        /// <summary>
        /// Returns average difference between the (x, y) pixel and other pixels in radius*2+1 x radius*2+1 square around it
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="radius"></param>
        public int AbsoluteNoiseValue(int x, int y, int radius)
        {
            if (x < 0 || y < 0 || x > Width || y > Height || radius<1)
                return -1;

            if (AbsoluteNoises[x, y]!=-1)
                return AbsoluteNoises[x, y];

            int sum = 0;
            int upperBoundX = Math.Min(x + radius + 1, Width); 
            int upperBoundY = Math.Min(y + radius + 1, Height);

            Color centerPix = InitialImage.GetPixel(x, y);

            for (int i = Math.Max(0, x - radius); i < upperBoundX; i++)
                for (int j = Math.Max(0, y - radius); j < upperBoundY; j++)
                {
                    Color currentPix = InitialImage.GetPixel(i, j);

                    sum += Math.Abs(currentPix.R - centerPix.R) 
                        + Math.Abs(currentPix.G - centerPix.G) 
                        + Math.Abs(currentPix.B - centerPix.B);
                }

            sum /= (4 * radius + 4) * radius;

            AbsoluteNoises[x, y] = sum;

            return sum;
        }

        /// <summary>
        /// Uses AbsoluteNoiseValue on all cells
        /// </summary>
        public void FillAbsoluteNoises(int radius)
        {
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                    AbsoluteNoiseValue(i, j, radius);
        }
    }
}