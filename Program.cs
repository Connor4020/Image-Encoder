using Barton___Y2_Project;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

class Program
{ 
    static void Main(string[] args)
    {

        string inputBeautify = "\n>>> ";
        string lineBreaker = "\n=========================================================================\n";
        string fileLoc;

        while (true)
        {
            Console.Write("-- Please input the file location of an image to encode. --" + inputBeautify);
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
        
        var bitmap = new Bitmap(fileLoc);

        int width = bitmap.Width;
        int height = bitmap.Height;
        int bytesPerPixel = Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;

        // C:\Users\proga\Desktop\24Depth.png
        // C:\Users\proga\Desktop\32Depth.png

        Console.WriteLine(lineBreaker);

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

        Console.WriteLine(lineBreaker);

        Console.Write("-- Please input the message to hide within this image. --" + inputBeautify);
        string inputtedMessage = Console.ReadLine();
        int messageLength = inputtedMessage.Length;
        string binary = Convert.ToString(messageLength, 2).PadLeft(32, '0');
        string hiddenMessage = StringToBinary(inputtedMessage);
        

        Console.WriteLine(lineBreaker);

        Rectangle dimensions = new Rectangle(0, 0, width, height);

        BitmapData bitmapData = bitmap.LockBits(dimensions, ImageLockMode.ReadWrite, bitmap.PixelFormat);

        // Gets the first point in memory where the image is stored and saves as a var.
        IntPtr intPtr = bitmapData.Scan0;

        // Gets total number of bytes in an image including padding.
        // Gets absoulte cos sometimes parts of the image can be stored in a "negative" space in memory.
        int bytesAmount = Math.Abs(bitmapData.Stride) * bitmap.Height;

        // Byte list which is made a size of the amount of bytes defined above.
        byte[] rgbValues = new byte[bytesAmount];

        // Copies the image data from memory to rgbValues using variables defined earlier.
        Marshal.Copy(intPtr, rgbValues, 0, bytesAmount);

        int messageIndex = 0;



        // Loops through each pixel and puts the leading bit as a 0.
        for (int i = 0; i < rgbValues.Length; i += bytesPerPixel)
        {
            byte blue = rgbValues[i];
            byte green = rgbValues[i + 1];
            byte red = rgbValues[i + 2];
            byte alpha = rgbValues[i + 3];
            // TODO: Handle if their is alpha channel when modifying bits.

            byte newBlue;

            if (hiddenMessage[messageIndex] == '1')
            {
                newBlue = (byte)(blue | 1);
            }
            else
            {
                newBlue = (byte)(blue & ~1);
            }

            messageIndex++;
            rgbValues[i] = newBlue;

            if (messageIndex >= hiddenMessage.Length)
            {
                break;
            }
        }

        Marshal.Copy(rgbValues, 0, intPtr, bytesAmount);

        bitmap.UnlockBits(bitmapData);

        string savePath = "C:\\Users\\proga\\Desktop\\Modified_Lockbits.png";
        bitmap.Save(savePath);

        Console.WriteLine("Image successfully modified and saved to: " + savePath);
    }

    static string StringToBinary(string input)
    {
        StringBuilder sb = new StringBuilder();
        foreach (char c in input.ToCharArray())
        {
            sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
        }
        return sb.ToString();
    }
}

