using System;
using System.Linq.Expressions;
using ChameleonForms.FieldGenerators;
using ChameleonForms.Templates;
using ChameleonForms.Utils;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChameleonForms
{
    /// <summary>
    /// Form that looks like the parent form, but writes to the text writer for the partial which is a different model type otherwise the output is out of order.
    /// </summary>
    internal class PartialViewForm<TModel, TPartialModel> : IForm<TPartialModel>
    {
        private readonly IForm<TModel> _parentForm;
        private readonly IHtmlHelper<TPartialModel> _partialViewHtmlHelper;
        private readonly Expression<Func<TModel, TPartialModel>> _partialModelProperty;

        public PartialViewForm(IForm<TModel> parentForm, IHtmlHelper<TPartialModel> partialViewHtmlHelper, Expression<Func<TModel, TPartialModel>> partialModelProperty)
        {
            partialViewHtmlHelper.ViewData[Constants.ViewDataFormKey] = this;
            _parentForm = parentForm;
            _partialViewHtmlHelper = partialViewHtmlHelper;
            _partialModelProperty = partialModelProperty;
        }

        public IHtmlHelper<TPartialModel> HtmlHelper { get { return _partialViewHtmlHelper; } }
        public IFormTemplate Template { get { return _parentForm.Template; } }

        public void Write(IHtmlContent content)
        {
            _partialViewHtmlHelper.ViewContext.Writer.Write(content);
        }

        public IFieldGenerator GetFieldGenerator<T>(Expression<Func<TPartialModel, T>> property)
        {
            using (new SwapHtmlHelperWriter<TModel>(_parentForm.HtmlHelper, _partialViewHtmlHelper.ViewContext.Writer))
            {
                return _parentForm.GetFieldGenerator(_partialModelProperty.Combine(property));
            }
        }

        public IForm<TPartialModel> CreatePartialForm(IHtmlHelper<TPartialModel> partialViewHelper)
        {
            return partialViewHelper == HtmlHelper
                ? (IForm<TPartialModel>) this
                : new PartialViewForm<TPartialModel>(this, partialViewHelper);
        }

        public IForm<TChildPartialModel> CreatePartialForm<TChildPartialModel>(LambdaExpression childPartialModelExpression, IHtmlHelper<TChildPartialModel> partialViewHelper)
        {
            var childPartialModelAsExpression = childPartialModelExpression as Expression<Func<TPartialModel, TChildPartialModel>>;
            var partialModelAsExpression = _partialModelProperty.Combine(childPartialModelAsExpression);
            return new PartialViewForm<TModel, TChildPartialModel>(_parentForm, partialViewHelper, partialModelAsExpression);
        }

        public void Dispose()
        {
            _partialViewHtmlHelper.ViewData[Constants.ViewDataFormKey] = _parentForm;
        }
    }

    /// <summary>
    /// Form that looks like the parent form, but writes to the text writer for the partial otherwise the output is out of order.
    /// </summary>
    internal class PartialViewForm<TModel> : IForm<TModel>
    {
        private readonly IForm<TModel> _parentForm;
        private readonly IHtmlHelper<TModel> _partialViewHtmlHelper;

        public PartialViewForm(IForm<TModel> parentForm, IHtmlHelper<TModel> partialViewHtmlHelper)
        {
            partialViewHtmlHelper.ViewData[Constants.ViewDataFormKey] = this;
            _parentForm = parentForm;
            _partialViewHtmlHelper = partialViewHtmlHelper;
        }

        public IHtmlHelper<TModel> HtmlHelper { get { return _partialViewHtmlHelper; } }
        public IFormTemplate Template { get { return _parentForm.Template; } }

        public void Write(IHtmlContent content)
        {
            _partialViewHtmlHelper.ViewContext.Writer.Write(content);
        }

        public IFieldGenerator GetFieldGenerator<T>(Expression<Func<TModel, T>> property)
        {
            using (new SwapHtmlHelperWriter<TModel>(_parentForm.HtmlHelper, _partialViewHtmlHelper.ViewContext.Writer))
            {
                return _parentForm.GetFieldGenerator(property);
            }
        }

        public IForm<TModel> CreatePartialForm(IHtmlHelper<TModel> partialViewHelper)
        {
            return partialViewHelper == HtmlHelper
                ? this
                : new PartialViewForm<TModel>(this, partialViewHelper);
        }

        public IForm<TChildPartialModel> CreatePartialForm<TChildPartialModel>(LambdaExpression childPartialModelExpression, IHtmlHelper<TChildPartialModel> partialViewHelper)
        {
            var childPartialModelAsExpression = childPartialModelExpression as Expression<Func<TModel, TChildPartialModel>>;
            return new PartialViewForm<TModel, TChildPartialModel>(_parentForm, partialViewHelper, childPartialModelAsExpression);
        }

        public void Dispose()
        {
            _partialViewHtmlHelper.ViewData[Constants.ViewDataFormKey] = _parentForm;
        }
    }
}
