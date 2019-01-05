
using ChameleonForms.ModelBinders;
using ChameleonForms.Tests.Helpers;
using NUnit.Framework;
using System.Globalization;
using ChameleonForms.Tests.FieldGenerator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.Internal;
using System;

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
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        }

        private ModelBindingContext ArrangeBindingContext(Action<DefaultMetadataDetails> action = null)
        {
            DefaultMetadataDetails details = new DefaultMetadataDetails(ModelMetadataIdentity.ForType(typeof(T)), ModelAttributes.GetAttributesForType(typeof(T)));
            details.DisplayMetadata = new DisplayMetadata();
            details.DisplayMetadata.DisplayName = () => DisplayName;
            action?.Invoke(details);

            var modelMetadata = new DefaultModelMetadata(new EmptyModelMetadataProvider(), new DefaultCompositeMetadataDetailsProvider(new IMetadataDetailsProvider[0]), details);

            return new DefaultModelBindingContext
            {
                ModelName = PropertyName,
                ValueProvider = new FormValueProvider(BindingSource.Form, _formCollection, CultureInfo.CurrentCulture),
                ModelMetadata = modelMetadata,
                ModelState = new ModelStateDictionary(),
            };
        }

        private T BindModel(ModelBindingContext bindingContext)
        {
            new FlagsEnumModelBinder().BindModelAsync(bindingContext).Wait();
            return (T) (bindingContext.Result.Model ?? default(T));
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
            _formCollection = new FormCollection(new Dictionary<string, StringValues>{ {PropertyName, value } });
            
            var context = ArrangeBindingContext();

            var model = BindModel(context);

            Assert.That(model, Is.EqualTo(default(T)));
            Assert.That(context.ModelState.IsValid);
        }

        [Test]
        public void Use_default_value_when_there_is_an_invalid_value()
        {
            _formCollection = new FormCollection(new Dictionary<string, StringValues> { { PropertyName, "invalid" } });
            var context = ArrangeBindingContext();

            var model = BindModel(context);

            Assert.That(model, Is.EqualTo(default(T)));
        }

        [Test]
        public void Add_modelstate_error_when_there_is_an_invalid_value()
        {
            _formCollection = new FormCollection(new Dictionary<string, StringValues> { { PropertyName, "invalid" } });
            var context = ArrangeBindingContext();

            BindModel(context);

            AssertModelError(context, string.Format("The value 'invalid' is not valid for {0}.", DisplayName));
        }

        [Test]
        public void Return_and_bind_value_if_single_value_ok()
        {
            _formCollection = new FormCollection(new Dictionary<string, StringValues> { { PropertyName, TestFlagsEnum.Simplevalue.ToString() } });
            var context = ArrangeBindingContext();

            var model = BindModel(context);

            Assert.That(model, Is.EqualTo(TestFlagsEnum.Simplevalue));
        }

        [Test]
        public void Return_and_bind_value_if_multiple_value_ok()
        {
            _formCollection = new FormCollection(new Dictionary<string, StringValues>
            {
                { PropertyName, new[] { TestFlagsEnum.Simplevalue.ToString(), TestFlagsEnum.ValueWithDescriptionAttribute.ToString() } }
            });
            var context = ArrangeBindingContext();

            var model = BindModel(context);

            Assert.That(model, Is.EqualTo(TestFlagsEnum.Simplevalue | TestFlagsEnum.ValueWithDescriptionAttribute));
        }
    }
}
