using ChameleonForms.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ChameleonForms.FieldGenerators
{
    class BooleanFieldGenerator<TModel, T> : DefaultFieldGenerator<TModel, T>
    {
        public BooleanFieldGenerator(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, T>> fieldProperty, IFormTemplate template)
            : base(htmlHelper, fieldProperty, template)
        {
        }

        public override System.Web.IHtmlString GetLabelHtml(Component.Config.IReadonlyFieldConfiguration fieldConfiguration)
        {
            return new HtmlString(string.Empty);
        }
    }
}
