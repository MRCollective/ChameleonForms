using System;
using System.Web.Mvc;

namespace ChameleonForms.Example.Forms.Components
{
    public class Section<TModel> : IDisposable, IFormComponent<TModel>
    {
        public HtmlHelper<TModel> HtmlHelper { get; private set; }

        public Section(HtmlHelper<TModel> helper)
        {
            HtmlHelper = helper;
            // todo
            HtmlHelper.ViewContext.Writer.Write("TEST");
        }

        public void Dispose()
        {
            // todo
            HtmlHelper.ViewContext.Writer.Write("TEST2");
        }
    }

    public static class SectionExtensions
    {
        public static Section<TModel> BeginSection<TModel>(this ChameleonForm<TModel> form)
        {
            return new Section<TModel>(form.HtmlHelper);
        }
    }
}