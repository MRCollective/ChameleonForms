using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ChameleonForms.AcceptanceTests.Helpers.Pages.Fields;

namespace ChameleonForms.AcceptanceTests.Helpers.Pages
{
    public interface IChameleonFormsPage {}

    public abstract class ChameleonFormsPage<T> : IChameleonFormsPage where T : class, new()
    {
        protected ChameleonFormsPage(IDocument content)
        {
            Content = content;
        }

        protected IDocument Content { get; set; }

        public string Source => Content.Body.InnerHtml;

        public void InputModel(T model)
        {
            InputModel(model, "");
        }

        private void InputModel(object model, string prefix)
        {
            foreach (var property in model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var propertyName = string.IsNullOrEmpty(prefix)
                    ? property.Name
                    : $"{prefix}.{property.Name}";

                if (property.IsReadonly())
                    continue;

                if (!property.PropertyType.IsValueType && property.PropertyType != typeof(string) && !typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    InputModel(property.GetValue(model, null), propertyName);
                    continue;
                }

                var elements = Content.GetElementsByName(propertyName);
                var format = property.GetFormatString();
                FieldFactory.Create(elements).Set(new ModelFieldValue(property.GetValue(model, null), format));
            }
        }

        public T GetFormValues()
        {
            return (T)GetFormValues(typeof(T), "");
        }

        private object GetFormValues(Type typeOfModel, string prefix)
        {
            var vm = Activator.CreateInstance(typeOfModel);
            foreach (var property in vm.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var propertyName = string.IsNullOrEmpty(prefix)
                    ? property.Name
                    : $"{prefix}.{property.Name}";

                if (property.IsReadonly())
                    continue;

                if (!property.PropertyType.IsValueType && property.PropertyType != typeof(string) && !typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    var childValue = GetFormValues(property.PropertyType, propertyName);
                    property.SetValue(vm, childValue, null);
                    continue;
                }

                var elements = Content.GetElementsByName(propertyName);
                var format = property.GetFormatString();
                property.SetValue(vm, FieldFactory.Create(elements).Get(new ModelFieldType(property.PropertyType, format)), null);
            }
            return vm;
        }

        public bool HasValidationErrors()
        {
            return Content.QuerySelectorAll(".field-validation-error").Any();
        }
        
        public TNewPageType GetComponent<TNewPageType, TNewModelType>() where TNewPageType : ChameleonFormsPage<TNewModelType>
            where TNewModelType : class, new()
        {
            return (TNewPageType) Activator.CreateInstance(typeof(TNewPageType), Content);
        }

        public TNewPageType GetComponent<TNewPageType>() where TNewPageType : ChameleonFormsPage<T>
        {
            return (TNewPageType)Activator.CreateInstance(typeof(TNewPageType), Content);
        }

        public async Task<TNewPageType> NavigateToAsync<TNewPageType>(IElement elementToNavigateBy)
        {
            IDocument newPage;

            if (elementToNavigateBy is IHtmlButtonElement button)
            {
                newPage = await button.SubmitAsync();
            }
            else if (elementToNavigateBy is IHtmlInputElement input)
            {
                newPage = await input.SubmitAsync();
            }
            else
            {
                throw new NotSupportedException(elementToNavigateBy.GetType() + " can't be used for navigation.");
            }

            return (TNewPageType) Activator.CreateInstance(typeof(TNewPageType), newPage);
        }
    }
}
