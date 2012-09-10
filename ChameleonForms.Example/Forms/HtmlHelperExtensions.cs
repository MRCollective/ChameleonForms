using System.Web.Mvc;

namespace ChameleonForms.Example.Forms
{
    public static class HtmlHelperExtensions
    {
        public static ChameleonForm<TModel> BeginChameleonForm<TModel>(this HtmlHelper<TModel> helper)
        {
            return new ChameleonForm<TModel>(helper);
        }
    }
}