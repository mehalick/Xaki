using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xaki.Configuration;
using Xaki.LanguageResolvers;
using Xaki.Tests.Common;
using Xunit;

namespace Xaki.Tests
{
    [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
    public class ObjectLocalizerFacts
    {
        public abstract class TestBase
        {
            protected readonly IObjectLocalizer ObjectLocalizer = new ObjectLocalizer
            {
                RequiredLanguages = new HashSet<string>(new[] { Constants.LanguageCode1, Constants.LanguageCode2 })
            };
        }

        public class GetEmptyJsonString : TestBase
        {
            [Fact]
            public void ReturnsCorrectJsonStringForSupportedLanguages()
            {
                var actual = ObjectLocalizer.GetEmptyJsonString();

                Assert.Equal("{\"en\":\"\",\"xx\":\"\"}", actual);
            }
        }

        public class Serialize : TestBase
        {
            [Fact]
            public void ReturnsCorrectJsonForValidContent()
            {
                var contents = new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1,
                    [Constants.LanguageCode2] = Constants.AnyString2
                };

                var actual = ObjectLocalizer.Serialize(contents);

                var expected = $"{{\"{Constants.LanguageCode1}\":\"{Constants.AnyString1}\",\"{Constants.LanguageCode2}\":\"{Constants.AnyString2}\"}}";

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ReturnsJsonInOrderWhenInputOutOfOrder()
            {
                var contents = new Dictionary<string, string>
                {
                    [Constants.LanguageCode2] = Constants.AnyString2,
                    [Constants.LanguageCode1] = Constants.AnyString1
                };

                var actual = ObjectLocalizer.Serialize(contents);

                var expected = $"{{\"{Constants.LanguageCode1}\":\"{Constants.AnyString1}\",\"{Constants.LanguageCode2}\":\"{Constants.AnyString2}\"}}";

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ReturnsJsonWithoutInvalidLanguageCodesWhenInputContainsInvalidLanguageCodes()
            {
                var contents = new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1,
                    [Constants.LanguageCode2] = Constants.AnyString2,
                    [Constants.LanguageCode3] = Constants.AnyString2
                };

                var actual = ObjectLocalizer.Serialize(contents);

                var expected = $"{{\"{Constants.LanguageCode1}\":\"{Constants.AnyString1}\",\"{Constants.LanguageCode2}\":\"{Constants.AnyString2}\"}}";

                Assert.Equal(expected, actual);
            }
        }

        public class Deserialize : TestBase
        {
            [Fact]
            public void ReturnsValidContentWhenJsonIsValid()
            {
                var json = $"{{\"{Constants.LanguageCode1}\":\"{Constants.AnyString1}\",\"{Constants.LanguageCode2}\":\"{Constants.AnyString2}\"}}";

                var result = ObjectLocalizer.Deserialize(json);

                Assert.Collection(result,
                    item => Assert.Equal(Constants.LanguageCode1, item.Key),
                    item => Assert.Equal(Constants.LanguageCode2, item.Key)
                );

                Assert.Collection(result,
                    item => Assert.Equal(Constants.AnyString1, item.Value),
                    item => Assert.Equal(Constants.AnyString2, item.Value)
                );
            }
        }

        public class TryDeserialize : TestBase
        {
            [Fact]
            public void Returns()
            {
                var result = ObjectLocalizer.TryDeserialize("NOT JSON", out var localizedContent);

                Assert.False(result);
                Assert.Equal(Constants.LanguageCode1, localizedContent.Keys.Single());
                Assert.Equal(string.Empty, localizedContent.Values.Single());
            }
        }

        public class Localize : TestBase
        {
            [Fact]
            public void ReturnsNullWhenItemIsNull()
            {
                TestClass testClass = null;

                var result = ObjectLocalizer.Localize(testClass, null);

                Assert.Null(result);
            }

            [Fact]
            public void ReturnsFirstLocalizationWhenLanguageCodeIsNull()
            {
                var name = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1,
                    [Constants.LanguageCode2] = Constants.AnyString2
                });

                var testClass = new TestClass(name);

                var result = ObjectLocalizer.Localize(testClass, null);

                Assert.Equal(Constants.AnyString1, result.Name);
            }

            [Fact]
            public void ReturnsFirstLocalizationWhenLanguageCodeIsNotSupported()
            {
                var name = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1,
                    [Constants.LanguageCode2] = Constants.AnyString2
                });

                var testClass = new TestClass(name);

                var result = ObjectLocalizer.Localize(testClass, Constants.LanguageCode3);

