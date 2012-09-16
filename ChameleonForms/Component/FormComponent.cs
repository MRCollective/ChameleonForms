using System;
using System.Web;
using ChameleonForms.Templates;

namespace ChameleonForms.Component
{
    public interface IFormComponent<TModel, out TTemplate> where TTemplate : IFormTemplate
    {
        IForm<TModel, TTemplate> Form { get; }
    }

    public abstract class FormComponent<TModel, TTemplate> : IFormComponent<TModel, TTemplate>, IHtmlString, IDisposable where TTemplate : IFormTemplate
    {
        protected readonly bool IsSelfClosing;
        public IForm<TModel, TTemplate> Form { get; private set; }

        protected FormComponent(IForm<TModel, TTemplate> form, bool isSelfClosing)
        {
            Form = form;
            IsSelfClosing = isSelfClosing;
        }

        public void Initialise()
        {
            if (!IsSelfClosing)
                Form.Write(Begin());
        }

        public abstract IHtmlString Begin();
        public abstract IHtmlString End();
        public string ToHtmlString()
        {
            if (!IsSelfClosing)
                return null;

            return string.Format("{0}{1}", Begin().ToHtmlString(), End().ToHtmlString());
        }

        public void Dispose()
        {
            if (!IsSelfClosing)
                Form.Write(End());
        }
    }
}
