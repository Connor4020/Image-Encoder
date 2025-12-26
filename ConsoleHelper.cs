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
            consoleBlock.Append("\n" + " --- " + consoleOutput + " --- ");
            if (hasUserInput)
            {
                consoleBlock.Append("\n>>> ");
            }
            else
            {
                consoleBlock.Append('\n');
            }
                Console.Write(consoleBlock.ToString());
        }



        // When you select an option in the terminal, this will print those options again but with no functionality and higlights-
        // the option you chose.
        public static void PrintDummyChoices(int highlightedIndex, ToolOption[] userOptions)
        {
            PrintConsoleTitle();
            PrintDivider();
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
       



        // Allows the user to return to the menu when they press enter.
        // More user control IMO.
        public static void ReturnToMenuPrompt()
        {
            ConsoleHelper.PrintConsoleBlock("Please press enter to return to the menu.", true);
            Console.ReadLine();
            Console.Clear();
        }



        // Method to return bool based on whether value is an integer.
        public static bool isInt(string num)
        {
            if (!int.TryParse(num, out _))
            {
                return false;
            }
            else
            {
                return true;
            }
        }



        // Used rarely if there's certain formatting nuances the console block can't do as cleanly.
        public static void PrintDivider()
        {
            Console.WriteLine("\n------------------------------------------------------------------------------------------\n");
        }



        // Loops through each option in userOptions and prints the name and number of it.
        public static void PrintUserChoices(ToolOption[] userOptions)
        {
            PrintDivider();
            foreach (var option in userOptions)
            {
                Console.WriteLine($"--> {option.OptionNumber}. {option.ConsoleDescription}");
            }
        }



        public static void PrintConsoleTitle()
        {
            PrintDivider();
            Console.Write("  _____ __  __          _____ ______   _______ ____   ____  _      ____   ______   __\r\n |_   _|  \\/  |   /\\   / ____|  ____| |__   __/ __ \\ / __ \\| |    |  _ \\ / __ \\ \\ / /\r\n   | | | \\  / |  /  \\ | |  __| |__       | | | |  | | |  | | |    | |_) | |  | \\ V / \r\n   | | | |\\/| | / /\\ \\| | |_ |  __|      | | | |  | | |  | | |    |  _ <| |  | |> <  \r\n  _| |_| |  | |/ ____ \\ |__| | |____     | | | |__| | |__| | |____| |_) | |__| / . \\ \r\n |_____|_|  |_/_/    \\_\\_____|______|    |_|  \\____/ \\____/|______|____/ \\____/_/ \\_\\                                                         ");
        }
    }
}
