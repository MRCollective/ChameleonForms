using System;
using ChameleonForms.Example.Forms.Templates;
using ChameleonForms.Templates;

namespace ChameleonForms.Example.Forms.Components
{
    public class Section<TModel, TTemplate> : IDisposable, IFormComponent<TModel, TTemplate> where TTemplate : IFormTemplate, new()
    {
        public Form<TModel, TTemplate> Form { get; private set; }

        public Section(Form<TModel, TTemplate> form, string title)
        {
            Form = form;
            Form.Write(Form.Template.BeginSection(title));
        }

        public void Dispose()
        {
            Form.Write(Form.Template.EndSection());
        }
    }

    public static class SectionExtensions
    {
        public static Section<TModel, TTemplate> BeginSection<TModel, TTemplate>(this Form<TModel, TTemplate> form, string title) where TTemplate : IFormTemplate, new()
        {
            return new Section<TModel, TTemplate>(form, title);
        }
    }
}