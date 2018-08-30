using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xaki.Sample.Models;
using Xaki.Web.Configuration;

namespace Xaki.Sample
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(GetDbContext());

            services.AddXaki(new XakiOptions
            {
                RequiredLanguages = new List<string> { "en", "zh", "ar", "es", "hi" },
                OptionalLanguages = new List<string> { "pt", "ru", "ja", "de", "el" }
            });

            services.AddMvc().AddXakiModelBinder().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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
