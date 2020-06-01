using System;
using ChameleonForms.Component.Config;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ChameleonForms.TagHelpers
{
    /// <summary>
    /// Extension methods on <see cref="TagHelperContext"/>.
    /// </summary>
    public static class TagHelperContextExtensions
    {
        /// <summary>
        /// The key that's used in <see cref="TagHelperContext"/> Items property to store <see cref="IFieldConfiguration"/>.
        /// </summary>
        public const string FieldConfigurationItemsKey = "ChameleonForms:FieldConfiguration";

        /// <summary>
        /// Idempotently gets a <see cref="IFieldConfiguration"/> from <see cref="TagHelperContext"/> Items.
        /// </summary>
        /// <param name="context">The tag helper context</param>
        /// <returns>The field configuration</returns>
        public static IFieldConfiguration GetFieldConfiguration(this TagHelperContext context)
        {
            var key = $"{FieldConfigurationItemsKey}:{context.UniqueId}";
            if (context.Items.ContainsKey(key))
            {
                var fc = context.Items[key] as IFieldConfiguration;
                if (fc == null)
                    throw new InvalidOperationException($"Found object in context.Items.{key} that wasn't of type IFieldConfiguration, but instead was {context.Items[key]?.GetType()}");

                return fc;
            }
            else
            {
                var fc = new FieldConfiguration();
                context.Items[key] = fc;
                return fc;
            }
        }

        /// <summary>
        /// Idempotently gets a <see cref="HtmlAttributes"/> from <see cref="TagHelperContext"/> Items.
        /// </summary>
        /// <param name="context">The tag helper context</param>
        /// <returns>The HTML attributes</returns>
        public static HtmlAttributes GetHtmlAttributes(this TagHelperContext context)
        {
            return context.GetFieldConfiguration().Attributes;
        }
    }
}
