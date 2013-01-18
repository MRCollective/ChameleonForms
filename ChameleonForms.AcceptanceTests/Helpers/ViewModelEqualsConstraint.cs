using System.Collections;
using System.Linq;
using System.Reflection;
using ChameleonForms.AcceptanceTests.ModelBinding.Pages;
using ChameleonForms.Example.Controllers;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace ChameleonForms.AcceptanceTests.Helpers
{
    public static class IsSame
    {
        public static Constraint ViewModelAs(object expectedViewModel)
        {
            return new ViewModelEqualsConstraint(expectedViewModel);
        }
    }

    public class ViewModelEqualsConstraint : Constraint
    {
        private readonly object _expectedViewModel;

        public ViewModelEqualsConstraint(object expectedViewModel)
        {
            _expectedViewModel = expectedViewModel;
        }

        public override bool Matches(object actualViewModel)
        {
            foreach (var property in typeof(ModelBindingViewModel).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (property.IsReadonly())
                    continue;
                var expectedValue = property.GetValue(_expectedViewModel, null);
                var actualValue = property.GetValue(actualViewModel, null);

                if (expectedValue is IEnumerable && !(expectedValue as IEnumerable).Cast<object>().Any())
                    Assert.That(actualValue, Is.Null.Or.Empty);
                else
                    Assert.That(actualValue, Is.EqualTo(expectedValue), property.Name);
            }

            return true;
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }
}
