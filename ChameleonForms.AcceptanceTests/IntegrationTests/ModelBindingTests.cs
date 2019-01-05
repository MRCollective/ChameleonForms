using ChameleonForms.AcceptanceTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using RazorPagesProject.Tests.Helpers;
using AngleSharp.Dom.Html;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;
using System.Collections;
using ChameleonForms.AcceptanceTests.ModelBinding.Pages;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System;
using ChameleonForms.Example.Controllers;
using ChameleonForms.AcceptanceTests.ModelBinding.Pages.Fields;

namespace ChameleonForms.AcceptanceTests.ModelBinding
{
    public class ModelBindingShould : IClassFixture<WebApplicationFactory<ChameleonForms.Example.Startup>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<ChameleonForms.Example.Startup>
            _factory;

        public ModelBindingShould(WebApplicationFactory<ChameleonForms.Example.Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Function_correctly_with_default_form()
        {
            var enteredViewModel = ObjectMother.ModelBindingViewModels.BasicValid;

            var page = await _client.GetPageAsync<ModelBindingExamplePage>("/ExampleForms/ModelBindingExample");
            page = await page.SubmitAsync(_client, enteredViewModel);

            enteredViewModel.OptionalNullableEnums = null; // basicvalid.OptionalNullableEnums is List[null], so backward it's ok to have null here
            IsSame.ViewModelAs(enteredViewModel, page.GetFormValues());
            Assert.False(page.HasValidationErrors(), "HasValidationErrors");
        }

        [Fact]
        public async Task Function_correctly_with_checkbox_and_radio_lists()
        {
            var enteredViewModel = ObjectMother.ModelBindingViewModels.BasicValid;

            var page = await _client.GetPageAsync<ModelBindingExamplePage>("/ExampleForms/ModelBindingExample2");
            page = await page.SubmitAsync(_client, enteredViewModel);

            enteredViewModel.OptionalNullableEnums = null; // basicvalid.OptionalNullableEnums is List[null], so backward it's ok to have null here
            IsSame.ViewModelAs(enteredViewModel, page.GetFormValues());
            Assert.False(page.HasValidationErrors());
        }
    }
}
