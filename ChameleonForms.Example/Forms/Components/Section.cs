using System;
using ChameleonForms.Example.Forms.Templates;

namespace ChameleonForms.Example.Forms.Components
{
    public class Section<TModel, TTemplate> : IDisposable, IFormComponent<TModel, TTemplate> where TTemplate : IFormTemplate, new()
    {
        public ChameleonForm<TModel, TTemplate> Form { get; private set; }

        public Section(ChameleonForm<TModel, TTemplate> form, string title)
        {
            Form = form;
            Form.HtmlHelper.ViewContext.Writer.Write(Form.Template.BeginSection(title));
        }

        public void Dispose()
        {
            Form.HtmlHelper.ViewContext.Writer.Write(Form.Template.EndSection());
        }

        
    }

    public static class SectionExtensions
    {
        public static Section<TModel, TTemplate> BeginSection<TModel, TTemplate>(this ChameleonForm<TModel, TTemplate> form, string title) where TTemplate : IFormTemplate, new()
        {
            return new Section<TModel, TTemplate>(form, title);
        }
    }
}