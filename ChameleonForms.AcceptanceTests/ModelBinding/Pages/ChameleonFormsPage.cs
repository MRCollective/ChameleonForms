using System;
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
            foreach (var property in model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.IsReadonly())
                    continue;

                var elements = ((RemoteWebDriver)Browser).FindElementsByName(property.Name);
                var displayFormatAttr = property.GetCustomAttributes(typeof (DisplayFormatAttribute), false);
                var format = "{0}";
                if (displayFormatAttr.Any())
                    format = displayFormatAttr.Cast<DisplayFormatAttribute>().First().DataFormatString;
                FieldFactory.Create(elements).Set(new ModelFieldValue(property.GetValue(model, null), format));
            }
        }

        public T GetFormValues()
        {
            var vm = new T();
            foreach (var property in vm.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.IsReadonly())
                    continue;

                var elements = ((RemoteWebDriver)Browser).FindElementsByName(property.Name);
                property.SetValue(vm, FieldFactory.Create(elements).Get(new ModelFieldType(property.PropertyType)), null);
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
