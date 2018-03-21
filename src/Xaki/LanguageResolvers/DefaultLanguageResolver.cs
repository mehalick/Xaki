namespace Xaki.LanguageResolvers
{
    public class DefaultLanguageResolver : ILanguageResolver
    {
        private readonly string _languageCode;

        public DefaultLanguageResolver(string languageCode)
        {
            _languageCode = languageCode;
        }

        public string GetLanguageCode()
        {
            return _languageCode;
        }
    }
}
