using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Westwind.AspNetCore.Markdown;

namespace Xaki.Docs
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
            services.AddMarkdown();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts(options => options.MaxAge(365).IncludeSubdomains());
            }

            app.UseHttpsRedirection();
            app.UseReferrerPolicy(opts => opts.NoReferrer());
            app.UseXContentTypeOptions();
            app.UseXfo(options => options.Deny());
            app.UseXXssProtection(options => options.EnabledWithBlockMode());
            app.UseCsp(options => options
                .BlockAllMixedContent()
                .DefaultSources(c => c.Self())
                .FontSources(s => s.CustomSources("data:", "https://cdnjs.cloudflare.com", "https://fonts.gstatic.com"))
                .ImageSources(s => s.Self().CustomSources("data:", "https://xaki.azureedge.net", "https://s3.amazonaws.com"))
                .ConnectSources(s => s.Self().CustomSources("https://xaki.azureedge.net", "https://cdnjs.cloudflare.com", "https://fonts.googleapis.com", "https://fonts.gstatic.com", "https://s3.amazonaws.com"))
                .ScriptSources(s => s.Self().UnsafeInline().CustomSources("https://cdnjs.cloudflare.com"))
                .StyleSources(s => s.Self().UnsafeInline().CustomSources("https://cdnjs.cloudflare.com", "https://fonts.googleapis.com")));

            app.UseStaticFiles();
            app.UseMarkdown();
            app.UseMvc();
        }
    }
}
