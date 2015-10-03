using System;
using System.Web;
using System.Web.Mvc;
using ChameleonForms.Component.Config;
using ChameleonForms.FieldGenerators;

namespace ChameleonForms.Component
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface ISection<TModel> : IDisposable
    {
        /// <summary>
        /// Outputs a field with passed in HTML.
        /// </summary>
        /// <param name="labelHtml">The HTML for the label part of the field</param>
        /// <param name="elementHtml">The HTML for the field element part of the field</param>
        /// <param name="validationHtml">The HTML for the validation markup part of the field</param>
        /// <param name="metadata">Any field metadata</param>
        /// <param name="isValid">Whether or not the field is valid</param>
        /// <returns>A field configuration that can be used to output the field as well as configure it fluently</returns>
        IFieldConfiguration Field(IHtmlString labelHtml
            , IHtmlString elementHtml
            , IHtmlString validationHtml = null
            , ModelMetadata metadata = null
            , bool isValid = true);

        /// <inheritdoc />
        IHtmlString Begin();

        /// <inheritdoc />
        IHtmlString End();

        /// <inheritdoc />
        IForm<TModel> Form { get; }

        /// <summary>
        /// Initialises the form component; should be called at the end of the constructor of any derived classes.
        /// Writes HTML directly to the page is the component isn't self-closing
        /// </summary>
        void Initialise();

        /// <inheritdoc />
        string ToHtmlString();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        IFieldGenerator GetFieldGenerator<TChild>(System.Linq.Expressions.Expression<Func<TModel, TChild>> property);
    }
}