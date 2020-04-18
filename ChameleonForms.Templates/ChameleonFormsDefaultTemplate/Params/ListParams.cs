using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace ChameleonForms.Templates.ChameleonFormsDefaultTemplate.Params
{
    internal class ListParams
    {
        public IEnumerable<IHtmlContent> Items { get; set; }
    }
}
