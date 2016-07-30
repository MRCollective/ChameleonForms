using System.Web;
using ChameleonForms.Enums;
using ChameleonForms.FieldGenerators;
using ChameleonForms.FieldGenerators.Handlers;
using NUnit.Framework;

namespace ChameleonForms.Tests.FieldGenerator.Handlers
{
    class FileHandlerShould : FieldGeneratorHandlerTest<HttpPostedFileBase>
    {
        protected override IFieldGeneratorHandler<TestFieldViewModel, HttpPostedFileBase> GetHandler(IFieldGenerator<TestFieldViewModel, HttpPostedFileBase> handler)
        {
            return new FileHandler<TestFieldViewModel, HttpPostedFileBase>(handler);
        }

        [Test]
        public void Return_file_upload_control_for_display_type()
        {
            var type = GetDisplayType();

            Assert.That(type, Is.EqualTo(FieldDisplayType.FileUpload));
        }
    }
}
