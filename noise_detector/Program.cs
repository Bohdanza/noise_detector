using System.Drawing;

namespace noise_detector
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Image path:");
            string path = Console.ReadLine();

            Console.WriteLine("Noise detect radius:");
            int rad = Int32.Parse(Console.ReadLine());

            Console.WriteLine("Plant tolerance:");
            float pt = float.Parse(Console.ReadLine());

            NoiseMap myLittleNoiseMap = new NoiseMap(new Bitmap(path));

            BitmapProcessor btp = new BitmapProcessor();

            myLittleNoiseMap.FillAbsoluteNoises(rad, pt);

            Bitmap res1 = btp.HeightsToBitmap(myLittleNoiseMap.AverageNoiseMap(
                myLittleNoiseMap.Width / 50, myLittleNoiseMap.Width / 50, rad, pt), 0.33f);
            Bitmap res2 = btp.HeightsToBitmap(myLittleNoiseMap.AbsoluteNoises, 0.33f);

            res1.Save("output1.png");
            res2.Save("output2.png");
        }
    }
}