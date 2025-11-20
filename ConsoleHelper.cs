using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barton___Y2_Project
{
    internal class ConsoleHelper
    {

        // Formats messages printed to the console in a nicer way.
        public static void PrintConsoleBlock(string consoleOutput, bool hasUserInput)
        {
            StringBuilder consoleBlock = new StringBuilder();
            consoleBlock.Append("\n=========================================================================\n");
            consoleBlock.Append("\n" + consoleOutput);
            if (hasUserInput)
            {
                consoleBlock.Append("\n>>> ");
            }
            Console.Write(consoleBlock.ToString());
        }



        // Lists of available user options at the starts.
        static ToolOption[] userOptions = new ToolOption[]
        {
        new ToolOption("Read Hidden Message", 1, ImageDecoder.GetImageInfo),
        new ToolOption("Write Hidden Message", 2, ImageEncoder.GetMessageInfo), // TODO: Move Writer syntax to other file and call here.
        new ToolOption("Exit Program", 3, () => Environment.Exit(0))
        };



        // Prints user options and handles user input.
        public static void PrintUserChoices()
        {
            while (true)
            {
                PrintConsoleBlock("", false);
                foreach (var option in userOptions)
                {
                    Console.WriteLine($"--> {option.OptionNumber}. {option.ConsoleDescription}");
                }
                PrintConsoleBlock("Please select an option above:", true);
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    var selectedOption = userOptions.FirstOrDefault(option => option.OptionNumber == choice);
                    if (selectedOption != null)
                    {
                        selectedOption.Execute();
                        break;
                    }
                    else
                    {
                        Console.Clear();
                        PrintConsoleTitle();
                        PrintConsoleBlock(" --- INVALID: NUMBER OUT OF RANGE --- ", false);
                    }
                }
                else
                {
                    Console.Clear();
                    PrintConsoleTitle();
                    PrintConsoleBlock(" --- INVALID: PLEASE INPUT A NUMBER --- ", false);
                }
            }
        }



        // Initial method that presents list of options to the user.
        // Asks user what they want to do.
        // Repeats forever until valid input.
        // TODO: Handle null or whitespace.
        public static void PrintConsoleTitle()
        {
            PrintConsoleBlock("  _____ __  __          _____ ______   _______ ____   ____  _      ____   ______   __\r\n |_   _|  \\/  |   /\\   / ____|  ____| |__   __/ __ \\ / __ \\| |    |  _ \\ / __ \\ \\ / /\r\n   | | | \\  / |  /  \\ | |  __| |__       | | | |  | | |  | | |    | |_) | |  | \\ V / \r\n   | | | |\\/| | / /\\ \\| | |_ |  __|      | | | |  | | |  | | |    |  _ <| |  | |> <  \r\n  _| |_| |  | |/ ____ \\ |__| | |____     | | | |__| | |__| | |____| |_) | |__| / . \\ \r\n |_____|_|  |_/_/    \\_\\_____|______|    |_|  \\____/ \\____/|______|____/ \\____/_/ \\_\\                                                         ", false);
        }



        public static void DisplayImageDetails(string imagePath)
        {
            // ImageHelper.GetImageMetadata
        }
    }
}
