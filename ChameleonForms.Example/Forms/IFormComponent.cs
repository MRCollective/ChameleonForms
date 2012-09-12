using ChameleonForms.Example.Forms.Templates;
using ChameleonForms.Templates;

namespace ChameleonForms.Example.Forms
{
    public interface IFormComponent<TModel, TTemplate> where TTemplate : IFormTemplate, new()
    {
        Form<TModel, TTemplate> Form { get; }
    }
}