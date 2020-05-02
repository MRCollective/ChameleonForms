using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.IO;


namespace ChameleonForms.Utils
{
    internal class SwapHtmlHelperWriter<TModel> : IDisposable
    {
        private readonly IHtmlHelper<TModel> _htmlHelper;
        private readonly TextWriter _oldWriter;

        public SwapHtmlHelperWriter(IHtmlHelper<TModel> htmlHelper, TextWriter writer)
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