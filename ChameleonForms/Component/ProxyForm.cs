using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace ChameleonForms.Component
{
    class ProxyForm<TParent, TChild> : IForm<TChild>
    {
        private class FakeViewDataContainer : IViewDataContainer
        {
            public ViewDataDictionary ViewData { get; set; }
        }

        private readonly IForm<TParent> form;
        private readonly Expression<Func<TParent, TChild>> parEx;

        public ProxyForm(IForm<TParent> form, Expression<Func<TParent, TChild>> parEx)
        {
            this.form = form;
            this.parEx = parEx;
        }

        public HtmlHelper<TChild> HtmlHelper
        {
            get
            {
                var parentHelper = this.form.HtmlHelper;
                var data = new ViewDataDictionary<TChild>();
                foreach (var item in parentHelper.ViewDataContainer.ViewData)
                {
                    data.Add(item.Key, item.Value);
                }

                var container = new FakeViewDataContainer {ViewData = data};

                var child = new HtmlHelper<TChild>(parentHelper.ViewContext, container, parentHelper.RouteCollection);
                return child;
            }
        }

        public Templates.IFormTemplate Template
        {
            get { return this.form.Template; }
        }

        public void Write(System.Web.IHtmlString htmlString)
        {
            this.form.Write(htmlString);
        }

        public FieldGenerators.IFieldGenerator GetFieldGenerator<T>(Expression<Func<TChild, T>> property)
        {
            return this.form.GetFieldGenerator(ExpressionExtensions.Combine(parEx, property));
        }

        public void Dispose()
        {
            this.form.Dispose();
        }
    }
}
