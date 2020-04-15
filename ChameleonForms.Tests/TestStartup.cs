using ChameleonForms.Templates;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ChameleonForms.Tests
{
    class TestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IRazorViewToStringRenderer, RazorViewToStringRenderer>();
            services.AddSingleton<DefaultTemplate>();
            services.AddMvc();
            services.AddChameleonForms(humanizedLabels: false);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }
}
