using System.Web.Mvc;
using ChameleonForms.ModelBinders;
using ChameleonForms.Tests.Helpers;
using NUnit.Framework;
using System.Globalization;
using ChameleonForms.Tests.FieldGenerator;

namespace ChameleonForms.Tests.ModelBinders
{
    [TestFixture(TypeArgs = new[]{typeof(TestFlagsEnum)})]
    [TestFixture(TypeArgs = new[]{typeof(TestFlagsEnum?)})]
    class FlagsEnumModelBinderShould<T>
    {
        #region Setup
        private ControllerContext _context;
        private FormCollection _formCollection;

        private const string PropertyName = "Property";
        private const string DisplayName = "Display Name";

        [SetUp]
        public void Setup()
        {
            var c = AutoSubstituteContainer.Create();
            _context = c.Resolve<ControllerContext>();
            _formCollection = new FormCollection();

            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        }

        private ModelBindingContext ArrangeBindingContext()
        {
            var modelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(T));
            modelMetadata.DisplayName = DisplayName;
            return new ModelBindingContext
            {
                ModelName = PropertyName,
                ValueProvider = _formCollection.ToValueProvider(),
                ModelMetadata = modelMetadata,
                ModelState = new ModelStateDictionary(),
            };
        }

        private T BindModel(ModelBindingContext bindingContext)
        {
            return (T) (new FlagsEnumModelBinder().BindModel(_context, bindingContext) ?? default(T));
        }

        private static void AssertModelError(ModelBindingContext context, string error)
        {
            Assert.That(context.ModelState.ContainsKey(PropertyName), PropertyName + " not present in model state");
            Assert.That(context.ModelState[PropertyName].Errors.Count, Is.EqualTo(1), "Expecting an error against " + PropertyName);
            Assert.That(context.ModelState[PropertyName].Errors[0].ErrorMessage, Is.EqualTo(error), "Expecting different error message for model state against " + PropertyName);
        }
        #endregion
        
        [Test]
        public void Use_default_model_binder_when_there_is_no_value([Values("", null)] string value)
        {
            _formCollection[PropertyName] = value;
            var context = ArrangeBindingContext();

            var model = BindModel(context);

            Assert.That(model, Is.EqualTo(default(T)));
            Assert.That(context.ModelState.IsValid);
        }

        [Test]
        public void Use_default_value_when_there_is_an_invalid_value()
        {
            _formCollection[PropertyName] = "invalid";
            var context = ArrangeBindingContext();

            var model = BindModel(context);

            Assert.That(model, Is.EqualTo(default(T)));
        }

        [Test]
        public void Add_modelstate_error_when_there_is_an_invalid_value()
        {
            _formCollection[PropertyName] = "invalid";
            var context = ArrangeBindingContext();

            BindModel(context);

            AssertModelError(context, string.Format("The value 'invalid' is not valid for {0}.", DisplayName));
        }

        [Test]
        public void Return_and_bind_value_if_single_value_ok()
        {
            _formCollection[PropertyName] = TestFlagsEnum.Simplevalue.ToString();
            var context = ArrangeBindingContext();

            var model = BindModel(context);

            Assert.That(model, Is.EqualTo(TestFlagsEnum.Simplevalue));
            Assert.That(context.Model, Is.EqualTo(TestFlagsEnum.Simplevalue));
        }

        [Test]
        public void Return_and_bind_value_if_multiple_value_ok()
        {
            _formCollection[PropertyName] = TestFlagsEnum.Simplevalue + "," + TestFlagsEnum.ValueWithDescriptionAttribute;
            var context = ArrangeBindingContext();

            var model = BindModel(context);

            Assert.That(model, Is.EqualTo(TestFlagsEnum.Simplevalue | TestFlagsEnum.ValueWithDescriptionAttribute));
            Assert.That(context.Model, Is.EqualTo(TestFlagsEnum.Simplevalue | TestFlagsEnum.ValueWithDescriptionAttribute));
        }
    }
}
