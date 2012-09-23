using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Humanizer;

namespace ChameleonForms.FieldGenerator
{
    public class DefaultFieldGenerator<TModel, T> : IFieldGenerator
    {
        #region Setup
        private readonly HtmlHelper<TModel> _helper;
        private readonly Expression<Func<TModel, T>> _property;

        public DefaultFieldGenerator(HtmlHelper<TModel> helper, Expression<Func<TModel, T>> property)
        {
            _helper = helper;
            _property = property;
        }
        #endregion

        #region IFieldGenerator Methods
        public IHtmlString GetLabelHtml()
        {
            return _helper.LabelFor(_property);
        }

        public IHtmlString GetValidationHtml()
        {
            return _helper.ValidationMessageFor(_property);
        }

        public IHtmlString GetFieldHtml()
        {
            var metadata = ModelMetadata.FromLambdaExpression(_property, _helper.ViewData);

            if (metadata.ModelType.IsEnum)
                return GetEnumHtml(_property.Compile().Invoke((TModel) _helper.ViewData.ModelMetadata.Model));

            if (metadata.DataTypeName == "Password")
                return _helper.PasswordFor(_property);

            return _helper.TextBoxFor(_property);
        }
        #endregion

        #region Helpers
        public virtual IHtmlString GetEnumHtml(T value)
        {
            var selectList = Enum.GetValues(typeof(T)).OfType<T>().Select(i => new SelectListItem { Text = (i as Enum).Humanize(), Value = i.ToString(), Selected = i.Equals(value)});
            return _helper.DropDownListFor(_property, selectList);
        }
        #endregion
    }
}
