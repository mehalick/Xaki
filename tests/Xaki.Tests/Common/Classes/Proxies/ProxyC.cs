using System.Collections.Generic;
using Xaki.Tests.Common.Classes.Abstractions;

namespace Xaki.Tests.Common.Classes.Proxies
{
    public class ProxyC : ClassC, ITestProxyClass
    {
        public ProxyC(string name)
            : base(name)
        {
        }

        public ProxyA AProxy { get; set; }

        public List<ProxyA> ListOfAProxies { get; set; } = new List<ProxyA>(0);

        public ProxyB BProxy { get; set; }

        public List<ProxyB> ListOfBProxies { get; set; } = new List<ProxyB>(0);
    }
}
