using System.ComponentModel;
using System.Reflection;
using ChameleonForms.AcceptanceTests.ModelBinding.Pages.Fields;
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

                var elements = Browser.FindElementsByName(property.Name);
                FieldFactory.Create(elements).Set(new ModelFieldValue(property.GetValue(model, null)));
            }
        }

        public T GetFormValues()
        {
            var vm = new T();
            foreach (var property in vm.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.IsReadonly())
                    continue;

                var elements = Browser.FindElementsByName(property.Name);
                property.SetValue(vm, FieldFactory.Create(elements).Get(new ModelFieldType(property.PropertyType)), null);
            }
            return vm;
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
