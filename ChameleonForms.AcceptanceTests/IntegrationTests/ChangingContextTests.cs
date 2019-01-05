using ChameleonForms.AcceptanceTests.Helpers;
using ChameleonForms.AcceptanceTests.ModelBinding.Pages;
using Microsoft.AspNetCore.Mvc.Testing;
using RazorPagesProject.Tests.Helpers;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ChameleonForms.AcceptanceTests.ModelBinding
{
    public class ChangingContextShould : IClassFixture<WebApplicationFactory<ChameleonForms.Example.Startup>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<ChameleonForms.Example.Startup>
            _factory;

        public ChangingContextShould(WebApplicationFactory<ChameleonForms.Example.Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Post_different_view_model_and_bind_on_postback()
        {
            var enteredViewModel = ObjectMother.ChangingContextViewModels.DifferentViewModel;

            var page = await _client.GetPageAsync<ChangingContextPage>("/ExampleForms/ChangingContext");
            page = await page.PostDifferentModelAsync(_client, enteredViewModel);;

            IsSame.ViewModelAs(enteredViewModel, page.ReadDifferentModel());
            Assert.False(page.HasValidationErrors());
        }

        [Fact]
        public async Task Post_child_view_model_and_bind_on_postback()
        {
            var enteredViewModel = ObjectMother.ChangingContextViewModels.ChildViewModel;

            var page = await _client.GetPageAsync<ChangingContextPage>("/ExampleForms/ChangingContext");
            page = await page.PostChildModelAsync(_client, enteredViewModel);

            IsSame.ViewModelAs(enteredViewModel, page.ReadChildModel());
            Assert.False(page.HasValidationErrors());
        }

        [Fact]
        public async Task Post_child_view_model_and_bind_to_parent_on_postback()
        {
            var enteredViewModel = ObjectMother.ChangingContextViewModels.ParentViewModel;

            var page = await _client.GetPageAsync<ChangingContextPage>("/ExampleForms/ChangingContext");
            page = await page.PostParentModelAsync(_client, enteredViewModel);
            
            IsSame.ViewModelAs(enteredViewModel, page.ReadParentModel());
            Assert.False(page.HasValidationErrors());
        }
    }
}
