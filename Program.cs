using System;
using TimerControl;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {


        static void Main(string[] args)
        {
            TimerX.Start(() =>
            {
                Console.WriteLine($"输出任务{DateTime.Now}");

                return false;

            },1000);
            Console.ReadKey(); 
        }
    }
}