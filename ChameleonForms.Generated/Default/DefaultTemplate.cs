using System.Collections.Generic;
using ChameleonForms.Component;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;
using ChameleonForms.FieldGenerators;
using ChameleonForms.FieldGenerators.Handlers;
using ChameleonForms.Templates;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorLight;

namespace ChameleonForms.Generated.Default
{
    public class DefaultTemplate : IFormTemplate
    {
        private readonly RazorLightEngine _razorEngine;

        public DefaultTemplate(RazorLightEngine razorEngine)
        {
            _razorEngine = razorEngine;
        }

        public IHtmlContent Render<TModel>(string template, TModel model)
        {
            var result = _razorEngine.CompileRenderAsync($"Default.{template}", model).GetAwaiter().GetResult();
            return new HtmlString(result);
        }

        public void PrepareFieldConfiguration<TModel, T>(IFieldGenerator<TModel, T> fieldGenerator, IFieldGeneratorHandler<TModel, T> fieldGeneratorHandler,
            IFieldConfiguration fieldConfiguration, FieldParent fieldParent)
        {
            throw new System.NotImplementedException();
        }

        public IHtmlContent BeginForm(string action, FormMethod method, HtmlAttributes htmlAttributes, EncType? enctype)
        {
            throw new System.NotImplementedException();
        }

        public IHtmlContent EndForm()
        {
            throw new System.NotImplementedException();
        }

        public IHtmlContent BeginSection(IHtmlContent heading = null, IHtmlContent leadingHtml = null,
            HtmlAttributes htmlAttributes = null)
        {
            throw new System.NotImplementedException();
        }

        public IHtmlContent EndSection()
        {
            throw new System.NotImplementedException();
        }

        public IHtmlContent BeginNestedSection(IHtmlContent heading = null, IHtmlContent leadingHtml = null,
            HtmlAttributes htmlAttributes = null)
        {
            throw new System.NotImplementedException();
        }

        public IHtmlContent EndNestedSection()
        {
            throw new System.NotImplementedException();
        }

        public IHtmlContent Field(IHtmlContent labelHtml, IHtmlContent elementHtml, IHtmlContent validationHtml,
            ModelMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid)
        {
            throw new System.NotImplementedException();
        }

        public IHtmlContent BeginField(IHtmlContent labelHtml, IHtmlContent elementHtml, IHtmlContent validationHtml,
            ModelMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid)
        {
            throw new System.NotImplementedException();
        }

        public IHtmlContent EndField()
        {
            throw new System.NotImplementedException();
        }

        public IHtmlContent BeginNavigation()
        {
            throw new System.NotImplementedException();
        }

        public IHtmlContent EndNavigation()
        {
            throw new System.NotImplementedException();
        }

        public IHtmlContent BeginMessage(MessageType messageType, IHtmlContent heading, bool emptyHeading)
        {
            throw new System.NotImplementedException();
        }

        public IHtmlContent EndMessage()
        {
            throw new System.NotImplementedException();
        }

        public IHtmlContent MessageParagraph(IHtmlContent paragraph)
        {
            return Render("MessageParagraph", new MessageParagraphParams {Paragraph = paragraph});
        }

        public IHtmlContent Button(IHtmlContent content, string type, string id, string value, HtmlAttributes htmlAttributes)
        {
            throw new System.NotImplementedException();
        }

        public IHtmlContent RadioOrCheckboxList(IEnumerable<IHtmlContent> list, bool isCheckbox)
        {
            throw new System.NotImplementedException();
        }
    }
}
