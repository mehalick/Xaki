using System.Collections.Generic;

namespace Xaki
{
    public interface IObjectLocalizer
    {
        string Serialize(IDictionary<string, string> content);

        IDictionary<string, string> Deserialize(string json);

        bool TryDeserialize(string json, out IDictionary<string, string> localizedContent);

        T Localize<T>(T item) where T : class, ILocalizable;

        T Localize<T>(T item, string languageCode) where T : class, ILocalizable;

        IEnumerable<T> Localize<T>(IEnumerable<T> items) where T : class, ILocalizable;

        IEnumerable<T> Localize<T>(IEnumerable<T> items, string languageCode) where T : class, ILocalizable;

        string GetLanguageCode();

        HashSet<string> RequiredLanguages { get; }
        HashSet<string> OptionalLanguages { get; }
        HashSet<string> SupportedLanguages { get; }
    }
}
