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

            Bitmap result = myLittleNoiseMap.AverageNoiseMap(myLittleNoiseMap.Width / 100, myLittleNoiseMap.Width / 100);

            result.Save("output.png");
        }
    }
}