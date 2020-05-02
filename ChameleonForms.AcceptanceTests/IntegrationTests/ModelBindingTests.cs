using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ChameleonForms.AcceptanceTests.Helpers;
using ChameleonForms.AcceptanceTests.Helpers.Pages;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace ChameleonForms.AcceptanceTests.IntegrationTests
{
    public class ModelBindingShould : IClassFixture<WebApplicationFactory<Example.Startup>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Example.Startup> _factory;
        private readonly ITestOutputHelper _output;
        private readonly HttpPostCaptureDelegatingHandler _postCapture;

        public ModelBindingShould(WebApplicationFactory<Example.Startup> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
            _postCapture = new HttpPostCaptureDelegatingHandler();
            _client = factory.CreateDefaultClient(_postCapture);
        }

        [Fact]
        public async Task Function_correctly_with_default_form()
        {
            var enteredViewModel = ObjectMother.ModelBindingViewModels.BasicValid;

            var page = await _client.GetPageAsync<ModelBindingExamplePage>("/ExampleForms/ModelBindingExample");
            var pageAfterPostback = await page.SubmitAsync(enteredViewModel);


            _output.WriteLine("### Debug output - POST contents:");
            _postCapture.Posts.ToList().ForEach(p => _output.WriteLine(p));
            _output.WriteLine("###");
            _output.WriteLine("### Debug output - Page HTML after postback:");
            _output.WriteLine(pageAfterPostback.Source);
            _output.WriteLine("###");
            IsSame.ViewModelAs(enteredViewModel, pageAfterPostback.GetFormValues());
            page.HasValidationErrors().ShouldBeFalse();
        }

        [Fact]
        public async Task Function_correctly_with_checkbox_and_radio_lists()
        {
            var enteredViewModel = ObjectMother.ModelBindingViewModels.BasicValid;

            var page = await _client.GetPageAsync<ModelBindingExamplePage>("/ExampleForms/ModelBindingExample2");
            var pageAfterPostback = await page.SubmitAsync(enteredViewModel);

            _output.WriteLine("### Debug output - POST contents:");
            _postCapture.Posts.ToList().ForEach(p => _output.WriteLine(p));
            _output.WriteLine("###");
            _output.WriteLine("### Debug output - Page HTML after postback:");
            _output.WriteLine(pageAfterPostback.Source);
            _output.WriteLine("###");
            IsSame.ViewModelAs(enteredViewModel, pageAfterPostback.GetFormValues());
            page.HasValidationErrors().ShouldBeFalse();
        }
    }
}
