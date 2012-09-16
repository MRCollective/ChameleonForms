using System.Web;
using ChameleonForms.Templates;

namespace ChameleonForms.Component
{
    public class Section<TModel, TTemplate> : FormComponent<TModel, TTemplate> where TTemplate : IFormTemplate
    {
        private readonly string _title;

        public Section(IForm<TModel, TTemplate> form, bool isSelfClosing, string title) : base(form, isSelfClosing)
        {
            _title = title;
        }

        public override IHtmlString Begin()
        {
            return Form.Template.BeginSection(_title);
        }

        public override IHtmlString End()
        {
            return Form.Template.EndSection();
        }
    }

    public static class SectionExtensions
    {
        public static Section<TModel, TTemplate> BeginSection<TModel, TTemplate>(this Form<TModel, TTemplate> form, string title) where TTemplate : IFormTemplate
        {
            return new Section<TModel, TTemplate>(form, false, title);
        }
    }
}
