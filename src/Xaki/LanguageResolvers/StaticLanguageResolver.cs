namespace Xaki.LanguageResolvers
{
    public class StaticLanguageResolver : ILanguageResolver
    {
        private readonly string _languageCode;

        public StaticLanguageResolver(string languageCode)
        {
            _languageCode = languageCode;
        }

        public string GetLanguageCode()
        {
            return _languageCode;
        }
    }
}
