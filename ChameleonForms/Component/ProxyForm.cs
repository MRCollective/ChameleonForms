using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;

namespace ChameleonForms.Component
{
    class ProxyForm<TParent, TChild> : IForm<TChild>
    {
        private readonly Form<TParent> form;
        private readonly Expression<Func<TParent, TChild>> parEx;

        public ProxyForm(Form<TParent> form, Expression<Func<TParent, TChild>> parEx)
        {
            this.form = form;
            this.parEx = parEx;
        }

        public HtmlHelper<TChild> HtmlHelper
        {
            get
            {
                HtmlHelper<TParent> parentHelper = this.form.HtmlHelper;
                HtmlHelper<TChild> child = new HtmlHelper<TChild>(parentHelper.ViewContext, parentHelper.ViewDataContainer, parentHelper.RouteCollection);
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

        public FieldGenerators.IFieldGenerator GetFieldGenerator<T>(System.Linq.Expressions.Expression<Func<TChild, T>> property)
        {
            return this.form.GetFieldGenerator(ExpressionExtensions.Combine(parEx, property));
        }

        public void Dispose()
        {
            this.form.Dispose();
        }
    }
}
