using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Barton___Y2_Project
{
    internal class ImageMetadata
    {
        public string FileLocation { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int BitDepth { get; set; }
        public int MaxCharLength { get; set; }
        public ImageFormat ImgFormat { get; set; }
        public DateTime DateCreated { get; set; }
        public ColorPalette Palette { get; set; }



        public ImageMetadata(string fileLocation)
        {
            FileLocation = fileLocation.Trim().Trim('"');
            Width = Image.FromFile(FileLocation).Width;
            Height = Image.FromFile(FileLocation).Height;

            var img = Image.FromFile(FileLocation);
            PixelFormat pf = img.PixelFormat;
            BitDepth = Image.GetPixelFormatSize(pf);

            MaxCharLength = (Width * Height * BitDepth) / 8;
            ImgFormat = Image.FromFile(FileLocation).RawFormat;
            DateCreated = File.GetCreationTime(FileLocation);
        }



        // Creates instance of ImageMetadata and displays all info to the user.
        public static void DisplayImageMetadata()
        {
            while (true)
            {
                ConsoleHelper.PrintConsoleBlock("Please input the file location of an image to view its metadata:", true);
                string fileLoc = Console.ReadLine().Trim().Trim('"');
                if (!ImageHelper.VerifyFileExists(fileLoc))
                {
                    ConsoleHelper.PrintConsoleBlock("The inputted image location does not exist, try again. \n", false);
                }
                else
                {
                    ImageMetadata imgMetaData = new ImageMetadata(fileLoc);
                    ConsoleHelper.PrintConsoleBlock("Image Meta Details: \n" +
                        "\nWidth:             " + imgMetaData.Width + "px" +
                        "\nHeight:            " + imgMetaData.Height + "px" +
                        "\nBitDepth:          " + imgMetaData.BitDepth + "bits per pixel" +
                        "\nMax Encoded Chars: " + String.Format($"{imgMetaData.MaxCharLength:N0}") +
                        "\nFormat:            " + imgMetaData.ImgFormat +
                        "\nDate Created:      " + imgMetaData.DateCreated + "\n"
                        , false);

                    ConsoleHelper.PrintConsoleBlock("Press any button to return to the starting menu.", true);
                    Console.ReadLine();
                    Console.Clear();
                    break;
                }
            }
        }
    }
}
