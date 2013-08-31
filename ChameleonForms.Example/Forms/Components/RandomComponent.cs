﻿using System.Web;
using System.Web.Mvc;
using ChameleonForms.Component;
using ChameleonForms.Enums;
using ChameleonForms.Example.Forms.Templates;

namespace ChameleonForms.Example.Forms.Components
{
    /// <summary>
    /// Illustrates using a custom template class and wrapping an unencoded string output.
    /// </summary>
    public class RandomComponent<TModel> : IFormComponent<TModel, RandomFormTemplate>
    {
        public IForm<TModel, RandomFormTemplate> Form { get; private set; }

        public RandomComponent(IForm<TModel, RandomFormTemplate> form)
        {
            Form = form;
        }

        public override string ToString()
        {
            return Form.Template.RandomComponent();
        }
    }

    /// <summary>
    /// Illustrates using a custom template class and wrapping an encoded HTML string output.
    /// </summary>
    public class RandomComponent2<TModel> : IFormComponent<TModel, RandomFormTemplate>, IHtmlString
    {
        public IForm<TModel, RandomFormTemplate> Form { get; private set; }

        public RandomComponent2(IForm<TModel, RandomFormTemplate> form)
        {
            Form = form;
        }

        public string ToHtmlString()
        {
            return Form.Template.RandomComponent2().ToHtmlString();
        }
    }

    public static class RandomComponentExtensions
    {
        public static Form<TModel, RandomFormTemplate> BeginRandomForm<TModel>(this HtmlHelper<TModel> helper, string action, FormMethod method, object htmlAttributes = null, EncType? enctype = null)
        {
            return new Form<TModel, RandomFormTemplate>(helper, new RandomFormTemplate(), action, method, htmlAttributes.ToHtmlAttributes(), enctype);
        }

        public static RandomComponent<TModel> RandomComponent<TModel>(this Form<TModel, RandomFormTemplate>  form)
        {
            return new RandomComponent<TModel>(form);
        }

        public static RandomComponent2<TModel> RandomComponent2<TModel>(this Form<TModel, RandomFormTemplate> form)
        {
            return new RandomComponent2<TModel>(form);
        }
    }
}