using ChameleonForms.Example.Forms.Templates;

namespace ChameleonForms.Example.Forms
{
    public interface IFormComponent<TModel, TTemplate> where TTemplate : IFormTemplate, new()
    {
        ChameleonForm<TModel, TTemplate> Form { get; }
    }
}