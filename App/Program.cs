using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IranSystemConvertor;
using System.Data.OleDb;
using System.Data;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            string output = ConvertTo.UnicodeFrom(TextEncoding.Arabic1256, " ِگ¤ْ— ë123¯‘÷ُ");
            Console.WriteLine(output);
        }
    }
}
