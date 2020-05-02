using ApprovalTests;
using ApprovalTests.Reporters;
using ChameleonForms.Component.Config;
using ChameleonForms.Templates.Default;
using NUnit.Framework;
using PublicApiGenerator;

namespace ChameleonForms.Tests
{
    public class PublicApiTests
    {
        [Test]
        [UseReporter(typeof(DiffReporter))]
        public void ChameleonFormsPublicApi_ShouldNotChange_WithoutApproval()
        {
            var coreApi = typeof(FieldConfiguration).Assembly.GeneratePublicApi();
            var cfApi = typeof(ServiceCollectionExtensions).Assembly.GeneratePublicApi();
            var templateApi = typeof(DefaultFormTemplate).Assembly.GeneratePublicApi();

            var apiSurface = $"ASSEMBLY ChameleonForms.Core\n{coreApi}\nASSEMBLY ChameleonForms\n{cfApi}\nASSEMBLY ChameleonForms.Templates\n{templateApi}\n";
            apiSurface = apiSurface.Replace(@"[System.Runtime.CompilerServices.Dynamic(new bool[] {
                false,
                true,
                false})] System.Func<object", "System.Func<dynamic")
                .Replace(@"[System.Runtime.CompilerServices.Dynamic]
        public object", "public dynamic")
                .Replace(@"[System.Runtime.CompilerServices.Dynamic]
        object", "dynamic");

            Approvals.Verify(apiSurface);
        }
    }
}
