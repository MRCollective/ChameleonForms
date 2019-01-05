using AngleSharp.Dom.Html;
using ChameleonForms.AcceptanceTests.ModelBinding.Pages;
using ChameleonForms.AcceptanceTests.ModelBinding.Pages.Fields;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ChameleonForms.AcceptanceTests.Helpers.Pages
{
    public abstract class ChameleongFormsPageBase
    {
        protected ChameleongFormsPageBase(IHtmlDocument content)
        {
            this.Content = content;
        }

        protected IHtmlDocument Content { get; set; }

        protected static void InputModel(object model, string prefix, List<KeyValuePair<string, string>> values)
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
                    InputModel(property.GetValue(model, null), propertyName, values);
                    continue;
                }

                var format = PropertyExtensions.GetFormatStringForProperty(property);
                foreach (var val in new ModelFieldValue(property.GetValue(model, null), format).Values)
                {
                    if (val != null)
                    {
                        values.Add(new KeyValuePair<string, string>(propertyName, val));
                    }
                }
            }
        }

        protected object GetFormValues(Type typeOfModel, string prefix)
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

                var elements = Content.GetElementsByName(propertyName);
                var format = PropertyExtensions.GetFormatStringForProperty(property);
                property.SetValue(vm, FieldFactory.Create(elements).Get(new ModelFieldType(property.PropertyType, format)), null);
            }
            return vm;
        }

        public bool HasValidationErrors()
        {
            return Content.QuerySelectorAll(".field-validation-error").Any();
        }
    }

    public abstract class ChameleonFormsPage<TViewModel> : ChameleongFormsPageBase
    {
        protected ChameleonFormsPage(IHtmlDocument content) : base(content)
        {
        }

        internal static IEnumerable<KeyValuePair<string, string>> InputModel(TViewModel model, string prefix = "")
        {
            var list = new List<KeyValuePair<string, string>>();
            InputModel(model, prefix, list);
            return list;
        }

        public TViewModel GetFormValues()
        {
            return (TViewModel)GetFormValues(typeof(TViewModel), "");
        }

        public T GetComponent<T>() where T : ChameleongFormsPageBase
        {
            return (T)Activator.CreateInstance(typeof(T), Content);
        }
    }
}
