using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace ChameleonForms.FieldGenerator
{
    public class DefaultFieldGenerator<TModel, T> : IFieldGenerator
    {
        private readonly HtmlHelper<TModel> _helper;
        private readonly Expression<Func<TModel, T>> _property;

        public DefaultFieldGenerator(HtmlHelper<TModel> helper, Expression<Func<TModel, T>> property)
        {
            _helper = helper;
            _property = property;
        }

        public IHtmlString GetFieldHtml()
        {
            //var metadata = ModelMetadata.FromLambdaExpression(_property, Form.HtmlHelper.ViewData);
            return _helper.TextBoxFor(_property);
        }

        public IHtmlString GetLabelHtml()
        {
            return _helper.LabelFor(_property);
        }

        public IHtmlString GetValidationHtml()
        {
            return _helper.ValidationMessageFor(_property);
        }
    }
}
