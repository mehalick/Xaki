using System.Collections.Generic;
using Xaki.Tests.Common.Classes.Abstractions;

namespace Xaki.Tests.Common.Classes.Proxies
{
    public class ProxyB : ClassB, ITestProxyClass
    {
        public ProxyB(string name)
            : base(name)
        {
        }

        public ProxyB BProxy { get; set; }

        public List<ProxyB> ListOfBProxies { get; set; } = new List<ProxyB>(0);

        public ProxyA AProxy { get; set; }

        public List<ProxyA> ListOfAProxies { get; set; } = new List<ProxyA>(0);
    }
}
