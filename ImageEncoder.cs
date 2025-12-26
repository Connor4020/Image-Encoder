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
            // TODO: Display image metadata before encoding.

            // TODO: change to while loop with break instead of recursion.
            if (ImageHelper.VerifyFileExists(fileLoc) == false)
            {
                ConsoleHelper.PrintConsoleBlock(" --- The file path you have entered does not exist. Please try again. \n--- ", false);
                GetMessageInfo();
            }
            ConsoleHelper.PrintConsoleBlock(" --- Please input the message to hide within this image. --- ", true);
            string inputtedMessage = Console.ReadLine();

            //TODO: Add herror handling.
            
            ConsoleHelper.PrintConsoleBlock(" --- Please input a password to protect your message with (leave blank for no password). --- ", true);
            string inputtedPassword = Console.ReadLine();

            // TODO: Ask if user wants this done.
            File.SetCreationTime(fileLoc, DateTime.Now);



            HiddenMessage hiddenmessage = new HiddenMessage(inputtedMessage);

            EncodeHiddenMessage(fileLoc, hiddenmessage);
            Console.Clear();

            ConsoleHelper.PrintConsoleBlock($"Image encoded to: {Path.GetDirectoryName(fileLoc)}\\COPY.png\n", false);
        }



        public static void EncodeHiddenMessage(string fileLocation, HiddenMessage message)
        {

            byte[] fileBytes = File.ReadAllBytes(fileLocation);
            using (var ms = new MemoryStream(fileBytes))
            using (var bitmap = new Bitmap(ms))
            {
                Rectangle dimensions = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                BitmapData bitmapData = bitmap.LockBits(dimensions, ImageLockMode.ReadWrite, bitmap.PixelFormat);

                IntPtr ptr = bitmapData.Scan0;
                int totalBytes = Math.Abs(bitmapData.Stride) * bitmap.Height;
                byte[] rgbValues = new byte[totalBytes];

                Marshal.Copy(ptr, rgbValues, 0, totalBytes);         
                
                for (int i = 0; i < message.FullEncodedMessage.Length; i++)
                {
                    byte oldByte = rgbValues[i];
                    byte newBit = (byte)(message.FullEncodedMessage[i] - '0');
                    rgbValues[i] = (byte)((oldByte & ~1) | newBit);
                }

                Marshal.Copy(rgbValues, 0, ptr, totalBytes);
                bitmap.UnlockBits(bitmapData);

                bitmap.Save(Path.GetDirectoryName(fileLocation) + "\\COPY.png" , ImageFormat.Png);
            }
        }
    }
}
