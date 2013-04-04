using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Predication.Experiment.Library
{
    [Demo("Task", "An example of using the Task<T> type", ConsoleColor.Green, ConsoleColor.Yellow)]
    public class TaskExample : ExampleBase
    {
        public override void Execute()
        {
            // we will try to count the characters in the following web pages
            Fetch(@"http://www.bing.com/");
            Fetch(@"http://en.wikipedia.org/wiki/Main_Page");
            Fetch(@"http://slashdot.org/");
        }

        // adding async to this function means that it can make use of the await keyword internally
        private async void Fetch(string url)
        {
            Task<int> result = GetPageCharacterCount(url);
            Console.WriteLine(@"waiting for the result from [{0}]", url);
            int count = await result;
            // the app may have finished and have set the console color to something else
            ConsoleColor previous = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("answer from [{0}] is {1}", url, count);
            Console.ForegroundColor = previous;
        }

        private async Task<int> GetPageCharacterCount(string url)
        {
            HttpClient client = new HttpClient();
            Task<string> fetch = client.GetStringAsync(url);
            // await can only be used in a method that is marked as async (otherwise a compile error)
            string contents = await fetch;
            return contents.Length;
        }
    }

   
}
