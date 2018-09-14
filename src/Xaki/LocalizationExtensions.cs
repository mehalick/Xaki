using System.Collections.Generic;
using Xaki.Configuration;

namespace Xaki
{
    public static class LocalizationExtensions
    {
        public static T Localize<T>(this T item, IObjectLocalizer localizer) where T : class, ILocalizable
        {
            return localizer.Localize(item);
        }

        public static IEnumerable<T> Localize<T>(this IEnumerable<T> items, IObjectLocalizer localizer) where T : class, ILocalizable
        {
            return localizer.Localize(items);
        }

        public static T Localize<T>(this T item) where T : class, ILocalizable
        {
            return ObjectLocalizerConfig.Get().Localize(item);
        }

        public static IEnumerable<T> Localize<T>(this IEnumerable<T> items) where T : class, ILocalizable
        {
            return ObjectLocalizerConfig.Get().Localize(items);
        }

        public static T Localize<T>(this T item, string languageCode) where T : class, ILocalizable
        {
            return ObjectLocalizerConfig.Get().Localize(item, languageCode);
        }

        public static IEnumerable<T> Localize<T>(this IEnumerable<T> items, string languageCode) where T : class, ILocalizable
        {
            return ObjectLocalizerConfig.Get().Localize(items, languageCode);
        }
    }
}
