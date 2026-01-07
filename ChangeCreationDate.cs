using System;
using System.Collections.Generic;
using System.Globalization;
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
            fileLoc = ConsoleHelper.VerifyUserPath(Console.ReadLine());
            while (fileLoc == null)
            {
                ConsoleHelper.PrintConsoleBlock("Invalid file path, please try again:", true);
                fileLoc = ConsoleHelper.VerifyUserPath(Console.ReadLine());
            }


            // If input is either 1 or 2 then convert 'decision' to int and move on.
            // If a decision isn't equal to 1 or 2 then keep asking them.
            ConsoleHelper.PrintConsoleBlock("Would you like to set a custom date or use the current date/time?\n(1) Custom Date \n(2) Current Date/Time", true);
            string decision = Console.ReadLine();
            int decisionInt;
            while (!int.TryParse(decision, out decisionInt) || decisionInt > 2 || decisionInt < 1)
            {
                ConsoleHelper.PrintConsoleBlock("Please select either (1) or (2):", true);
                decision = Console.ReadLine();
            }



            if (decisionInt == 1)
            {
                ConsoleHelper.PrintConsoleBlock("Please input your custom date and time in the format: DD/MM/YYYY HH:MM:SS\n(Please not that dates in the future won't work):", true);
                string format = "dd/MM/yyyy HH:mm:ss";
                string customDT = Console.ReadLine();
                DateTime newDate;
                while (!DateTime.TryParseExact(customDT, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out newDate))
                {
                    ConsoleHelper.PrintConsoleBlock("Invalid format, ensure dashes for the date and colons for the time:", true);
                    customDT = Console.ReadLine();
                }
                File.SetCreationTime(fileLoc, newDate);
                ConsoleHelper.PrintConsoleBlock($"The file at {fileLoc} has had their data and time changed to {newDate}.", false);
            }
            if (decisionInt == 2)
            {
                File.SetCreationTime(fileLoc, DateTime.Now);
                ConsoleHelper.PrintConsoleBlock($"The file at {fileLoc} has had their date and time changed to {DateTime.Now}.", false);
            }
            ConsoleHelper.ReturnToMenuPrompt();
        }
    }
}
