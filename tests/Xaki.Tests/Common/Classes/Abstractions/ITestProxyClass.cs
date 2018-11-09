using System.Collections.Generic;
using Xaki.Tests.Common.Classes.Proxies;

namespace Xaki.Tests.Common.Classes.Abstractions
{
    public interface ITestProxyClass : ITestClass
    {
        ProxyA AProxy { get; set; }

        List<ProxyA> ListOfAProxies { get; set; }

        ProxyB BProxy { get; set; }

        List<ProxyB> ListOfBProxies { get; set; }
    }
}
