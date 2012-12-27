using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;
using ChameleonForms.Templates;

namespace ChameleonForms.FieldGenerators.Handlers
{
    internal abstract class FieldGeneratorHandler<TModel, T>
    {
        protected readonly IFieldGenerator<TModel, T> FieldGenerator;
        protected readonly IFieldConfiguration FieldConfiguration;

        protected FieldGeneratorHandler(IFieldGenerator<TModel, T> fieldGenerator, IFieldConfiguration fieldConfiguration)
        {
            this.FieldGenerator = fieldGenerator;
            this.FieldConfiguration = fieldConfiguration;
        }

        public abstract HandleAction Handle();

        protected IHtmlString GetInputHtml(TextInputType inputType)
        {
            if (inputType == TextInputType.Password)
                return FieldGenerator.HtmlHelper.PasswordFor(FieldGenerator.FieldProperty, FieldConfiguration.Attributes.ToDictionary());

            FieldConfiguration.Attributes.Attr(type => inputType.ToString().ToLower());
            return FieldGenerator.HtmlHelper.TextBoxFor(FieldGenerator.FieldProperty, FieldConfiguration.Attributes.ToDictionary());
        }

        protected IHtmlString GetSelectListHtml(IEnumerable<SelectListItem> selectList)
        {
            switch (FieldConfiguration.DisplayType)
            {
                case FieldDisplayType.List:
                    var list = SelectListToRadioList(selectList);
                    return HtmlHelpers.List(list);
                case FieldDisplayType.DropDown:
                case FieldDisplayType.Default:
                    return FieldGenerator.HtmlHelper.DropDownListFor(
                        FieldGenerator.FieldProperty, selectList,
                        FieldConfiguration.Attributes.ToDictionary()
                    );
            }

            return null;
        }

        private IEnumerable<IHtmlString> SelectListToRadioList(IEnumerable<SelectListItem> selectList)
        {
            var count = 0;
            foreach (var item in selectList)
            {
                var id = string.Format("{0}_{1}", GetFieldName(), ++count);
                var attrs = new HtmlAttributes(FieldConfiguration.Attributes.ToDictionary());
                if (item.Selected)
                    attrs.Attr("checked", "checked");
                attrs.Attr("id", id);
                yield return new HtmlString(string.Format("{0} {1}",
                    FieldGenerator.HtmlHelper.RadioButtonFor(FieldGenerator.FieldProperty, item.Value, attrs.ToDictionary()),
                    FieldGenerator.HtmlHelper.Label(id, item.Text)
                ));
            }
        }
        
        protected string GetFieldName()
        {
            var name = ExpressionHelper.GetExpressionText(FieldGenerator.FieldProperty);
            return FieldGenerator.HtmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
        }
        
        protected void AdjustHtmlForModelState()
        {
            var name = ExpressionHelper.GetExpressionText(FieldGenerator.FieldProperty);
            var fullName = FieldGenerator.HtmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

            ModelState modelState;
            if (FieldGenerator.HtmlHelper.ViewContext.ViewData.ModelState.TryGetValue(fullName, out modelState))
            {
                if (modelState.Errors.Count > 0)
                {
                    FieldConfiguration.Attributes.AddClass(HtmlHelper.ValidationInputCssClassName);
                }
            }

            FieldConfiguration.Attributes.Attrs(FieldGenerator.HtmlHelper.GetUnobtrusiveValidationAttributes(name, ModelMetadata.FromLambdaExpression(FieldGenerator.FieldProperty, FieldGenerator.HtmlHelper.ViewData)));
        }
    }

}
