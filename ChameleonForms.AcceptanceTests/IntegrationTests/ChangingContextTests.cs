using System;
using System.Net.Http;
using System.Threading.Tasks;
using ChameleonForms.AcceptanceTests.Helpers;
using ChameleonForms.AcceptanceTests.Helpers.Pages;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using Xunit;

namespace ChameleonForms.AcceptanceTests.IntegrationTests
{
    public class ChangingContextShould : IClassFixture<WebApplicationFactory<Example.Startup>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Example.Startup>
            _factory;

        public ChangingContextShould(WebApplicationFactory<Example.Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Post_different_view_model_and_bind_on_postback()
        {
            var enteredViewModel = ObjectMother.ChangingContextViewModels.DifferentViewModel;

            var page = await _client.GetPageAsync<ChangingContextPage>("/ExampleForms/ChangingContext");
            page = await page.PostDifferentModelAsync(enteredViewModel);;

            IsSame.ViewModelAs(enteredViewModel, page.ReadDifferentModel());
            page.HasValidationErrors().ShouldBeFalse();
        }

        [Fact]
        public async Task Post_child_view_model_and_bind_on_postback()
        {
            var enteredViewModel = ObjectMother.ChangingContextViewModels.ChildViewModel;

            var page = await _client.GetPageAsync<ChangingContextPage>("/ExampleForms/ChangingContext");
            page = await page.PostChildModelAsync(enteredViewModel);

            IsSame.ViewModelAs(enteredViewModel, page.ReadChildModel());
            page.HasValidationErrors().ShouldBeFalse();
        }

        [Fact]
        public async Task Post_child_view_model_and_bind_to_parent_on_postback()
        {
            var enteredViewModel = ObjectMother.ChangingContextViewModels.ParentViewModel;

            var page = await _client.GetPageAsync<ChangingContextPage>("/ExampleForms/ChangingContext");
            page = await page.PostParentModelAsync(enteredViewModel);
            
            IsSame.ViewModelAs(enteredViewModel, page.ReadParentModel());
            page.HasValidationErrors().ShouldBeFalse();
        }

        [Fact]
        public async Task Post_different_view_model_and_bind_on_postback_taghelpers()
        {
            var enteredViewModel = ObjectMother.ChangingContextViewModels.DifferentViewModel;

            var page = await _client.GetPageAsync<ChangingContextPage>("/ExampleForms/ChangingContextTH");
            page = await page.PostDifferentModelAsync(enteredViewModel); ;

            IsSame.ViewModelAs(enteredViewModel, page.ReadDifferentModel());
            page.HasValidationErrors().ShouldBeFalse();
        }

        [Fact]
        public async Task Post_child_view_model_and_bind_on_postback_taghelpers()
        {
            var enteredViewModel = ObjectMother.ChangingContextViewModels.ChildViewModel;

            var page = await _client.GetPageAsync<ChangingContextPage>("/ExampleForms/ChangingContextTH");
            page = await page.PostChildModelAsync(enteredViewModel);

            IsSame.ViewModelAs(enteredViewModel, page.ReadChildModel());
            page.HasValidationErrors().ShouldBeFalse();
        }

        [Fact]
        public async Task Post_child_view_model_and_bind_to_parent_on_postback_taghelpers()
        {
            var enteredViewModel = ObjectMother.ChangingContextViewModels.ParentViewModel;

            var page = await _client.GetPageAsync<ChangingContextPage>("/ExampleForms/ChangingContextTH");
            page = await page.PostParentModelAsync(enteredViewModel);

            IsSame.ViewModelAs(enteredViewModel, page.ReadParentModel());
            page.HasValidationErrors().ShouldBeFalse();
        }
    }
}
