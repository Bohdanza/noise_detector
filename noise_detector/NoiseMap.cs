using System.Drawing;

namespace noise_detector
{
    public class NoiseMap
    {
        public int Width { get; protected set; }
        public int Height { get; protected set; }

        //using arrays instead of bitmaps for speed
        public byte[,,] InitialImage { get; protected set; }
        public int[,] AbsoluteNoises { get; protected set; }

        //On all image
        public int AverageNoiseOnImage { get; protected set; }

        public NoiseMap(Bitmap initialImage)
        {
            Width = initialImage.Width;
            Height = initialImage.Height;

            InitialImage = new BitmapProcessor().BitmapToArray(initialImage);

            AbsoluteNoises = new int[Width, Height];
        }

        /// <summary>
        /// Returns average difference between the (x, y) pixel and other pixels in radius*2+1 x radius*2+1 square around it
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="radius"></param>
        public int AbsoluteNoiseValue(int x, int y, int radius, float plantTolerance)
        {
            if (x < 0 || y < 0 || x > Width || y > Height || radius<1)
                return -1;

            int sum = 0;

            //TODO: greens
            if (InitialImage[x, y, 1] == 0 ||
                InitialImage[x, y, 0] > InitialImage[x, y, 1] * plantTolerance ||
                InitialImage[x, y, 2] > InitialImage[x, y, 1] * plantTolerance)
            {
                int upperBoundX = Math.Min(x + radius + 1, Width);
                int upperBoundY = Math.Min(y + radius + 1, Height);

                for (int i = Math.Max(0, x - radius); i < upperBoundX; i++)
                    for (int j = Math.Max(0, y - radius); j < upperBoundY; j++)
                        sum += Math.Abs(InitialImage[i, j, 0] - InitialImage[x, y, 0])
                            + Math.Abs(InitialImage[i, j, 1] - InitialImage[x, y, 1])
                            + Math.Abs(InitialImage[i, j, 2] - InitialImage[x, y, 2]);

                sum /= (4 * radius + 4) * radius;
            }

            AbsoluteNoises[x, y] = sum;

            return sum;
        }

        /// <summary>
        /// DOES FILL absolutenoises
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="y1"></param>
        /// <param name="y2"></param>
        /// <param name="radius"></param>
        /// <param name="plantTolerance"></param>
        /// <returns></returns>
        public int AverageAbsoluteNoise(int x1, int x2, int y1, int y2, int radius, float plantTolerance)
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

        public int[,] AverageNoiseMap(int squareWidth, int squareHeight, int radius, float plantTolerance)
        {
            int tmpW = Width / squareWidth + Convert.ToInt32(Width % squareWidth != 0);
            int tmpH = Height / squareHeight + Convert.ToInt32(Height % squareHeight != 0);

            int[,] result = new int[tmpW, tmpH];

            for(int i=0; i<tmpW; i++)
                for(int j=0; j<tmpH; j++)
                {
                    int localNoise = AverageAbsoluteNoise(i * squareWidth, (i + 1) * squareWidth, 
                        j * squareHeight, (j + 1) * squareHeight, radius, plantTolerance);
                     
                    result[i, j]=localNoise;
                }

            return result;
        }

        public void FillAbsoluteNoises(int radius, float plantTolerance)
        {
            AverageNoiseOnImage = 0;
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                    AverageNoiseOnImage+=AbsoluteNoiseValue(i, j, radius, plantTolerance);

            AverageNoiseOnImage /= Width * Height;
        }
    }
}