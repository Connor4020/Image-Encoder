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



        // Defines variables.
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



        // TODO: FUncion here to handle asking fileLoc to display metadata.



        // Creates instance of ImageMetadata and displays all info to the user.
        public static void DisplayImageMetadata(string fileLoc)
        {
            var meta = new ImageMetadata(fileLoc);
            var sb = new StringBuilder();
            sb.AppendLine("Image metadata:");
            sb.AppendLine();
            sb.AppendLine($"{"Width",-18}: {meta.Width:N0}px");
            sb.AppendLine($"{"Height",-18}: {meta.Height:N0}px");
            sb.AppendLine($"{"Bit depth",-18}: {meta.BitDepth} bpp");
            sb.AppendLine($"{"Max encoded chars",-18}: {meta.MaxCharLength:N0}");
            sb.AppendLine($"{"Format",-18}: {meta.ImgFormat}");
            sb.AppendLine($"{"Date created",-18}: {meta.DateCreated:yyyy-MM-dd HH:mm:ss}");

            ConsoleHelper.PrintConsoleBlock(sb.ToString(), false);
        }
    }
}
