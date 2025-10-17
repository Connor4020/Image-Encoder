using System.Drawing;

class Program
{
    static void Main(string[] args)
    {

        String inputBeautify = "\n>>> ";

        Console.Write("-- Please input the file location of an image to encode. --" + inputBeautify);
        String fileLoc = Console.ReadLine();

        var bitmap = new Bitmap(fileLoc);

        // C:\Users\proga\Desktop\Second.png

        // Loop.
        // Get x and y sizes.
        // Loop through all x axis.
        // When finished, add 1 to y.

        //int width = bitmap.Width;
        //int height = bitmap.Height;

        int width = 5;
        int height = 5;

        // Runs until most far-right pixel is reached.
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color pixelColour = bitmap.GetPixel(x, y);
                Console.WriteLine(pixelColour);
            }

        }
    }
}

