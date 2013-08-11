﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;
using ChameleonForms.Templates;

namespace ChameleonForms.FieldGenerators.Handlers
{
    internal class BooleanHandler<TModel, T> : FieldGeneratorHandler<TModel, T>
    {
        public BooleanHandler(IFieldGenerator<TModel, T> fieldGenerator, IFieldConfiguration fieldConfiguration)
            : base(fieldGenerator, fieldConfiguration)
        {}

        public override HandleAction Handle()
        {
            if (FieldGenerator.Metadata.ModelType != typeof(bool) && FieldGenerator.Metadata.ModelType != typeof(bool?))
                return HandleAction.Continue;

            if (FieldConfiguration.DisplayType == FieldDisplayType.Default && FieldGenerator.Metadata.ModelType == typeof(bool))
                return HandleAction.Return(GetSingleCheckboxHtml());

            var selectList = GetBooleanSelectList();
            var html = GetSelectListHtml(selectList);
            return HandleAction.Return(html);
        }

        private bool? GetValue()
        {
            return FieldGenerator.GetValue() as bool?;
        }

        private IHtmlString GetSingleCheckboxHtml()
        {
            AdjustHtmlForModelState(FieldConfiguration.Attributes);
            
            var fieldhtml = HtmlCreator.BuildSingleCheckbox(GetFieldName(), GetValue() ?? false, FieldConfiguration.Attributes);
            var labelHtml = HtmlCreator.BuildLabel(
                GetFieldName(),
                FieldConfiguration.InlineLabelText ?? new HtmlString(FieldGenerator.GetFieldDisplayName()),
                null
            );

            return new HtmlString(string.Format("{0} {1}", fieldhtml, labelHtml));
        }

        private IEnumerable<SelectListItem> GetBooleanSelectList()
        {
            var value = GetValue();
            yield return new SelectListItem { Value = "true", Text = FieldConfiguration.TrueString, Selected = value == true };
            yield return new SelectListItem { Value = "false", Text = FieldConfiguration.FalseString, Selected = value == false };
        }
    }
}
