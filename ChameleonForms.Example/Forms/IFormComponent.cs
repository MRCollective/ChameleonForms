using System.Web.Mvc;

namespace ChameleonForms.Example.Forms
{
    public interface IFormComponent<TModel>
    {
        HtmlHelper<TModel> HtmlHelper { get; }
    }
}