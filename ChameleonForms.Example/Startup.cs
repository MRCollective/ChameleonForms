using ChameleonForms.Example.Controllers.Filters;
using ChameleonForms.Templates;
using ChameleonForms.Templates.Bootstrap4;
using ChameleonForms.Templates.Default;
using ChameleonForms.Templates.TwitterBootstrap3;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChameleonForms.Example
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            var views = services.AddControllersWithViews(options => { options.Filters.Add<FormTemplateFilter>(); });

            #if (DEBUG)
                views.AddRazorRuntimeCompilation();
            #endif

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IFormTemplate>(provider =>
            {
                var accessor = provider.GetRequiredService<IHttpContextAccessor>();
                var template = accessor.HttpContext.Request.Cookies["template"] ?? "default";
                if (template.StartsWith("default"))
                    return new DefaultFormTemplate();

                if (template == "bootstrap4")
                    return new Bootstrap4FormTemplate();

                return new TwitterBootstrap3FormTemplate();
            });
            services.AddChameleonForms(b => b.WithoutTemplateTypeRegistration());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
