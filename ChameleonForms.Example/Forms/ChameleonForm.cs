using System;
using System.Web.Mvc;

namespace ChameleonForms.Example.Forms
{
    public class ChameleonForm<TModel> : IDisposable
    {
        private readonly HtmlHelper<TModel> _helper;

        public ChameleonForm(HtmlHelper<TModel> helper)
        {
            _helper = helper;
            // todo
        }

        public void Dispose()
        {
            // todo
        }
    }
}