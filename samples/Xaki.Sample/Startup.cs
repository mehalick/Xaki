using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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

            services.AddScoped<ILocalizationService>(_ => new LocalizationService
            {
                RequiredLanguages = new[] { "en", "xx" },
                LanguageResolvers = new List<ILanguageResolver>
                {
                    new CookieLanguageResolver(_.GetService<IHttpContextAccessor>()),
                    new StaticLanguageResolver("en")
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

            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
