using ChameleonForms.AcceptanceTests.Helpers.Pages;
using System.Collections;
using System.Linq;
using System.Reflection;
using Shouldly;

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
                var viewModelPropertyValue = property.GetValue(actualViewModel, null);

                if (!property.PropertyType.IsValueType && property.PropertyType != typeof(string) && !typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    IsSame.ViewModelAs(expectedValue, viewModelPropertyValue);
                    continue;
                }

                if (expectedValue is IEnumerable && !(expectedValue as IEnumerable).Cast<object>().Any())
                {
                    viewModelPropertyValue.ShouldBeNull(customMessage: $"View model property: {property.Name}");
                }
                else
                {
                    viewModelPropertyValue.ShouldBe(expectedValue, $"View model property: {property.Name}");
                }
            }

            return true;
        }
    }
}
