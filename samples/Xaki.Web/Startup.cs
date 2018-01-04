using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xaki.Web.Models;

namespace Xaki.Web
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
            services.AddMvc();

            const string connection = @"Server=(localdb)\mssqllocaldb;Database=Xaki;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<DataContext>(options => options.UseSqlServer(connection));

            services.AddSingleton<ILocalizationService>(new LocalizationService("en", "xx"));
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
