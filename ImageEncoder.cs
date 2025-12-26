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
        public static void GetMessageInfo()
        {
            // Asks for location of image to encode.
            ConsoleHelper.PrintConsoleBlock("Please input the file location of an image to encode.", true);
            string fileLoc = ImageHelper.VerifyUserPath(Console.ReadLine());
            while (fileLoc == null)
            {
                ConsoleHelper.PrintConsoleBlock("Invalid file path, please try again:", true);
                fileLoc = Console.ReadLine();
            }
            

            // Displays metadata before asking other questions.
            // This is so they know how much info they can store.
            ImageMetadata.DisplayImageMetadata(fileLoc);


            // Asks for message to encode.
            ConsoleHelper.PrintConsoleBlock("Please input the message to hide within this image:", true);
            string inputtedMessage = Console.ReadLine();
            while (!String.IsNullOrWhiteSpace(inputtedMessage))
            {
                ConsoleHelper.PrintConsoleBlock("Invalid. The message must be at least one character long.", true);
                inputtedMessage = Console.ReadLine();
            }

            
            // Asks for password.
            // Doesn't need error handling cos null values are accepted and whitespace is allowed in the password.
            ConsoleHelper.PrintConsoleBlock("Please input a password to protect your message with (leave blank for no password):", true);
            string inputtedPassword = Console.ReadLine();


            // Creates new instance of class using req field of 'message'.
            // It passes this into the main function. This is done cos the message class has stuff like 'FullEncodedMessage' to make stuff a lil easier. (Also just better reusability).
            HiddenMessage hiddenmessage = new HiddenMessage(inputtedMessage);
            bool succession = EncodeHiddenMessage(fileLoc, hiddenmessage);
            if (succession)
            {
                ConsoleHelper.PrintConsoleBlock($"Image encoded to: {Path.GetDirectoryName(fileLoc)}\\COPY.png.", false);
            }
            else
            {
                ConsoleHelper.PrintConsoleBlock("There may have been an issue encoding your message into the image.", false);
            }
            ConsoleHelper.ReturnToMenuPrompt();
        }



        // Main function.
        public static bool EncodeHiddenMessage(string fileLocation, HiddenMessage message)
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

                // Error handling just in case anything goes wrong.
                try
                {
                    bitmap.Save(Path.GetDirectoryName(fileLocation) + "\\COPY.png", ImageFormat.Png);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
