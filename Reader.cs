using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Barton___Y2_Project
{
    internal class Reader
    {
        public static void ReadHiddenMessage()
        {
            string inputBeautify = "\n>>> ";
            string lineBreaker = "\n=========================================================================\n";
            string fileLoc;

            while (true)
            {
                Console.Write("-- Please input the file location of an image to read. --" + inputBeautify);
                fileLoc = Console.ReadLine().Trim().Trim('"');
                if (!File.Exists(fileLoc))
                {
                    Console.WriteLine(lineBreaker);
                    Console.WriteLine("---INVALID: FILE DOES NOT EXIST---");
                    Console.WriteLine(lineBreaker);
                }
                else
                {
                    break;
                }
            }

            Bitmap bitmap = new Bitmap(fileLoc);
            int width = bitmap.Width;
            int height = bitmap.Height;
            int bytesPerPixel = Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;

            if (bytesPerPixel == 3)
            {
                long maxLength = ((long)width * height * 3);
                Console.WriteLine(
                    $"---IMAGE DETAILS---" +
                    $"\n ==Width:      {width}px" +
                    $"\n ==Height:     {height}px" +
                    $"\n ==Bit Depth : 24" +
                    $"\n---MAX MESSAGE LENGTH: {maxLength:N0} characters---");
            }
            else if (bytesPerPixel == 4)
            {
                long maxLength = ((long)width * height * 4);
                Console.WriteLine(
                    $"---IMAGE DETAILS---" +
                    $"\n ==Width:      {width}" +
                    $"\n ==Height:     {height}" +
                    $"\n ==Bit Depth : 32" +
                    $"\n---MAX MESSAGE LENGTH: {maxLength:N0} characters---");
            }
            else
            {
                Console.WriteLine("---INVALID: BIT DEPTH UNSUPPORTED---");
            }

            Rectangle dimensions = new Rectangle(0, 0, width, height);
            BitmapData bmd = bitmap.LockBits(dimensions, ImageLockMode.ReadOnly, bitmap.PixelFormat);
            IntPtr intPtr = bmd.Scan0;

            int amountOfBytes = Math.Abs(bmd.Stride) * bitmap.Height;
            byte[] rgbValues = new byte[amountOfBytes];
            Marshal.Copy(intPtr, rgbValues, 0, amountOfBytes);
        }
    }
}
