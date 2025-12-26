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
            ConsoleHelper.PrintConsoleBlock("--- Please input the file location of an image to decode. ---", true);
            string fileLoc = Console.ReadLine();
            fileLoc = fileLoc.Trim().Trim('"');
            if (ImageHelper.VerifyFileExists(fileLoc) == false)
            {
                ConsoleHelper.PrintConsoleBlock(" --- The file path you have entered does not exist. Please try again. --- \n", false);
                GetImageInfo();
                return;
            }



            // Puts into method to decode image.
            // Returns null if file doesn't exist or password is incorrect.
            var decoded = Temp(fileLoc);
            if (decoded is null)
            {
                ConsoleHelper.PrintConsoleBlock(" --- Incorrect password or file does not exist ---\n ", false);
            }
            else
            {
                ConsoleHelper.PrintConsoleBlock($" --- Hidden message decoded: ", false);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(decoded + "\n");
            }
            Console.ResetColor();
            ConsoleHelper.PrintConsoleBlock(" --- Please press enter to return to the menu. --- ", true);
            Console.ReadLine();
            Console.Clear();
        }



        // Main function.
        public static string DecodeHiddenMessage(string fileLocation)
        {
            // Trims and check file exists.
            fileLocation.Trim().Trim('"');
            if (!ImageHelper.VerifyFileExists(fileLocation))
            {
                return null;
            }
            


            byte[] rgbValues;
            int totalBytes;
            // Creates new bitmap data to copy info from selected image.
            using (var bitmap = new Bitmap(fileLocation))
            {
                Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                PixelFormat pixelFormat = bitmap.PixelFormat;

                BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, pixelFormat);
                try
                {
                    IntPtr ptr = bitmapData.Scan0;
                    totalBytes = Math.Abs(bitmapData.Stride) * bitmap.Height;
                    rgbValues = new byte[totalBytes];
                    Marshal.Copy(ptr, rgbValues, 0, totalBytes);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }
            }



            // If there's no header saying how big the message is, return null (the image is too small in this case).
            if (totalBytes < 32)
            {
                return null;
            }



            // Create stringbuilder and append the 1s and 0s of the header bits to determing the message length.
            var headerBits = new StringBuilder(32);
            for (int i = 0; i < 32; i++)
            {
                headerBits.Append((rgbValues[i] & 1) == 1 ? '1' : '0');
            }
            Console.WriteLine(headerBits);



            // How many characters the hidden message is as an int.
            int messageLengthChars;
            try
            {
                messageLengthChars = Convert.ToInt32(headerBits.ToString(), 2);
                Console.WriteLine(messageLengthChars);
            }
            // If it doesn't work return null.
            catch
            {
                return null;
            }



            long messageBits = (long)messageLengthChars * 8L;
            long availableBits = totalBytes - 32L;
            if (messageBits <= 0 || messageBits > availableBits)
                return null;



            StringBuilder messageBitsBuilder = new StringBuilder((int)messageBits);
            for (long bitIndex = 0; bitIndex < messageBits; bitIndex++)
            {
                int byteIndex = 32 + (int)bitIndex;
                messageBitsBuilder.Append((rgbValues[byteIndex] & 1) == 1 ? '1' : '0');
            }



            string binaryMessage = messageBitsBuilder.ToString();
            string decoded;
            try
            {
                decoded = HiddenMessage.ConvertBinaryToString(binaryMessage);
                return decoded;
            }
            catch
            {
                return null;
            }


            // TODO: Handle encryption.
            while (true)
            //  C:\Users\proga\Desktop\24Depth.png
            {
                
            }
        }


        public static string Temp(string fileLocation)
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
                Console.WriteLine($"Message bit count: {messageBits.Length}");

                return HiddenMessage.ConvertBinaryToString(messageBits);
            }
        }

    }
}
