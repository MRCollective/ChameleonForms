using ChameleonForms.Templates;
using ChameleonForms.Templates.Default;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ChameleonForms.Tests
{
    class TestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddScoped<FormTemplateProvider>();
            services.AddScoped(provider => provider.GetRequiredService<FormTemplateProvider>().Template);
            services.AddChameleonForms(humanizeLabels: false, registerDefaultTemplate: false);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }

    class FormTemplateProvider
    {
        public IFormTemplate Template { get; set; } = new DefaultFormTemplate();
    }
}
