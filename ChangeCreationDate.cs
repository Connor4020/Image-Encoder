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
        public static void AlterCreationDate()
        {

            ConsoleHelper.PrintConsoleBlock(" --- Please input the file location of an image to set it's creation date. --- ", true);
            string fileLoc = Console.ReadLine();
            fileLoc = fileLoc.Trim('"').Trim();
            // Checks if filepath exists.
            while (ImageHelper.VerifyFileExists(fileLoc) == false)
            {
                ConsoleHelper.PrintConsoleBlock(" --- Invalid file path, please try again. --- ", true);
            }
            
            ConsoleHelper.PrintConsoleBlock(" --- Would you like to set a custom date or use the current date/time? --- \n(1) Custom Date \n(2) Current Date/Time", true);
            int decision = Convert.ToInt32(Console.ReadLine());
            // Checks for valid decision.
            while (decision == null || decision > 3 || decision < 0)
            {
                ConsoleHelper.PrintConsoleBlock(" --- Invalid option, please select (1) or (2) ---", true);
                decision = Convert.ToInt32(Console.ReadLine());
            }

            // Asks for custome data and time.
            // Uses that for file D&T.
            // 20 / 12 / 2025 15:31:36
            if (decision == 1)
            {
                ConsoleHelper.PrintConsoleBlock(" --- Please input your custom date and time in the format: DD/MM/YYYY HH:MM:SS --- \n(Please not that dates in the future won't work).", true);
                string customDT = Console.ReadLine();
                while (!DateTime.TryParse(customDT, out _))
                {
                    ConsoleHelper.PrintConsoleBlock(" --- Invalid format, please try again: \n", true);
                    customDT = Console.ReadLine();
                }
                File.SetCreationTime(fileLoc, DateTime.Parse(customDT));
                ConsoleHelper.PrintConsoleBlock($" --- The file at {fileLoc} has had their data and time changed to {customDT}\n", false);
            }

            // Sets time and date to rn.
            if (decision == 2)
            {
                File.SetCreationTime(fileLoc, DateTime.Now);
                ConsoleHelper.PrintConsoleBlock($" --- The file at {fileLoc} has had their data and time changed to {DateTime.Now}\n", false);
            }

            ConsoleHelper.PrintConsoleBlock(" --- Please press enter to return to the menu: ", true);
            Console.ReadLine();
            Console.Clear();
        }
    }
}
