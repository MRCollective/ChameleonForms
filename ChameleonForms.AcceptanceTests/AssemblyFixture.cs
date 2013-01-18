using NUnit.Framework;
using TestStack.Seleno.Configuration;

namespace ChameleonForms.AcceptanceTests
{
    [SetUpFixture]
    public class AssemblyFixture
    {
        [SetUp]
        public void SetUp()
        {
            SelenoApplicationRunner.Run("ChameleonForms.Example", 12345);
        }
    }
}
