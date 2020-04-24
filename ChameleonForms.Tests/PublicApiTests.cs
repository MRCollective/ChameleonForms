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

            Approvals.Verify($"ChameleonForms.Core\n{coreApi}\n\nChameleonForms\n{cfApi}\n\nChameleonForms.Templates\n{templateApi}");
        }
    }
}
