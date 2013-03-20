using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predication.Experiment.Library
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class DemoAttribute : Attribute
    {
        readonly string title;
        readonly string description;
        readonly ConsoleColor foreground;
        readonly ConsoleColor background;

        public DemoAttribute(){}

        public DemoAttribute(string title, string description, ConsoleColor foreground = ConsoleColor.White, ConsoleColor background = ConsoleColor.Gray)
            :this()
        {
            this.title = title;
            this.description = description;
            this.foreground = foreground;
            this.background = background;
        }

        public string Title { get { return title; } }
        public string Description { get { return description; } }
        public ConsoleColor Background { get { return background; } }
        public ConsoleColor Foreground { get { return foreground; } }

       
    }

}
