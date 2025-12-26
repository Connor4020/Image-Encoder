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
            inputtedPath = inputtedPath.Trim().Trim('"');
            return File.Exists(inputtedPath);   
        }
    }
}
