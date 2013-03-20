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
            
            LazyExample example = new LazyExample();
            example.Demonstrate();

            Console.WriteLine("\n\nPress ENTER to Quit.");
            Console.ReadLine();
        }
    }
}
