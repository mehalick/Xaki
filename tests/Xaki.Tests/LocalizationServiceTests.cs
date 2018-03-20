using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

namespace Xaki.Tests
{
    [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
    public class LocalizationServiceTests
    {
        private readonly ILocalizationService _localizationService = new LocalizationService
        {
            LanguageCodes = new[] { Constants.LanguageCode1, Constants.LanguageCode2 }
        };

        [Fact]
        public void Serialize_ContentIsValid_ReturnsString()
        {
            var contents = new Dictionary<string, string>
            {
                {Constants.LanguageCode1, Constants.AnyString1 },
                {Constants.LanguageCode2, Constants.AnyString2 }
            };

            var actual = _localizationService.Serialize(contents);

            var expected = $"{{\"{Constants.LanguageCode1}\":\"{Constants.AnyString1}\",\"{Constants.LanguageCode2}\":\"{Constants.AnyString2}\"}}";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Serialize_ContentIsOutOfOrder_ReturnsStringInOrder()
        {
            var contents = new Dictionary<string, string>
            {
                {Constants.LanguageCode2, Constants.AnyString2 },
                {Constants.LanguageCode1, Constants.AnyString1 }
            };

            var actual = _localizationService.Serialize(contents);

            var expected = $"{{\"{Constants.LanguageCode1}\":\"{Constants.AnyString1}\",\"{Constants.LanguageCode2}\":\"{Constants.AnyString2}\"}}";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Serialize_ContentIncludesInvalidLanguageCode_InvalidLanguageCodeDoesNotSerialize()
        {
            var contents = new Dictionary<string, string>
            {
                {Constants.LanguageCode1, Constants.AnyString1 },
                {Constants.LanguageCode2, Constants.AnyString2 },
                {Constants.LanguageCode3, Constants.AnyString2 }
            };

            var actual = _localizationService.Serialize(contents);

            var expected = $"{{\"{Constants.LanguageCode1}\":\"{Constants.AnyString1}\",\"{Constants.LanguageCode2}\":\"{Constants.AnyString2}\"}}";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Deserialize_ValidJsonString_ReturnsValidContent()
        {
            var json = $"{{\"{Constants.LanguageCode1}\":\"{Constants.AnyString1}\",\"{Constants.LanguageCode2}\":\"{Constants.AnyString2}\"}}";

            var result = _localizationService.Deserialize(json);

            Assert.Collection(result,
                item => Assert.Equal(Constants.LanguageCode1, item.Key),
                item => Assert.Equal(Constants.LanguageCode2, item.Key)
            );

            Assert.Collection(result,
                item => Assert.Equal(Constants.AnyString1, item.Value),
                item => Assert.Equal(Constants.AnyString2, item.Value)
            );
        }

        [Fact]
        public void LocalizeItem_ItemIsNull_ReturnsNull()
        {
            TestClass testClass = null;

            var result = _localizationService.Localize(testClass, null);

            Assert.Null(result);
        }

        [Fact]
        public void LocalizeItem_LanguageCodeIsNull_ReturnsFirstLocalization()
        {
            var name = _localizationService.Serialize(new Dictionary<string, string>
            {
                { Constants.LanguageCode1, Constants.AnyString1 },
                { Constants.LanguageCode2, Constants.AnyString2 }
            });

            var testClass = new TestClass(name);

            var result = _localizationService.Localize(testClass, null);

            Assert.Equal(Constants.AnyString1, result.Name);
        }

        [Fact]
        public void LocalizeItem_LanguageCodeIsNotSupported_ReturnsFirstLocalization()
        {
            var name = _localizationService.Serialize(new Dictionary<string, string>
            {
                { Constants.LanguageCode1, Constants.AnyString1 },
                { Constants.LanguageCode2, Constants.AnyString2 }
            });

            var testClass = new TestClass(name);

            var result = _localizationService.Localize(testClass, Constants.LanguageCode3);

            Assert.Equal(Constants.AnyString1, result.Name);
        }

        [Fact]
        public void LocalizeItem_ItemHasOneLanguage_ReturnsCorrectLocalization()
        {
            var name = _localizationService.Serialize(new Dictionary<string, string>
            {
                { Constants.LanguageCode1, Constants.AnyString1 }
            });

            var testClass = new TestClass(name);

            var result = _localizationService.Localize(testClass, Constants.LanguageCode1);

            Assert.Equal(Constants.AnyString1, result.Name);
        }

        [Fact]
        public void LocalizeItem_ItemHasMultipleLanguages_ReturnsCorrectLocalization()
        {
            var name = _localizationService.Serialize(new Dictionary<string, string>
            {
                { Constants.LanguageCode1, Constants.AnyString1 },
                { Constants.LanguageCode2, Constants.AnyString2 }
            });

            var testClass = new TestClass(name);

            var result = _localizationService.Localize(testClass, Constants.LanguageCode2);

            Assert.Equal(Constants.AnyString2, result.Name);
        }

        [Fact]
        public void LocalizeCollection_ItemsHaveMultipleLanguages_ReturnsCorrectLocalizations()
        {
            var name = _localizationService.Serialize(new Dictionary<string, string>
            {
                { Constants.LanguageCode1, Constants.AnyString1 },
                { Constants.LanguageCode2, Constants.AnyString2 }
            });

            var testClasses = new[]
            {
                new TestClass(name),
                new TestClass(name)
            };

            var results = _localizationService.Localize<TestClass>(testClasses, Constants.LanguageCode1).ToList();

            Assert.Equal(Constants.AnyString1, results.ElementAt(0).Name);
            Assert.Equal(Constants.AnyString1, results.ElementAt(1).Name);
        }
    }
}
