using System;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using ChameleonForms.Enums;
using ChameleonForms.Templates;
using Humanizer;

namespace ChameleonForms
{
    public interface IForm<TModel, out TTemplate> : IDisposable where TTemplate : IFormTemplate
    {
        HtmlHelper<TModel> HtmlHelper { get; }
        TTemplate Template { get; }
        void Write(IHtmlString htmlString);
    }

    public class Form<TModel, TTemplate> : IForm<TModel, TTemplate> where TTemplate : IFormTemplate
    {
        public HtmlHelper<TModel> HtmlHelper { get; private set; }
        public TTemplate Template { get; private set; }

        public Form(HtmlHelper<TModel> helper, TTemplate template, string action, HttpMethod method, EncType? enctype)
        {
            HtmlHelper = helper;
            Template = template;
            Write(Template.BeginForm(action, method, enctype));
        }

        public virtual void Write(IHtmlString htmlString)
        {
            HtmlHelper.ViewContext.Writer.Write(htmlString);
        }

        public void Dispose()
        {
            Write(Template.EndForm());
        }
    }

    public static class ChameleonFormExtensions
    {
        public static IForm<TModel, DefaultFormTemplate> BeginChameleonForm<TModel>(this HtmlHelper<TModel> helper, string action, HttpMethod method, EncType? enctype = null)
        {
            return new Form<TModel, DefaultFormTemplate>(helper, new DefaultFormTemplate(), action, method, enctype);
        }
    }
}