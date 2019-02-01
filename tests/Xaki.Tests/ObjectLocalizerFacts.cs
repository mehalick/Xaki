using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xaki.LanguageResolvers;
using Xaki.Tests.Common;
using Xaki.Tests.Common.Classes;
using Xaki.Tests.Common.Classes.Abstractions;
using Xaki.Tests.Common.Classes.Proxies;
using Xaki.Tests.Common.Factories;
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

        public class LocalizeShallow : TestBase
        {
            [Fact]
            public void ReturnsNullWhenItemIsNull()
            {
                ClassC testClass = null;

                var result = ObjectLocalizer.Localize(testClass, null, LocalizationDepth.Shallow);

                Assert.Null(result);
            }

            [Fact]
            public void ReturnsFirstLocalizationWhenLanguageCodeIsNull()
            {
                var json = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1,
                    [Constants.LanguageCode2] = Constants.AnyString2
                });

                var testClass = ClassFactories.CreateClassC(json);
                var result = ObjectLocalizer.Localize(testClass, null, LocalizationDepth.Shallow);

                Assert.Equal(Constants.AnyString1, result.Name);
                AssertReferencePropertiesAreNotLocalized(json, result);
                AssertReferencePropertiesAreNotLocalized(json, result.A);
                AssertReferencePropertiesAreNotLocalized(json, result.B);
            }

            [Fact]
            public void ReturnsFirstLocalizationWhenLanguageCodeIsNotSupported()
            {
                var json = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1,
                    [Constants.LanguageCode2] = Constants.AnyString2
                });

                var testClass = ClassFactories.CreateClassC(json);
                var result = ObjectLocalizer.Localize(testClass, Constants.LanguageCode3, LocalizationDepth.Shallow);

                Assert.Equal(Constants.AnyString1, result.Name);
                AssertReferencePropertiesAreNotLocalized(json, result);
                AssertReferencePropertiesAreNotLocalized(json, result.A);
                AssertReferencePropertiesAreNotLocalized(json, result.B);
            }

            [Fact]
            public void ReturnsCorrectLocalizationWhenItemHasOneLanguage()
            {
                var json = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1
                });

                var testClass = ClassFactories.CreateClassC(json);
                var result = ObjectLocalizer.Localize(testClass, Constants.LanguageCode1, LocalizationDepth.Shallow);

                Assert.Equal(Constants.AnyString1, result.Name);
                AssertReferencePropertiesAreNotLocalized(json, result);
                AssertReferencePropertiesAreNotLocalized(json, result.A);
                AssertReferencePropertiesAreNotLocalized(json, result.B);
            }

            [Fact]
            public void ReturnsCorrectLocalizationWhenItemHasMultipleLanguages()
            {
                var json = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1,
                    [Constants.LanguageCode2] = Constants.AnyString2
                });

                var testClass = ClassFactories.CreateClassC(json);
                var result = ObjectLocalizer.Localize(testClass, Constants.LanguageCode2, LocalizationDepth.Shallow);

                Assert.Equal(Constants.AnyString2, result.Name);
                AssertReferencePropertiesAreNotLocalized(json, result);
                AssertReferencePropertiesAreNotLocalized(json, result.A);
                AssertReferencePropertiesAreNotLocalized(json, result.B);
            }

            [Fact]
            public void ReturnsCorrectLocalizationsItemsHaveMultipleLanguages()
            {
                var json = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1,
                    [Constants.LanguageCode2] = Constants.AnyString2
                });

                var testClasses = new[]
                {
                    ClassFactories.CreateClassC(json),
                    ClassFactories.CreateClassC(json)
                };

                var results = ObjectLocalizer.Localize<ClassC>(testClasses, Constants.LanguageCode1, LocalizationDepth.Shallow).ToList();

                Assert.Equal(Constants.AnyString1, results.ElementAt(0).Name);
                Assert.Equal(Constants.AnyString1, results.ElementAt(1).Name);
                AssertReferencePropertiesAreNotLocalized(json, results.ElementAt(0));
                AssertReferencePropertiesAreNotLocalized(json, results.ElementAt(0).A);
                AssertReferencePropertiesAreNotLocalized(json, results.ElementAt(0).B);
                AssertReferencePropertiesAreNotLocalized(json, results.ElementAt(1));
                AssertReferencePropertiesAreNotLocalized(json, results.ElementAt(1).A);
                AssertReferencePropertiesAreNotLocalized(json, results.ElementAt(1).B);
            }

            private static void AssertReferencePropertiesAreNotLocalized(string json, ITestClass testClass)
            {
                // You can even add more to this go wild and try to wrap your head around the references.
                Assert.Equal(json, testClass.A.Name);

                Assert.Equal(json, testClass.A.A.Name);
                AssertReferenceListPropertiesAreNotLocalized(json, testClass.A);

                Assert.Equal(json, testClass.A.B.Name);
                AssertReferenceListPropertiesAreNotLocalized(json, testClass.B);
            }

            private static void AssertReferenceListPropertiesAreNotLocalized(string json, ITestClass testClass)
            {
                // You can even add more to this go wild and try to wrap your head around the references.
                foreach (var a in testClass.ListOfA)
                {
                    Assert.Equal(json, a.Name);
                }
                foreach (var a in testClass.A.ListOfA)
                {
                    Assert.Equal(json, a.Name);
                }
                foreach (var b in testClass.A.ListOfB)
                {
                    Assert.Equal(json, b.Name);
                }
                foreach (var b in testClass.A.B.ListOfA)
                {
                    Assert.Equal(json, b.Name);
                }
                foreach (var b in testClass.A.B.ListOfB)
                {
                    Assert.Equal(json, b.Name);
                }

                foreach (var b in testClass.ListOfB)
                {
                    Assert.Equal(json, b.Name);
                }
                foreach (var a in testClass.B.ListOfA)
                {
                    Assert.Equal(json, a.Name);
                }
                foreach (var b in testClass.B.ListOfB)
                {
                    Assert.Equal(json, b.Name);
                }
                foreach (var a in testClass.B.B.ListOfA)
                {
                    Assert.Equal(json, a.Name);
                }
                foreach (var b in testClass.B.B.ListOfB)
                {
                    Assert.Equal(json, b.Name);
                }
            }
        }

        public class LocalizeProxyShallow : TestBase
        {
            [Fact]
            public void ReturnsNullWhenItemIsNull()
            {
                ProxyC testClass = null;

                var result = ObjectLocalizer.Localize(testClass, null, LocalizationDepth.Shallow);

                Assert.Null(result);
            }

            [Fact]
            public void ReturnsFirstLocalizationWhenLanguageCodeIsNull()
            {
                var json = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1,
                    [Constants.LanguageCode2] = Constants.AnyString2
                });

                var testClass = ProxyFactories.CreateProxyC(ClassFactories.CreateClassC(json));
                var result = ObjectLocalizer.Localize(testClass, null, LocalizationDepth.Shallow);

                Assert.Equal(Constants.AnyString1, result.Name);
                AssertReferencePropertiesAreNotLocalized(json, result as ITestClass);
                AssertReferencePropertiesAreNotLocalized(json, result);
                AssertReferencePropertiesAreNotLocalized(json, result.A);
                AssertReferencePropertiesAreNotLocalized(json, result.B);
                AssertReferencePropertiesAreNotLocalized(json, result.AProxy);
                AssertReferencePropertiesAreNotLocalized(json, result.BProxy);
            }

            [Fact]
            public void ReturnsFirstLocalizationWhenLanguageCodeIsNotSupported()
            {
                var json = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1,
                    [Constants.LanguageCode2] = Constants.AnyString2
                });

                var testClass = ProxyFactories.CreateProxyC(ClassFactories.CreateClassC(json));
                var result = ObjectLocalizer.Localize(testClass, Constants.LanguageCode3, LocalizationDepth.Shallow);

                Assert.Equal(Constants.AnyString1, result.Name);
                AssertReferencePropertiesAreNotLocalized(json, result as ITestClass);
                AssertReferencePropertiesAreNotLocalized(json, result);
                AssertReferencePropertiesAreNotLocalized(json, result.A);
                AssertReferencePropertiesAreNotLocalized(json, result.B);
                AssertReferencePropertiesAreNotLocalized(json, result.AProxy);
                AssertReferencePropertiesAreNotLocalized(json, result.BProxy);
            }

            [Fact]
            public void ReturnsCorrectLocalizationWhenItemHasOneLanguage()
            {
                var json = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1
                });

                var testClass = ProxyFactories.CreateProxyC(ClassFactories.CreateClassC(json));
                var result = ObjectLocalizer.Localize(testClass, Constants.LanguageCode1, LocalizationDepth.Shallow);

                Assert.Equal(Constants.AnyString1, result.Name);
                AssertReferencePropertiesAreNotLocalized(json, result as ITestClass);
                AssertReferencePropertiesAreNotLocalized(json, result);
                AssertReferencePropertiesAreNotLocalized(json, result.A);
                AssertReferencePropertiesAreNotLocalized(json, result.B);
                AssertReferencePropertiesAreNotLocalized(json, result.AProxy);
                AssertReferencePropertiesAreNotLocalized(json, result.BProxy);
            }

            [Fact]
            public void ReturnsCorrectLocalizationWhenItemHasMultipleLanguages()
            {
                var json = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1,
                    [Constants.LanguageCode2] = Constants.AnyString2
                });

                var testClass = ProxyFactories.CreateProxyC(ClassFactories.CreateClassC(json));
                var result = ObjectLocalizer.Localize(testClass, Constants.LanguageCode2, LocalizationDepth.Shallow);

                Assert.Equal(Constants.AnyString2, result.Name);
                AssertReferencePropertiesAreNotLocalized(json, result as ITestClass);
                AssertReferencePropertiesAreNotLocalized(json, result);
                AssertReferencePropertiesAreNotLocalized(json, result.A);
                AssertReferencePropertiesAreNotLocalized(json, result.B);
                AssertReferencePropertiesAreNotLocalized(json, result.AProxy);
                AssertReferencePropertiesAreNotLocalized(json, result.BProxy);
            }

            [Fact]
            public void ReturnsCorrectLocalizationsItemsHaveMultipleLanguages()
            {
                var json = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1,
                    [Constants.LanguageCode2] = Constants.AnyString2
                });

                var testClasses = new[]
                {
                    ProxyFactories.CreateProxyC(ClassFactories.CreateClassC(json)),
                    ProxyFactories.CreateProxyC(ClassFactories.CreateClassC(json))
                };

                var results = ObjectLocalizer.Localize<ProxyC>(testClasses, Constants.LanguageCode1, LocalizationDepth.Shallow).ToList();

                Assert.Equal(Constants.AnyString1, results.ElementAt(0).Name);
                Assert.Equal(Constants.AnyString1, results.ElementAt(1).Name);
                AssertReferencePropertiesAreNotLocalized(json, results.ElementAt(0) as ITestClass);
                AssertReferencePropertiesAreNotLocalized(json, results.ElementAt(0));
                AssertReferencePropertiesAreNotLocalized(json, results.ElementAt(0).A);
                AssertReferencePropertiesAreNotLocalized(json, results.ElementAt(0).B);
                AssertReferencePropertiesAreNotLocalized(json, results.ElementAt(0).AProxy);
                AssertReferencePropertiesAreNotLocalized(json, results.ElementAt(0).BProxy);
                AssertReferencePropertiesAreNotLocalized(json, results.ElementAt(1) as ITestClass);
                AssertReferencePropertiesAreNotLocalized(json, results.ElementAt(1));
                AssertReferencePropertiesAreNotLocalized(json, results.ElementAt(1).A);
                AssertReferencePropertiesAreNotLocalized(json, results.ElementAt(1).B);
                AssertReferencePropertiesAreNotLocalized(json, results.ElementAt(1).AProxy);
                AssertReferencePropertiesAreNotLocalized(json, results.ElementAt(1).BProxy);
            }

            private static void AssertReferencePropertiesAreNotLocalized(string json, ITestClass testClass)
            {
                // You can even add more to this go wild and try to wrap your head around the references.
                Assert.Equal(json, testClass.A.Name);

                Assert.Equal(json, testClass.A.A.Name);
                AssertReferenceListPropertiesAreNotLocalized(json, testClass.A);

                Assert.Equal(json, testClass.A.B.Name);
                AssertReferenceListPropertiesAreNotLocalized(json, testClass.B);
            }

            private static void AssertReferencePropertiesAreNotLocalized(string json, ITestProxyClass testClass)
            {
                // You can even add more to this go wild and try to wrap your head around the references.
                Assert.Equal(json, testClass.AProxy.Name);

                Assert.Equal(json, testClass.AProxy.AProxy.Name);
                AssertReferenceListPropertiesAreNotLocalized(json, testClass.AProxy);

                Assert.Equal(json, testClass.AProxy.BProxy.Name);
                AssertReferenceListPropertiesAreNotLocalized(json, testClass.BProxy);
            }

            private static void AssertReferenceListPropertiesAreNotLocalized(string json, ITestClass testClass)
            {
                // You can even add more to this go wild and try to wrap your head around the references.
                foreach (var a in testClass.ListOfA)
                {
                    Assert.Equal(json, a.Name);
                }
                foreach (var a in testClass.A.ListOfA)
                {
                    Assert.Equal(json, a.Name);
                }
                foreach (var b in testClass.A.ListOfB)
                {
                    Assert.Equal(json, b.Name);
                }
                foreach (var b in testClass.A.B.ListOfA)
                {
                    Assert.Equal(json, b.Name);
                }
                foreach (var b in testClass.A.B.ListOfB)
                {
                    Assert.Equal(json, b.Name);
                }

                foreach (var b in testClass.ListOfB)
                {
                    Assert.Equal(json, b.Name);
                }
                foreach (var a in testClass.B.ListOfA)
                {
                    Assert.Equal(json, a.Name);
                }
                foreach (var b in testClass.B.ListOfB)
                {
                    Assert.Equal(json, b.Name);
                }
                foreach (var a in testClass.B.B.ListOfA)
                {
                    Assert.Equal(json, a.Name);
                }
                foreach (var b in testClass.B.B.ListOfB)
                {
                    Assert.Equal(json, b.Name);
                }
            }

            private static void AssertReferenceListPropertiesAreNotLocalized(string json, ITestProxyClass testClass)
            {
                // You can even add more to this go wild and try to wrap your head around the references.
                foreach (var a in testClass.ListOfAProxies)
                {
                    Assert.Equal(json, a.Name);
                }
                foreach (var a in testClass.AProxy.ListOfAProxies)
                {
                    Assert.Equal(json, a.Name);
                }
                foreach (var b in testClass.AProxy.ListOfBProxies)
                {
                    Assert.Equal(json, b.Name);
                }
                foreach (var b in testClass.AProxy.BProxy.ListOfAProxies)
                {
                    Assert.Equal(json, b.Name);
                }
                foreach (var b in testClass.AProxy.BProxy.ListOfBProxies)
                {
                    Assert.Equal(json, b.Name);
                }

                foreach (var b in testClass.ListOfBProxies)
                {
                    Assert.Equal(json, b.Name);
                }
                foreach (var a in testClass.BProxy.ListOfAProxies)
                {
                    Assert.Equal(json, a.Name);
                }
                foreach (var b in testClass.BProxy.ListOfBProxies)
                {
                    Assert.Equal(json, b.Name);
                }
                foreach (var a in testClass.BProxy.BProxy.ListOfAProxies)
                {
                    Assert.Equal(json, a.Name);
                }
                foreach (var b in testClass.BProxy.BProxy.ListOfBProxies)
                {
                    Assert.Equal(json, b.Name);
                }
            }
        }

        public class LocalizeOneLevel : TestBase
        {
            [Fact]
            public void ReturnsNullWhenItemIsNull()
            {
                ClassC testClass = null;

                var result = ObjectLocalizer.Localize(testClass, null, LocalizationDepth.OneLevel);

                Assert.Null(result);
            }

            [Fact]
            public void ReturnsFirstLocalizationWhenLanguageCodeIsNull()
            {
                var json = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1,
                    [Constants.LanguageCode2] = Constants.AnyString2
                });

                var testClass = ClassFactories.CreateClassC(json);
                var result = ObjectLocalizer.Localize(testClass, null, LocalizationDepth.OneLevel);

                Assert.Equal(Constants.AnyString1, result.Name);
                AssertObjectIsLocalized(Constants.AnyString1, result);
                AssertObjectIsNotLocalized(Constants.AnyString1, json, result.A);
                AssertObjectIsNotLocalized(Constants.AnyString1, json, result.B);
            }

            [Fact]
            public void ReturnsFirstLocalizationWhenLanguageCodeIsNotSupported()
            {
                var json = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1,
                    [Constants.LanguageCode2] = Constants.AnyString2
                });

                var testClass = ClassFactories.CreateClassC(json);
                var result = ObjectLocalizer.Localize(testClass, Constants.LanguageCode3, LocalizationDepth.OneLevel);

                Assert.Equal(Constants.AnyString1, result.Name);
                AssertObjectIsLocalized(Constants.AnyString1, result);
                AssertObjectIsNotLocalized(Constants.AnyString1, json, result.A);
                AssertObjectIsNotLocalized(Constants.AnyString1, json, result.B);
            }

            [Fact]
            public void ReturnsCorrectLocalizationWhenItemHasOneLanguage()
            {
                var json = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1
                });

                var testClass = ClassFactories.CreateClassC(json);
                var result = ObjectLocalizer.Localize(testClass, Constants.LanguageCode1, LocalizationDepth.OneLevel);

                Assert.Equal(Constants.AnyString1, result.Name);
                AssertObjectIsLocalized(Constants.AnyString1, result);
                AssertObjectIsNotLocalized(Constants.AnyString1, json, result.A);
                AssertObjectIsNotLocalized(Constants.AnyString1, json, result.B);
            }

            [Fact]
            public void ReturnsCorrectLocalizationWhenItemHasMultipleLanguages()
            {
                var json = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1,
                    [Constants.LanguageCode2] = Constants.AnyString2
                });

                var testClass = ClassFactories.CreateClassC(json);
                var result = ObjectLocalizer.Localize(testClass, Constants.LanguageCode2, LocalizationDepth.OneLevel);

                Assert.Equal(Constants.AnyString2, result.Name);
                AssertObjectIsLocalized(Constants.AnyString2, result);
                AssertObjectIsNotLocalized(Constants.AnyString2, json, result.A);
                AssertObjectIsNotLocalized(Constants.AnyString2, json, result.B);
            }

            [Fact]
            public void ReturnsCorrectLocalizationsItemsHaveMultipleLanguages()
            {
                var json = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1,
                    [Constants.LanguageCode2] = Constants.AnyString2
                });

                var testClasses = new[]
                {
                    ClassFactories.CreateClassC(json),
                    ClassFactories.CreateClassC(json)
                };

                var results = ObjectLocalizer.Localize<ClassC>(testClasses, Constants.LanguageCode1, LocalizationDepth.OneLevel).ToList();

                Assert.Equal(Constants.AnyString1, results.ElementAt(0).Name);
                AssertObjectIsLocalized(Constants.AnyString1, results.ElementAt(0));
                AssertObjectIsNotLocalized(Constants.AnyString1, json, results.ElementAt(0).A);
                AssertObjectIsNotLocalized(Constants.AnyString1, json, results.ElementAt(0).B);

                Assert.Equal(Constants.AnyString1, results.ElementAt(1).Name);
                AssertObjectIsLocalized(Constants.AnyString1, results.ElementAt(1));
                AssertObjectIsNotLocalized(Constants.AnyString1, json, results.ElementAt(1).A);
                AssertObjectIsNotLocalized(Constants.AnyString1, json, results.ElementAt(1).B);
            }

            private static void AssertObjectIsLocalized(string name, ITestClass testClass)
            {
                Assert.Equal(name, testClass.A.Name);
                foreach (var a in testClass.ListOfA)
                {
                    Assert.Equal(name, a.Name);
                }
                Assert.Equal(name, testClass.B.Name);
                foreach (var b in testClass.ListOfB)
                {
                    Assert.Equal(name, b.Name);
                }
            }

            private static void AssertObjectIsNotLocalized(string name, string json, ITestClass testClass)
            {
                var type = testClass.GetType();

                if (type == typeof(ClassA))
                {
                    Assert.Equal(name, testClass.A.Name);
                    Assert.Equal(name, testClass.ListOfA[0].Name);
                    Assert.Equal(json, testClass.ListOfA[1].Name);

                    Assert.Equal(json, testClass.B.Name);
                    foreach (var b in testClass.ListOfB)
                    {
                        Assert.Equal(json, b.Name);
                    }
                }
                else if (type == typeof(ClassB))
                {
                    Assert.Equal(json, testClass.A.Name);
                    foreach (var a in testClass.ListOfA)
                    {
                        Assert.Equal(json, a.Name);
                    }

                    Assert.Equal(name, testClass.B.Name);
                    Assert.Equal(name, testClass.ListOfB[0].Name);
                    Assert.Equal(json, testClass.ListOfB[1].Name);
                }
                else
                {
                    throw new ArgumentException($"Given test class has type: {type.FullName} is not of supported type");
                }
            }
        }

        public class LocalizeProxyOneLevel : TestBase
        {
            [Fact]
            public void ReturnsNullWhenItemIsNull()
            {
                ProxyC testClass = null;

                var result = ObjectLocalizer.Localize(testClass, null, LocalizationDepth.OneLevel);

                Assert.Null(result);
            }

            [Fact]
            public void ReturnsFirstLocalizationWhenLanguageCodeIsNull()
            {
                var json = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1,
                    [Constants.LanguageCode2] = Constants.AnyString2
                });

                var testClass = ProxyFactories.CreateProxyC(ClassFactories.CreateClassC(json));
                var result = ObjectLocalizer.Localize(testClass, null, LocalizationDepth.OneLevel);

                Assert.Equal(Constants.AnyString1, result.Name);
                AssertObjectIsLocalized(Constants.AnyString1, result as ITestClass);
                AssertObjectIsLocalized(Constants.AnyString1, result);
                AssertObjectIsNotLocalized(Constants.AnyString1, json, result.A);
                AssertObjectIsNotLocalized(Constants.AnyString1, json, result.B);
                AssertObjectIsNotLocalized(Constants.AnyString1, json, result.AProxy);
                AssertObjectIsNotLocalized(Constants.AnyString1, json, result.BProxy);
            }

            [Fact]
            public void ReturnsFirstLocalizationWhenLanguageCodeIsNotSupported()
            {
                var json = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1,
                    [Constants.LanguageCode2] = Constants.AnyString2
                });

                var testClass = ProxyFactories.CreateProxyC(ClassFactories.CreateClassC(json));
                var result = ObjectLocalizer.Localize(testClass, Constants.LanguageCode3, LocalizationDepth.OneLevel);

                Assert.Equal(Constants.AnyString1, result.Name);
                AssertObjectIsLocalized(Constants.AnyString1, result as ITestClass);
                AssertObjectIsLocalized(Constants.AnyString1, result);
                AssertObjectIsNotLocalized(Constants.AnyString1, json, result.A);
                AssertObjectIsNotLocalized(Constants.AnyString1, json, result.B);
                AssertObjectIsNotLocalized(Constants.AnyString1, json, result.AProxy);
                AssertObjectIsNotLocalized(Constants.AnyString1, json, result.BProxy);
            }

            [Fact]
            public void ReturnsCorrectLocalizationWhenItemHasOneLanguage()
            {
                var json = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1
                });

                var testClass = ProxyFactories.CreateProxyC(ClassFactories.CreateClassC(json));
                var result = ObjectLocalizer.Localize(testClass, Constants.LanguageCode1, LocalizationDepth.OneLevel);

                Assert.Equal(Constants.AnyString1, result.Name);
                AssertObjectIsLocalized(Constants.AnyString1, result as ITestClass);
                AssertObjectIsLocalized(Constants.AnyString1, result);
                AssertObjectIsNotLocalized(Constants.AnyString1, json, result.A);
                AssertObjectIsNotLocalized(Constants.AnyString1, json, result.B);
                AssertObjectIsNotLocalized(Constants.AnyString1, json, result.AProxy);
                AssertObjectIsNotLocalized(Constants.AnyString1, json, result.BProxy);
            }

            [Fact]
            public void ReturnsCorrectLocalizationWhenItemHasMultipleLanguages()
            {
                var json = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1,
                    [Constants.LanguageCode2] = Constants.AnyString2
                });

                var testClass = ProxyFactories.CreateProxyC(ClassFactories.CreateClassC(json));
                var result = ObjectLocalizer.Localize(testClass, Constants.LanguageCode2, LocalizationDepth.OneLevel);

                Assert.Equal(Constants.AnyString2, result.Name);
                AssertObjectIsLocalized(Constants.AnyString2, result as ITestClass);
                AssertObjectIsLocalized(Constants.AnyString2, result);
                AssertObjectIsNotLocalized(Constants.AnyString2, json, result.A);
                AssertObjectIsNotLocalized(Constants.AnyString2, json, result.B);
                AssertObjectIsNotLocalized(Constants.AnyString2, json, result.AProxy);
                AssertObjectIsNotLocalized(Constants.AnyString2, json, result.BProxy);
            }

            [Fact]
            public void ReturnsCorrectLocalizationsItemsHaveMultipleLanguages()
            {
                var json = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1,
                    [Constants.LanguageCode2] = Constants.AnyString2
                });

                var testClasses = new[]
                {
                    ProxyFactories.CreateProxyC(ClassFactories.CreateClassC(json)),
                    ProxyFactories.CreateProxyC(ClassFactories.CreateClassC(json))
                };

                var results = ObjectLocalizer.Localize<ProxyC>(testClasses, Constants.LanguageCode1, LocalizationDepth.OneLevel).ToList();

                Assert.Equal(Constants.AnyString1, results.ElementAt(0).Name);
                AssertObjectIsLocalized(Constants.AnyString1, results.ElementAt(0) as ITestClass);
                AssertObjectIsLocalized(Constants.AnyString1, results.ElementAt(0));
                AssertObjectIsNotLocalized(Constants.AnyString1, json, results.ElementAt(0).A);
                AssertObjectIsNotLocalized(Constants.AnyString1, json, results.ElementAt(0).B);
                AssertObjectIsNotLocalized(Constants.AnyString1, json, results.ElementAt(0).AProxy);
                AssertObjectIsNotLocalized(Constants.AnyString1, json, results.ElementAt(0).BProxy);

                Assert.Equal(Constants.AnyString1, results.ElementAt(1).Name);
                AssertObjectIsLocalized(Constants.AnyString1, results.ElementAt(1) as ITestClass);
                AssertObjectIsLocalized(Constants.AnyString1, results.ElementAt(1));
                AssertObjectIsNotLocalized(Constants.AnyString1, json, results.ElementAt(1).A);
                AssertObjectIsNotLocalized(Constants.AnyString1, json, results.ElementAt(1).B);
                AssertObjectIsNotLocalized(Constants.AnyString1, json, results.ElementAt(1).AProxy);
                AssertObjectIsNotLocalized(Constants.AnyString1, json, results.ElementAt(1).BProxy);
            }

            private static void AssertObjectIsLocalized(string name, ITestClass testClass)
            {
                Assert.Equal(name, testClass.A.Name);
                foreach (var a in testClass.ListOfA)
                {
                    Assert.Equal(name, a.Name);
                }
                Assert.Equal(name, testClass.B.Name);
                foreach (var b in testClass.ListOfB)
                {
                    Assert.Equal(name, b.Name);
                }
            }

            private static void AssertObjectIsLocalized(string name, ITestProxyClass testClass)
            {
                Assert.Equal(name, testClass.AProxy.Name);
                foreach (var a in testClass.ListOfAProxies)
                {
                    Assert.Equal(name, a.Name);
                }
                Assert.Equal(name, testClass.BProxy.Name);
                foreach (var b in testClass.ListOfBProxies)
                {
                    Assert.Equal(name, b.Name);
                }
            }

            private static void AssertObjectIsNotLocalized(string name, string json, ITestClass testClass)
            {
                var type = testClass.GetType();

                if (type == typeof(ClassA))
                {
                    Assert.Equal(name, testClass.A.Name);
                    Assert.Equal(name, testClass.ListOfA[0].Name);
                    Assert.Equal(json, testClass.ListOfA[1].Name);

                    Assert.Equal(json, testClass.B.Name);
                    foreach (var b in testClass.ListOfB)
                    {
                        Assert.Equal(json, b.Name);
                    }
                }
                else if (type == typeof(ClassB))
                {
                    Assert.Equal(json, testClass.A.Name);
                    foreach (var a in testClass.ListOfA)
                    {
                        Assert.Equal(json, a.Name);
                    }

                    Assert.Equal(name, testClass.B.Name);
                    Assert.Equal(name, testClass.ListOfB[0].Name);
                    Assert.Equal(json, testClass.ListOfB[1].Name);
                }
                else
                {
                    throw new ArgumentException($"Given test class has type: {type.FullName} is not of supported type");
                }
            }

            private static void AssertObjectIsNotLocalized(string name, string json, ITestProxyClass testClass)
            {
                var type = testClass.GetType();

                if (type == typeof(ProxyA))
                {
                    Assert.Equal(name, testClass.AProxy.Name);
                    Assert.Equal(name, testClass.ListOfAProxies[0].Name);
                    Assert.Equal(json, testClass.ListOfAProxies[1].Name);

                    Assert.Equal(json, testClass.BProxy.Name);
                    foreach (var b in testClass.ListOfBProxies)
                    {
                        Assert.Equal(json, b.Name);
                    }
                }
                else if (type == typeof(ProxyB))
                {
                    Assert.Equal(json, testClass.AProxy.Name);
                    foreach (var a in testClass.ListOfAProxies)
                    {
                        Assert.Equal(json, a.Name);
                    }

                    Assert.Equal(name, testClass.BProxy.Name);
                    Assert.Equal(name, testClass.ListOfBProxies[0].Name);
                    Assert.Equal(json, testClass.ListOfBProxies[1].Name);
                }
                else
                {
                    throw new ArgumentException($"Given test class has type: {type.FullName} is not of supported type");
                }
            }
        }

        public class LocalizeDeep : TestBase
        {
            [Fact]
            public void ReturnsNullWhenItemIsNull()
            {
                ClassC testClass = null;

                var result = ObjectLocalizer.Localize(testClass, null, LocalizationDepth.Deep);

                Assert.Null(result);
            }

            [Fact]
            public void ReturnsFirstLocalizationWhenLanguageCodeIsNull()
            {
                var json = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1,
                    [Constants.LanguageCode2] = Constants.AnyString2
                });

                var testClass = ClassFactories.CreateClassC(json);
                var result = ObjectLocalizer.Localize(testClass, null, LocalizationDepth.Deep);

                Assert.Equal(Constants.AnyString1, result.Name);
                AssertObjectIsLocalized(Constants.AnyString1, result);
                AssertObjectIsLocalized(Constants.AnyString1, result.A);
                AssertObjectIsLocalized(Constants.AnyString1, result.B);
            }

            [Fact]
            public void ReturnsFirstLocalizationWhenLanguageCodeIsNotSupported()
            {
                var json = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1,
                    [Constants.LanguageCode2] = Constants.AnyString2
                });

                var testClass = ClassFactories.CreateClassC(json);
                var result = ObjectLocalizer.Localize(testClass, Constants.LanguageCode3, LocalizationDepth.Deep);

                Assert.Equal(Constants.AnyString1, result.Name);
                AssertObjectIsLocalized(Constants.AnyString1, result);
                AssertObjectIsLocalized(Constants.AnyString1, result.A);
                AssertObjectIsLocalized(Constants.AnyString1, result.B);
            }

            [Fact]
            public void ReturnsCorrectLocalizationWhenItemHasOneLanguage()
            {
                var json = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1
                });

                var testClass = ClassFactories.CreateClassC(json);
                var result = ObjectLocalizer.Localize(testClass, Constants.LanguageCode1, LocalizationDepth.Deep);

                Assert.Equal(Constants.AnyString1, result.Name);
                AssertObjectIsLocalized(Constants.AnyString1, result);
                AssertObjectIsLocalized(Constants.AnyString1, result.A);
                AssertObjectIsLocalized(Constants.AnyString1, result.B);
            }

            [Fact]
            public void ReturnsCorrectLocalizationWhenItemHasMultipleLanguages()
            {
                var json = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1,
                    [Constants.LanguageCode2] = Constants.AnyString2
                });

                var testClass = ClassFactories.CreateClassC(json);
                var result = ObjectLocalizer.Localize(testClass, Constants.LanguageCode2, LocalizationDepth.Deep);

                Assert.Equal(Constants.AnyString2, result.Name);
                AssertObjectIsLocalized(Constants.AnyString2, result);
                AssertObjectIsLocalized(Constants.AnyString2, result.A);
                AssertObjectIsLocalized(Constants.AnyString2, result.B);
            }

            [Fact]
            public void ReturnsCorrectLocalizationsItemsHaveMultipleLanguages()
            {
                var json = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1,
                    [Constants.LanguageCode2] = Constants.AnyString2
                });

                var testClasses = new[]
                {
                    ClassFactories.CreateClassC(json),
                    ClassFactories.CreateClassC(json)
                };

                var results = ObjectLocalizer.Localize<ClassC>(testClasses, Constants.LanguageCode1, LocalizationDepth.Deep).ToList();

                Assert.Equal(Constants.AnyString1, results.ElementAt(0).Name);
                AssertObjectIsLocalized(Constants.AnyString1, results.ElementAt(0));
                AssertObjectIsLocalized(Constants.AnyString1, results.ElementAt(0).A);
                AssertObjectIsLocalized(Constants.AnyString1, results.ElementAt(0).B);

                Assert.Equal(Constants.AnyString1, results.ElementAt(1).Name);
                AssertObjectIsLocalized(Constants.AnyString1, results.ElementAt(1));
                AssertObjectIsLocalized(Constants.AnyString1, results.ElementAt(1).A);
                AssertObjectIsLocalized(Constants.AnyString1, results.ElementAt(1).B);
            }

            private static void AssertObjectIsLocalized(string name, ITestClass testClass)
            {
                // Feel free to  go crazy with recursion here, or add more tests, but you are warned.
                Assert.Equal(name, testClass.A.Name);
                foreach (var a in testClass.ListOfA)
                {
                    Assert.Equal(name, a.Name);
                }
                Assert.Equal(name, testClass.B.Name);
                foreach (var b in testClass.ListOfB)
                {
                    Assert.Equal(name, b.Name);
                }
            }
        }

        public class LocalizeProxyDeep : TestBase
        {
            [Fact]
            public void ReturnsNullWhenItemIsNull()
            {
                ProxyC testClass = null;

                var result = ObjectLocalizer.Localize(testClass, null, LocalizationDepth.Deep);

                Assert.Null(result);
            }

            [Fact]
            public void ReturnsFirstLocalizationWhenLanguageCodeIsNull()
            {
                var json = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1,
                    [Constants.LanguageCode2] = Constants.AnyString2
                });

                var testClass = ProxyFactories.CreateProxyC(ClassFactories.CreateClassC(json));
                var result = ObjectLocalizer.Localize(testClass, null, LocalizationDepth.Deep);

                Assert.Equal(Constants.AnyString1, result.Name);
                AssertObjectIsLocalized(Constants.AnyString1, result as ITestClass);
                AssertObjectIsLocalized(Constants.AnyString1, result);
                AssertObjectIsLocalized(Constants.AnyString1, result.A);
                AssertObjectIsLocalized(Constants.AnyString1, result.B);
                AssertObjectIsLocalized(Constants.AnyString1, result.AProxy);
                AssertObjectIsLocalized(Constants.AnyString1, result.BProxy);
            }

            [Fact]
            public void ReturnsFirstLocalizationWhenLanguageCodeIsNotSupported()
            {
                var json = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1,
                    [Constants.LanguageCode2] = Constants.AnyString2
                });

                var testClass = ProxyFactories.CreateProxyC(ClassFactories.CreateClassC(json));
                var result = ObjectLocalizer.Localize(testClass, Constants.LanguageCode3, LocalizationDepth.Deep);

                Assert.Equal(Constants.AnyString1, result.Name);
                AssertObjectIsLocalized(Constants.AnyString1, result as ITestClass);
                AssertObjectIsLocalized(Constants.AnyString1, result);
                AssertObjectIsLocalized(Constants.AnyString1, result.A);
                AssertObjectIsLocalized(Constants.AnyString1, result.B);
                AssertObjectIsLocalized(Constants.AnyString1, result.AProxy);
                AssertObjectIsLocalized(Constants.AnyString1, result.BProxy);
            }

            [Fact]
            public void ReturnsCorrectLocalizationWhenItemHasOneLanguage()
            {
                var json = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1
                });

                var testClass = ProxyFactories.CreateProxyC(ClassFactories.CreateClassC(json));
                var result = ObjectLocalizer.Localize(testClass, Constants.LanguageCode1, LocalizationDepth.Deep);

                Assert.Equal(Constants.AnyString1, result.Name);
                AssertObjectIsLocalized(Constants.AnyString1, result as ITestClass);
                AssertObjectIsLocalized(Constants.AnyString1, result);
                AssertObjectIsLocalized(Constants.AnyString1, result.A);
                AssertObjectIsLocalized(Constants.AnyString1, result.A);
                AssertObjectIsLocalized(Constants.AnyString1, result.AProxy);
                AssertObjectIsLocalized(Constants.AnyString1, result.BProxy);
            }

            [Fact]
            public void ReturnsCorrectLocalizationWhenItemHasMultipleLanguages()
            {
                var json = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1,
                    [Constants.LanguageCode2] = Constants.AnyString2
                });

                var testClass = ProxyFactories.CreateProxyC(ClassFactories.CreateClassC(json));
                var result = ObjectLocalizer.Localize(testClass, Constants.LanguageCode2, LocalizationDepth.Deep);

                Assert.Equal(Constants.AnyString2, result.Name);
                AssertObjectIsLocalized(Constants.AnyString2, result as ITestClass);
                AssertObjectIsLocalized(Constants.AnyString2, result);
                AssertObjectIsLocalized(Constants.AnyString2, result.A);
                AssertObjectIsLocalized(Constants.AnyString2, result.B);
                AssertObjectIsLocalized(Constants.AnyString2, result.AProxy);
                AssertObjectIsLocalized(Constants.AnyString2, result.BProxy);
            }

            [Fact]
            public void ReturnsCorrectLocalizationsItemsHaveMultipleLanguages()
            {
                var json = ObjectLocalizer.Serialize(new Dictionary<string, string>
                {
                    [Constants.LanguageCode1] = Constants.AnyString1,
                    [Constants.LanguageCode2] = Constants.AnyString2
                });

                var testClasses = new[]
                {
                    ProxyFactories.CreateProxyC(ClassFactories.CreateClassC(json)),
                    ProxyFactories.CreateProxyC(ClassFactories.CreateClassC(json))
                };

                var results = ObjectLocalizer.Localize<ProxyC>(testClasses, Constants.LanguageCode1, LocalizationDepth.Deep).ToList();

                Assert.Equal(Constants.AnyString1, results.ElementAt(0).Name);
                AssertObjectIsLocalized(Constants.AnyString1, results.ElementAt(0) as ITestClass);
                AssertObjectIsLocalized(Constants.AnyString1, results.ElementAt(0));
                AssertObjectIsLocalized(Constants.AnyString1, results.ElementAt(0).A);
                AssertObjectIsLocalized(Constants.AnyString1, results.ElementAt(0).B);
                AssertObjectIsLocalized(Constants.AnyString1, results.ElementAt(0).AProxy);
                AssertObjectIsLocalized(Constants.AnyString1, results.ElementAt(0).BProxy);

                Assert.Equal(Constants.AnyString1, results.ElementAt(1).Name);
                AssertObjectIsLocalized(Constants.AnyString1, results.ElementAt(1) as ITestClass);
                AssertObjectIsLocalized(Constants.AnyString1, results.ElementAt(1));
                AssertObjectIsLocalized(Constants.AnyString1, results.ElementAt(1).A);
                AssertObjectIsLocalized(Constants.AnyString1, results.ElementAt(1).B);
                AssertObjectIsLocalized(Constants.AnyString1, results.ElementAt(1).AProxy);
                AssertObjectIsLocalized(Constants.AnyString1, results.ElementAt(1).BProxy);
            }

            private static void AssertObjectIsLocalized(string name, ITestClass testClass)
            {
                // Feel free to  go crazy with recursion here, or add more tests, but you are warned.
                Assert.Equal(name, testClass.A.Name);
                foreach (var a in testClass.ListOfA)
                {
                    Assert.Equal(name, a.Name);
                }
                Assert.Equal(name, testClass.B.Name);
                foreach (var b in testClass.ListOfB)
                {
                    Assert.Equal(name, b.Name);
                }
            }

            private static void AssertObjectIsLocalized(string name, ITestProxyClass testClass)
            {
                // Feel free to  go crazy with recursion here, or add more tests, but you are warned.
                Assert.Equal(name, testClass.AProxy.Name);
                foreach (var a in testClass.ListOfAProxies)
                {
                    Assert.Equal(name, a.Name);
                }
                Assert.Equal(name, testClass.BProxy.Name);
                foreach (var b in testClass.ListOfBProxies)
                {
                    Assert.Equal(name, b.Name);
                }
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
    }
}
