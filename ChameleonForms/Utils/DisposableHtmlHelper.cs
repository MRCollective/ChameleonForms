using System;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;

namespace ChameleonForms.Utils
{
    /// <summary>
    /// HTML helper that can be created in a using block.
    /// </summary>
    /// <typeparam name="TModel">The model type of the HTML helper</typeparam>
    public class DisposableHtmlHelper<TModel> : HtmlHelper<TModel>, IDisposable
    {
        /// <summary>
        /// Creates a <see cref="DisposableHtmlHelper{TModel}"/> to wrap a scope around a new HtmlHelper instance.
        /// </summary>
        /// <param name="htmlGenerator">The HTML generator to use</param>
        /// <param name="viewEngine">The view engine to use</param>
        /// <param name="metadataProvider">The metadata provider to use</param>
        /// <param name="bufferScope">The buffer scope to use</param>
        /// <param name="htmlEncoder">The HTML encoder to use</param>
        /// <param name="urlEncoder">The URL encoder to use</param>
        /// <param name="modelExpressionProvider">The model expression provider</param>
        /// <param name="viewContext">The new view context to wrap</param>
        public DisposableHtmlHelper(IHtmlGenerator htmlGenerator, ICompositeViewEngine viewEngine, IModelMetadataProvider metadataProvider, IViewBufferScope bufferScope, HtmlEncoder htmlEncoder, UrlEncoder urlEncoder, ModelExpressionProvider modelExpressionProvider, ViewContext viewContext)
            : base(htmlGenerator, viewEngine, metadataProvider, bufferScope, htmlEncoder, urlEncoder, modelExpressionProvider)
        {
            Contextualize(viewContext);
        }

        /// <summary>
        /// Dispose of the scope.
        /// </summary>
        public void Dispose() { }
    }
}
