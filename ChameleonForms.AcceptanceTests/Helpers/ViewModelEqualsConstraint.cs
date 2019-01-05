using ChameleonForms.AcceptanceTests.Helpers.Pages;
using ChameleonForms.AcceptanceTests.ModelBinding;
using System.Collections;
using System.Linq;
using System.Reflection;
using Xunit;

namespace ChameleonForms.AcceptanceTests.Helpers
{
    public static class IsSame
    {
        public static void ViewModelAs(object expectedViewModel, object actualViewModel)
        {
            new ViewModelEqualsConstraint(expectedViewModel).Matches(actualViewModel);
        }
    }

    public class ViewModelEqualsConstraint
    {
        private readonly object _expectedViewModel;

        public ViewModelEqualsConstraint(object expectedViewModel)
        {
            _expectedViewModel = expectedViewModel;
        }

        public bool Matches(object actualViewModel)
        {
            foreach (var property in actualViewModel.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (property.IsReadonly())
                {
                    continue;
                }

                var expectedValue = property.GetValue(_expectedViewModel, null);
                var actualValue = property.GetValue(actualViewModel, null);

                if (!property.PropertyType.IsValueType && property.PropertyType != typeof(string) && !typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    IsSame.ViewModelAs(expectedValue, actualValue);
                    continue;
                }

                if (expectedValue is IEnumerable && !(expectedValue as IEnumerable).Cast<object>().Any())
                {
                    Assert.Null(actualValue);
                }
                else
                {
                    Assert.Equal(expectedValue, actualValue);
                }
            }

            return true;
        }
    }
}
