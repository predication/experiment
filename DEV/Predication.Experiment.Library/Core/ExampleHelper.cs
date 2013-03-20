using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Predication.Experiment.Library
{
    public class ExampleHelper
    {
        public static IEnumerable<ExampleBase> GetExamples()
        {
            List<ExampleBase> examples = new List<ExampleBase>();
            Type attributeType = new DemoAttribute().GetType();
            Assembly assm = Assembly.GetExecutingAssembly();
            foreach (Type t in assm.GetTypes())
            {
                foreach (CustomAttributeData data in t.CustomAttributes)
                {
                    if (data.AttributeType == attributeType)
                    {
                        ExampleBase example = (ExampleBase)Activator.CreateInstance(t);
                        // populate the values
                        DemoAttribute demoAttribute = (DemoAttribute)t.GetCustomAttribute(attributeType);
                        example.Title = demoAttribute.Title;
                        example.Description = demoAttribute.Description;
                        example.Foreground = demoAttribute.Foreground;
                        example.Background = demoAttribute.Background;
                        examples.Add(example);
                    }
                }
            }

            return examples;
        }
    }
}
