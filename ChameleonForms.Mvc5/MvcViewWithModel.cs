using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using ChameleonForms.Component;
using ChameleonForms.Templates;
using ModelState = System.Web.ModelBinding.ModelState;
using SelectListItem = ChameleonForms.FieldGenerators.SelectListItem;

namespace ChameleonForms
{
    public class MvcViewWithModel<TModel> : IViewWithModel<TModel>
    {
        private readonly IDictionary<string, ModelState> _modelState;
        public readonly HtmlHelper<TModel> HtmlHelper;

        public MvcViewWithModel(HtmlHelper<TModel> htmlHelper)
        {
            HtmlHelper = htmlHelper;
            _modelState = htmlHelper.ViewData.ModelState
                .ToDictionary(kvp => kvp.Key, kvp =>
                {
                    var state = new ModelState();
                    kvp.Value.Errors.ToList().ForEach(e =>
                        state.Errors.Add(e.Exception != null
                            ? new System.Web.ModelBinding.ModelError(e.Exception)
                            : new System.Web.ModelBinding.ModelError(e.ErrorMessage)));
                    state.Value = kvp.Value.Value == null ? null : new System.Web.ModelBinding.ValueProviderResult(kvp.Value.Value.RawValue, kvp.Value.Value.AttemptedValue, kvp.Value.Value.Culture);
                    return state;
                });
        }

        public TModel Model
        {
            get { return HtmlHelper.ViewData.Model; }
        }

        public IDictionary<string, ModelState> ModelState
        {
            get { return _modelState; }
        }

        public IFieldMetadata GetFieldMetadata<TProperty>(Expression<Func<TModel, TProperty>> property)
        {
            var metadata = ModelMetadata.FromLambdaExpression(property, HtmlHelper.ViewData);
            return new ModelMetadataFieldMetadata(metadata, HtmlHelper.ViewData.ModelState.IsValidField(GetFieldName(property)));
        }

        // todo: abstract this away so we don't need to leak the writer
        public TextWriter Writer
        {
            get { return HtmlHelper.ViewContext.Writer; }
            set { HtmlHelper.ViewContext.Writer = value; }
        }

        public void Write(IHtml html)
        {
            HtmlHelper.ViewContext.Writer.Write(html.ToIHtmlString());
        }

        public string GetFieldName<TProperty>(Expression<Func<TModel, TProperty>> property)
        {
            return GetFieldName(ExpressionHelper.GetExpressionText(property));
        }

        public string GetFieldName(string propertyName)
        {
            return HtmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(propertyName);
        }

        public string GetFieldId<TProperty>(Expression<Func<TModel, TProperty>> property)
        {
            return GetFieldId(ExpressionHelper.GetExpressionText(property));
        }

        public string GetFieldId(string propertyName)
        {
            return HtmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(propertyName);
        }

        public IHtml ValidationMessageFor<TProperty>(Expression<Func<TModel, TProperty>> property, string validationMessage = null, HtmlAttributes htmlAttributes = null)
        {
            return htmlAttributes != null
                ? HtmlHelper.ValidationMessageFor(property, validationMessage, htmlAttributes.Attributes).ToIHtml()
                : HtmlHelper.ValidationMessageFor(property, validationMessage).ToIHtml();
        }

        public IHtml Partial<TPartialModel>(IForm<TModel> form, Expression<Func<TModel, TPartialModel>> partialModelProperty, string partialViewName)
        {
            var formModel = Model;
            var viewData = new ViewDataDictionary(HtmlHelper.ViewData);
            viewData[WebViewPageExtensions.PartialViewModelExpressionViewDataKey] = partialModelProperty;
            viewData[WebViewPageExtensions.CurrentFormViewDataKey] = form;
            return HtmlHelper.Partial(partialViewName, partialModelProperty.Compile().Invoke(formModel), viewData).ToIHtml();
        }

        public IHtml Partial<TPartialModel>(ISection<TModel> section, Expression<Func<TModel, TPartialModel>> partialModelProperty, string partialViewName)
        {
            var formModel = Model;
            var viewData = new ViewDataDictionary(HtmlHelper.ViewData);
            viewData[WebViewPageExtensions.PartialViewModelExpressionViewDataKey] = partialModelProperty;
            viewData[WebViewPageExtensions.CurrentFormViewDataKey] = section.Form;
            viewData[WebViewPageExtensions.CurrentFormSectionViewDataKey] = section;
            return HtmlHelper.Partial(partialViewName, partialModelProperty.Compile().Invoke(formModel), viewData).ToIHtml();
        }

        public void AddValidationErrorAttributes<TProperty>(HtmlAttributes attrs, Expression<Func<TModel, TProperty>> fieldProperty, ModelState modelState)
        {
            attrs.AddClass(System.Web.Mvc.HtmlHelper.ValidationInputCssClassName);
        }

        public void AddValidationAttributes<TProperty>(HtmlAttributes attrs, Expression<Func<TModel, TProperty>> fieldProperty)
        {
            attrs.Attrs(HtmlHelper.GetUnobtrusiveValidationAttributes(GetFieldName(fieldProperty), ModelMetadata.FromLambdaExpression(fieldProperty, HtmlHelper.ViewData)));
        }

        public IHtml TextareaFor<TProperty>(Expression<Func<TModel, TProperty>> fieldProperty, HtmlAttributes htmlAttributes)
        {
            return HtmlHelper.TextAreaFor(fieldProperty, htmlAttributes.ToDictionary()).ToIHtml();
        }

        public IHtml InputFor<TProperty>(Expression<Func<TModel, TProperty>> fieldProperty, HtmlAttributes htmlAttributes, string formatString = null)
        {
            return !string.IsNullOrEmpty(formatString)
                ? HtmlHelper.TextBoxFor(fieldProperty, formatString, htmlAttributes.ToDictionary()).ToIHtml()
                : HtmlHelper.TextBoxFor(fieldProperty, htmlAttributes.ToDictionary()).ToIHtml();
        }

        public IHtml SelectListFor<TProperty>(Expression<Func<TModel, TProperty>> fieldProperty, IEnumerable<SelectListItem> selectList, bool allowMultipleSelect,
            HtmlAttributes htmlAttributes)
        {
            return allowMultipleSelect
                ? HtmlHelper.ListBoxFor(fieldProperty, selectList.ToMvcSelectList(), htmlAttributes.ToDictionary()).ToIHtml()
                : HtmlHelper.DropDownListFor(fieldProperty, selectList.ToMvcSelectList(), htmlAttributes.ToDictionary()).ToIHtml();
        }

        public IHtml RadioItemFor<TProperty>(Expression<Func<TModel, TProperty>> fieldProperty, string value, HtmlAttributes htmlAttributes)
        {
            return HtmlHelper.RadioButtonFor(fieldProperty, value, htmlAttributes.ToDictionary()).ToIHtml();
        }

        public IHtml Label(string id, string text, HtmlAttributes htmlAttributes)
        {
            return HtmlCreator.BuildLabel(id, text.ToIHtml(), htmlAttributes);
        }
    }
}
