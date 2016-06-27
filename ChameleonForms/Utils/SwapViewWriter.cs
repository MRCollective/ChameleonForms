using System;
using System.IO;

namespace ChameleonForms.Utils
{
    internal class SwapViewWriter<TModel> : IDisposable
    {
        private readonly IViewWithModel<TModel> _view;
        private readonly TextWriter _oldWriter;

        public SwapViewWriter(IViewWithModel<TModel> view, TextWriter writer)
        {
            _view = view;
            _oldWriter = view.Writer;
            view.Writer = writer;
        }

        public void Dispose()
        {
            _view.Writer = _oldWriter;
        }
    }
}