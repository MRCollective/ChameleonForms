using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ChameleonForms.TagHelpers
{
    /// <summary>
    /// Different field rendering options. 
    /// </summary>
    public enum RenderAs
    {
        /// <summary>
        /// Whatever the default render option is.
        /// </summary>
        Default,
        /// <summary>
        /// Renders the field as a list of radio options for selecting single values or checkbox items for selecting multiple values.
        /// Use for a list or boolean value.
        /// </summary>
        RadioList,
        /// <summary>
        /// Renders the field as a list of radio options for selecting single values or checkbox items for selecting multiple values.
        /// Use for a list or boolean value.
        /// </summary>
        CheckboxList,
        /// <summary>
        /// Renders the field as a drop-down control.
        /// Use for a list or boolean value.
        /// </summary>
        Dropdown
    }

    /// <summary>
    /// Tag helper that accepts field configuration attributes for a <field /> or <field-element />.
    /// </summary>
    [HtmlTargetElement("field")]
    [HtmlTargetElement("field-element")]
    public class FieldElementConfigurationTagHelper : TagHelper
    {
        /// <summary>
        /// Class(es) to add to the field element.
        /// </summary>
        public string AddClass { get; set; }

        /// <summary>
        /// Field element id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Inline label text override
        /// </summary>
        public string InlineLabel { get; set; }

        /// <summary>
        /// Inline label as templated razor delegate.
        /// </summary>
        public Func<dynamic, IHtmlContent> InlineLabelHtml { get; set; }

        /// <summary>
        /// Inline label as a <see cref="IHtmlContent"/>.
        /// </summary>
        public IHtmlContent InlineLabelHtmlContent { get; set; }

        /// <summary>
        /// Placeholder text for the field.
        /// </summary>
        public string Placeholder { get; set; }

        /// <summary>
        /// Sets the number of rows for a textarea to use.
        /// </summary>
        public int? Rows { get; set; }

        /// <summary>
        /// Sets the number of cols for a textarea to use.
        /// </summary>
        public int? Cols { get; set; }

        /// <summary>
        /// Sets the minimum value to accept for numeric text controls.
        /// </summary>
        public string Min { get; set; }

        /// <summary>
        /// Sets the maximum value to accept for numeric text controls.
        /// </summary>
        public string Max { get; set; }

        /// <summary>
        /// Sets the stepping interval to use for numeric text controls.
        /// </summary>
        public decimal? Step { get; set; }

        /// <summary>
        /// Whether the field is disabled (value not submitted, can not click).
        /// </summary>
        public bool? Disabled { get; set; }

        /// <summary>
        /// Whether the field is readonly (value vannot be modified).
        /// </summary>
        public bool? Readonly { get; set; }

        /// <summary>
        /// Whether the field is required.
        /// </summary>
        public bool? Required { get; set; }

        /// <summary>
        /// Override the render type of the field.
        /// </summary>
        public RenderAs As { get; set; }

        /// <summary>
        /// Change the label that represents true.
        /// </summary>
        public string TrueLabel { get; set; }

        /// <summary>
        /// Change the label that represents false.
        /// </summary>
        public string FalseLabel { get; set; }

        /// <summary>
        /// Change the label that represents none.
        /// </summary>
        public string NoneLabel { get; set; }

        /// <summary>
        /// Uses the given format string when outputting the field value.
        /// </summary>
        public string FormatString { get; set; }

        /// <summary>
        /// Hide the empty item that would normally display for the field.
        /// </summary>
        public bool HideEmptyItem { get; set; }

        /// <summary>
        /// Excludes one or more Enum values from the generated field.
        /// </summary>
        public Enum[] Exclude { get; set; }

        /// <summary>
        /// Specify that no inline label should be generated.
        /// </summary>
        public bool WithoutInlineLabel { get; set; }

        /// <summary>
        /// Specify that inline labels should wrap their input element.
        /// </summary>
        public bool? InlineLabelWrapsElement { get; set; }

        /// <summary>
        /// Arbitrary attributes to add to the field element. Add as a dictionary (attrs="@attrsDictionary") or as individual attributes (attr-id="id" attr-data-xyz="asdf").
        /// </summary>
        [HtmlAttributeName("attrs", DictionaryAttributePrefix = "attr-")]
        public IDictionary<string, string> Attrs { get; set; } = new Dictionary<string, string>();

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var fc = context.GetFieldConfiguration();

            if (AddClass != null)
                fc.AddClass(AddClass);

            if (Id != null)
                fc.Id(Id);

            if (InlineLabel != null)
                fc.InlineLabel(InlineLabel);

            if (InlineLabelHtml != null)
                fc.InlineLabel(InlineLabelHtml);

            if (InlineLabelHtmlContent != null)
                fc.InlineLabel(InlineLabelHtmlContent);

            if (Placeholder != null)
                fc.Placeholder(Placeholder);

            if (Rows.HasValue)
                fc.Rows(Rows.Value);

            if (Cols.HasValue)
                fc.Cols(Cols.Value);

            if (!string.IsNullOrWhiteSpace(Min))
                fc.Min(Min);

            if (!string.IsNullOrWhiteSpace(Max))
                fc.Max(Max);

            if (Step.HasValue)
                fc.Step(Step.Value);

            if (Disabled.HasValue)
                fc.Disabled(Disabled.Value);

            if (Readonly.HasValue)
                fc.Readonly(Readonly.Value);

            if (Required.HasValue)
                fc.Required(Required.Value);

            switch (As)
            {
                case RenderAs.CheckboxList:
                    fc.AsCheckboxList();
                    break;
                case RenderAs.RadioList:
                    fc.AsRadioList();
                    break;
                case RenderAs.Dropdown:
                    fc.AsDropDown();
                    break;
            }

            if (!string.IsNullOrWhiteSpace(TrueLabel))
                fc.WithTrueAs(TrueLabel);

            if (!string.IsNullOrWhiteSpace(FalseLabel))
                fc.WithFalseAs(FalseLabel);

            if (!string.IsNullOrWhiteSpace(NoneLabel))
                fc.WithNoneAs(NoneLabel);

            if (!string.IsNullOrWhiteSpace(FormatString))
                fc.WithFormatString(FormatString);

            if (HideEmptyItem)
                fc.HideEmptyItem();

            if (Exclude != null)
                fc.Exclude(Exclude);

            if (WithoutInlineLabel)
                fc.WithoutInlineLabel();

            if (InlineLabelWrapsElement.HasValue)
                fc.InlineLabelWrapsElement(InlineLabelWrapsElement.Value);

            fc.Attrs(Attrs);

            return Task.CompletedTask;
        }
    }
}
