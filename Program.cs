using Barton___Y2_Project;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

class Program
{ 
    static void Main(string[] args)
    {

        Reader.ReadHiddenMessage();

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

        // "C:\Users\proga\Desktop\Modified_Lockbits.png"

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
        // Reads and converts inputted message to Binary.
        string inputtedMessage = Console.ReadLine();
        string hiddenMessagePreDeclaration = StringToBinary(inputtedMessage);

        // Gets the length of the converted-to-binary message and stores that length as a 32-bit binary string.
        int messageLength = inputtedMessage.Length;
        Console.WriteLine(messageLength);
        string hiddenMessageBinaryDeclaration = Convert.ToString(messageLength, 2).PadLeft(32, '0');
        Console.WriteLine(hiddenMessageBinaryDeclaration);
        Console.WriteLine(hiddenMessageBinaryDeclaration.Length);

        string hiddenMessage = hiddenMessageBinaryDeclaration + hiddenMessagePreDeclaration;

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
        for (int i = 0; i < rgbValues.Length && messageIndex < hiddenMessage.Length; i += bytesPerPixel)
        {
            byte blue = rgbValues[i];
            byte green = rgbValues[i + 1];
            byte red = rgbValues[i + 2];
            byte alpha = bytesPerPixel == 4 ? rgbValues[i + 3] : (byte)255;

            // Encode into Blue
            if (messageIndex < hiddenMessage.Length)
                blue = (byte)((blue & ~1) | (hiddenMessage[messageIndex++] - '0'));

            // Encode into Green
            if (messageIndex < hiddenMessage.Length)
                green = (byte)((green & ~1) | (hiddenMessage[messageIndex++] - '0'));

            // Encode into Red
            if (messageIndex < hiddenMessage.Length)
                red = (byte)((red & ~1) | (hiddenMessage[messageIndex++] - '0'));

            // Encode into Alpha (if present)
            if (bytesPerPixel == 4 && messageIndex < hiddenMessage.Length)
                alpha = (byte)((alpha & ~1) | (hiddenMessage[messageIndex++] - '0'));

            // Save modified bytes
            rgbValues[i] = blue;
            rgbValues[i + 1] = green;
            rgbValues[i + 2] = red;
            if (bytesPerPixel == 4)
                rgbValues[i + 3] = alpha;
        }


        Marshal.Copy(rgbValues, 0, intPtr, bytesAmount);

        bitmap.UnlockBits(bitmapData);

        string savePath = "C:\\Users\\proga\\Desktop\\Modified_Lockbits.png";
        bitmap.Save(savePath);

        Console.WriteLine(hiddenMessage);

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

