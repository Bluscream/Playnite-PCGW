using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bluscream
{
    internal class Utils
    {
        public class Console
        {
            public static bool Confirm(string title)
            {
                ConsoleKey response;
                do
                {
                    System.Console.Write($"{ title } [y/n] ");
                    response = System.Console.ReadKey(false).Key;
                    if (response != ConsoleKey.Enter)
                    {
                        System.Console.WriteLine();
                    }
                } while (response != ConsoleKey.Y && response != ConsoleKey.N);

                return (response == ConsoleKey.Y);
            }
        }
    }
}