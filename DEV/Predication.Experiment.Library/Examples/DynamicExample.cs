using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using Predication.Experiment.VBLibrary;

namespace Predication.Experiment.Library
{
    [Demo("Dynamic", "An example of using the late-bound Dynamic programming", ConsoleColor.Black, ConsoleColor.White)]
    class DynamicExample : ExampleBase
    {
        public override void Execute()
        {
            Console.WriteLine("dynamic item = new ExpandoObject();");
            dynamic item = new ExpandoObject();

            // we can add properties ad-hoc
            Console.WriteLine("item.Name = \"Billy\";");
            item.Name = "Billy";

            Console.WriteLine("item.DoB = new DateTime(1999, 02, 22);");
            item.DoB = new DateTime(1999, 02, 22);

            // and add methods
            item.DoBIncrement = (Action)(() => { item.DoB = item.DoB.AddYears(+1); });
            
            // and call them
            item.DoBIncrement();

            Console.WriteLine("item.DoB.ToString()");
            Console.WriteLine(item.DoB.ToString());

            Console.WriteLine("item.Name");
            Console.WriteLine(item.Name);

            // we can also change the properties radically
            Console.WriteLine("item.Name = 1.2;");
            item.Name = 1.2;
            Console.WriteLine("item.Name.GetType().ToString()");
            Console.WriteLine(item.Name.GetType().ToString());

            // we can pass it as a parameter (as long as the dynamic keyword is used)
            ShowName(item);

            // we can also add events (but we must first initialise them with null!)
            item.SomethingHappened = null;
            item.SomethingHappened += new EventHandler<NameEventArgs>(NowWeNowThatSomethingHappened);
            // also implement the handler in a fairly normal way
            item.OnSomethingHappened =  (Action)(() => 
                {
                    if (item.SomethingHappened != null)
                    {
                        item.SomethingHappened(item, new NameEventArgs(item.Name));
                    }
                });

            // and hook up to them externally...
            item.Name = "Billy Bunter";
            item.OnSomethingHappened();

            // we can also list the properties
            Console.WriteLine("Properties, Methods, etc... are stored as a IDictionary<String,Object> :");
            foreach (var property in (IDictionary<String, Object>)item)
            {
                Console.WriteLine("\tkey: " + property.Key + ", value: " + property.Value);
            }

            // throws a LANGUAGE-SPECIFIC error when retrieving a non-existant property!
            // ?? presumably a VB.NET authored DLL would throw a _different_ error? ??
            try
            {
                Console.WriteLine("item.Salary");
                Console.WriteLine(item.Salary);
            }
            catch(Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            // VB.NET has had late-binding dynamic-like behaviour - but the behaviours/exceptions are different
            // keyword "dynamic" is a C# ONLY feature
            Console.WriteLine("(* VB.NET *)");
            Console.WriteLine("ExpandoTest test = new ExpandoTest();");
            ExpandoTest test = new ExpandoTest();
            try
            {
                Console.WriteLine("test.NonExistantProperty();");
                Console.WriteLine(test.NonExistantProperty());
            }
            catch (MissingMemberException e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ResetColor();
        }

        public class NameEventArgs : EventArgs
        {
            public object Name { get; set; }

            public NameEventArgs(object name): base()
            {
                Name = name;
            }
        }

        private void NowWeNowThatSomethingHappened(object sender, NameEventArgs e)
        {
            Console.WriteLine("Now we now that something happened to Name = " + e.Name.ToString());
        }

        private void ShowName(dynamic thingy)
        {
            Console.WriteLine("ShowName(dynamic thingy){ Console.WriteLine(thingy.Name); }");
            Console.WriteLine(thingy.Name);
        }
    }
}
