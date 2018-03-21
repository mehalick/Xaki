using System.Globalization;

namespace Xaki.LanguageResolvers
{
    public class CultureInfoLanguageResolver : ILanguageResolver
    {
        public string GetLanguageCode()
        {
            return CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        }
    }
}
