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



            // --- FORMATTING ---
            string inputBeautify = "\n>>> ";
            string lineBreaker = "\n=========================================================================\n";
            string fileLoc;



            // Prompts user as to retrieve file loc to read.
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



            // Creates new bitmap.
            // Gets basic img details.
            Bitmap bitmap = new Bitmap(fileLoc);
            int width = bitmap.Width;
            int height = bitmap.Height;
            int bytesPerPixel = Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;



            // Displays image details.
            // TODO: Make the details correspond with reading.
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



            // Defines vars to be used later.
            Rectangle dimensions = new Rectangle(0, 0, width, height);
            BitmapData bmd = bitmap.LockBits(dimensions, ImageLockMode.ReadOnly, bitmap.PixelFormat);
            IntPtr intPtr = bmd.Scan0;
            int amountOfBytes = Math.Abs(bmd.Stride) * bitmap.Height;



            // Copies the imaged's rgb values to byte list 'rgbValues'.
            byte[] rgbValues = new byte[amountOfBytes];
            Marshal.Copy(intPtr, rgbValues, 0, amountOfBytes);



            // --- FORMATTING ---
            Console.WriteLine(lineBreaker);



            string messageLengthAsBinary = "";

            for (int i = 0; i <= 31; i++)
            {
                byte blueByte = rgbValues[i];
                string blue = Convert.ToString(blueByte, 2);
                blue = blue.Length > 0 ? blue[^1].ToString() : string.Empty;
                messageLengthAsBinary += blue;
            }



            // Gets the message length as an amount of bits.
            int messageLength = Convert.ToInt32(messageLengthAsBinary, 2);
            messageLength = messageLength * 8;



            // New byte list set to the size of the message length.
            string binaryMessage = "";



            // "C:\Users\proga\Desktop\Modified_Lockbits.png"
            for (int i = 32; i < 32 + messageLength; i++)
            {
                byte currentByte = rgbValues[i];
                string currentBinary = Convert.ToString(currentByte, 2);
                string lsb = currentBinary[^1].ToString();
                binaryMessage += lsb;
            }

            Console.WriteLine(BinaryToAscii(binaryMessage));

            /*
            foreach (string b in binaryMessageArray)
            {
                Console.WriteLine(b);
            }
            */

            static string BinaryToAscii(string binary)
            {
                StringBuilder text = new StringBuilder();

                for (int i = 0; i < binary.Length; i += 8)
                {
                    string byteString = binary.Substring(i, 8);
                    byte b = Convert.ToByte(byteString, 2);
                    text.Append((char)b);
                }
                return text.ToString();
            }
        }
    }
}
