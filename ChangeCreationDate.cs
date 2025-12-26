using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Barton___Y2_Project
{
    internal class ChangeCreationDate
    {
        // Needs to be global for this class so all the functions can use it.
        private static string fileLoc;
        public static void AlterCreationDate()
        {
            // Asks for fileLoc.
            ConsoleHelper.PrintConsoleBlock("Please input the file location of an image to change it's creation date:", true);
            fileLoc = ImageHelper.VerifyUserPath(Console.ReadLine());
            while (fileLoc == null)
            {
                ConsoleHelper.PrintConsoleBlock("Invalid file path, please try again:", true);
                fileLoc = Console.ReadLine();
            }


            // If input is either 1 or 2 then convert 'decision' to int and move on.
            // If a decision isn't equal to 1 or 2 then keep asking them.
            ConsoleHelper.PrintConsoleBlock("Would you like to set a custom date or use the current date/time?\n(1) Custom Date \n(2) Current Date/Time", true);
            string decision = Console.ReadLine();
            while (decision != "1" || decision != "2")
            {
                ConsoleHelper.PrintConsoleBlock("Please enter either (1) or (2):", true);
                decision = Console.ReadLine();
            }


            if (decision == "1")
            {
                ConsoleHelper.PrintConsoleBlock("Please input your custom date and time in the format: DD/MM/YYYY HH:MM:SS\n(Please not that dates in the future won't work):", true);
                string customDT = Console.ReadLine();
                while (!DateTime.TryParse(customDT, out _))
                {
                    ConsoleHelper.PrintConsoleBlock("Invalid format, please try again:", true);
                    customDT = Console.ReadLine();
                }
                SetCustomDate(DateTime.Parse(customDT));
            }
            if (decision == "2")
            {
                SetCurrentDate(DateTime.Now);
            }
        }



        private static void SetCurrentDate(DateTime date)
        {
            File.SetCreationTime(fileLoc, DateTime.Now);
            ConsoleHelper.PrintConsoleBlock($"The file at {fileLoc} has had their date and time changed to {DateTime.Now}.", false);
            ConsoleHelper.ReturnToMenuPrompt();
        }



        private static void SetCustomDate(DateTime dateTime)
        {
            // Made this a sb in case I ever wanna add more stuff.
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Please input your custom date & time in the following format: DD/MM/YYYY HH:MM:SS.\n");
            sb.AppendLine("(Please note that dates in the future won't work).");

            // Formatting needed cos of sb.
            ConsoleHelper.PrintDivider();
            Console.Write(sb.ToString());


            ConsoleHelper.PrintConsoleBlock(sb.ToString(), true);
            string customDT = Console.ReadLine();
            while (!DateTime.TryParse(customDT, out _))
            {
                ConsoleHelper.PrintConsoleBlock("Invalid format, please try again:", true);
                customDT = Console.ReadLine();
            }
            File.SetCreationTime(fileLoc, DateTime.Parse(customDT));
            ConsoleHelper.PrintConsoleBlock($"The file at {fileLoc} has had their data and time changed to {customDT}.", false);
            ConsoleHelper.ReturnToMenuPrompt();
        }
    }
}
