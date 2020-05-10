using System;
using ChameleonForms.Component;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChameleonForms.Utils
{
    /// <summary>
    /// Extension methods on <see cref="IHtmlHelper"/>.
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Returns whether or not the view is currently within the context of a ChameleonForms form.
        /// If this returns true then you can safely call <see cref="GetChameleonForm{TModel}"/>.
        /// </summary>
        /// <param name="helper">The current HTML helper</param>
        /// <returns>Whether or not there is a ChameleonForms form in context</returns>
        public static bool IsInChameleonForm(this IHtmlHelper helper)
        {
            return helper.ViewData.ContainsKey(Constants.ViewDataFormKey);
        }

        /// <summary>
        /// Returns the current ChameleonForm that is in context for the view.
        /// </summary>
        /// <typeparam name="TModel">The page model type</typeparam>
        /// <param name="helper">The current HTML helper</param>
        /// <returns>The ChameleonForms <see cref="Form{TModel}"/> instance</returns>
        public static Form<TModel> GetChameleonForm<TModel>(this IHtmlHelper<TModel> helper)
        {
            if (!helper.IsInChameleonForm())
                throw new InvalidOperationException("Attempted to retrieve a ChameleonForms form instance, but one wasn't in context.");

            var form = helper.ViewData[Constants.ViewDataFormKey];
            if (form is Form<TModel> castedForm)
                return castedForm;

            throw new InvalidOperationException($"Attempted to retrieve a ChameleonForms form instance as Form<{typeof(TModel).Name}>, but instead found a {form.GetType().Name}.");
        }

        /// <summary>
        /// Returns whether or not the view is currently within the context of a ChameleonForms form section.
        /// If this returns true then you can safely call <see cref="GetChameleonFormsSection{TModel}"/>.
        /// </summary>
        /// <param name="helper">The current HTML helper</param>
        /// <returns>Whether or not there is a ChameleonForms form section in context</returns>
        public static bool IsInChameleonFormsSection(this IHtmlHelper helper)
        {
            return helper.ViewData.ContainsKey(Constants.ViewDataSectionKey);
        }

        /// <summary>
        /// Returns the current ChameleonForms section that is in context for the view.
        /// </summary>
        /// <typeparam name="TModel">The page model type</typeparam>
        /// <param name="helper">The current HTML helper</param>
        /// <returns>The ChameleonForms <see cref="Section{TModel}"/> instance</returns>
        public static Section<TModel> GetChameleonFormsSection<TModel>(this IHtmlHelper<TModel> helper)
        {
            if (!helper.IsInChameleonFormsSection())
                throw new InvalidOperationException("Attempted to retrieve a ChameleonForms form section instance, but one wasn't in context.");

            var section = helper.ViewData[Constants.ViewDataSectionKey];
            if (section is Section<TModel> castedSection)
                return castedSection;

            throw new InvalidOperationException($"Attempted to retrieve a ChameleonForms form secton instance as Section<{typeof(TModel).Name}>, but instead found a {section.GetType().Name}.");
        }

        /// <summary>
        /// Returns whether or not the view is currently within the context of a ChameleonForms form navigation.
        /// If this returns true then you can safely call <see cref="GetChameleonFormsNavigation{TModel}"/>.
        /// </summary>
        /// <param name="helper">The current HTML helper</param>
        /// <returns>Whether or not there is a ChameleonForms form navigation in context</returns>
        public static bool IsInChameleonFormsNavigation(this IHtmlHelper helper)
        {
            return helper.ViewData.ContainsKey(Constants.ViewDataNavigationKey);
        }

        /// <summary>
        /// Returns the current ChameleonForms navigation that is in context for the view.
        /// </summary>
        /// <typeparam name="TModel">The page model type</typeparam>
        /// <param name="helper">The current HTML helper</param>
        /// <returns>The ChameleonForms <see cref="Navigation{TModel}"/> instance</returns>
        public static Navigation<TModel> GetChameleonFormsNavigation<TModel>(this IHtmlHelper<TModel> helper)
        {
            if (!helper.IsInChameleonFormsNavigation())
                throw new InvalidOperationException("Attempted to retrieve a ChameleonForms form navigation instance, but one wasn't in context.");

            var navigation = helper.ViewData[Constants.ViewDataNavigationKey];
            if (navigation is Navigation<TModel> castedNavigation)
                return castedNavigation;

            throw new InvalidOperationException($"Attempted to retrieve a ChameleonForms form secton instance as Navigation<{typeof(TModel).Name}>, but instead found a {navigation.GetType().Name}.");
        }
    }
}
