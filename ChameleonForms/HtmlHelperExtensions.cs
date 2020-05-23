using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Encodings.Web;
using ChameleonForms.Component;
using ChameleonForms.Templates;
using ChameleonForms.Utils;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;

namespace ChameleonForms
{
    /// <summary>
    /// Extension methods against HtmlHelper.
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Creates a HTML helper from a parent model to use a sub-property as it's model.
        /// </summary>
        /// <typeparam name="TParentModel">The model of the parent type</typeparam>
        /// <typeparam name="TChildModel">The model of the sub-property to use</typeparam>
        /// <param name="helper">The parent HTML helper</param>
        /// <param name="propertyFor">The sub-property to use</param>
        /// <param name="bindFieldsToParent">Whether to set field names to bind to the parent model type (true) or the sub-property type (false)</param>
        /// <returns>A HTML helper against the sub-property</returns>
        public static DisposableHtmlHelper<TChildModel> For<TParentModel, TChildModel>(this IHtmlHelper<TParentModel> helper,
            Expression<Func<TParentModel, TChildModel>> propertyFor, bool bindFieldsToParent)
        {
            return helper.For(helper.ViewData.Model != null ? propertyFor.Compile().Invoke(helper.ViewData.Model) : default(TChildModel),
                bindFieldsToParent ? helper.GetFieldName(propertyFor) : null);
        }
        
        /// <summary>
        /// Creates a HTML helper based on another HTML helper against a different model type.
        /// </summary>
        /// <typeparam name="TModel">The model type to create a helper for</typeparam>
        /// <param name="htmlHelper">The original HTML helper</param>
        /// <param name="model">An instance of the model type to use as the model</param>
        /// <param name="htmlFieldPrefix">A prefix value to use for field names</param>
        /// <returns>The HTML helper against the other model type</returns>
        public static DisposableHtmlHelper<TModel> For<TModel>(this IHtmlHelper htmlHelper,
            TModel model = default(TModel), string htmlFieldPrefix = null)
        {
            var viewContext = htmlHelper.ViewContext;
            var newViewData = new ViewDataDictionary<TModel>(viewContext.ViewData, model);

            if (!string.IsNullOrEmpty(htmlFieldPrefix))
            {
                newViewData.TemplateInfo.HtmlFieldPrefix = htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldPrefix);
            }

            var newViewContext = new ViewContext(viewContext, viewContext.View, newViewData, viewContext.Writer);

            var services = htmlHelper.ViewContext.HttpContext.RequestServices;

            return new DisposableHtmlHelper<TModel>(
                services.GetRequiredService<IHtmlGenerator>(),
                services.GetRequiredService<ICompositeViewEngine>(),
                services.GetRequiredService<IModelMetadataProvider>(),
                services.GetRequiredService<IViewBufferScope>(),
                HtmlEncoder.Default,
                UrlEncoder.Default,
                new ModelExpressionProvider(services.GetRequiredService<IModelMetadataProvider>()), 
                newViewContext);
        }

        /// <summary>
        /// Gets the registered default form template from RequestServices.
        /// </summary>
        /// <param name="htmlHelper">The HTML Helper</param>
        /// <returns>An instance of the default <see cref="IFormTemplate"/></returns>
        public static IFormTemplate GetDefaultFormTemplate(this IHtmlHelper htmlHelper)
        {
            return htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<IFormTemplate>();
        }

        /// <summary>
        /// Returns the full HTML field name for a field in a view model within the current context / prefix.
        /// </summary>
        /// <typeparam name="TModel">The view model type</typeparam>
        /// <typeparam name="TResult">The field type</typeparam>
        /// <param name="htmlHelper">The HTML helper</param>
        /// <param name="field">The field</param>
        /// <returns>The full HTML field name</returns>
        public static string GetFullHtmlFieldName<TModel, TResult>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TResult>> field)
        {
            return htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlHelper.GetFieldName(field));
        }

        /// <summary>
        /// Returns the field name for a field in a view model.
        /// </summary>
        /// <typeparam name="TModel">The view model type</typeparam>
        /// <typeparam name="TResult">The field type</typeparam>
        /// <param name="htmlHelper">The HTML helper</param>
        /// <param name="field">The field</param>
        /// <returns>The field name</returns>
        public static string GetFieldName<TModel, TResult>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TResult>> field)
        {
            var expressionProvider = htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<ModelExpressionProvider>();
            return expressionProvider.CreateModelExpression(htmlHelper.ViewData, field).Name;
        }


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
            {
                // The HTML helper is different - this probably means this section is being created from a partial view
                // We need to switch the HTML helper otherwise the output will be out of order
                if (castedForm.HtmlHelper != helper)
                    return castedForm.CreatePartialForm(helper);

                return castedForm;

            }

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
                    return ((IForm)form).CreatePartialForm<TModel>(lambda, helper);
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
            {
                // The HTML helper is different - this probably means this section is being created from a partial view
                // We need to switch the HTML helper otherwise the output will be out of order
                if (castedSection.Form.HtmlHelper != helper)
                    return castedSection.CreatePartialSection(helper);

                return castedSection;
            }

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

            throw new InvalidOperationException($"Attempted to retrieve a ChameleonForms form navigation instance as Navigation<{typeof(TModel).Name}>, but instead found a {navigation.GetType().Name}.");
        }

        /// <summary>
        /// Returns whether or not the view is currently within the context of a ChameleonForms form message.
        /// If this returns true then you can safely call <see cref="GetChameleonFormsMessage{TModel}"/>.
        /// </summary>
        /// <param name="helper">The current HTML helper</param>
        /// <returns>Whether or not there is a ChameleonForms form message in context</returns>
        public static bool IsInChameleonFormsMessage(this IHtmlHelper helper)
        {
            return helper.ViewData.ContainsKey(Constants.ViewDataMessageKey);
        }

        /// <summary>
        /// Returns the current ChameleonForms message that is in context for the view.
        /// </summary>
        /// <typeparam name="TModel">The page model type</typeparam>
        /// <param name="helper">The current HTML helper</param>
        /// <returns>The ChameleonForms <see cref="Message{TModel}"/> instance</returns>
        public static Message<TModel> GetChameleonFormsMessage<TModel>(this IHtmlHelper<TModel> helper)
        {
            if (!helper.IsInChameleonFormsMessage())
                throw new InvalidOperationException("Attempted to retrieve a ChameleonForms form message instance, but one wasn't in context.");

            var message = helper.ViewData[Constants.ViewDataMessageKey];
            if (message is Message<TModel> castedMessage)
                return castedMessage;

            throw new InvalidOperationException($"Attempted to retrieve a ChameleonForms form message instance as Message<{typeof(TModel).Name}>, but instead found a {message.GetType().Name}.");
        }
    }
}
