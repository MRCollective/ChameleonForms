using System.ComponentModel.DataAnnotations;

namespace ChameleonForms.Tests.ModelBinders.DateTimeModelBinderTests
{
    public class TestViewModel<TBaseType>
    {
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public TBaseType DateTimeWithEditFormat { get; set; }

        [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public TBaseType DateTimeWithGFormat { get; set; }
    }
}
