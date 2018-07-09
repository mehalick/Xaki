using Microsoft.AspNetCore.Http;

namespace Xaki.AspNetCore.LanguageResolvers
{
    public class CookieLanguageResolver : ILanguageResolver
    {
        public const string CookieKey = ".AspNetCore.Culture";

        private readonly IHttpContextAccessor _contextAccessor;

        public CookieLanguageResolver(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string GetLanguageCode()
        {
            return _contextAccessor.HttpContext.Request.Cookies[CookieKey];
        }
    }
}
