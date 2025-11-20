using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace Barton___Y2_Project
{
    internal class ImageDecoder
    {


        public static void GetImageInfo()
        {
            ConsoleHelper.PrintConsoleBlock(" --- Please input the file location of an image to decode. --- ", true);
            string fileLoc = Console.ReadLine();

            if (ImageHelper.VerifyFileExists(fileLoc) == false)
            {
                ConsoleHelper.PrintConsoleBlock(" --- The file path you have entered does not exist. Please try again. --- ", false);
                return;
            }

            var decoded = DecodeHiddenMessage(fileLoc);
            if (decoded is null)
            {
                ConsoleHelper.PrintConsoleBlock(" --- No valid hidden message could be decoded from that image. --- ", false);
            }
            else
            {
                ConsoleHelper.PrintConsoleBlock(" --- Hidden message decoded: --- ", false);
                Console.WriteLine(decoded);
            }
        }

        // Reads LSBs from the image bytes to reconstruct the encoded message.
        // Returns the decoded string or null when decoding fails.
        public static string? DecodeHiddenMessage(string fileLocation)
        {
            if (!ImageHelper.VerifyFileExists(fileLocation))
                return null;

            byte[] rgbValues;
            int totalBytes;

            // Read underlying bytes from the image
            using (var bitmap = new Bitmap(fileLocation))
            {
                var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                var pixelFormat = bitmap.PixelFormat;

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

            // Need at least 32 bits for header
            if (totalBytes < 32)
                return null;

            // Extract 32 LSBs for the header (message length in characters)
            var headerBits = new StringBuilder(32);
            for (int i = 0; i < 32; i++)
            {
                headerBits.Append((rgbValues[i] & 1) == 1 ? '1' : '0');
            }

            // Convert header to integer (number of characters)
            if (!int.TryParse(headerBits.ToString(), System.Globalization.NumberStyles.None, null, out _))
            {
                // fallback to binary conversion
            }
            int messageLengthChars;
            try
            {
                messageLengthChars = Convert.ToInt32(headerBits.ToString(), 2);
            }
            catch
            {
                return null;
            }

            // Total bits to read for the message
            long messageBits = (long)messageLengthChars * 8L;

            // Sanity check: available bits after header
            long availableBits = totalBytes - 32L;
            if (messageBits <= 0 || messageBits > availableBits)
                return null;

            // Extract messageBits from subsequent LSBs
            var messageBitsBuilder = new StringBuilder((int)messageBits);
            for (long bitIndex = 0; bitIndex < messageBits; bitIndex++)
            {
                int byteIndex = 32 + (int)bitIndex;
                messageBitsBuilder.Append((rgbValues[byteIndex] & 1) == 1 ? '1' : '0');
            }

            // Convert binary -> ASCII string using existing helper
            string binaryMessage = messageBitsBuilder.ToString();
            string decoded;
            try
            {
                decoded = HiddenMessage.ConvertBinaryToString(binaryMessage);
            }
            catch
            {
                return null;
            }

            return decoded;
        }
    }
}