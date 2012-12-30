using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TestStack.Seleno.PageObjects;

namespace ChameleonForms.Tests.ModelBinding.Pages
{
    public abstract class ChameleonFormsPage<T> : Page<T> where T : class, new()
    {
        protected void InputModel(T model)
        {
            foreach (var property in model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!property.IsReadonly())
                    Set(property.Name, property.GetValue(model, null));
            }
        }

        private void Set(string fieldId, object value)
        {
            var elementById = Find().TryFindElement(By.Id(fieldId));
            if (elementById == null)
                return;

            var tagName = elementById.TagName.ToLower();
            var type = elementById.GetAttribute("type").ToLower();

            if (tagName == "input" && type == "checkbox")
            {
                if (elementById.Selected)
                    elementById.Click();
                if (value as bool? == true)
                    elementById.Click();
            }
            else if (tagName == "input" || tagName == "textarea")
            {
                elementById.Clear();
                if (value != null)
                    elementById.SendKeys(value.ToString());
            }
            else if (tagName == "select")
            {
                var select = new SelectElement(elementById);
                if (select.IsMultiple)
                {
                    select.DeselectAll();
                    if (value == null)
                    {
                        select.SelectByIndex(0);
                    }
                    else if (value is IEnumerable)
                    {
                        foreach (var selectedValue in (IEnumerable) value)
                            select.SelectByValue(selectedValue.ToString());
                    }
                    else
                    {
                        select.SelectByValue(value.ToString());
                    }
                }
                else
                {
                    if (value != null)
                        select.SelectByValue(value is bool ? value.ToString().ToLower() : value.ToString());
                    else
                        select.SelectByIndex(0);
                }
            }
        }

        public T GetFormValues()
        {
            var vm = new T();
            foreach (var property in vm.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.IsReadonly())
                    continue;
                var type = property.PropertyType;
                var element = Find().TryFindElement(By.Id(property.Name));
                if (element == null)
                    continue;
                var value = element.GetAttribute("value");
                if (string.IsNullOrEmpty(value))
                {
                    var defaultValue = type.IsValueType ? Activator.CreateInstance(type) : null;
                    property.SetValue(vm, defaultValue, null);
                }
                else
                {
                    var multiple = false;
                    var baseType = type;
                    if (type.IsGenericType && typeof(IEnumerable<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
                    {
                        multiple = true;
                        baseType = type.GetGenericArguments()[0];
                    }

                    var underlyingType = Nullable.GetUnderlyingType(baseType) ?? baseType;
                    var valueToSet = GetValue(underlyingType, value);

                    if (multiple)
                    {
                        var castMethod = typeof(Enumerable).GetMethod("Cast").MakeGenericMethod(type.GetGenericArguments()[0]);
                        var select = new SelectElement(element);
                        var values = select.AllSelectedOptions.Select(o => GetValue(underlyingType, o.GetAttribute("value")));
                        valueToSet = castMethod.Invoke(null, new[] { values });
                    }

                    property.SetValue(vm, valueToSet, null);
                }
            }

            return vm;
        }

        private static object GetValue(Type underlyingType, string value)
        {
            object valueToSet;
            if (underlyingType.IsEnum)
                valueToSet = Enum.Parse(underlyingType, value);
            else
                valueToSet = Convert.ChangeType(value, underlyingType);
            return valueToSet;
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
