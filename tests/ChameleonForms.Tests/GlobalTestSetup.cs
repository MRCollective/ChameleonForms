using ChameleonForms.Templates.Default;
using NUnit.Framework;

namespace ChameleonForms.Tests
{
    [SetUpFixture]
    class GlobalTestSetup
    {
        [OneTimeSetUp]
        public void GlobalSetup()
        {
            FormTemplate.Default = new DefaultFormTemplate();
        }
    }
}
