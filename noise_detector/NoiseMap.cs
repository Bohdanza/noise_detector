using System.Drawing;
using System.Drawing.Printing;

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
        public int AbsoluteNoiseValue(int x, int y, int radius, int plantTolerance)
        {
            if (x < 0 || y < 0 || x > Width || y > Height || radius<1)
                return -1;

            if (AbsoluteNoises[x, y]!=-1)
                return AbsoluteNoises[x, y];

            int sum = 0; 
            Color centerPix = InitialImage.GetPixel(x, y);

            if (centerPix.G == 0 || centerPix.R > centerPix.G / plantTolerance || centerPix.B > centerPix.G / plantTolerance)
            {
                int upperBoundX = Math.Min(x + radius + 1, Width);
                int upperBoundY = Math.Min(y + radius + 1, Height);

                for (int i = Math.Max(0, x - radius); i < upperBoundX; i++)
                    for (int j = Math.Max(0, y - radius); j < upperBoundY; j++)
                    {
                        Color currentPix = InitialImage.GetPixel(i, j);

                        sum += Math.Abs(currentPix.R - centerPix.R)
                            + Math.Abs(currentPix.G - centerPix.G)
                            + Math.Abs(currentPix.B - centerPix.B);
                    }

                sum /= (4 * radius + 4) * radius;
            }

            AbsoluteNoises[x, y] = sum;

            return sum;
        }

        public int AverageAbsoluteNoise(int x1, int x2, int y1, int y2, int radius, int plantTolerance)
        {
            int sum = 0;

            x1 = Math.Max(0, x1);
            y1 = Math.Max(0, y1);
            x2 = Math.Min(Width, x2);
            y2 = Math.Min(Height, y2);

            for (int i = x1; i < x2; i++)
                for (int j = y1; j < y2; j++)
                    sum += AbsoluteNoiseValue(i, j, radius, plantTolerance);

            return sum / Math.Abs((x1 - x2) * (y1 - y2));
        }

        public Bitmap AverageNoiseMap(int squareWidth, int squareHeight)
        {
            int tmpW = Width / squareWidth + Convert.ToInt32((Width % squareWidth) != 0);
            int tmpH = Height / squareHeight + Convert.ToInt32(Height % squareHeight != 0);

            Bitmap result = new Bitmap(tmpW, tmpH);

            for(int i=0; i<tmpW; i++)
                for(int j=0; j<tmpH; j++)
                {
                    int localNoise = AverageAbsoluteNoise(i * squareWidth, (i + 1) * squareWidth, 
                        j * squareHeight, (j + 1) * squareHeight, 1, 1);
                    result.SetPixel(i, j, Color.FromArgb(localNoise / 3, localNoise/3, localNoise/3));
                }

            return result;
        }
    }
}