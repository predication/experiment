using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predication.Experiment.Library
{
    public abstract class ExampleBase
    {
        public virtual void Execute()
        {

        }

        public string Title { get; set; }
        public string Description { get; set; }
        public ConsoleColor Background { get; set; }
        public ConsoleColor Foreground { get; set; }
    }
}
