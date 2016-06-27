using System;
using System.Linq.Expressions;
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
        private readonly IViewWithModel<TPartialModel> _partialView;
        private readonly Expression<Func<TModel, TPartialModel>> _partialModelProperty;

        public PartialViewForm(IForm<TModel> parentForm, IViewWithModel<TPartialModel> partialView, Expression<Func<TModel, TPartialModel>> partialModelProperty)
        {
            _parentForm = parentForm;
            _partialView = partialView;
            _partialModelProperty = partialModelProperty;
        }

        public IViewWithModel<TPartialModel> View { get { return _partialView; } }
        public IFormTemplate Template { get { return _parentForm.Template; } }

        public void Write(IHtml htmlString)
        {
            _partialView.Write(htmlString);
        }

        public IFieldGenerator GetFieldGenerator<T>(Expression<Func<TPartialModel, T>> property)
        {
            using (new SwapViewWriter<TModel>(_parentForm.View, _partialView.Writer))
            {
                return new DefaultFieldGenerator<TModel, T>(_parentForm.View, _partialModelProperty.Combine(property), Template);
            }
        }

        public IForm<TChildPartialModel> CreatePartialForm<TChildPartialModel>(object childPartialModelExpression, IViewWithModel<TChildPartialModel> partialViewHelper)
        {
            var childPartialModelAsExpression = childPartialModelExpression as Expression<Func<TPartialModel, TChildPartialModel>>;
            var partialModelAsExpression = _partialModelProperty.Combine(childPartialModelAsExpression);
            return new PartialViewForm<TModel, TChildPartialModel>(_parentForm, partialViewHelper, partialModelAsExpression);
        }

        public void Dispose() {}
    }
}
