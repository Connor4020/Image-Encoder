using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barton___Y2_Project
{
    internal class ImageMetadata
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int BitDepth { get; set; }
        public int MaxCharLength { get; set; }



        public ImageMetadata(int width, int height, int bitDepth, int maxCharLength)
        {
            Width = width;
            Height = height;
            BitDepth = bitDepth;
            MaxCharLength = maxCharLength;
        }
    }
}
