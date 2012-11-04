using ChameleonForms.Templates;

namespace ChameleonForms.Component.Config
{
    public interface IFieldConfiguration
    {
        HtmlAttributes Attributes { get; set; }
    }

    public class FieldConfiguration : IFieldConfiguration
    {
        public FieldConfiguration()
        {
            Attributes = new HtmlAttributes();
        }

        public HtmlAttributes Attributes { get; set; }
    }
}
