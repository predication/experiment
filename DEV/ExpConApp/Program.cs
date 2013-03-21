using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Predication.Experiment.Library;

namespace ExpConApp
{
    class Program
    {
       
        static void Main(string[] args)
        {
            IEnumerable<ExampleBase> examples = ExampleHelper.GetExamples();
            ExampleMenu menu = new ExampleMenu(examples);   
        }

        
    }
}
