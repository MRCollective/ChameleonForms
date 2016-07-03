Using different form templates
==============================

Default form template
---------------------
See [Configuring the Default Form Template](the-form#configuring-the-default-form-template).

Custom extension method
-----------------------
If you would like to change the template being used across one or more forms, or would like a method name that is more meaningful to your application than `BeginChameleonForm`, you can simply define you own extension method instead, e.g.:

```csharp
    public static class FormHelpers
    {
        public static IForm<TModel> BeginMyApplicationNameForm<TModel>(this HtmlHelper<TModel> htmlhelper, string action = "", FormMethod method = FormSubmitMethod.Post, HtmlAttributes htmlAttributes = null, EncType? enctype = null)
        {
            return new Form<TModel>(htmlhelper, new MyFormTemplate(), action, method, htmlAttributes, enctype);
        }
    }
```

You can exclude some of the optional parameters if you never have a need to configure them per form. Also, this assumes you have a custom template called `MyFormTemplate`, but you could just as easily use a [built-in template inside ChameleonForms](https://github.com/MRCollective/ChameleonForms/tree/master/ChameleonForms/Templates).

Using multiple templates in a single application
------------------------------------------------
If you want to use different form templates across multiple forms in a single application then you can't make use of `FormTemplate.Default` and you will need to create multiple extension methods (as shown above) for each form template you want to use. Then you opt in to a particular type of template by using the corresponding extension method for a given form.