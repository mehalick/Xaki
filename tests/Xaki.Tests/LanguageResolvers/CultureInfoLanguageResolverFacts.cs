using System.Globalization;
using Xaki.LanguageResolvers;
using Xunit;

namespace Xaki.Tests.LanguageResolvers
{
    public class CultureInfoLanguageResolverFacts
    {
        public class GetLanguageCode
        {
            [Fact]
            public void ReturnsCultureInfoLanguageCodeWhenExists()
            {
                var resolver = new CultureInfoLanguageResolver();

                CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("ar-KW");

                Assert.Equal("ar", resolver.GetLanguageCode());
            }
        }
    }
}
