using System.Collections.Generic;

namespace Xaki
{
    public static class LocalizationExtensions
    {
        public static IEnumerable<T> Localize<T>(this IEnumerable<T> items, IObjectLocalizer localizer) where T : class, ILocalizable
        {
            return localizer.Localize(items);
        }
    }
}
