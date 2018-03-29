using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Xaki.LanguageResolvers;
using Xaki.Sample.Models;
using Xaki.Web.LanguageResolvers;

namespace Xaki.Sample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("zh"),
                    new CultureInfo("ar"),
                    new CultureInfo("es"),
                    new CultureInfo("hi"),
                    new CultureInfo("pt"),
                    new CultureInfo("ru"),
                    new CultureInfo("ja"),
                    new CultureInfo("de"),
                    new CultureInfo("el")
                };

                options.DefaultRequestCulture = new RequestCulture("en", "en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                // TODO support RouteDataRequestCultureProvider (https://joonasw.net/view/aspnet-core-localization-deep-dive)
            });

            services.AddScoped<IObjectLocalizer>(provider => new ObjectLocalizer
            {
                RequiredLanguages = new[] { "en", "zh", "ar", "es", "hi" },
                OptionalLanguages = new[] { "pt", "ru", "ja", "de", "el" },
                LanguageResolvers = new List<ILanguageResolver>
                {
                    new RequestLanguageResolver(provider.GetService<IHttpContextAccessor>()),
                    new CultureInfoLanguageResolver(),
                    new DefaultLanguageResolver("en")
                }
            });

            services.AddDbContext<DataContext>(GetDbContext());

            services.AddMvc();
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
