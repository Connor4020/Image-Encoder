using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barton___Y2_Project
{
    internal class ImageHelper
    {



        // May be unneaded but still here for expandability.
        public static bool VerifyFileExists(string inputtedPath)
        {
            inputtedPath.Trim().Trim('"');
            return File.Exists(inputtedPath);   
        }



        // Returns ImageMetadata object. 
        public static ImageMetadata GetImageMetadata(string inputtedPath)
        {
            using (var img = Image.FromFile(inputtedPath))
            {
                int width = img.Width;
                int height = img.Height;
                int bitDepth = Image.GetPixelFormatSize(img.PixelFormat);
                int maxCharLength = ((width * height * bitDepth) / 8) - 32;

                return new ImageMetadata(width, height, bitDepth, maxCharLength);
            }
        }
    }
}
