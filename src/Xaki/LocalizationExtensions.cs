using System.Collections.Generic;

namespace Xaki
{
    public static class LocalizationExtensions
    {
        public static IEnumerable<T> Localize<T>(this IEnumerable<T> items, ILocalizationService localizationService) where T : class, ILocalizable
        {
            return localizationService.Localize(items);
        }
    }
}
