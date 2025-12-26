using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barton___Y2_Project
{
    internal class ImageHelper
    {



        // May not need to be in it's own class but still here for expandability.
        public static bool VerifyFileExists(string inputtedPath)
        {
            return File.Exists(inputtedPath);   
        }

        public static string VerifyUserPath(string inputtedPath)
        {
            inputtedPath = inputtedPath.Trim().Trim('"').Trim('\'');
            if (File.Exists(inputtedPath))
            {
                return inputtedPath;
            }
            else
            {
                return null;
            }
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
    }
}
