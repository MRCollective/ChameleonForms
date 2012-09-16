using System.Web;
using ChameleonForms.Templates;

namespace ChameleonForms.Component
{
    public class Section<TModel, TTemplate> : FormComponent<TModel, TTemplate> where TTemplate : IFormTemplate
    {
        private readonly string _title;
        private readonly bool _nested;

        public Section(IForm<TModel, TTemplate> form, string title, bool nested) : base(form, false)
        {
            _title = title;
            _nested = nested;
            Initialise();
        }

        public override IHtmlString Begin()
        {
            return _nested ? Form.Template.BeginNestedSection(_title) : Form.Template.BeginSection(_title);
        }

        public override IHtmlString End()
        {
            return _nested ? Form.Template.EndNestedSection() : Form.Template.EndSection();
        }
    }

    public static class SectionExtensions
    {
        public static Section<TModel, TTemplate> BeginSection<TModel, TTemplate>(this IForm<TModel, TTemplate> form, string title) where TTemplate : IFormTemplate
        {
            return new Section<TModel, TTemplate>(form, title, false);
        }

        public static Section<TModel, TTemplate> BeginSection<TModel, TTemplate>(this Section<TModel, TTemplate> section, string title) where TTemplate : IFormTemplate
        {
            return new Section<TModel, TTemplate>(section.Form, title, true);
        }
    }
}
