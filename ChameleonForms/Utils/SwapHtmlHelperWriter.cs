using System;
using System.IO;
using System.Web.Mvc;

namespace ChameleonForms.Utils
{
    internal class SwapHtmlHelperWriter<TModel> : IDisposable
    {
        private readonly HtmlHelper<TModel> _htmlHelper;
        private readonly TextWriter _oldWriter;

        public SwapHtmlHelperWriter(HtmlHelper<TModel> htmlHelper, TextWriter writer)
        {
            _htmlHelper = htmlHelper;
            _oldWriter = htmlHelper.ViewContext.Writer;
            htmlHelper.ViewContext.Writer = writer;
        }

        public void Dispose()
        {
            _htmlHelper.ViewContext.Writer = _oldWriter;
        }
    }
}