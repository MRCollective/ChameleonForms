using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace ChameleonForms.Tests.ModelBinders
{
    class ModelBinderTestBase<TModelType>
    {
        protected async Task<(ModelStateDictionary modelState, TProperty model)> BindAsync<TProperty>(Expression<Func<TModelType, TProperty>> propertyToBind, params string[] submittedValues)
        {
            var serviceProvider = BuildServiceProvider();

            var optionsAccessor = serviceProvider.GetService<IOptions<MvcOptions>>();
            var parameterBinder = serviceProvider.GetService<ParameterBinder>();
            var parameter = new ParameterDescriptor
            {
                Name = ((MemberExpression)propertyToBind.Body).Member.Name,
                ParameterType = typeof(TProperty)
            };

            var form = new FormUrlEncodedContent(
                (submittedValues ?? new string[0]).Select(v => new KeyValuePair<string, string>(parameter.Name, v))
            );
            
            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    RequestServices = serviceProvider,
                    Request = {
                        Body = await form.ReadAsStreamAsync(),
                        ContentType = form.Headers.ContentType.ToString()
                    }
                },
                RouteData = new RouteData(),
                ValueProviderFactories = optionsAccessor.Value.ValueProviderFactories.ToArray()
            };
            var metadataProvider = serviceProvider.GetService<IModelMetadataProvider>();
            var modelMetadata = metadataProvider.GetMetadataForProperty(typeof(TModelType), parameter.Name);

            var modelBinderFactory = serviceProvider.GetService<IModelBinderFactory>();
            var modelBinder = modelBinderFactory.CreateBinder(new ModelBinderFactoryContext
            {
                BindingInfo = parameter.BindingInfo,
                Metadata = modelMetadata,
                CacheToken = parameter
            });

            var x = await controllerContext.HttpContext.Request.ReadFormAsync();

            var modelBindingResult = await parameterBinder.BindModelAsync(
                controllerContext,
                modelBinder,
                await CompositeValueProvider.CreateAsync(controllerContext),
                parameter,
                modelMetadata,
                value: null);

            return (controllerContext.ModelState, (TProperty)modelBindingResult.Model);
        }

        private static ServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddSingleton<ILoggerFactory, NullLoggerFactory>();
            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddMvc();
            services.AddChameleonForms();
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }

        protected void AssertPropertyError<TProperty>(ModelStateDictionary state, Expression<Func<TModelType, TProperty>> property, string error)
        {
            var propertyName = ((MemberExpression)property.Body).Member.Name;
            Assert.That(state.ContainsKey(propertyName), propertyName + " not present in model state");
            Assert.That(state[propertyName].Errors.Count, Is.EqualTo(1), "Expecting an error against " + propertyName);
            Assert.That(state[propertyName].Errors[0].ErrorMessage, Is.EqualTo(error), "Expecting different error message for model state against " + propertyName);
        }
    }
}
