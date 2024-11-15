using System.Drawing;

namespace noise_detector
{
    public class BitmapProcessor
    {
        public BitmapProcessor()
        { }

        public Bitmap ArrayToBitmap(byte[,,] ar)
        {
            int w = ar.GetLength(0), h = ar.GetLength(1);
            Bitmap result = new Bitmap(w, h);

            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                    result.SetPixel(i, j, Color.FromArgb(ar[i, j, 0], ar[i, j, 1], ar[i, j, 2]));

            return result;
        }

        public byte[,,] BitmapToArray(Bitmap btm)
        {
            byte[,,] result = new byte[btm.Width, btm.Height, 3];

            for (int i = 0; i < btm.Width; i++)
                for (int j = 0; j < btm.Height; j++)
                {
                    Color cpx = btm.GetPixel(i, j);

                    result[i, j, 0] = cpx.R;
                    result[i, j, 1] = cpx.G;
                    result[i, j, 2] = cpx.B;
                }

            return result;
        }

        public Bitmap HeightsToBitmap(int[,] heights, float scale)
        {
            int w = heights.GetLength(0), h = heights.GetLength(1);
            Bitmap result = new Bitmap(w, h);

            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                {
                    byte val = (byte)Math.Max(0, Math.Min(255, (int)(heights[i, j] * scale)));
                    result.SetPixel(i, j, Color.FromArgb(val, val, val));
                }

            return result;
        }

        public Bitmap HeightsToStrictBitmap(int[,] heights, float scale, int margin)
        {
            int w = heights.GetLength(0), h = heights.GetLength(1);
            Bitmap result = new Bitmap(w, h);
            bool[,] marginCross = new bool[w, h];

            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                {
                    byte val = (byte)Math.Max(0, Math.Min(255, (int)(heights[i, j] * scale)));

                    marginCross[i, j] = val > margin;
                    val = (byte)Math.Min(255, Math.Max(0, val * 2 - margin));

                    result.SetPixel(i, j, Color.FromArgb(val, val, val));
                }


            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                {
                    if (marginCross[i, j])
                    {
                        bool ajacentBright = false;
                        for (int k = Math.Max(0, i - 1); k < Math.Min(i + 2, w) && !ajacentBright; k++)
                            for (int q = Math.Max(0, j - 1); q < Math.Min(j + 2, h) && !ajacentBright; q++)
                                if(k!=i||q!=j)
                                    ajacentBright = marginCross[k, q];

                        if (!ajacentBright)
                        {
                            byte val = (byte)(Math.Max(0, Math.Min(255, (int)(heights[i, j] * scale)))-margin);
                           
                            result.SetPixel(i, j, Color.FromArgb(val, val, val));
                        }
                    }
                }
                 
            return result;
        }

        public Bitmap HeightsToColoredBitmap(int[,] heights, float scale, int greenMax, int YellowMax)
        {
            int w = heights.GetLength(0), h = heights.GetLength(1);
            Bitmap result = new Bitmap(w, h);

            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                {
                    byte val = (byte)Math.Max(0, Math.Min(255, (int)(heights[i, j] * scale)));
                    byte r=0, g=0, b=0;

                    if (val <= greenMax)
                        g = val;
                    else if (val <= YellowMax)
                    {
                        g = val;
                        r = val;
                    }
                    else
                        r = val;

                    result.SetPixel(i, j, Color.FromArgb(r, g, b));
                }

            return result;
        }
    }
}