using Barton___Y2_Project;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;

class Program
{

    // Lists of available user options at the starts.
    static ToolOption[] userOptions = new ToolOption[]
    {
        new ToolOption("Exit Program", 0, () => Environment.Exit(0)),
        new ToolOption("Read Hidden Message", 1, ImageDecoder.GetImageInfo),
        new ToolOption("Write Hidden Message", 2, ImageEncoder.GetMessageInfo),
        new ToolOption("View Image Metadata", 3, ImageMetadata.ToolOptionPrintMetadata),
        new ToolOption("Change Image Creation Date", 4, ChangeCreationDate.AlterCreationDate),
        new ToolOption("Convert Image Format", 5, ConvertImageFormat.ConvertFormat),
    };



    static void Main(string[] args)
    {
        // First functions that are run.
        // Loops indefintetly until option '0' in the program is selected.
        while (true)
        {
            Console.WriteLine("1");
            ConsoleHelper.PrintConsoleTitle();
            ConsoleHelper.PrintUserChoices(userOptions);
            int userChoice = AskForUserDecision();
            ExecuteUserDecision(userChoice);
            Console.WriteLine("3");
        }
        // Files on my desktop for quick access during testing:
        // C:\Users\proga\Desktop\24Depth.png
        // C:\Users\proga\Desktop\32Depth.png
        // "C:\Users\proga\Desktop\COPY.png"
    }



    // Just executes function inside userOption object.
    private static void ExecuteUserDecision(int optionNum)
    {
        Console.WriteLine("3");
        Console.Clear();
        ConsoleHelper.PrintDummyChoices(optionNum, userOptions);
        userOptions[optionNum].Execute();
    }



    // Returns integer based on what option user wants to choose.
    private static int AskForUserDecision()
    {
        ConsoleHelper.PrintConsoleBlock("Please selection an option above:", true);
        string userDecision = Console.ReadLine();
        int choice;


        // Checks input is an int.
        // Also checks if input matches with any optionNumbers in userOptions above.
        while (!int.TryParse(userDecision, out choice) || !userOptions.Any(option => option.OptionNumber == choice))
        {
            ConsoleHelper.PrintConsoleBlock("Invalid option, please input a number from one of the above:", true);
            userDecision = Console.ReadLine();
        }
        return choice;
    }
}

