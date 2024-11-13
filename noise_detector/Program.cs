using noise_detector;
using System;
using System.Drawing;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Image path:");
            string path = Console.ReadLine();
            
            NoiseMap myLittleNoiseMap = new NoiseMap(new Bitmap(path));

            Bitmap result = new Bitmap(myLittleNoiseMap.Width, myLittleNoiseMap.Height);

            for (int i = 0; i < myLittleNoiseMap.Width; i++)
                for (int j = 0; j < myLittleNoiseMap.Height; j++)
                {
                    int val = myLittleNoiseMap.AbsoluteNoiseValue(i, j, 1)/3;
                    result.SetPixel(i, j, Color.FromArgb(val, val, val));
                }

            result.Save("output.png");
        }
    }
}