using IBL;
using IDAL.DO;
using System;
namespace ConsoleUI_BL
{
    class Program
    {
        static void Main(string[] args)
        {
            IBL.BL IBL = new();
            int choice;
            do
            {
                Console.WriteLine("Press anykey to start the program...");
                Console.ReadKey();
                Console.Clear();
                choice = Menus.Main(IBL);                                /// Receives a choice from the user.
            } while (choice != 0);
        }
    }
}
