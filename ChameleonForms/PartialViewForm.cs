using System;
using System.Linq.Expressions;
using System.Web;

using ChameleonForms.FieldGenerators;
using ChameleonForms.Templates;
using ChameleonForms.Utils;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ChameleonForms
{
    /// <summary>
    /// Form that looks like the parent form, but writes to the text writer for the partial otherwise the output is out of order.
    /// </summary>
    internal class PartialViewForm<TModel, TPartialModel> : IForm<TPartialModel>
    {
        private readonly IForm<TModel> _parentForm;
        private readonly IHtmlHelper<TPartialModel> _partialViewHtmlHelper;
        private readonly Expression<Func<TModel, TPartialModel>> _partialModelProperty;

        public PartialViewForm(IForm<TModel> parentForm, IHtmlHelper<TPartialModel> partialViewHtmlHelper, Expression<Func<TModel, TPartialModel>> partialModelProperty)
        {
            _parentForm = parentForm;
            _partialViewHtmlHelper = partialViewHtmlHelper;
            _partialModelProperty = partialModelProperty;
        }

        public IHtmlHelper<TPartialModel> HtmlHelper { get { return _partialViewHtmlHelper; } }
        public IFormTemplate Template { get { return _parentForm.Template; } }

        public void Write(IHtmlContent IHtmlContent)
        {
            _partialViewHtmlHelper.ViewContext.Writer.Write(IHtmlContent);
        }

        public IFieldGenerator GetFieldGenerator<T>(Expression<Func<TPartialModel, T>> property)
        {
            using (new SwapHtmlHelperWriter<TModel>(_parentForm.HtmlHelper, _partialViewHtmlHelper.ViewContext.Writer))
            {
                return _parentForm.GetFieldGenerator(_partialModelProperty.Combine(property));
            }
        }

        public IForm<TChildPartialModel> CreatePartialForm<TChildPartialModel>(object childPartialModelExpression, HtmlHelper<TChildPartialModel> partialViewHelper)
        {
            var childPartialModelAsExpression = childPartialModelExpression as Expression<Func<TPartialModel, TChildPartialModel>>;
            var partialModelAsExpression = _partialModelProperty.Combine(childPartialModelAsExpression);
            return new PartialViewForm<TModel, TChildPartialModel>(_parentForm, partialViewHelper, partialModelAsExpression);
        }

        public void Dispose() {}
    }
}
