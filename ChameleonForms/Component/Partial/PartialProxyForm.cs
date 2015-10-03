using ChameleonForms.Utils;
using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace ChameleonForms.Component.Partial
{
    internal class PartialProxyForm<TParent, TChild> : IForm<TChild>
    {
        private class FakeViewDataContainer : IViewDataContainer
        {
            public ViewDataDictionary ViewData { get; set; }
        }

        private readonly IForm<TParent> form;
        private readonly Expression<Func<TParent, TChild>> parEx;

        public PartialProxyForm(IForm<TParent> form, Expression<Func<TParent, TChild>> parEx, HtmlHelper<TChild> htmlHelper)
        {
            this.form = form;
            this.parEx = parEx;
            this.HtmlHelper = htmlHelper ?? this.InitializeChildHtmlHelper();
        }

        public HtmlHelper<TChild> HtmlHelper { get; private set; }

        private HtmlHelper<TChild> InitializeChildHtmlHelper()
        {
            var parentHelper = this.form.HtmlHelper;
            var data = new ViewDataDictionary<TChild>();
            foreach (var item in parentHelper.ViewDataContainer.ViewData)
            {
                data.Add(item.Key, item.Value);
            }

            var container = new FakeViewDataContainer { ViewData = data };

            var child = new HtmlHelper<TChild>(parentHelper.ViewContext, container, parentHelper.RouteCollection);
            child.ViewData.Model = this.parEx.Compile()(parentHelper.ViewData.Model);
            return child;
        }

        public Templates.IFormTemplate Template
        {
            get { return this.form.Template; }
        }

        public void Write(System.Web.IHtmlString htmlString)
        {
            this.HtmlHelper.ViewContext.Writer.Write(htmlString);
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