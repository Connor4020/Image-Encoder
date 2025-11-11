using Barton___Y2_Project;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

class Program
{ 
    static void Main(string[] args)
    {



        string fileLoc;



        // --- FORMATTING ---
        // "IMAGE TOOLBOX".
        Console.Write(PrintConsoleBlock("  _____ __  __          _____ ______   _______ ____   ____  _      ____   ______   __\r\n |_   _|  \\/  |   /\\   / ____|  ____| |__   __/ __ \\ / __ \\| |    |  _ \\ / __ \\ \\ / /\r\n   | | | \\  / |  /  \\ | |  __| |__       | | | |  | | |  | | |    | |_) | |  | \\ V / \r\n   | | | |\\/| | / /\\ \\| | |_ |  __|      | | | |  | | |  | | |    |  _ <| |  | |> <  \r\n  _| |_| |  | |/ ____ \\ |__| | |____     | | | |__| | |__| | |____| |_) | |__| / . \\ \r\n |_____|_|  |_/_/    \\_\\_____|______|    |_|  \\____/ \\____/|______|____/ \\____/_/ \\_\\\r\n                                                                                     \r\n                                                                                     ", false));



        // Asks user what they want to do.
        // Repeats forever until valid input.
        // TODO: Add option to exit program.
        // TODO: Refactor into own method.
        // TODO: Clear console when necessary.
        // TODO: Handle null or whitespace.
        while (true)
        {
            Console.Write(PrintConsoleBlock("Would you like to read or write 1/2", true));
            string choice = Console.ReadLine().Trim();
            if (choice == "1")
            {
                break;
            }
            else if (choice == "2")
            {
                Reader.ReadHiddenMessage();
                return;
            }
            else
            {
                Console.Write(PrintConsoleBlock("--- INVALID: PLEASE SELECT A NUMBER ---", false));
            }
        }



        // Infinite loop to ask user for file loc.
        while (true)
        {
            Console.Write(PrintConsoleBlock("Please enter the file location of an image to read.", true));
            fileLoc = Console.ReadLine().Trim().Trim('"');
            if (!File.Exists(fileLoc))
            {
                Console.Write(PrintConsoleBlock("--- INVALID: FILE DOES NOT EXIST ---s", false));
            }
            else
            {
                break;
            }
        }



        // Defines vars to be used later.
        var bitmap = new Bitmap(fileLoc);
        int width = bitmap.Width;
        int height = bitmap.Height;
        int bytesPerPixel = Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;



        // C:\Users\proga\Desktop\24Depth.png
        // C:\Users\proga\Desktop\32Depth.png
        // "C:\Users\proga\Desktop\Modified_Lockbits.png



        // Displays imag details based on if image is 24 of 32 bit depth.
        if (bytesPerPixel == 3)
        {
            long maxLength = ((long)(width * height * 3) / 8);
            Console.Write(
                $"---IMAGE DETAILS---" +
                $"\n ==Width:      {width}px" +
                $"\n ==Height:     {height}px" +
                $"\n ==Bit Depth : 24" +
                $"\n---MAX MESSAGE LENGTH: {maxLength:N0} characters---\n");
        }
        else if (bytesPerPixel == 4)
        {
            long maxLength = ((long)width * height * 4);
            Console.Write(
                $"---IMAGE DETAILS---" +
                $"\n ==Width:      {width}" +
                $"\n ==Height:     {height}" +
                $"\n ==Bit Depth : 32" +
                $"\n---MAX MESSAGE LENGTH: {maxLength:N0} characters---\n");
        }
        else
        {
            Console.Write("---INVALID: BIT DEPTH UNSUPPORTED---");
        }



        // TODO: Handle special chars.
        // Gets inputted message.
        Console.Write(PrintConsoleBlock("-- Please input the message to hide within this image. --", true));
        string inputtedMessage = Console.ReadLine();
        string hiddenMessagePreDeclaration = ConvertStringToBinary(inputtedMessage);



        // Gets the length of the converted-to-binary message and stores that length as a 32-bit binary string.
        int messageLength = inputtedMessage.Length;
        Console.Write(messageLength);
        string hiddenMessageBinaryDeclaration = Convert.ToString(messageLength, 2).PadLeft(32, '0');
        Console.Write(hiddenMessageBinaryDeclaration);
        Console.Write(hiddenMessageBinaryDeclaration.Length);

        string hiddenMessage = hiddenMessageBinaryDeclaration + hiddenMessagePreDeclaration;



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


        // C:\Users\proga\Desktop\24Depth.png
        // Loops through each pixel and puts the leading bit as a 0.
        for (int i = 0; i < rgbValues.Length; i++)
        {
            byte blue = rgbValues[i];

            // Encode into Blue
            if (messageIndex < hiddenMessage.Length)
            {
                blue = (byte)((blue & ~1) | (hiddenMessage[messageIndex++] - '0'));

            }

            // Save modified bytes
            rgbValues[i] = blue;
        }


        Marshal.Copy(rgbValues, 0, intPtr, bytesAmount);

        bitmap.UnlockBits(bitmapData);

        string savePath = "C:\\Users\\proga\\Desktop\\Modified_Lockbits.png";
        Console.Write(savePath);
        Console.Write(fileLoc);
        bitmap.Save(savePath);

        Console.Write(hiddenMessage);

        Console.Write("Image successfully modified and saved to: " + savePath);
    }



    // Takes a binary string and returns it in ASCII.
    static string ConvertStringToBinary(string stringToConvert)
    {
        StringBuilder convertedString = new StringBuilder();
        foreach (char c in stringToConvert.ToCharArray())
        {
            convertedString.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
        }
        return convertedString.ToString();
    }



    // Takes message to print to console in a more appealing way.
    // If inputted message wants user input it formats as such.
    static string PrintConsoleBlock(string consoleOutput, bool hasUserInput)
    {
        StringBuilder consoleBlock = new StringBuilder();
        consoleBlock.Append("\n=========================================================================\n");
        consoleBlock.Append(consoleOutput);
        if (hasUserInput)
        {
            consoleBlock.Append("\n>>> ");
        }
        return consoleBlock.ToString();
    }
}

