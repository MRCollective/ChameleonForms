
using AutofacContrib.NSubstitute;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;
using ChameleonForms.FieldGenerators;
using ChameleonForms.FieldGenerators.Handlers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NSubstitute;
using NUnit.Framework;

namespace ChameleonForms.Tests.FieldGenerator.Handlers
{
    abstract class FieldGeneratorHandlerTest<T>
    {
        protected AutoSubstitute Container;
        private IFieldGeneratorHandler<TestFieldViewModel, T> _handler;

        [SetUp]
        public void Setup()
        {
            Container = new AutoSubstitute();
            var fg = Container.Resolve<IFieldGenerator<TestFieldViewModel, T>>();
            var metadata = new EmptyModelMetadataProvider().GetMetadataForType(typeof(T));
            fg.Metadata.Returns(metadata);
            _handler = GetHandler(fg);
        }

        protected abstract IFieldGeneratorHandler<TestFieldViewModel, T> GetHandler(IFieldGenerator<TestFieldViewModel, T> handler);
        
        protected void SetDisplayConfiguration(FieldDisplayType type)
        {
            Container.Resolve<IReadonlyFieldConfiguration>().DisplayType
                .Returns(type);
        }
        
        protected FieldDisplayType GetDisplayType()
        {
            return _handler.GetDisplayType(Container.Resolve<IReadonlyFieldConfiguration>());
        }
    }
}
