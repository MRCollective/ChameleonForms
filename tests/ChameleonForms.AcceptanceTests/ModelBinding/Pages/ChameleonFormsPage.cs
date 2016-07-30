using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using ChameleonForms.AcceptanceTests.ModelBinding.Pages.Fields;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using TestStack.Seleno.PageObjects;

namespace ChameleonForms.AcceptanceTests.ModelBinding.Pages
{
    public abstract class ChameleonFormsPage<T> : Page<T> where T : class, new()
    {
        protected void InputModel(T model)
        {
            InputModel(model, "");
        }

        private void InputModel(object model, string prefix)
        {
            foreach (var property in model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var propertyName = string.IsNullOrEmpty(prefix)
                    ? property.Name
                    : string.Format("{0}.{1}", prefix, property.Name);

                if (property.IsReadonly())
                    continue;

                if (!property.PropertyType.IsValueType && property.PropertyType != typeof(string) && !typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    InputModel(property.GetValue(model, null), propertyName);
                    continue;
                }

                var elements = ((RemoteWebDriver)Browser).FindElementsByName(propertyName);
                var format = GetFormatStringForProperty(property);
                FieldFactory.Create(elements).Set(new ModelFieldValue(property.GetValue(model, null), format));
            }
        }

        public T GetFormValues()
        {
            return (T) GetFormValues(typeof (T), "");
        }

        private object GetFormValues(Type typeOfModel, string prefix)
        {
            var vm = Activator.CreateInstance(typeOfModel);
            foreach (var property in vm.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var propertyName = string.IsNullOrEmpty(prefix)
                    ? property.Name
                    : string.Format("{0}.{1}", prefix, property.Name);

                if (property.IsReadonly())
                    continue;

                if (!property.PropertyType.IsValueType && property.PropertyType != typeof(string) && !typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    var childValue = GetFormValues(property.PropertyType, propertyName);
                    property.SetValue(vm, childValue, null);
                    continue;
                }

                var elements = ((RemoteWebDriver)Browser).FindElementsByName(propertyName);
                var format = GetFormatStringForProperty(property);
                property.SetValue(vm, FieldFactory.Create(elements).Get(new ModelFieldType(property.PropertyType, format)), null);
            }
            return vm;
        }

        public bool HasValidationErrors()
        {
            return new WebDriverWait(Browser, TimeSpan.FromSeconds(1))
                .Until(d => d.FindElements(
                    By.CssSelector(".field-validation-error")
                ))
                .Any();
        }

        private static string GetFormatStringForProperty(PropertyInfo property)
        {
            var displayFormatAttr = property.GetCustomAttributes(typeof(DisplayFormatAttribute), false);
            var format = "{0}";
            if (displayFormatAttr.Any())
                format = displayFormatAttr.Cast<DisplayFormatAttribute>().First().DataFormatString;
            return format;
        }
    }

    public static class PropertyExtensions
    {
        public static bool IsReadonly(this PropertyInfo property)
        {
            var attrs = property.GetCustomAttributes(typeof (ReadOnlyAttribute), false);
            if (attrs.Length > 0)
                return ((ReadOnlyAttribute) attrs[0]).IsReadOnly;
            return false;
        }
    }
}
