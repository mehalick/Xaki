using System.Collections.Generic;

namespace Xaki
{
    public interface ILocalizationService
    {
        string Serialize(IDictionary<string, string> contents);

        IDictionary<string, string> Deserialize(string json);

        T Localize<T>(T item, string languageCode) where T : class, ILocalizable;

        IEnumerable<T> Localize<T>(IEnumerable<T> items, string languageCode) where T : class, ILocalizable;
    }
}
