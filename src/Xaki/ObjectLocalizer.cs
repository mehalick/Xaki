using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Xaki.LanguageResolvers;

namespace Xaki
{
    public class ObjectLocalizer : IObjectLocalizer
    {
        public const string FallbackLanguageCode = "en";

        public IEnumerable<ILanguageResolver> LanguageResolvers { get; set; } = new[] { new DefaultLanguageResolver(FallbackLanguageCode) };
        public HashSet<string> RequiredLanguages { get; set; } = new HashSet<string>(new[] { FallbackLanguageCode }, StringComparer.InvariantCultureIgnoreCase);
        public HashSet<string> OptionalLanguages { get; set; } = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
        public HashSet<string> SupportedLanguages => new HashSet<string>(RequiredLanguages.Union(OptionalLanguages), StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// Initializes a new serialized collection of localized content items using all supported languages.
        /// </summary>
        public string GetEmptyJsonString()
        {
            var dictionary = SupportedLanguages.ToDictionary(i => i, _ => "");

            return Serialize(dictionary);
        }

        /// <summary>
        /// Serializes a localized content <see cref="IDictionary{TKey,TValue}"/> to JSON.
        /// </summary>
        public string Serialize(IDictionary<string, string> content)
        {
            using (var sw = new StringWriter())
            using (var jw = new JsonTextWriter(sw))
            {
                jw.WriteStartObject();
                foreach (var languageCode in SupportedLanguages)
                {
                    if (content.TryGetValue(languageCode, out var value))
                    {
                        jw.WritePropertyName(languageCode);
                        jw.WriteValue(value);
                    }
                }

                jw.WriteEndObject();
                return sw.ToString();
            }
        }

        /// <summary>
        /// Deserializes JSON localized content to <see cref="IDictionary{TKey,TValue}"/>.
        /// </summary>
        public IDictionary<string, string> Deserialize(string json)
        {
            var item = JsonConvert.DeserializeObject<IDictionary<string, string>>(json);

            foreach (var key in item.Keys)
            {
                if (!SupportedLanguages.Contains(key))
                {
                    item.Remove(key);
                }
            }

            return item;
        }

        /// <summary>
        /// Deserializes a serialized collection of <see cref="IDictionary{TKey,TValue}"/> items, returns a new collection with default languageCode if JSON reader exception occurs.
        /// </summary>
        /// <param name="json">JSON serialized localized content.</param>
        /// <param name="localizedContent">A <see cref="IDictionary{TKey,TValue}"/> of localized content.</param>
        public bool TryDeserialize(string json, out IDictionary<string, string> localizedContent)
        {
            try
            {
                localizedContent = Deserialize(json);

                return true;
            }
            catch (Exception ex) when (ex is JsonReaderException || ex is JsonSerializationException)
            {
                localizedContent = new Dictionary<string, string>
                {
                    [SupportedLanguages.First()] = string.Empty
                };

                return false;
            }
        }

        /// <summary>
        /// Localizes all properties on an <see cref="ILocalizable"/> item with the language code provided by <see cref="ILanguageResolver"/>.
        /// </summary>
        public T Localize<T>(T item, LocalizationDepth depth = LocalizationDepth.Shallow) where T : class, ILocalizable
        {
            var languageCode = GetLanguageCode();

            return Localize(item, languageCode, depth);
        }

        /// <summary>
        /// Localizes all properties on an <see cref="ILocalizable"/> item with the specified language code.
        /// </summary>
        public T Localize<T>(T item, string languageCode, LocalizationDepth depth = LocalizationDepth.Shallow) where T : class, ILocalizable
        {
            if (item is null)
            {
                return null;
            }

            if (!SupportedLanguages.Contains(languageCode))
            {
                languageCode = SupportedLanguages.First();
            }

            LocalizeItem(item, languageCode, depth);

            return item;
        }

        /// <summary>
        /// Localizes all properties on each <see cref="ILocalizable"/> item in a collection with the language code provided by <see cref="ILanguageResolver"/>.
        /// </summary>
        public IEnumerable<T> Localize<T>(IEnumerable<T> items, LocalizationDepth depth = LocalizationDepth.Shallow) where T : class, ILocalizable
        {
            var languageCode = GetLanguageCode();

            return items.Select(item => Localize(item, languageCode, depth));
        }

        /// <summary>
        /// Localizes all properties on each <see cref="ILocalizable"/> item in a collection with the specified language code.
        /// </summary>
        public IEnumerable<T> Localize<T>(IEnumerable<T> items, string languageCode, LocalizationDepth depth = LocalizationDepth.Shallow) where T : class, ILocalizable
        {
            return items.Select(item => Localize(item, languageCode, depth));
        }

        /// <summary>
        /// Gets the current language code provided by <see cref="LanguageResolvers"/>.
        /// </summary>
        public string GetLanguageCode()
        {
            foreach (var languageResolver in LanguageResolvers)
            {
                var languageCode = languageResolver.GetLanguageCode();
                if (!string.IsNullOrWhiteSpace(languageCode))
                {
                    return languageCode;
                }
            }

            return FallbackLanguageCode;
        }

        private void LocalizeItem<T>(T item, string languageCode, LocalizationDepth depth = LocalizationDepth.Shallow)
            where T : class, ILocalizable
        {
            //TODO(t): Cache the three following steps/props (in memory ConcurentDictionary) for better pref (if possible)
            foreach (var property in typeof(T).GetTypeInfo().DeclaredProperties)
            {
                if (property.IsDefined(typeof(LocalizedAttribute)))
                {
                    LocalizeProperty(item, property, languageCode);
                }
                else if (depth != LocalizationDepth.Shallow)
                {
                    if (typeof(ILocalizable).IsAssignableFrom(property.DeclaringType))
                    {
                        LocalizeItem(property.GetValue(item, null) as ILocalizable, languageCode, LocalizationDepth.Shallow);
                    }
                    else if (typeof(IEnumerable<ILocalizable>).IsAssignableFrom(property.DeclaringType))
                    {
                        foreach (var child in property.GetValue(item, null) as IEnumerable<ILocalizable>)
                        {
                            LocalizeItem(child, languageCode, LocalizationDepth.Shallow);
                        }
                    }
                }
            }
        }

        private void LocalizeProperty<T>(T item, PropertyInfo propertyInfo, string languageCode)
            where T : class, ILocalizable
        {
            var propertyValue = propertyInfo.GetValue(item)?.ToString();
            if (string.IsNullOrWhiteSpace(propertyValue))
            {
                return;
            }

            if (!TryDeserialize(propertyValue, out var localizedContents))
            {
                return;
            }

            var contentForLanguage = GetContentForLanguage(localizedContents, languageCode);
            propertyInfo.SetValue(item, contentForLanguage, null);
        }

        private string GetContentForLanguage(IDictionary<string, string> localizedContents, string languageCode)
        {
            if (!localizedContents.Keys.Any())
            {
                throw new ArgumentException("Cannot localize property, no localized property values exist.", nameof(localizedContents));
            }

            if (localizedContents.TryGetValue(languageCode, out var content) && !string.IsNullOrWhiteSpace(content))
            {
                return content;
            }

            return GetContentForFirstLanguage(localizedContents);
        }

        private string GetContentForFirstLanguage(IDictionary<string, string> localizedContents)
        {
            if (localizedContents.TryGetValue(SupportedLanguages.First(), out var content))
            {
                return content;
            }

            return localizedContents.First().Value; //Note: Keys are not ordered in a dictionary.
        }
    }
}
