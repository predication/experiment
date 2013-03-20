using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Predication.Experiment.Library;

namespace ExpConApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(".NET 4.5 Experiments");

            foreach (ExampleBase example in ExampleHelper.GetExamples())
            {
                Console.WriteLine("\n"); 
                Console.ForegroundColor = example.Foreground;
                Console.BackgroundColor = example.Background;
                Console.WriteLine(example.Title);
                Console.ResetColor();
                Console.WriteLine("  " + example.Description);
                Console.WriteLine("\n");
                example.Execute();
            }

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\n\nPress ENTER to Quit.");
            Console.ResetColor();
            Console.ReadLine();
        }
    }
}
