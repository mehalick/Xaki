namespace Xaki.Tests.Common
{
    public class NullLanguageResolver : ILanguageResolver
    {
        public string GetLanguageCode()
        {
            return null;
        }
    }
}
