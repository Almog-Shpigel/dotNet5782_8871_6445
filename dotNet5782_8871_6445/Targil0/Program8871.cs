using System;

namespace Targil0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome8871();
            Welcome6445();
            Console.ReadKey();

        }
        static partial void Welcome6445();
        private static void Welcome8871()
        {
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", name);
        }
    }
}
