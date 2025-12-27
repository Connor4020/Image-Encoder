using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Barton___Y2_Project
{
    internal class ConvertImageFormat
    {
        public static void ConvertFormat()
        {

            // Dictionary of most common image file types.
            Dictionary<int, ImageFormat> imageTypesDict = new Dictionary<int, ImageFormat>
            {
                {1, ImageFormat.Png},
                {2, ImageFormat.Jpeg},
                {3, ImageFormat.Tiff},
                {4, ImageFormat.Wmf},
                {5, ImageFormat.Emf},
                {6, ImageFormat.Webp},
                {7, ImageFormat.Gif},
                {8, ImageFormat.Bmp},
                {9, ImageFormat.Exif},
                {10, ImageFormat.Heif},
                {11, ImageFormat.Icon},
                {12, ImageFormat.MemoryBmp},
            };



            // Gets image loc to manipluate.
            ConsoleHelper.PrintConsoleBlock("Please input the file location of an image to change it's format:", true);
            string fileLoc = ImageHelper.VerifyUserPath(Console.ReadLine());
            while (fileLoc == null)
            {
                ConsoleHelper.PrintConsoleBlock("Invalid file path, please try again:", true);
                fileLoc = Console.ReadLine();
            }


            // Confirms current file type and prints options.
            string imageType = Path.GetExtension(fileLoc);
            ConsoleHelper.PrintConsoleBlock($"The Image Type you have used is '{imageType}' what would you like to convert it to?", true);
            foreach (KeyValuePair<int, ImageFormat> entry in imageTypesDict)
            {
                Console.WriteLine($"({entry.Key}) {entry.Value}");
            }


            // Gets users decision.
            ConsoleHelper.PrintConsoleBlock("Please enter a file format from 1 through 12:", true);
            string decision = Console.ReadLine();
            int decisionInt;
            while (!int.TryParse(decision, out decisionInt) || !imageTypesDict.Any(option => option.Key == decisionInt))
            {
                ConsoleHelper.PrintConsoleBlock("Please select a valid value from 1 through 12:", true);
                decision = Console.ReadLine();
            }


            // Saves to place where original image was as new image.
            Image img = Image.FromFile(fileLoc);
            img.Save($"{Path.GetDirectoryName(fileLoc)}\\NEW.{imageTypesDict[decisionInt]}", imageTypesDict[decisionInt]);
            ConsoleHelper.PrintConsoleBlock($"Image converted and save to: {Path.GetDirectoryName(fileLoc)}\\CONVERTED.{imageTypesDict[decisionInt]}.", false);
        }
    }
}
