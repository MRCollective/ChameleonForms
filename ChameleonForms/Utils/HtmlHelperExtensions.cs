using System;
using System.Linq;
using System.Linq.Expressions;
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
        public static IForm<TModel> GetChameleonForm<TModel>(this IHtmlHelper<TModel> helper)
        {
            if (!helper.IsInChameleonForm())
                throw new InvalidOperationException("Attempted to retrieve a ChameleonForms form instance, but one wasn't in context.");

            var form = helper.ViewData[Constants.ViewDataFormKey];
            if (form is IForm<TModel> castedForm)
                return castedForm;

            // It's not an IForm<TModel>, but it is an IForm<Something>
            if (form.GetType().GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IForm<>)))
            {
                var formInterface = form.GetType().GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IForm<>));
                var originalType = formInterface.GenericTypeArguments[0];

                // It's an IForm<BaseType> - it's probably because GetChameleonForm() has been called from a partial where the model has been casted to a base type
                // Let's do a form.CreatePartialForm(m => (BaseType)m)
                if (typeof(TModel).IsAssignableFrom(originalType))
                {
                    var parameter = Expression.Parameter(originalType);
                    var lambda = Expression.Lambda(Expression.Convert(parameter, typeof(TModel)), parameter);
                    return ((IForm) form).CreatePartialForm<TModel>(lambda, helper);
                }

                throw new InvalidOperationException($"Attempted to retrieve a ChameleonForms form instance as Form<{typeof(TModel).Name}>, but instead found a Form<{originalType.Name}>.");
            }

            throw new InvalidOperationException($"Attempted to retrieve a ChameleonForms form instance as Form<{typeof(TModel).Name}>, but instead found a {form.GetType().FullName}.");
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
        /// Returns the current ChameleonForms form section that is in context for the view.
        /// </summary>
        /// <typeparam name="TModel">The page model type</typeparam>
        /// <param name="helper">The current HTML helper</param>
        /// <returns>The ChameleonForms <see cref="Section{TModel}"/> instance</returns>
        public static ISection<TModel> GetChameleonFormsSection<TModel>(this IHtmlHelper<TModel> helper)
        {
            if (!helper.IsInChameleonFormsSection())
                throw new InvalidOperationException("Attempted to retrieve a ChameleonForms form section instance, but one wasn't in context.");

            var section = helper.ViewData[Constants.ViewDataSectionKey];
            if (section is ISection<TModel> castedSection)
                return castedSection;

            // It's not an ISection<TModel>, but it is an ISection<Something>
            if (section.GetType().GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISection<>)))
            {
                var sectionInterface = section.GetType().GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISection<>));
                var originalType = sectionInterface.GenericTypeArguments[0];

                // It's an ISection<BaseType> - it's probably because GetChameleonFormSection() has been called from a partial where the model has been casted to a base type
                // Let's do a section.CreatePartialSection(m => (BaseType)m)
                if (typeof(TModel).IsAssignableFrom(originalType))
                {
                    var parameter = Expression.Parameter(originalType);
                    var lambda = Expression.Lambda(Expression.Convert(parameter, typeof(TModel)), parameter);
                    return ((ISection)section).CreatePartialSection<TModel>(helper.GetChameleonForm());
                }

                throw new InvalidOperationException($"Attempted to retrieve a ChameleonForms form section instance as Section<{typeof(TModel).Name}>, but instead found a Section<{originalType.Name}>.");
            }

            throw new InvalidOperationException($"Attempted to retrieve a ChameleonForms form section instance as Section<{typeof(TModel).Name}>, but instead found a {section.GetType().FullName}.");
        }

        /// <summary>
        /// Returns whether or not the view is currently within the context of a ChameleonForms form field.
        /// If this returns true then you can safely call <see cref="GetChameleonFormsField{TModel}"/>.
        /// </summary>
        /// <param name="helper">The current HTML helper</param>
        /// <returns>Whether or not there is a ChameleonForms form field in context</returns>
        public static bool IsInChameleonFormsField(this IHtmlHelper helper)
        {
            return helper.ViewData.ContainsKey(Constants.ViewDataFieldKey);
        }

        /// <summary>
        /// Returns the current ChameleonForms form field that is in context for the view.
        /// </summary>
        /// <typeparam name="TModel">The page model type</typeparam>
        /// <param name="helper">The current HTML helper</param>
        /// <returns>The ChameleonForms <see cref="Field{TModel}"/> instance</returns>
        public static Field<TModel> GetChameleonFormsField<TModel>(this IHtmlHelper<TModel> helper)
        {
            if (!helper.IsInChameleonFormsField())
                throw new InvalidOperationException("Attempted to retrieve a ChameleonForms form field instance, but one wasn't in context.");

            var field = helper.ViewData[Constants.ViewDataFieldKey];
            if (field is Field<TModel> castedField)
                return castedField;

            throw new InvalidOperationException($"Attempted to retrieve a ChameleonForms form field instance as Field<{typeof(TModel).Name}>, but instead found a {field.GetType().Name}.");
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
