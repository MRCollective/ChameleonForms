using System.Web;
using ChameleonForms.Templates;

namespace ChameleonForms.Component
{
    public class Section<TModel, TTemplate> : FormComponent<TModel, TTemplate> where TTemplate : IFormTemplate
    {
        private readonly string _title;
        private readonly bool _nested;
        private readonly IHtmlString _leadingHtml;
        private readonly object _htmlAttributes;

        public Section(IForm<TModel, TTemplate> form, string title, bool nested, IHtmlString leadingHtml = null, object htmlAttributes = null) : base(form, false)
        {
            _title = title;
            _nested = nested;
            _leadingHtml = leadingHtml;
            _htmlAttributes = htmlAttributes;
            Initialise();
        }

        public override IHtmlString Begin()
        {
            return _nested ? Form.Template.BeginNestedSection(_title, _leadingHtml, _htmlAttributes) : Form.Template.BeginSection(_title, _leadingHtml, _htmlAttributes);
        }

        public override IHtmlString End()
        {
            return _nested ? Form.Template.EndNestedSection() : Form.Template.EndSection();
        }
    }

    public static class SectionExtensions
    {
        public static Section<TModel, TTemplate> BeginSection<TModel, TTemplate>(this IForm<TModel, TTemplate> form, string title,  IHtmlString leadingHtml = null, object htmlAttributes = null) where TTemplate : IFormTemplate
        {
            return new Section<TModel, TTemplate>(form, title, false, leadingHtml, htmlAttributes);
        }

        public static Section<TModel, TTemplate> BeginSection<TModel, TTemplate>(this Section<TModel, TTemplate> section, string title, IHtmlString leadingHtml = null, object htmlAttributes = null) where TTemplate : IFormTemplate
        {
            return new Section<TModel, TTemplate>(section.Form, title, true, leadingHtml, htmlAttributes);
        }
    }
}
