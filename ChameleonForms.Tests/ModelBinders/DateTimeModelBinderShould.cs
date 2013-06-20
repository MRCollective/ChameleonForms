using System;
using System.Web.Mvc;
using ChameleonForms.ModelBinders;
using ChameleonForms.Tests.Helpers;
using NUnit.Framework;

namespace ChameleonForms.Tests.ModelBinders
{
    [TestFixture(TypeArgs = new[]{typeof(DateTime)})]
    [TestFixture(TypeArgs = new[]{typeof(DateTime?)})]
    class DateTimeModelBinderShould<T>
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
            return (T) (new DateTimeModelBinder().BindModel(_context, bindingContext) ?? default(T));
        }

        private static void AssertModelError(ModelBindingContext context, string error)
        {
            Assert.That(context.ModelState.ContainsKey(PropertyName), PropertyName + " not present in model state");
            Assert.That(context.ModelState[PropertyName].Errors.Count, Is.EqualTo(1), "Expecting an error against " + PropertyName);
            Assert.That(context.ModelState[PropertyName].Errors[0].ErrorMessage, Is.EqualTo(error), "Expecting different error message for model state against " + PropertyName);
        }
        #endregion

        [Test]
        public void Use_default_model_binder_when_there_is_no_display_format()
        {
            _formCollection[PropertyName] = "01/13/2013";
            var context = ArrangeBindingContext();

            var model = BindModel(context);

            Assert.That(model, Is.EqualTo(default(T)));
            Assert.That(context.ModelState.IsValid, Is.False);
        }

        [Test]
        public void Use_default_model_binder_when_there_is_a_display_format_but_no_value([Values("", null)] string value)
        {
            _formCollection[PropertyName] = value;
            var context = ArrangeBindingContext();
            context.ModelMetadata.DisplayFormatString = "{0:dd/MM/yyyy}";

            var model = BindModel(context);

            Assert.That(model, Is.EqualTo(default(T)));
            Assert.That(context.ModelState.IsValid);
        }

        [Test]
        public void Use_default_value_when_there_is_a_display_format_and_an_invalid_value()
        {
            _formCollection[PropertyName] = "invalid";
            var context = ArrangeBindingContext();
            context.ModelMetadata.DisplayFormatString = "{0:dd/MM/yyyy}";

            var model = BindModel(context);

            Assert.That(model, Is.EqualTo(default(T)));
        }

        [Test]
        public void Add_modelstate_error_when_there_is_a_display_format_and_an_invalid_value()
        {
            _formCollection[PropertyName] = "invalid";
            var context = ArrangeBindingContext();
            context.ModelMetadata.DisplayFormatString = "{0:dd/MM/yyyy}";

            BindModel(context);

            AssertModelError(context, string.Format("The value 'invalid' is not valid for {0}.", DisplayName));
        }
    }
}
