using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barton___Y2_Project
{
    internal class ToolOption
    {


        public string ConsoleDescription { get; set; }
        public int OptionNumber { get; set; }
        public Action OptionAction;



        public ToolOption(string consoleDescription, int optionNumber, Action optionAction)
        {
            ConsoleDescription = consoleDescription;
            OptionNumber = optionNumber;
            OptionAction = optionAction;
        }



        public void Execute()
        {
            OptionAction?.Invoke();
        }
    }
}
