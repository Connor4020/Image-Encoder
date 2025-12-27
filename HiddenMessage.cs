using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Barton___Y2_Project
{
    internal class HiddenMessage
    {
        public string Message { get; set; }
        public int Length { get; set; }
        public string BinaryMessage { get; set; }
        public string BinaryHeader { get; set; }
        public string FullEncodedMessage { get; set; }



        public HiddenMessage (string message)
        {
            Message = message;
            Length = message.Length;
            BinaryMessage = ConvertStringToBinary(message);
            BinaryHeader = SetBinaryDeclaration(Length);
            FullEncodedMessage = BinaryHeader + BinaryMessage;
        }



        // Converts inputted string to binary and returns it.
        public static string ConvertStringToBinary(string stringToConvert)
        {
            StringBuilder convertedString = new StringBuilder();
            foreach (char c in stringToConvert.ToCharArray())
            {
                convertedString.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
            }
            return convertedString.ToString();
        }



        public static string ConvertBinaryToString(string binaryToConvert)
        {
            StringBuilder text = new StringBuilder();

            for (int i = 0; i < binaryToConvert.Length; i += 8)
            {
                string byteString = binaryToConvert.Substring(i, 8);
                byte letter = Convert.ToByte(byteString, 2);
                text.Append((char)letter);
            }
            return text.ToString();
        }



        // Returns 32 bit binary number set to the length of the message.
        public static string SetBinaryDeclaration(int messageLength)
        {
            return Convert.ToString(messageLength, 2).PadLeft(32, '0');
        }
    }
}
