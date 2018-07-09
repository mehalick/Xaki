using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace Xaki.AspNetCore.LanguageResolvers
{
    public class RequestLanguageResolver : ILanguageResolver
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public RequestLanguageResolver(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string GetLanguageCode()
        {
            return _contextAccessor
                .HttpContext
                .Features
                .Get<IRequestCultureFeature>()
                .RequestCulture
                .Culture
                .TwoLetterISOLanguageName;
        }
    }
}
