using System;
using System.Web.Mvc;

namespace ChameleonForms.Example.Forms
{
    public class ChameleonForm<TModel> : IDisposable, IFormComponent<TModel>
    {
        public HtmlHelper<TModel> HtmlHelper { get; private set; }

        public ChameleonForm(HtmlHelper<TModel> helper)
        {
            HtmlHelper = helper;
            // todo
        }

        public void Dispose()
        {
            // todo
        }
    }
}