                Assert.Equal(Constants.AnyString1, result.Name);
            }

            [Fact]
            public void ReturnsCorrectLocalizationWhenItemHasOneLanguage()
            {
                var name = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1
                });

                var testClass = new TestClass(name);

                var result = ObjectLocalizer.Localize(testClass, Constants.LanguageCode1);

                Assert.Equal(Constants.AnyString1, result.Name);
            }

            [Fact]
            public void ReturnsCorrectLocalizationWhenItemHasMultipleLanguages()
            {
                var name = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1,
                    [Constants.LanguageCode2] = Constants.AnyString2
                });

                var testClass = new TestClass(name);

                var result = ObjectLocalizer.Localize(testClass, Constants.LanguageCode2);

                Assert.Equal(Constants.AnyString2, result.Name);
            }

            [Fact]
            public void ReturnsCorrectLocalizationsItemsHaveMultipleLanguages()
            {
                var name = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1,
                    [Constants.LanguageCode2] = Constants.AnyString2
                });

                var testClasses = new[]
                {
                    new TestClass(name),
                    new TestClass(name)
                };

                var results = ObjectLocalizer.Localize<TestClass>(testClasses, Constants.LanguageCode1).ToList();

                Assert.Equal(Constants.AnyString1, results.ElementAt(0).Name);
                Assert.Equal(Constants.AnyString1, results.ElementAt(1).Name);
            }
        }

        public class GetLanguageCode
        {
            [Fact]
            public void ReturnsFallbackLanguageCodeWhenNoLanguageResolversSpecified()
            {
                var objectLocalizer = new ObjectLocalizer();

                Assert.Equal(ObjectLocalizer.FallbackLanguageCode, objectLocalizer.GetLanguageCode());
            }

            [Fact]
            public void ReturnsFallbackLanguageCodeWhenNullLanguageResolversSpecified()
            {
                var objectLocalizer = new ObjectLocalizer
                {
                    LanguageResolvers = new List<ILanguageResolver>
                    {
                        new NullLanguageResolver()
                    }
                };

                Assert.Equal(ObjectLocalizer.FallbackLanguageCode, objectLocalizer.GetLanguageCode());
            }

            [Fact]
            public void ReturnsDefaultLanguageCodeWhenFirstLanguageResolverIsNull()
            {
                var objectLocalizer = new ObjectLocalizer
                {
                    LanguageResolvers = new List<ILanguageResolver>
                    {
                        new NullLanguageResolver(),
                        new DefaultLanguageResolver(Constants.LanguageCode1)
                    }
                };

                Assert.Equal(Constants.LanguageCode1, objectLocalizer.GetLanguageCode());
            }
        }

        public class ObjectLocalizerConfigExtensions : TestBase
        {
            [Fact]
            public void ReturnsNullWhenItemIsNull()
            {
                TestClass testClass = null;

                ObjectLocalizerConfig.Set(() => ObjectLocalizer);
                var result = testClass.Localize();

                Assert.Null(result);
            }

            [Fact]
            public void ReturnsFirstLocalizationWhenLanguageCodeIsNull()
            {
                var name = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1,
                    [Constants.LanguageCode2] = Constants.AnyString2
                });

                var testClass = new TestClass(name);

                ObjectLocalizerConfig.Set(() => ObjectLocalizer);
                var result = testClass.Localize();

                Assert.Equal(Constants.AnyString1, result.Name);
            }

            [Fact]
            public void ReturnsFirstLocalizationWhenLanguageCodeIsNotSupported()
            {
                var name = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1,
                    [Constants.LanguageCode2] = Constants.AnyString2
                });

                var testClass = new TestClass(name);

                ObjectLocalizerConfig.Set(() => ObjectLocalizer);
                var result = testClass.Localize(Constants.LanguageCode3);

                Assert.Equal(Constants.AnyString1, result.Name);
            }

            [Fact]
            public void ReturnsCorrectLocalizationWhenItemHasOneLanguage()
            {
                var name = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1
                });

                var testClass = new TestClass(name);

                ObjectLocalizerConfig.Set(() => ObjectLocalizer);
                var result = testClass.Localize(Constants.LanguageCode1);

                Assert.Equal(Constants.AnyString1, result.Name);
            }

            [Fact]
            public void ReturnsCorrectLocalizationWhenItemHasMultipleLanguages()
            {
                var name = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1,
                    [Constants.LanguageCode2] = Constants.AnyString2
                });

                var testClass = new TestClass(name);

                ObjectLocalizerConfig.Set(() => ObjectLocalizer);
                var result = testClass.Localize(Constants.LanguageCode2);

                Assert.Equal(Constants.AnyString2, result.Name);
            }

            [Fact]
            public void ReturnsCorrectLocalizationsItemsHaveMultipleLanguages()
            {
                var name = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1,
                    [Constants.LanguageCode2] = Constants.AnyString2
                });

                var testClasses = new[]
                {
                    new TestClass(name),
                    new TestClass(name)
                };

                ObjectLocalizerConfig.Set(() => ObjectLocalizer);
                var results = testClasses.Localize<TestClass>(Constants.LanguageCode1).ToList();

                Assert.Equal(Constants.AnyString1, results.ElementAt(0).Name);
                Assert.Equal(Constants.AnyString1, results.ElementAt(1).Name);
            }
        }
    }
}
