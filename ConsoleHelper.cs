using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
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
            consoleBlock.Append("\n------------------------------------------------------------------------------------------\n");
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
        new ToolOption("Exit Program", 0, () => Environment.Exit(0)),
        new ToolOption("Read Hidden Message", 1, ImageDecoder.GetImageInfo),
        new ToolOption("Write Hidden Message", 2, ImageEncoder.GetMessageInfo),
        new ToolOption("View Image Metadata", 3, DisplayMetaData.DisplayImageMetadata),
        new ToolOption("Change Image Creation Date", 4, ChangeCreationDate.AlterCreationDate),
        new ToolOption("Convert Image Format", 5, ConvertImageFormat.ConvertFormat),
        };



        // When you select an option in the terminal, this will print those options again but with no functionality and higlights-
        // the option you chose.
        public static void PrintDummyChoices(int highlightedIndex)
        {
            PrintConsoleTitle();
            ConsoleHelper.PrintConsoleBlock("", false);
            foreach (var option in userOptions)
            {
                if (option.OptionNumber == highlightedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($">>> {option.OptionNumber}. {option.ConsoleDescription} <<<");
                    Console.ResetColor();
                } else
                {
                    Console.WriteLine($"--> {option.OptionNumber}. {option.ConsoleDescription}");
                }
            }
        }



        // Prints user options and gets user input
        public static void PrintUserChoices()
        {
            while (true)
            {
                PrintConsoleBlock("", false);
                foreach (var option in userOptions)
                {
                    Console.WriteLine($"--> {option.OptionNumber}. {option.ConsoleDescription}");
                }
                PrintConsoleBlock("--- Please select an option above: ---", true);
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    var selectedOption = userOptions.FirstOrDefault(option => option.OptionNumber == choice);
                    if (selectedOption != null)
                    {
                        Console.Clear();
                        PrintDummyChoices(choice);
                        // The first decision of the program will always start here.
                        selectedOption.Execute();
                        break;
                    }
                    else
                    {
                        Console.Clear();
                        PrintConsoleTitle();
                        PrintConsoleBlock(" --- INVALID: NUMBER OUT OF RANGE --- \n", false);
                    }
                }
                else
                {
                    Console.Clear();
                    PrintConsoleTitle();
                    PrintConsoleBlock(" --- INVALID: PLEASE INPUT A NUMBER --- \n", false);
                }
            }
        }



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
