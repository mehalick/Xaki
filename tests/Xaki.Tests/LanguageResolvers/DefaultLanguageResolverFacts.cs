using Xaki.LanguageResolvers;
using Xaki.Tests.Common;
using Xunit;

namespace Xaki.Tests.LanguageResolvers
{
    public class DefaultLanguageResolverFacts
    {
        public class GetLanguageCode
        {
            [Fact]
            public void ReturnsDefaultLanguageCode()
            {
                var resolver = new DefaultLanguageResolver(Constants.LanguageCode1);

                Assert.Equal(Constants.LanguageCode1, resolver.GetLanguageCode());
            }
        }
    }
}
