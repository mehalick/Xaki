﻿using System;
using System.Collections.Concurrent;
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

        private static readonly ConcurrentDictionary<Type, IEnumerable<PropertyInfo>> _propertyCache = new ConcurrentDictionary<Type, IEnumerable<PropertyInfo>>();

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

        /// <summary>
        /// Initializes a new serialized collection of localized content items using all supported languages.
        /// </summary>
        public string GetEmptyJsonString()
        {
            var dictionary = SupportedLanguages.ToDictionary(i => i, _ => string.Empty);

            return Serialize(dictionary);
        }

        /// <summary>
        /// Serializes a localized content <see cref="IDictionary{TKey,TValue}"/> to JSON.
        /// </summary>
        public string Serialize(in IDictionary<string, string> content)
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
        public IDictionary<string, string> Deserialize(in string json)
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
        public bool TryDeserialize(in string json, out IDictionary<string, string> localizedContent)
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
        public T Localize<T>(in T item, in LocalizationDepth depth = LocalizationDepth.Shallow)
            where T : class, ILocalizable
        {
            var languageCode = GetLanguageCode();

            return Localize(item, languageCode, depth);
        }

        /// <summary>
        /// Localizes all properties on an <see cref="ILocalizable"/> item with the specified language code.
        /// </summary>
        public T Localize<T>(in T item, in string languageCode, in LocalizationDepth depth = LocalizationDepth.Shallow)
            where T : class, ILocalizable
        {
            if (item is null)
            {
                return null;
            }

            var depthChain = new List<object>(32);

            LocalizeItem(item, SupportedLanguages.Contains(languageCode) ? languageCode : SupportedLanguages.First(), depthChain, depth);

            return item;
        }

        /// <summary>
        /// Localizes all properties on each <see cref="ILocalizable"/> item in a collection with the language code provided by <see cref="ILanguageResolver"/>.
        /// </summary>
        public IEnumerable<T> Localize<T>(in IEnumerable<T> items, LocalizationDepth depth = LocalizationDepth.Shallow)
            where T : class, ILocalizable
        {
            var languageCode = GetLanguageCode();

            return items.Select(item => Localize(item, languageCode, depth));
        }

        /// <summary>
        /// Localizes all properties on each <see cref="ILocalizable"/> item in a collection with the specified language code.
        /// </summary>
        public IEnumerable<T> Localize<T>(in IEnumerable<T> items, string languageCode, LocalizationDepth depth = LocalizationDepth.Shallow)
            where T : class, ILocalizable
        {
            return items.Select(item => Localize(item, languageCode, depth));
        }

        private void LocalizeItem(in object item, in string languageCode, in List<object> depthChain, in LocalizationDepth depth = LocalizationDepth.Shallow)
        {
            foreach (var property in _propertyCache.GetOrAdd(item.GetType(), t => t.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)))
            {
                if (property.IsDefined(typeof(LocalizedAttribute), true))
                {
                    TryLocalizeProperty(item, property, languageCode);
                }
                else if (depth != LocalizationDepth.Shallow)
                {
                    TryLocalizeChildren(item, property, languageCode, depthChain, depth);
                }
            }
        }

        private void TryLocalizeChildren(in object item, in PropertyInfo property, in string languageCode, in List<object> depthChain, in LocalizationDepth depth)
        {
            if (typeof(ILocalizable).IsAssignableFrom(property.PropertyType))
            {
                TryLocalizeProperty(item, property.GetValue(item, null), languageCode, depthChain, depth);
            }
            else if (typeof(IEnumerable<ILocalizable>).IsAssignableFrom(property.PropertyType))
            {
                foreach (var member in (IEnumerable<object>)property.GetValue(item, null))
                {
                    TryLocalizeProperty(item, member, languageCode, depthChain, depth);
                }
            }
        }

        private void TryLocalizeProperty(in object item, in PropertyInfo propertyInfo, in string languageCode)
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

        private void TryLocalizeProperty(in object @base, in object member, in string languageCode, in List<object> depthChain, LocalizationDepth depth = LocalizationDepth.Shallow)
        {
            if (SkipItemLocalization(@base, member))
            {
                return;
            }

            TryAddToDepthChain(@base, depthChain);

            if (!TryAddToDepthChain(member, depthChain))
            {
                return;
            }

            if (depth == LocalizationDepth.OneLevel)
            {
                depth = LocalizationDepth.Shallow;
            }

            LocalizeItem(member, languageCode, depthChain, depth);
        }

        private static bool SkipItemLocalization(in object @base, in object member)
        {
            if (@base is null || member is null)
            {
                return true;
            }

            return ReferenceEquals(@base, member);
        }

        private static bool TryAddToDepthChain(object item, in List<object> depthChain)
        {
            if (item is null)
            {
                return false;
            }

            if (depthChain.AsParallel().Any(x => ReferenceEquals(item, x)))
            {
                return false;
            }

            depthChain.Add(item);

            return true;
        }

        private string GetContentForLanguage(in IDictionary<string, string> localizedContents, in string languageCode)
        {
            if (localizedContents.Count == 0)
            {
                throw new ArgumentException("Cannot localize property, no localized property values exist.", nameof(localizedContents));
            }

            if (localizedContents.TryGetValue(languageCode, out var content) && !string.IsNullOrWhiteSpace(content))
            {
                return content;
            }

            return GetContentForFirstLanguage(localizedContents);
        }

        private string GetContentForFirstLanguage(in IDictionary<string, string> localizedContents)
        {
            return localizedContents.TryGetValue(SupportedLanguages.First(), out var content) ? content : localizedContents.First().Value;
        }
    }
}
