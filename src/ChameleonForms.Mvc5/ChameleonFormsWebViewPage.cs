using System.Web.Mvc;

namespace ChameleonForms
{
    public abstract class ChameleonFormsWebViewPage : ChameleonFormsWebViewPage<dynamic> { }

    public abstract class ChameleonFormsWebViewPage<T> : WebViewPage<T>
    {
        public override void Write(object value)
        {
            if (value is IHtml)
                value = ((IHtml) value).ToIHtmlString();
            base.Write(value);
        }
    }
}
