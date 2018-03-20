using System.Collections.Generic;
using Xaki.LanguageResolvers;
using Xunit;

namespace Xaki.Tests
{
    public class StaticLanguageResolverTests
    {
        [Fact]
        public void LocalizeItem_ItemHasMultipleLanguages_ReturnsCorrectLocalization()
        {
            var localizationService = new LocalizationService
            {
                LanguageResolver = new StaticLanguageResolver(Constants.LanguageCode1),
                LanguageCodes = new[] { Constants.LanguageCode1, Constants.LanguageCode2 }
            };

            var name = localizationService.Serialize(new Dictionary<string, string>
            {
                { Constants.LanguageCode1, Constants.AnyString1 },
                { Constants.LanguageCode2, Constants.AnyString2 }
            });

            var testClass = new TestClass(name);

            var result = localizationService.Localize(testClass);

            Assert.Equal(Constants.AnyString1, result.Name);
        }
    }
}
