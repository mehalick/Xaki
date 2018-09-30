using System.Collections.Generic;
using Xaki.Configuration;

namespace Xaki
{
    public static class LocalizationExtensions
    {
        public static T Localize<T>(this T item, IObjectLocalizer localizer, LocalizationDepth depth = LocalizationDepth.Shallow) where T : class, ILocalizable
        {
            return localizer.Localize(item, depth);
        }

        public static IEnumerable<T> Localize<T>(this IEnumerable<T> items, IObjectLocalizer localizer, LocalizationDepth depth = LocalizationDepth.Shallow) where T : class, ILocalizable
        {
            return localizer.Localize(items, depth);
        }

        public static T Localize<T>(this T item, LocalizationDepth depth = LocalizationDepth.Shallow) where T : class, ILocalizable
        {
            return ObjectLocalizerConfig.Get().Localize(item, depth);
        }

        public static IEnumerable<T> Localize<T>(this IEnumerable<T> items, LocalizationDepth depth = LocalizationDepth.Shallow) where T : class, ILocalizable
        {
            return ObjectLocalizerConfig.Get().Localize(items, depth);
        }

        public static T Localize<T>(this T item, string languageCode, LocalizationDepth depth = LocalizationDepth.Shallow) where T : class, ILocalizable
        {
            return ObjectLocalizerConfig.Get().Localize(item, languageCode, depth);
        }

        public static IEnumerable<T> Localize<T>(this IEnumerable<T> items, string languageCode, LocalizationDepth depth = LocalizationDepth.Shallow) where T : class, ILocalizable
        {
            return ObjectLocalizerConfig.Get().Localize(items, languageCode, depth);
        }
    }
}
