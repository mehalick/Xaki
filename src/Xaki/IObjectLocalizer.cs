using System.Collections.Generic;

namespace Xaki
{
    public interface IObjectLocalizer
    {
        string GetLanguageCode();

        string GetEmptyJsonString();

        string Serialize(in IDictionary<string, string> content);

        IDictionary<string, string> Deserialize(in string json);

        bool TryDeserialize(in string json, out IDictionary<string, string> localizedContent);

        T Localize<T>(in T item, in LocalizationDepth depth = LocalizationDepth.Shallow) where T : class, ILocalizable;

        T Localize<T>(in T item, in string languageCode, in LocalizationDepth depth = LocalizationDepth.Shallow) where T : class, ILocalizable;

        IEnumerable<T> Localize<T>(in IEnumerable<T> items, LocalizationDepth depth = LocalizationDepth.Shallow) where T : class, ILocalizable;

        IEnumerable<T> Localize<T>(in IEnumerable<T> items, string languageCode, LocalizationDepth depth = LocalizationDepth.Shallow) where T : class, ILocalizable;

        HashSet<string> RequiredLanguages { get; }
        HashSet<string> OptionalLanguages { get; }
        HashSet<string> SupportedLanguages { get; }
    }
}
