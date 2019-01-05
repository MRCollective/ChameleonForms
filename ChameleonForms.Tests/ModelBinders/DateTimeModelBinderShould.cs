using System;

using ChameleonForms.ModelBinders;
using ChameleonForms.Tests.Helpers;
using NUnit.Framework;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.DataAnnotations.Internal;
using AutofacContrib.NSubstitute;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.Internal;

namespace ChameleonForms.Tests.ModelBinders
{
    [TestFixture(TypeArgs = new[]{typeof(DateTime)})]
    [TestFixture(TypeArgs = new[]{typeof(DateTime?)})]
    class DateTimeModelBinderShould<T>
    {
        #region Setup
        private ControllerContext _context;
        private FormCollection _formCollection;
        private AutoSubstitute _autosubstitute;

        private const string PropertyName = "Property";
        private const string DisplayName = "Display Name";

        [SetUp]
        public void Setup()
        {
            _autosubstitute = AutoSubstituteContainer.Create();
            _context = _autosubstitute.Resolve<ControllerContext>();

            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        }

        private ModelBindingContext ArrangeBindingContext(Action<DefaultMetadataDetails> action = null)
        {
            DefaultMetadataDetails details = new DefaultMetadataDetails(ModelMetadataIdentity.ForType(typeof(T)), ModelAttributes.GetAttributesForType(typeof(T)));
            details.DisplayMetadata = new DisplayMetadata();
            details.DisplayMetadata.DisplayName = ()=> DisplayName;
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
            new DateTimeModelBinder().BindModelAsync(bindingContext).Wait();
            return (T)(bindingContext.Result.Model ?? default(T));
        }

        private static void AssertModelError(ModelBindingContext context, string error)
        {
            Assert.That(context.ModelState.ContainsKey(PropertyName), PropertyName + " not present in model state");
            Assert.That(context.ModelState[PropertyName].Errors.Count, Is.EqualTo(1), "Expecting an error against " + PropertyName);
            Assert.That(context.ModelState[PropertyName].Errors[0].ErrorMessage, Is.EqualTo(error), "Expecting different error message for model state against " + PropertyName);
        }
        #endregion
        
        [Test]
        public void Use_default_model_binder_when_there_is_a_display_format_but_no_value([Values("", null)] string value)
        {
            _formCollection = new FormCollection(new Dictionary<string, StringValues> { { PropertyName, value } });
            
            var context = ArrangeBindingContext(x=> x.DisplayMetadata.DisplayFormatString = "{0:dd/MM/yyyy}");

            var model = BindModel(context);

            Assert.That(model, Is.EqualTo(default(T)));
            //Assert.That(context.ModelState.IsValid);
        }

        [Test]
        public void Use_default_value_when_there_is_a_display_format_and_an_invalid_value()
        {
            _formCollection = new FormCollection(new Dictionary<string, StringValues> { { PropertyName, "invalid" } });
            
            var context = ArrangeBindingContext(x => x.DisplayMetadata.DisplayFormatString = "{0:dd/MM/yyyy}");
            //context.ModelMetadata.DisplayFormatString = "{0:dd/MM/yyyy}";

            var model = BindModel(context);

            Assert.That(model, Is.EqualTo(default(T)));
        }

        [Test]
        public void Add_modelstate_error_when_there_is_a_display_format_and_an_invalid_value()
        {
            _formCollection = new FormCollection(new Dictionary<string, StringValues> { { PropertyName, "invalid" } });
            var context = ArrangeBindingContext(x => x.DisplayMetadata.DisplayFormatString = "{0:dd/MM/yyyy}");

            BindModel(context);

            AssertModelError(context, $"The value 'invalid' is not valid for {DisplayName}. Format of date is {{0:dd/MM/yyyy}}.");
        }

        [Test]
        public void Return_value_if_value_ok()
        {
            _formCollection = new FormCollection(new Dictionary<string, StringValues> { { PropertyName, "12/12/2000" } });
            var context = ArrangeBindingContext(x => x.DisplayMetadata.DisplayFormatString = "{0:dd/MM/yyyy}");
            //context.ModelMetadata.DisplayFormatString = "{0:dd/MM/yyyy}";

            var model = BindModel(context);

            Assert.That(model, Is.EqualTo(new DateTime(2000, 12, 12)));
        }

        [Test]
        public void Bind_value_if_value_ok()
        {
            _formCollection = new FormCollection(new Dictionary<string, StringValues> { { PropertyName, "12/12/2000" } });
            
            var context = ArrangeBindingContext(x => x.DisplayMetadata.DisplayFormatString = "{0:dd/MM/yyyy}");
            //context.ModelMetadata.DisplayFormatString = "{0:dd/MM/yyyy}";

            var model = BindModel(context);

            Assert.That(model, Is.EqualTo(new DateTime(2000, 12, 12)));
        }

        [Test]
        public void Use_default_model_binder_with_g_format_string([Values("en-GB", "uk-UA")]string culture)
        {
            using (ChangeCulture.To(culture))
            {
                var val = DateTime.UtcNow;
                string s = val.ToString("g");

                _formCollection = new FormCollection(new Dictionary<string, StringValues> { { PropertyName, s } });
                val = DateTime.ParseExact(s, "g", CultureInfo.CurrentCulture);
                var context = ArrangeBindingContext(x => x.DisplayMetadata.DisplayFormatString = "{0:g}");
                //context.ModelMetadata.DisplayFormatString = "{0:g}";

                var model = BindModel(context);

                Assert.That(model, Is.EqualTo(val));
                Assert.That(context.ModelState.IsValid, Is.True);
            }
        }
    }
}
