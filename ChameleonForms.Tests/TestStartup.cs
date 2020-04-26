using System.Collections.Generic;
using ChameleonForms.Templates;
using ChameleonForms.Templates.Default;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
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
            services.AddChameleonForms(b => b.WithoutTemplateTypeRegistration().WithoutHumanizedLabels());
            services.AddHttpContextAccessor();
            services.AddSingleton<RequestScopedModelMetadataDetailsProviderProvider>();
            services.AddScoped<ModelMetadataDetailsProviderProvider>();
            services.AddOptions<MvcOptions>().Configure<RequestScopedModelMetadataDetailsProviderProvider>((options, p) =>
            {
                options.ModelMetadataDetailsProviders.Add(p);
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }

    class ModelMetadataDetailsProviderProvider
    {
        public List<IDisplayMetadataProvider> DisplayMetadataProviders { get; private set; } = new List<IDisplayMetadataProvider>();
    }

    class RequestScopedModelMetadataDetailsProviderProvider : IDisplayMetadataProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequestScopedModelMetadataDetailsProviderProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
        {
            var providerProvider = _httpContextAccessor.HttpContext.RequestServices.GetService<ModelMetadataDetailsProviderProvider>();
            providerProvider.DisplayMetadataProviders.ForEach(dmp => dmp.CreateDisplayMetadata(context));
        }
    }

    class FormTemplateProvider
    {
        public IFormTemplate Template { get; set; } = new DefaultFormTemplate();
    }
}
