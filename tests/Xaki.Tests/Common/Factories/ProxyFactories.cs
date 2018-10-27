using System.Collections.Generic;
using Xaki.Tests.Common.Classes;
using Xaki.Tests.Common.Classes.Proxies;

namespace Xaki.Tests.Common.Factories
{
    public static class ProxyFactories
    {
        public static ProxyA CreateProxyWithSelfReference(ClassA aObject)
        {
            var a = new ProxyA(aObject.Name)
            {
                A = aObject.A,
                B = aObject.B,
                ListOfA = aObject.ListOfA,
                ListOfB = aObject.ListOfB
            };

            a.AProxy = a;
            a.ListOfAProxies = new List<ProxyA>
            {
                a,
                new ProxyA(aObject.Name)
            };

            var b = new ProxyB(aObject.Name);
            a.BProxy = b;
            a.ListOfBProxies = new List<ProxyB>
            {
                b,
                new ProxyB(aObject.Name)
            };

            b.BProxy = b;
            b.ListOfBProxies = new List<ProxyB>
            {
                b,
                new ProxyB(aObject.Name)
            };
            b.AProxy = a;
            b.ListOfAProxies = a.ListOfAProxies;

            return a;
        }

        public static ProxyB CreateProxyBWithSelfReference(ClassB bObject)
        {
            var b = new ProxyB(bObject.Name)
            {
                A = bObject.A,
                B = bObject.B,
                ListOfA = bObject.ListOfA,
                ListOfB = bObject.ListOfB
            };

            b.BProxy = b;
            b.ListOfBProxies = new List<ProxyB>
            {
                b,
                new ProxyB(bObject.Name)
            };

            var a = new ProxyA(bObject.Name);
            b.AProxy = a;
            b.ListOfAProxies = new List<ProxyA>
            {
                a,
                new ProxyA(bObject.Name)
            };

            a.AProxy = a;
            a.ListOfAProxies = new List<ProxyA>
            {
                a,
                new ProxyA(bObject.Name)
            };
            a.BProxy = b;
            a.ListOfBProxies = b.ListOfBProxies;

            return b;
        }

        public static ProxyC CreateProxyC(ClassC cObject)
        {
            var c = new ProxyC(cObject.Name)
            {
                A = cObject.A,
                B = cObject.B,
                ListOfA = cObject.ListOfA,
                ListOfB = cObject.ListOfB
            };
            c.AProxy = CreateProxyWithSelfReference(c.A);
            c.ListOfAProxies = new List<ProxyA>
            {
                c.AProxy,
                new ProxyA(cObject.Name)
            };

            c.BProxy = CreateProxyBWithSelfReference(c.B);
            c.ListOfBProxies = new List<ProxyB>
            {
                c.BProxy,
                new ProxyB(cObject.Name)
            };
            return c;
        }
    }
}
