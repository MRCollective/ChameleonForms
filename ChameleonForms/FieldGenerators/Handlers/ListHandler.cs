using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using ChameleonForms.Attributes;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;

namespace ChameleonForms.FieldGenerators.Handlers
{
    /// <summary>
    /// Generates the HTML for the Field Element of list fields as either a select list or a list of radio buttons.
    /// </summary>
    /// <typeparam name="TModel">The type of the model the form is being output for</typeparam>
    /// <typeparam name="T">The type of the property in the model that the specific field is being output for</typeparam>
    public class ListHandler<TModel, T> : FieldGeneratorHandler<TModel, T>
    {
        /// <summary>
        /// Constructor for the List Field Generator Handler.
        /// </summary>
        /// <param name="fieldGenerator">The field generator for the field</param>
        public ListHandler(IFieldGenerator<TModel, T> fieldGenerator)
            : base(fieldGenerator)
        {}

        public override bool CanHandle()
        {
            return FieldGenerator.Metadata.AdditionalValues.ContainsKey(ExistsInAttribute.ExistsKey)
                && FieldGenerator.Metadata.AdditionalValues[ExistsInAttribute.ExistsKey] as bool? == true;
        }

        public override IHtmlString GenerateFieldHtml(IReadonlyFieldConfiguration fieldConfiguration)
        {
            var model = FieldGenerator.GetModel();
            var selectList = GetSelectList(model);
            return GetSelectListHtml(selectList, FieldGenerator, fieldConfiguration);
        }

        public override void PrepareFieldConfiguration(IFieldConfiguration fieldConfiguration)
        {
            // There is a bug in the unobtrusive validation for numeric fields that are a radio button
            //  when there is a radio button for "no value selected" i.e. value="" then it can't be selected
            //  as an option since it tries to validate the empty string as a number.
            // This turns off unobtrusive validation in that circumstance
            if (fieldConfiguration.DisplayType == FieldDisplayType.List && !FieldGenerator.Metadata.IsRequired && IsNumeric(FieldGenerator) && !HasMultipleValues(FieldGenerator))
                fieldConfiguration.Attr("data-val", "false");

            // If a list is being displayed there is no element for the label to point to so drop it
            if (fieldConfiguration.DisplayType == FieldDisplayType.List)
                fieldConfiguration.WithoutLabel();
        }

        private IEnumerable<SelectListItem> GetSelectList(TModel model)
        {
            var propertyName = (string)FieldGenerator.Metadata.AdditionalValues[ExistsInAttribute.PropertyKey];
            var listProperty = typeof(TModel).GetProperty(propertyName);
            if (model == null)
                throw new ModelNullException(FieldGenerator.GetFieldId());
            var listValue = (IEnumerable)listProperty.GetValue(model, null);
            if (listValue == null)
                throw new ListPropertyNullException(propertyName, FieldGenerator.GetFieldId());
            return GetSelectListUsingPropertyReflection(
                listValue,
                (string)FieldGenerator.Metadata.AdditionalValues[ExistsInAttribute.NameKey],
                (string)FieldGenerator.Metadata.AdditionalValues[ExistsInAttribute.ValueKey]
            );
        }

        private IEnumerable<SelectListItem> GetSelectListUsingPropertyReflection(IEnumerable listValues, string nameProperty, string valueProperty)
        {
            foreach (var item in listValues)
            {
                var name = item.GetType().GetProperty(nameProperty).GetValue(item, null);
                var value = item.GetType().GetProperty(valueProperty).GetValue(item, null);
                yield return new SelectListItem { Selected = IsSelected(value, FieldGenerator), Value = value.ToString(), Text = name.ToString() };
            }
        }

    }
    
    /// <summary>
    /// Exception for when the list property for an [ExistsIn] is null.
    /// </summary>
    public class ListPropertyNullException : Exception
    {
        /// <summary>
        /// Creates a <see cref="ListPropertyNullException"/>.
        /// </summary>
        /// <param name="listPropertyName">The name of the list property that is null</param>
        /// <param name="propertyName">The name of the property that had the [ExistsIn] pointing to the list property</param>
        public ListPropertyNullException(string listPropertyName, string propertyName) : base(string.Format("The list property ({0}) specified in the [ExistsIn] on {1} is null.", listPropertyName, propertyName)) {}
    }

    /// <summary>
    /// Exception that denotes the model in the page is null when it was needed.
    /// </summary>
    public class ModelNullException : Exception
    {
        /// <summary>
        /// Creates a <see cref="ModelNullException"/>.
        /// </summary>
        /// <param name="propertyName">The name of the property that had the [ExistsIn] pointing to the list property</param>
        public ModelNullException(string propertyName) : base(string.Format("The page model is null; please specify a model because it's needed to generate the list for property {0}.", propertyName)) { }
    }
}
