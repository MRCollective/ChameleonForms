#pragma warning disable 1591
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace ChameleonForms.Templates.ChameleonFormsTwitterBootstrap3Template.Params
{
    public class ListParams
    {
        public IEnumerable<IHtmlContent> Items { get; set; }
        public bool IsCheckbox { get; set; }
    }
}
