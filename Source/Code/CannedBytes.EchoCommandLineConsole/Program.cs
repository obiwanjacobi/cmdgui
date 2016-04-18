using System;

namespace CannedBytes.EchoCommandLineConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                foreach (var arg in args)
                {
                    Console.WriteLine(arg);
                }
            }
            else
            {
                Console.WriteLine("No Command Line Arguments!");
            }
        }
    }
}