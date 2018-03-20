using Microsoft.AspNetCore.Http;

namespace Xaki.Web.LanguageResolvers
{
    public class CookieLanguageResolver : ILanguageResolver
    {
        public const string CookieKey = "xaki-lc";

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
