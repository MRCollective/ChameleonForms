using System.Web;

namespace ChameleonForms.FieldGenerator
{
    public interface IFieldGenerator
    {
        IHtmlString GetFieldHtml();
        IHtmlString GetLabelHtml();
        IHtmlString GetValidationHtml();
    }
}
