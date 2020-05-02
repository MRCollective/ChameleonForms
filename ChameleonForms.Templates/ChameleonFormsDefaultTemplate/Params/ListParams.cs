#pragma warning disable 1591
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace ChameleonForms.Templates.ChameleonFormsDefaultTemplate.Params
{
    public class ListParams
    {
        public IEnumerable<IHtmlContent> Items { get; set; }
    }
}
