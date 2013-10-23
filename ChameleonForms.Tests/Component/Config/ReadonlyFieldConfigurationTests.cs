using System.Reflection;
using System.Web;
using ChameleonForms.Component.Config;
using NSubstitute;
using NUnit.Framework;

namespace ChameleonForms.Tests.Component.Config
{
    class ReadonlyFieldConfigurationShould
    {
        private readonly IFieldConfiguration _exampleFieldConfiguration = new FieldConfiguration()
            .AddClass("aClass")
            .Append("append")
            .AsList()
            .HideEmptyItem()
            .InlineLabel("inlineLabel")
            .Label("label")
            .OverrideFieldHtml(new HtmlString("override"))
            .Placeholder("placeholder")
            .Prepend("prepend")
            .Readonly()
            .WithFalseAs("FALSE")
            .WithTrueAs("TRUE")
            .WithoutLabel();

        [Test]
        public void ProxyAllPublicGetterPropertiesOnIFieldConfiguration()
        {
            var readonlyFieldConfiguration = new ReadonlyFieldConfiguration(_exampleFieldConfiguration);
            var properties = typeof (IFieldConfiguration).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                if (property.Name == "Attributes")
                    return;

                var readonlyProperty = typeof (IReadonlyFieldConfiguration).GetProperty(property.Name);
                Assert.That(readonlyProperty, Is.Not.Null, string.Format("Expected IReadonlyFieldConfiguration to have a property with type {0} and name {1}.", property.PropertyType.Name, property.Name));

                var readonlyValue = readonlyProperty.GetValue(readonlyFieldConfiguration, null);
                var realValue = property.GetValue(_exampleFieldConfiguration, null);
                Assert.That(readonlyValue, Is.EqualTo(realValue), string.Format("Expected value in FieldConfiguration for property {0} to be equal to value in ReadonlyFieldConfiguration for the same property", property.Name));
            }
        }

        [Test]
        public void ProxyGetDataDataMethodOnIFieldConfiguration()
        {
            const string propertyName = "property";
            const string value = "asdF";
            var fc = Substitute.For<IFieldConfiguration>();
            var readonlyFc = new ReadonlyFieldConfiguration(fc);
            fc.GetBagData<string>(propertyName).Returns(value);

            Assert.That(readonlyFc.GetBagData<string>(propertyName), Is.EqualTo(value));
        }
    }
}
