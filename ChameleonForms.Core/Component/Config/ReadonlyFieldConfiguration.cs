using System;
using System.Collections.Generic;
using System.Web;
using ChameleonForms.Enums;
using Microsoft.AspNetCore.Html;

namespace ChameleonForms.Component.Config
{
    /// <summary>
    /// Immutable field configuration for use when generating a field's HTML.
    /// </summary>
    public interface IReadonlyFieldConfiguration
    {
        /// <summary>
        /// A dynamic bag to allow for custom extensions using the field configuration.
        /// </summary>
        dynamic Bag { get; }

        /// <summary>
        /// Returns data from the Bag stored in the given property or default(TData) if there is none present.
        /// </summary>
        /// <typeparam name="TData">The type of the expected data to return</typeparam>
        /// <param name="propertyName">The name of the property to retrieve the data for</param>
        /// <returns>The data from the Bag or default(TData) if there was no data against that property in the bag</returns>
        TData GetBagData<TData>(string propertyName);

        /// <summary>
        /// Attributes to add to the form element's HTML.
        /// </summary>
        // todo: consider making this a readonly dictionary
        IDictionary<string, object> HtmlAttributes { get; }

        /// <summary>
        /// Gets any text that has been set for an inline label.
        /// </summary>
        IHtmlContent InlineLabelText { get; }

        /// <summary>
        /// Gets any text that has been set for the label.
        /// </summary>
        IHtmlContent LabelText { get; }

        /// <summary>
        /// Returns the display type for the field.
        /// </summary>
        FieldDisplayType DisplayType { get; }

        /// <summary>
        /// The label that represents true.
        /// </summary>
        string TrueString { get; }

        /// <summary>
        /// The label that represents false.
        /// </summary>
        string FalseString { get; }

        /// <summary>
        /// The label that represents none.
        /// </summary>
        string NoneString { get; }

        /// <summary>
        /// Get the hint to display with the field.
        /// </summary>
        IHtmlContent Hint { get; }

        /// <summary>
        /// A list of HTML to be prepended to the form field in ltr order.
        /// </summary>
        IEnumerable<IHtmlContent> PrependedHtml { get; }

        /// <summary>
        /// A list of HTML to be appended to the form field in ltr order.
        /// </summary>
        IEnumerable<IHtmlContent> AppendedHtml { get; }

        /// <summary>
        /// The HTML to be used as the field html.
        /// </summary>
        IHtmlContent FieldHtml { get; }

        /// <summary>
        /// The format string to use for the field.
        /// </summary>
        string FormatString { get; }

        /// <summary>
        /// Whether or not the empty item is hidden.
        /// </summary>
        bool EmptyItemHidden { get; }
        
        /// <summary>
        /// Whether or not to use a &lt;label&gt;.
        /// </summary>
        bool HasLabelElement { get; }

        /// <summary>
        /// Any CSS class(es) to use for the field label.
        /// </summary>
        string LabelClasses { get; }

        /// <summary>
        /// Any CSS class(es) to use for the field container element.
        /// </summary>
        string FieldContainerClasses { get; }

        /// <summary>
        /// Any CSS class(es) to use for the field validation message.
        /// </summary>
        string ValidationClasses { get; }

        /// <summary>
        /// Enum value(s) to exclude from the generated field.
        /// </summary>
        Enum[] ExcludedEnums { get; }

        /// <summary>
        /// Whether or not to use an inline &lt;label&gt;.
        /// </summary>
        bool HasInlineLabel { get; }

        /// <summary>
        /// Whether or not inline &lt;label&gt; should wrap their &lt;input&gt; element.
        /// </summary>
        bool ShouldInlineLabelWrapElement { get; }
    }
}