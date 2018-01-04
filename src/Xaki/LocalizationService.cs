using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Xaki
{
    public class LocalizationService : ILocalizationService
    {
        private readonly IEnumerable<string> _languageCodes;

        public LocalizationService(params string[] languageCodes)
        {
            _languageCodes = languageCodes;
        }

        public string Serialize(IDictionary<string, string> contents)
        {
            var item = new JObject();

            foreach (var languageCode in _languageCodes)
            {
                if (contents.TryGetValue(languageCode, out var value))
                {
                    item[languageCode.ToLowerInvariant()] = value;
                }
            }

            return item.ToString(Formatting.None);
        }

        public IDictionary<string, string> Deserialize(string json)
        {
            var item = JObject.Parse(json);

            return _languageCodes
                .Where(i => item[i] != null)
                .ToDictionary(i => i, i => (string)item[i]);
        }

        /// <summary>
        /// Deserializes a serialized collection of <see cref="IDictionary{TKey,TValue}"/> items, returns a new collection with default languageCode if JSON reader exception occurs.
        /// </summary>
        /// <param name="serializedContents">A JSON serialized localized <see cref="string"/>.</param>
        /// <param name="localizedContent">The output deserialized collection of <see cref="IDictionary{TKey,TValue}"/>.</param>
        public bool TryDeserialize(string serializedContents, out IDictionary<string, string> localizedContent)
        {
            try
            {
                localizedContent = Deserialize(serializedContents);

                return true;
            }
            catch (Exception ex) when (ex is JsonReaderException || ex is JsonSerializationException)
            {
                localizedContent = new Dictionary<string, string>
                {
                    { _languageCodes.First(), "" }
                };

                return false;
            }
        }

        public T Localize<T>(T item, string languageCode) where T : class, ILocalizable
        {
            if (item == null)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(languageCode))
            {
                return item;
            }

            LocalizeProperties(item, languageCode);

            return item;
        }

        private void LocalizeProperties<T>(T item, string languageCode) where T : class, ILocalizable
        {
            var properties = item
                .GetType()
                .GetTypeInfo()
                .DeclaredProperties
                .Where(i => i.IsDefined(typeof(LocalizedAttribute)));

            foreach (var propertyInfo in properties)
            {
                var propertyValue = propertyInfo.GetValue(item)?.ToString();
                if (string.IsNullOrEmpty(propertyValue))
                {
                    continue;
                }

                if (!TryDeserialize(propertyValue, out var localizedContents))
                {
                    continue;
                }

                var contentForLanguage = GetContentForLanguage(localizedContents, languageCode);
                propertyInfo.SetValue(item, contentForLanguage, null);
            }
        }

        private string GetContentForLanguage(IDictionary<string, string> localizedContents, string languageCode)
        {
            if (!localizedContents.Any())
            {
                throw new ArgumentException("Cannot localize property, no localized property values exist.", nameof(localizedContents));
            }

            var localizedContent = localizedContents.SingleOrDefault(i => i.Key.Equals(languageCode, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(i.Value)).Value;

            return localizedContent ?? GetContentForFirstLanguage(localizedContents);
        }

        private string GetContentForFirstLanguage(IDictionary<string, string> localizedContents)
        {
            return localizedContents.SingleOrDefault(i => i.Key.Equals(_languageCodes.First())).Value ??
                   localizedContents.First().Value;
        }

        public IEnumerable<T> Localize<T>(IEnumerable<T> items, string languageCode) where T : class, ILocalizable
        {
            return items.Select(item => Localize(item, languageCode));
        }
    }
}
