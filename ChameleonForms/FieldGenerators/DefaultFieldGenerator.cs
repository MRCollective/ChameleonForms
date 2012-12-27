using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using ChameleonForms.Component.Config;

namespace ChameleonForms.FieldGenerators
{
    /// <summary>
    /// The default field HTML generator.
    /// </summary>
    /// <typeparam name="TModel">The type of the view model for the form</typeparam>
    /// <typeparam name="T">The type of the field being generated</typeparam>
    public class DefaultFieldGenerator<TModel, T> : IFieldGenerator<TModel, T>
    {
        /// <summary>
        /// Constructs the field generator.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper for the current view</param>
        /// <param name="fieldProperty">Expression to identify the property to generate the field for</param>
        public DefaultFieldGenerator(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, T>> fieldProperty)
        {
            HtmlHelper = htmlHelper;
            FieldProperty = fieldProperty;
            Metadata = ModelMetadata.FromLambdaExpression(FieldProperty, HtmlHelper.ViewData);
        }

        public ModelMetadata Metadata { get; private set; }
        public HtmlHelper<TModel> HtmlHelper { get; private set; }
        public Expression<Func<TModel, T>> FieldProperty { get; private set; }

        public IHtmlString GetLabelHtml(IFieldConfiguration fieldConfiguration)
        {
            return HtmlHelper.LabelFor(FieldProperty, fieldConfiguration == null ? null : fieldConfiguration.LabelText);
        }

        public IHtmlString GetValidationHtml(IFieldConfiguration fieldConfiguration)
        {
            return HtmlHelper.ValidationMessageFor(FieldProperty);
        }

        public IHtmlString GetFieldHtml(IFieldConfiguration fieldConfiguration)
        {
            fieldConfiguration = fieldConfiguration ?? new FieldConfiguration();
            return FieldGeneratorHandlersRouter<TModel, T>.GetFieldHtml(this, fieldConfiguration);
        }

        public T GetValue()
        {
            return FieldProperty.Compile().Invoke(GetModel());
        }

        public TModel GetModel()
        {
            return (TModel)HtmlHelper.ViewData.ModelMetadata.Model;
        }
    }
}
