using System.ComponentModel.DataAnnotations;
using ChameleonForms.Tests.FieldGenerator;

namespace ChameleonForms.Tests.ModelBinders.FlagsEnumModelBinderTests
{
    class TestViewModel<TFlagEnum>
    {
        public TFlagEnum FlagEnum { get; set; }

        [Required]
        public TFlagEnum RequiredFlagEnum { get; set; }

        public TestFlagsEnum ImplicitlyRequiredFlagEnum { get; set; }
    }
}
