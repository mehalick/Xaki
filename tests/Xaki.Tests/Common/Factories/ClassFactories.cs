using System.Collections.Generic;
using Xaki.Tests.Common.Classes;

namespace Xaki.Tests.Common.Factories
{
    public static class ClassFactories
    {
        public static ClassA CreateClassAWithSelfReference(string name)
        {
            var a = new ClassA(name);
            a.A = a;
            a.ListOfA = new List<ClassA>
            {
                a,
                new ClassA(name)
            };

            var b = new ClassB(name);
            a.B = b;
            a.ListOfB = new List<ClassB>
            {
                b,
                new ClassB(name)
            };

            b.B = b;
            b.ListOfB = new List<ClassB>
            {
                b,
                new ClassB(name)
            };
            b.A = a;
            b.ListOfA = a.ListOfA;

            return a;
        }

        public static ClassB CreateClassBWithSelfReference(string name)
        {
            var b = new ClassB(name);
            b.B = b;
            b.ListOfB = new List<ClassB>
            {
                b,
                new ClassB(name)
            };

            var a = new ClassA(name);
            b.A = a;
            b.ListOfA = new List<ClassA>
            {
                a,
                new ClassA(name)
            };

            a.A = a;
            a.ListOfA = new List<ClassA>
            {
                a,
                new ClassA(name)
            };
            a.B = b;
            a.ListOfB = b.ListOfB;

            return b;
        }

        public static ClassC CreateClassC(string name)
        {
            var c = new ClassC(name);
            c.A = CreateClassAWithSelfReference(name);
            c.ListOfA = new List<ClassA>
            {
                c.A,
                new ClassA(name)
            };

            c.B = CreateClassBWithSelfReference(name);
            c.ListOfB = new List<ClassB>
            {
                c.B,
                new ClassB(name)
            };

            return c;
        }
    }
}
