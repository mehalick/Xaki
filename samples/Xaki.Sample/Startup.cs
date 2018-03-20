using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xaki.Sample.Models;

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
            services.AddScoped<ILocalizationService>(_ => new LocalizationService
            {
                LanguageCodes = new[] { "en", "xx" }
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
