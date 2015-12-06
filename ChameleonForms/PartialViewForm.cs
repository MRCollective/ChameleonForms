using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using ChameleonForms.FieldGenerators;
using ChameleonForms.Templates;
using ChameleonForms.Utils;

namespace ChameleonForms
{
    /// <summary>
    /// Form that looks like the parent form, but writes to the text writer for the partial otherwise the output is out of order.
    /// </summary>
    internal class PartialViewForm<TModel, TPartialModel> : IForm<TPartialModel>
    {
        private readonly IForm<TModel> _parentForm;
        private readonly HtmlHelper<TPartialModel> _partialViewHtmlHelper;
        private readonly Expression<Func<TModel, TPartialModel>> _partialModelProperty;

        public PartialViewForm(IForm<TModel> parentForm, HtmlHelper<TPartialModel> partialViewHtmlHelper, Expression<Func<TModel, TPartialModel>> partialModelProperty)
        {
            _parentForm = parentForm;
            _partialViewHtmlHelper = partialViewHtmlHelper;
            _partialModelProperty = partialModelProperty;
        }

        public HtmlHelper<TPartialModel> HtmlHelper { get { return _partialViewHtmlHelper; } }
        public IFormTemplate Template { get { return _parentForm.Template; } }

        public void Write(IHtmlString htmlString)
        {
            _partialViewHtmlHelper.ViewContext.Writer.Write(htmlString);
        }

        public IFieldGenerator GetFieldGenerator<T>(Expression<Func<TPartialModel, T>> property)
        {
            using (new SwapHtmlHelperWriter<TModel>(_parentForm.HtmlHelper, _partialViewHtmlHelper.ViewContext.Writer))
            {
                return new DefaultFieldGenerator<TModel, T>(_parentForm.HtmlHelper, _partialModelProperty.Combine(property), Template);
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
