using System.Collections.Generic;
using Xaki.LanguageResolvers;
using Xaki.Tests.Common;
using Xunit;

namespace Xaki.Tests
{
    public class LanguageResolverTests
    {
        [Fact]
        public void LocalizeItem_ItemHasMultipleLanguages_ReturnsCorrectLocalization()
        {
            var localizationService = new ObjectLocalizer
            {
                LanguageResolvers = new[] { new DefaultLanguageResolver(Constants.LanguageCode1) },
                RequiredLanguages = new[] { Constants.LanguageCode1, Constants.LanguageCode2 }
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

        [Fact]
        public void LocalizeItem_FirstLanguageResolverIsNull_ReturnsSecondLanguage()
        {
            var localizationService = new ObjectLocalizer
            {
                LanguageResolvers = new List<ILanguageResolver>
                {
                    new NullLanguageResolver(),
                    new DefaultLanguageResolver(Constants.LanguageCode1)
                },
                RequiredLanguages = new[] { Constants.LanguageCode1, Constants.LanguageCode2 }
            };

            var name = localizationService.Serialize(new Dictionary<string, string>
            {
                { Constants.LanguageCode1, Constants.AnyString1 }
            });

            var testClass = new TestClass(name);

            var result = localizationService.Localize(testClass);

            Assert.Equal(Constants.AnyString1, result.Name);
        }
    }
}
