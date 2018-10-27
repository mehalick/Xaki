using System.Collections.Generic;
using Xaki.Tests.Common.Classes.Abstractions;

namespace Xaki.Tests.Common.Classes
{
    public class ClassB : ILocalizable, ITestClass
    {
        [Localized]
        public string Name { get; set; }

        public ClassB B { get; set; }

        public List<ClassB> ListOfB { get; set; } = new List<ClassB>(0);

        public ClassA A { get; set; }

        public List<ClassA> ListOfA { get; set; } = new List<ClassA>(0);

        public ClassB(string name)
        {
            Name = name;
        }
    }
}
