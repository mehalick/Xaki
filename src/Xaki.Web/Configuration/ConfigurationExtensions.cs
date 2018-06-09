using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xaki.LanguageResolvers;
using Xaki.Web.LanguageResolvers;
using Xaki.Web.ModelBinding;

namespace Xaki.Web.Configuration
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddXaki(this IServiceCollection services, XakiOptions xakiOptions)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = xakiOptions
                    .SupportedLanguages
                    .Select(i => new CultureInfo(i))
                    .ToList();

                options.DefaultRequestCulture = new RequestCulture("en", "en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                // TODO support RouteDataRequestCultureProvider (https://joonasw.net/view/aspnet-core-localization-deep-dive)
            });

            services.AddScoped<IObjectLocalizer>(provider => new ObjectLocalizer
            {
                RequiredLanguages = xakiOptions.RequiredLanguages,
                OptionalLanguages = xakiOptions.OptionalLanguages,
                LanguageResolvers = new List<ILanguageResolver>
                {
                    new RequestLanguageResolver(provider.GetService<IHttpContextAccessor>()),
                    new CultureInfoLanguageResolver(),
                    new DefaultLanguageResolver("en")
                }
            });

            return services;
        }

        public static IMvcBuilder WithXaki(this IMvcBuilder mvc)
        {
            return mvc.AddMvcOptions(options =>
            {
                options.ModelBinderProviders.Insert(0, new LocalizableModelBinderProvider());
            });
        }
    }
}
