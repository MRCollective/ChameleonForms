using System;
using ChameleonForms.Component;
using ChameleonForms.Component.Config;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChameleonForms
{
    internal class PartialViewSection<TPartialModel> : ISection, ISection<TPartialModel>
    {
        private readonly object _parentSection;

        public PartialViewSection(IForm<TPartialModel> form)
        {
            _parentSection = form.HtmlHelper.ViewData[Constants.ViewDataSectionKey];
            form.HtmlHelper.ViewData[Constants.ViewDataSectionKey] = this;
            Form = form;
        }

        public IForm<TPartialModel> Form { get; private set; }

        public ISection<TPartialModel1> CreatePartialSection<TPartialModel1>(IForm<TPartialModel1> partialModelForm)
        {
            return new PartialViewSection<TPartialModel1>(partialModelForm);
        }

        public ISection<TPartialModel> CreatePartialSection(IHtmlHelper<TPartialModel> partialHelper)
        {
            return partialHelper == Form.HtmlHelper
                ? this
                : new PartialViewSection<TPartialModel>(Form.CreatePartialForm(partialHelper));
        }

        public IFieldConfiguration Field(IHtmlContent labelHtml, IHtmlContent elementHtml, IHtmlContent validationHtml = null,
            ModelMetadata metadata = null, bool isValid = true, IFieldConfiguration fieldConfiguration = null)
        {
            return (_parentSection as ISection)?.Field(labelHtml, elementHtml, validationHtml, metadata, isValid, fieldConfiguration);
        }

        public void Dispose()
        {
            Form.HtmlHelper.ViewData[Constants.ViewDataSectionKey] = _parentSection;
        }
    }
}
