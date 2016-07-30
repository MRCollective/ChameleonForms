using ChameleonForms.Component;

namespace ChameleonForms
{
    internal class PartialViewSection<TPartialModel> : ISection<TPartialModel>
    {
        public PartialViewSection(IForm<TPartialModel> form)
        {
            Form = form;
        }

        public IForm<TPartialModel> Form { get; private set; }
    }
}
