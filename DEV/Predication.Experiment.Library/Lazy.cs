using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predication.Experiment.Library
{
    public class LazyExample
    {
        public void Demonstrate()
        {
            DateTime functionStart = DateTime.Now;
            Lazy<ExpensiveTimestampedClass> lazyObject = new Lazy<ExpensiveTimestampedClass>();
            Console.WriteLine("{0} : function started", functionStart.TimeOfDay);
            Console.WriteLine("{0} : Lazy<T> created", DateTime.Now);
            Console.WriteLine("{0} : lazyObject.IsValueCreated = {1}", DateTime.Now, lazyObject.IsValueCreated);
            Console.WriteLine("Press a key to continue");
            Console.ReadKey();
            Console.WriteLine("{0} : lazy.Value.Timestamp = {1}", DateTime.Now, lazyObject.Value.Timestamp);
            Console.WriteLine("{0} : lazyObject.IsValueCreated = {1}", DateTime.Now, lazyObject.IsValueCreated);
            Console.WriteLine("Press a key to continue");
            Console.ReadKey();
            Console.WriteLine("{0} : Static Timestamp = {1}", DateTime.Now, ExpensiveTimestampedClass.StaticTimestamp);
        }
    }

    public class ExpensiveTimestampedClass
    {
        public static DateTime StaticTimestamp { get; private set; }

        static ExpensiveTimestampedClass()
        {
            StaticTimestamp = DateTime.Now;
        }

        public DateTime Timestamp { get; private set; }

        public ExpensiveTimestampedClass()
        {
            Timestamp = DateTime.Now;
        }

        public DateTime GetNow()
        {
            return DateTime.Now;
        }
    }
}
