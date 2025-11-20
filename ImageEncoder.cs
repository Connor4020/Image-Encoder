using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Barton___Y2_Project
{
    internal class ImageEncoder
    {
        

        // Asks user for image location and message to hide. Passes into function underneath.
        public static void GetMessageInfo()
        {
            ConsoleHelper.PrintConsoleBlock(" --- Please input the file location of an image to encode. --- ", true);
            string fileLoc = Console.ReadLine();

            if (ImageHelper.VerifyFileExists(fileLoc) == false)
            {
                ConsoleHelper.PrintConsoleBlock(" --- The file path you have entered does not exist. Please try again. --- ", false);
                return;
            }

            ConsoleHelper.PrintConsoleBlock(" --- Please input the message to hide within this image. --- ", true);
            string inputtedMessage = Console.ReadLine();

            ImageMetadata imgData = ImageHelper.GetImageMetadata(fileLoc);
            HiddenMessage hiddenmessage = new HiddenMessage(inputtedMessage);

            EncodeHiddenMessage(fileLoc, hiddenmessage);
        }



        public static void EncodeHiddenMessage(string fileLocation, HiddenMessage message)
        {
            using (var bitmap = new Bitmap(fileLocation))
            {
                Rectangle dimensions = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                BitmapData bitmapData = bitmap.LockBits(dimensions, ImageLockMode.ReadWrite, bitmap.PixelFormat);
                IntPtr intPtr = bitmapData.Scan0;
                int totalBytes = Math.Abs(bitmapData.Stride) * bitmap.Height;
                byte[] rgbValues = new byte[totalBytes];
                Marshal.Copy(intPtr, rgbValues, 0, totalBytes);
                for (int i = 0; i < message.FullEncodedMessage.Length; i++)
                {
                    byte colorByte = rgbValues[i];
                    colorByte = (byte)((colorByte & ~1) | (message.FullEncodedMessage[i] - '0'));
                    rgbValues[i] = colorByte;
                }
            }
        }
    }
}
