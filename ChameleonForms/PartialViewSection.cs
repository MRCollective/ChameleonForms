using ChameleonForms.Component;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public ISection<TPartialModel> CreatePartialSection(IHtmlHelper<TPartialModel> partialHelper)
        {
            return partialHelper == Form.HtmlHelper
                ? this
                : new PartialViewSection<TPartialModel>(Form.CreatePartialForm(partialHelper));
        }

        public void Dispose()
        {
            Form.HtmlHelper.ViewData[Constants.ViewDataSectionKey] = _parentSection;
        }
    }
}
