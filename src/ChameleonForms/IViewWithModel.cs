using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using ChameleonForms.Component;
using ChameleonForms.FieldGenerators;

namespace ChameleonForms
{
    // todo: Should some of this stuff that's more related to a template (e.g. HTML for types of controls be split into a different interface?)
    /// <summary>
    /// Represents a HTML view along with it's model.
    /// </summary>
    /// <typeparam name="TModel">The model type</typeparam>
    public interface IViewWithModel<TModel>
    {
        /// <summary>
        /// Model instance.
        /// </summary>
        TModel Model { get; }
        
        /// <summary>
        /// Returns the metadata for a field represented by a property on the model.
        /// </summary>
        /// <typeparam name="TProperty">The property type</typeparam>
        /// <param name="property">The property to get the metadat for</param>
        /// <returns>The field metadata</returns>
        IFieldMetadata GetFieldMetadata<TProperty>(Expression<Func<TModel, TProperty>> property);

        /// <summary>
        /// The text writer used to write to the view.
        /// </summary>
        TextWriter Writer { get; set;}

        /// <summary>
        /// Write some HTML to the view.
        /// </summary>
        /// <param name="html">The HTML to write</param>
        void Write(IHtml html);

        /// <summary>
        /// Returns the binding name for a field represented by a property on the model.
        /// </summary>
        /// <typeparam name="TProperty">The property type</typeparam>
        /// <param name="property">The property on the model</param>
        /// <returns>The binding name</returns>
        string GetFieldName<TProperty>(Expression<Func<TModel, TProperty>> property);

        /// <summary>
        /// Returns the binding name for a field represented by a property name on the model.
        /// </summary>
        /// <param name="propertyName">The name of the property on the model</param>
        /// <returns>The binding name</returns>
        string GetFieldName(string propertyName);

        /// <summary>
        /// Returns the id for a field represented by a property on the model.
        /// </summary>
        /// <typeparam name="TProperty">The property type</typeparam>
        /// <param name="property">The property on the model</param>
        /// <returns>The id</returns>
        string GetFieldId<TProperty>(Expression<Func<TModel, TProperty>> property);

        /// <summary>
        /// Returns the id for a field represented by a property name on the model.
        /// </summary>
        /// <param name="propertyName">The name of the property on the model</param>
        /// <returns>The id</returns>
        string GetFieldId(string propertyName);

        /// <summary>
        /// Return the validation message HTML for the given property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property</typeparam>
        /// <param name="property">The property to return a validation message for</param>
        /// <param name="validationMessage">Optional message to display if the specified field contains an error</param>
        /// <param name="htmlAttributes">Any HTML attributes to apply to the validation message</param>
        /// <returns>The validation message HTML</returns>
        IHtml ValidationMessageFor<TProperty>(Expression<Func<TModel, TProperty>> property, string validationMessage = null, HtmlAttributes htmlAttributes = null);

        // todo: rename Partial to Subview?
        /// <summary>
        /// Invoke and return the HTML for a partial view against the form as a parent based on a property on the model.
        /// </summary>
        /// <typeparam name="TPartialModel">The type of the property</typeparam>
        /// <param name="form">The parent form</param>
        /// <param name="partialModelProperty">The property to return a partial view for</param>
        /// <param name="partialViewName">The name of the partial view to invoke</param>
        /// <returns></returns>
        IHtml Partial<TPartialModel>(IForm<TModel> form, Expression<Func<TModel, TPartialModel>> partialModelProperty, string partialViewName);

        /// <summary>
        /// Invoke and return the HTML for a partial view against a section as a parent based on a property on the model.
        /// </summary>
        /// <typeparam name="TPartialModel">The type of the property</typeparam>
        /// <param name="section">The parent section</param>
        /// <param name="partialModelProperty">The property to return a partial view for</param>
        /// <param name="partialViewName">The name of the partial view to invoke</param>
        /// <returns></returns>
        IHtml Partial<TPartialModel>(ISection<TModel> section, Expression<Func<TModel, TPartialModel>> partialModelProperty, string partialViewName);
        
        /// <summary>
        /// Add validation HTML attributes for a field.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property</typeparam>
        /// <param name="attrs">Field attributes to modify</param>
        /// <param name="fieldProperty">The property for the field</param>
        void AddValidationAttributes<TProperty>(HtmlAttributes attrs, Expression<Func<TModel, TProperty>> fieldProperty);

        /// <summary>
        /// Creates the HTML for a textarea control for a field represented by a property on the model.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property</typeparam>
        /// <param name="fieldProperty">The property to output a textarea for</param>
        /// <param name="htmlAttributes">Any HTML attributes to apply to the textarea</param>
        /// <returns>The HTML</returns>
        IHtml TextareaFor<TProperty>(Expression<Func<TModel, TProperty>> fieldProperty, HtmlAttributes htmlAttributes);

        /// <summary>
        /// Creates the HTML for an input control for a field represented by a property on the model.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property</typeparam>
        /// <param name="fieldProperty">The property to output an input for</param>
        /// <param name="htmlAttributes">Any HTML attributes to apply to the input</param>
        /// <param name="formatString">Optional format string to use to modify the value of the field</param>
        /// <returns>The HTML</returns>
        IHtml InputFor<TProperty>(Expression<Func<TModel, TProperty>> fieldProperty, HtmlAttributes htmlAttributes, string formatString = null);

        /// <summary>
        /// Creates the HTML for a select list control for a field represented by a property on the model.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property</typeparam>
        /// <param name="fieldProperty">The property to output a select list for</param>
        /// <param name="selectList">The list of items to select from</param>
        /// <param name="allowMultipleSelect">Whether or not multiple values are allowed to be selected</param>
        /// <param name="htmlAttributes">Any HTML attributes to apply to the select list</param>
        /// <returns>The HTML</returns>
        IHtml SelectListFor<TProperty>(Expression<Func<TModel, TProperty>> fieldProperty, IEnumerable<SelectListItem> selectList, bool allowMultipleSelect, HtmlAttributes htmlAttributes);

        /// <summary>
        /// Creates the HTML for a radio item control for a field represented by a property on the model.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property</typeparam>
        /// <param name="fieldProperty">The property to output a radio item for</param>
        /// <param name="value">The value of the current item</param>
        /// <param name="htmlAttributes">Any HTML attributes to apply to the radio item</param>
        /// <returns>The HTML</returns>
        IHtml RadioItemFor<TProperty>(Expression<Func<TModel, TProperty>> fieldProperty, string value, HtmlAttributes htmlAttributes);

        /// <summary>
        /// Creates the HTML for a label.
        /// </summary>
        /// <param name="id">The value of the id of the control the label is for</param>
        /// <param name="text">The text to display to the end user</param>
        /// <param name="htmlAttributes">Any HTML attributes to apply to the label</param>
        /// <returns>The HTML</returns>
        IHtml Label(string id, string text, HtmlAttributes htmlAttributes);

        /// <summary>
        /// Returns whether or not the given model type represents a file upload.
        /// </summary>
        /// <param name="modelType">The model type to check</param>
        /// <returns>Whether or not the model type represents a file upload</returns>
        bool IsFile(Type modelType);
    }
}
