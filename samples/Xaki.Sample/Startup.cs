using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Xaki.LanguageResolvers;
using Xaki.Sample.Models;
using Xaki.Web.LanguageResolvers;
using Xaki.Web.ModelBinding;

namespace Xaki.Sample
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var requiredLanguages = new List<string> { "en", "zh", "ar", "es", "hi" };
            var optionalLanguages = new List<string> { "pt", "ru", "ja", "de", "el" };

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = requiredLanguages
                    .Union(optionalLanguages)
                    .Select(i => new CultureInfo(i))
                    .ToList();

                options.DefaultRequestCulture = new RequestCulture("en", "en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                // TODO support RouteDataRequestCultureProvider (https://joonasw.net/view/aspnet-core-localization-deep-dive)
            });

            services.AddScoped<IObjectLocalizer>(provider => new ObjectLocalizer
            {
                RequiredLanguages = requiredLanguages,
                OptionalLanguages = optionalLanguages,
                LanguageResolvers = new List<ILanguageResolver>
                {
                    new RequestLanguageResolver(provider.GetService<IHttpContextAccessor>()),
                    new CultureInfoLanguageResolver(),
                    new DefaultLanguageResolver("en")
                }
            });

            services.AddDbContext<DataContext>(GetDbContext());

            Action<MvcOptions> setupAction = options =>
            {
                options.ModelBinderProviders.Insert(0, new LocalizableModelBinderProvider());
            };

            services.AddMvc(setupAction).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        private static Action<DbContextOptionsBuilder> GetDbContext()
        {
            const string connection = @"Server=(localdb)\mssqllocaldb;Database=Xaki;Trusted_Connection=True;ConnectRetryCount=0";

            return options => options.UseSqlServer(connection);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }

            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
