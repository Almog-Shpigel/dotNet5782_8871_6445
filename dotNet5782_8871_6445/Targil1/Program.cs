using IDAL.DO;
using System;

namespace ConsoleUI
{
    class Program
    {
        //DateTime currentDate = DateTime.Now;
        static void Main(string[] args)
        {
            //IDAL.DO.Station baseStation = new IDAL.DO.Station();
            //Console.WriteLine("Hello World!");
            string temp;
            int choice;
            Console.WriteLine("Please enter your choice here: ");
            temp = Console.ReadLine();
            choice = Convert.ToInt32(temp);
            do
            {
                switch (choice)
                {
                    case (int)CHOICE.ADD:
                        break;
                    case (int)CHOICE.UPDATE:
                        break;
                    case (int)CHOICE.DISPLAY:
                        break;
                    case (int)CHOICE.DATA_PRINT:
                        break;
                    case (int)CHOICE.EXIT:
                        break;
                    default:
                        break;
                }
            } while (choice != 0);

        }
    }
}
