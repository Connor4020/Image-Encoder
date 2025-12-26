using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;

namespace Barton___Y2_Project
{
    internal class ImageDecoder
    {
        public static void GetImageInfo()
        {
            // Gets file loc of image to decode.
            ConsoleHelper.PrintConsoleBlock("Please input the file location of an image to decode.", true);
            string fileLoc = ImageHelper.VerifyUserPath(Console.ReadLine());
            while (fileLoc == null)
            {
                ConsoleHelper.PrintConsoleBlock("Invalid file path, please try again:", true);
                fileLoc = Console.ReadLine();
            }


            // Puts into method to decode image.
            // Returns null if file doesn't exist or password is incorrect.
            var decoded = DecodeHiddenmessage(fileLoc);
            while (decoded is null)
            {
                ConsoleHelper.PrintConsoleBlock(" --- Incorrect password or file does not exist ---\n ", false);
                decoded = DecodeHiddenmessage(Console.ReadLine());
            }
            // Displays hiddenn message then prompts user to go back to main menu.
            ConsoleHelper.PrintConsoleBlock($"Hidden message decoded: " + $"\"{decoded}\".", false);
            ConsoleHelper.ReturnToMenuPrompt();
        }



        // TODO: Implement dcryption after encryption.
        // Main function that access the image's info in memory.
        public static string DecodeHiddenmessage(string fileLocation)
        {
            byte[] fileBytes = File.ReadAllBytes(fileLocation);

            using (var ms = new MemoryStream(fileBytes))
            using (var bitmap = new Bitmap(ms))
            {
                Rectangle dimensions = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                BitmapData bitmapData = bitmap.LockBits(dimensions, ImageLockMode.ReadOnly, bitmap.PixelFormat);

                IntPtr ptr = bitmapData.Scan0;
                int totalBytes = Math.Abs(bitmapData.Stride) * bitmap.Height;

                byte[] pixelBytes = new byte[totalBytes];
                Marshal.Copy(ptr, pixelBytes, 0, totalBytes);
                bitmap.UnlockBits(bitmapData);

                // Read header bits.
                StringBuilder headerBits = new StringBuilder(32);
                for (int i = 0; i < 32; i++)
                {
                    headerBits.Append((pixelBytes[i] & 1) == 1 ? '1' : '0');
                }
                int headerBytes = Convert.ToInt32(headerBits.ToString(), 2);
                // -- - --- - - - -



                // Reader message bits.
                int messageBitCount = headerBytes * 8;

                StringBuilder messageBitsSB = new StringBuilder(messageBitCount);
                for (int i = 32; i < 32 + messageBitCount; i++)
                {
                    messageBitsSB.Append((pixelBytes[i] & 1) == 1 ? '1' : '0');
                }
                string messageBits = messageBitsSB.ToString();
                return HiddenMessage.ConvertBinaryToString(messageBits);
                // ---- --- - - -- - - 
            }
        }
    }
}
