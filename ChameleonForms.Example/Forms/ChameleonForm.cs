using System.Web.Mvc;

namespace ChameleonForms.Example.Forms
{
    public class ChameleonForm<TModel>
    {
        private readonly HtmlHelper<TModel> _helper;

        public ChameleonForm(HtmlHelper<TModel> helper)
        {
            _helper = helper;
        }
    }
}