using System.Collections.Generic;

namespace Xaki.Tests.Common.Classes.Abstractions
{
    public interface ITestClass
    {
        string Name { get; set; }

        ClassA A { get; set; }

        List<ClassA> ListOfA { get; set; }

        ClassB B { get; set; }

        List<ClassB> ListOfB { get; set; }
    }
}
