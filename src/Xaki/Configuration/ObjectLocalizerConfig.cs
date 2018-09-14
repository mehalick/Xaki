using System;

namespace Xaki.Configuration
{
    public static class ObjectLocalizerConfig
    {
        private static Func<IObjectLocalizer> GetObjectLocalizer;

        public static void Set(Func<IObjectLocalizer> obtain)
        {
            if (obtain is null)
            {
                throw new ArgumentNullException(nameof(obtain));
            }

            GetObjectLocalizer = GetObjectLocalizer ?? obtain;
        }

        internal static IObjectLocalizer Get() => GetObjectLocalizer?.Invoke();
    }
}
