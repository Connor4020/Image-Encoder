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
            ConsoleHelper.PrintConsoleBlock("Please input the file location of an image to decode:", true);
            string fileLoc = ImageHelper.VerifyUserPath(Console.ReadLine());
            while (fileLoc == null)
            {
                ConsoleHelper.PrintConsoleBlock("Invalid file path, please try again:", true);
                fileLoc = ImageHelper.VerifyUserPath(Console.ReadLine());
            }


            // Gets file loc of image to decode.
            ConsoleHelper.PrintConsoleBlock("Please input the password for this image (if applicable):", true);
            string password = Console.ReadLine();


            // Puts into method to decode image.
            // Returns null if file doesn't exist or password is incorrect.
            string decoded = DecodeHiddenmessage(fileLoc);
            if (decoded is null)
            {
                ConsoleHelper.PrintConsoleBlock("This image has not been encoded with a message", false);
                ConsoleHelper.ReturnToMenuPrompt();
                return;
            }


            // Displays hiddenn message then prompts user to go back to main menu.
            string decrypted = CryptoHelper.DecryptString(password, decoded);
            if (decrypted is null)
            {
                ConsoleHelper.PrintConsoleBlock("Incorrect password. View encrypted data anyway? ('Y') otherwise press enter", true);
                string viewEncryptedData = Console.ReadLine().ToLower();
                if (viewEncryptedData == "y")
                {
                    ConsoleHelper.PrintConsoleBlock($"Hidden encrypted message: " + $"\"{decoded}\".", false);
                }
            }
            else
            {
                ConsoleHelper.PrintConsoleBlock($"Hidden message decoded: " + $"\"{decrypted}\".", false);
            }
            ConsoleHelper.ReturnToMenuPrompt();
        }



        // Main function that accesses the image's info in memory.
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


                if (!HasStampOfApproval(pixelBytes))
                {
                    return null;
                }


                uint messageLengthAsChars = GetMessageCharLength(pixelBytes);
                uint messageBitCount = messageLengthAsChars * 8;
                return HiddenMessage.ConvertBinaryToString(GetEncryptedMessage(pixelBytes, messageBitCount));
            }
        }



        // ---- ALL HELPER FUNCTIONS BELOW HERE ----

        // Takes in the length of the message (and the bytes to read from) and decodes it (not decrypting yet).
        public static string GetEncryptedMessage(byte[] pixelBytes, uint messageBitsAmount)
        {
            StringBuilder messageBitsSB = new StringBuilder();
            for (int i = 64; i < 64 + messageBitsAmount; i++)
            {
                messageBitsSB.Append((pixelBytes[i] & 1) == 1 ? '1' : '0');
            }
            return messageBitsSB.ToString();
        }



        // Finds the length of the encoded message and returns it as a uint.
        public static uint GetMessageCharLength(byte[] pixelBytes)
        { 
            StringBuilder headerBits = new StringBuilder(32);
            for (int i = 32; i < 64; i++)
            {
                headerBits.Append((pixelBytes[i] & 1) == 1 ? '1' : '0');
            }
            return Convert.ToUInt32(headerBits.ToString(), 2);
        }



        // Read stamp bits.
        public static bool HasStampOfApproval(byte[] pixelBytes)
        {
            StringBuilder stampBits = new StringBuilder(32);
            for (int i = 0; i < 32; i++)
            {
                stampBits.Append((pixelBytes[i] & 1) == 1 ? '1' : '0');
            }
            string stampString = Convert.ToString(stampBits.ToString());
            if (stampString != "10001000100010001000100010001000")
            {
                return false;
            }
            return true;
        }
    }
}
