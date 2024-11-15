using System.Diagnostics;
using System.Drawing;

namespace noise_detector
{
    public class UserInterface
    {
        public UserInterface() 
        { }

        public void Start()
        {
            Console.WriteLine("Directory with images:");
            string path = Console.ReadLine();

            Console.WriteLine("Noise detect radius (default is 1):");
            int rad = Int32.Parse(Console.ReadLine());

            Console.WriteLine("Plant tolerance (higher values mean less \"greenish\" parts will be considered plants, becomes useless at 2, default is 1):");
            float pt = float.Parse(Console.ReadLine());

            if (path[path.Length - 1] != '\\' && path[path.Length - 1] != '/')
                path += "\\";

            string savepath = path + "saves\\";

            Directory.CreateDirectory(savepath);

            string[] images = Directory.GetFiles(path);

            for (int i = 0; i < images.Length; i++)
            {
                ProcessImage(images[i], savepath, rad, pt);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            Console.WriteLine($"Work done. Results in {savepath}");
        }

        public void ProcessImage(string path, string savepath, int radius, float plantTolerance)
        {
            NoiseMap myLittleNoiseMap = new NoiseMap(new Bitmap(path));

            BitmapProcessor btp = new BitmapProcessor();

            Bitmap res1 = btp.HeightsToStrictBitmap(myLittleNoiseMap.AverageNoiseMap(
                myLittleNoiseMap.Width / 50, myLittleNoiseMap.Width / 50, radius, plantTolerance), 1f, 30);
            
            res1.Save(savepath+Path.GetFileNameWithoutExtension(path)+".png");
            Console.WriteLine("done processing \"" + Path.GetFileName(path) + "\"");
        }
    }
}