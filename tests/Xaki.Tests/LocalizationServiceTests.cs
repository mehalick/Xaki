using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Xaki.Tests
{
    [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
    public class LocalizationServiceTests
    {
        private const string LanguageCode1 = "en";
        private const string LanguageCode2 = "xx";
        private const string LanguageCode3 = "zz";
        private const string AnyString1 = "ABC123";
        private const string AnyString2 = "XYZ789";

        private readonly ILocalizationService _localizationService = new LocalizationService(LanguageCode1, LanguageCode2);

        [Fact]
        public void Serialize_ContentIsValid_ReturnsString()
        {
            var contents = new Dictionary<string, string>
            {
                { LanguageCode1, AnyString1 },
                { LanguageCode2, AnyString2 }
            };

            var actual = _localizationService.Serialize(contents);

            var expected = $"{{\"{LanguageCode1}\":\"{AnyString1}\",\"{LanguageCode2}\":\"{AnyString2}\"}}";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Serialize_ContentIsOutOfOrder_ReturnsStringInOrder()
        {
            var contents = new Dictionary<string, string>
            {
                { LanguageCode2, AnyString2 },
                { LanguageCode1, AnyString1 }
            };

            var actual = _localizationService.Serialize(contents);

            var expected = $"{{\"{LanguageCode1}\":\"{AnyString1}\",\"{LanguageCode2}\":\"{AnyString2}\"}}";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Serialize_ContentIncludesInvalidLanguageCode_InvalidLanguageCodeDoesNotSerialize()
        {
            var contents = new Dictionary<string, string>
            {
                { LanguageCode1, AnyString1 },
                { LanguageCode2, AnyString2 },
                { LanguageCode3, AnyString2 }
            };

            var actual = _localizationService.Serialize(contents);

            var expected = $"{{\"{LanguageCode1}\":\"{AnyString1}\",\"{LanguageCode2}\":\"{AnyString2}\"}}";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Deserialize_ValidJsonString_ReturnsValidContent()
        {
            var json = $"{{\"{LanguageCode1}\":\"{AnyString1}\",\"{LanguageCode2}\":\"{AnyString2}\"}}";

            var result = _localizationService.Deserialize(json);

            Assert.Collection(result,
                item => Assert.Equal(LanguageCode1, item.Key),
                item => Assert.Equal(LanguageCode2, item.Key)
            );

            Assert.Collection(result,
                item => Assert.Equal(AnyString1, item.Value),
                item => Assert.Equal(AnyString2, item.Value)
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
        public void LocalizeItem_LanguageCodeIsNull_ReturnsItem()
        {
            const string name = "{\"en\":\"SOME TEXT\"}";

            var testClass = new TestClass(name);

            var result = _localizationService.Localize(testClass, null);

            Assert.Equal(name, result.Name);
        }

        [Fact]
        public void LocalizeItem_ItemHasOneLanguage_ReturnsCorrectLocalization()
        {
            var name = _localizationService.Serialize(new Dictionary<string, string>
            {
                { LanguageCode1, AnyString1 }
            });

            var testClass = new TestClass(name);

            var result = _localizationService.Localize(testClass, LanguageCode1);

            Assert.Equal(AnyString1, result.Name);
        }
    }
}
