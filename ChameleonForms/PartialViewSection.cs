using ChameleonForms.Component;

namespace ChameleonForms
{
    internal class PartialViewSection<TPartialModel> : ISection<TPartialModel>
    {
        private readonly object _parentSection;

        public PartialViewSection(IForm<TPartialModel> form)
        {
            _parentSection = form.HtmlHelper.ViewData[Constants.ViewDataSectionKey];
            form.HtmlHelper.ViewData[Constants.ViewDataSectionKey] = this;
            Form = form;
        }

        public IForm<TPartialModel> Form { get; private set; }

        public ISection<TPartialModel1> CreatePartialSection<TPartialModel1>(IForm<TPartialModel1> partialModelForm)
        {
            return new PartialViewSection<TPartialModel1>(partialModelForm);
        }

        public void Dispose()
        {
            Form.HtmlHelper.ViewData[Constants.ViewDataSectionKey] = _parentSection;
        }
    }
}
