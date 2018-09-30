using System.Collections.Generic;

namespace Xaki
{
    public interface IObjectLocalizer
    {
        string Serialize(IDictionary<string, string> content);

        IDictionary<string, string> Deserialize(string json);

        bool TryDeserialize(string json, out IDictionary<string, string> localizedContent);

        T Localize<T>(T item, LocalizationDepth depth = LocalizationDepth.Shallow) where T : class, ILocalizable;

        T Localize<T>(T item, string languageCode, LocalizationDepth depth = LocalizationDepth.Shallow) where T : class, ILocalizable;

        IEnumerable<T> Localize<T>(IEnumerable<T> items, LocalizationDepth depth = LocalizationDepth.Shallow) where T : class, ILocalizable;

        IEnumerable<T> Localize<T>(IEnumerable<T> items, string languageCode, LocalizationDepth depth = LocalizationDepth.Shallow) where T : class, ILocalizable;

        HashSet<string> RequiredLanguages { get; }
        HashSet<string> OptionalLanguages { get; }
        HashSet<string> SupportedLanguages { get; }
    }
}
