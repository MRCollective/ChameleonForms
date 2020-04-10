using System;
using System.Linq.Expressions;
using ChameleonForms.Component;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;
using ChameleonForms.FieldGenerators;
using ChameleonForms.FieldGenerators.Handlers;
using ChameleonForms.Templates;
using ChameleonForms.Tests.Helpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using NUnit.Framework;

namespace ChameleonForms.Tests.FieldGenerator.Handlers
{
    abstract class FieldGeneratorHandlerTest<T>
    {
        private IFieldGeneratorHandler<TestFieldViewModel, T> _handler;
        private FieldDisplayType _type;

        [SetUp]
        public void Setup()
        {
            var context = new MvcTestContext();
            var fg = new TestFieldGenerator<TestFieldViewModel, T>(context);
            _handler = GetHandler(fg);
        }

        protected abstract IFieldGeneratorHandler<TestFieldViewModel, T> GetHandler(IFieldGenerator<TestFieldViewModel, T> handler);
        
        protected void SetDisplayConfiguration(FieldDisplayType type)
        {
            _type = type;
        }
        
        protected FieldDisplayType GetDisplayType()
        {
            var fc = Substitute.For<IReadonlyFieldConfiguration>();
            fc.DisplayType.Returns(_type);

            return _handler.GetDisplayType(fc);
        }
    }

    class TestFieldGenerator<TModel, T> : IFieldGenerator<TModel, T>
    {
        public TestFieldGenerator(MvcTestContext context)
        {
            var viewContext = context.GetViewTestContext<TModel>();
            HtmlHelper = viewContext.HtmlHelper;
            FieldProperty = null;
            Template = FormTemplate.Default;
            Metadata = new EmptyModelMetadataProvider().GetMetadataForType(typeof(T));
        }

        public ModelMetadata Metadata { get; }
        public IFormTemplate Template { get; }
        public IHtmlHelper<TModel> HtmlHelper { get; }
        public Expression<Func<TModel, T>> FieldProperty { get; }

        public IReadonlyFieldConfiguration PrepareFieldConfiguration(IFieldConfiguration fieldConfiguration, FieldParent fieldParent)
        {
            throw new NotImplementedException();
        }

        public IHtmlContent GetFieldHtml(IReadonlyFieldConfiguration fieldConfiguration)
        {
            throw new NotImplementedException();
        }

        public IHtmlContent GetLabelHtml(IReadonlyFieldConfiguration fieldConfiguration)
        {
            throw new NotImplementedException();
        }

        public IHtmlContent GetValidationHtml(IReadonlyFieldConfiguration fieldConfiguration)
        {
            throw new NotImplementedException();
        }

        public IHtmlContent GetFieldHtml(IFieldConfiguration fieldConfiguration)
        {
            throw new NotImplementedException();
        }

        public IHtmlContent GetLabelHtml(IFieldConfiguration fieldConfiguration)
        {
            throw new NotImplementedException();
        }

        public IHtmlContent GetValidationHtml(IFieldConfiguration fieldConfiguration)
        {
            throw new NotImplementedException();
        }

        public string GetFieldId()
        {
            throw new NotImplementedException();
        }

        public T GetValue()
        {
            throw new NotImplementedException();
        }

        public TModel GetModel()
        {
            throw new NotImplementedException();
        }

        public string GetFieldDisplayName()
        {
            throw new NotImplementedException();
        }
    }
}
