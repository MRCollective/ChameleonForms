# Message

The Message is a container to show a message to the user; you create a Message by instantiating a `Message<TModel>` within a `using` block. The start and end of the `using` block will output the start and end HTML for the Message and the inside of the `using` block will contain the Message content.

The `Message<TModel>` class looks like this and is in the `ChameleonForms.Component` namespace:

```cs
    /// <summary>
    /// Wraps the output of a message to display to a user.
    /// </summary>
    /// <typeparam name="TModel">The view model type for the current view</typeparam>
    
    public class Message<TModel> : FormComponent<TModel>
    {
        /// <summary>
        /// Creates a message.
        /// </summary>
        /// <param name="form">The form the message is being created in</param>
        /// <param name="messageType">The type of message to display</param>
        /// <param name="heading">The heading for the message</param>
        public Message(IForm<TModel> form, MessageType messageType, IHtmlContent heading);

        /// <summary>
        /// Creates a message.
        /// </summary>
        /// <param name="form">The form the message is being created in</param>
        /// <param name="messageType">The type of message to display</param>
        /// <param name="heading">The heading for the message</param>
        public Message(IForm<TModel> form, MessageType messageType, string heading);

        /// <summary>
        /// Returns the HTML representation of the beginning of the form component.
        /// </summary>
        /// <returns>The beginning HTML for the form component</returns>
        public virtual IHtmlContent Begin();

        /// <summary>
        /// Returns the HTML representation of the end of the form component.
        /// </summary>
        /// <returns>The ending HTML for the form component</returns>
        public virtual IHtmlContent End();

        /// <summary>
        /// Creates the HTML for a paragraph in the message.
        /// </summary>
        /// <param name="paragraph">The paragraph to output</param>
        /// <returns>The HTML for the paragraph</returns>
        public virtual IHtmlContent Paragraph(string paragraph);

        /// <summary>
        /// Creates the HTML for a paragraph in the message.
        /// </summary>
        /// <param name="paragraph">The paragraph to output</param>
        /// <returns>The HTML for the paragraph</returns>
        public virtual IHtmlContent Paragraph(IHtmlContent paragraph);

        /// <summary>
        /// Creates the HTML for a paragraph in the message.
        /// </summary>
        /// <param name="paragraph">The paragraph to output as a templated razor delegate</param>
        /// <returns>The HTML for the paragraph</returns>
        public virtual IHtmlContent Paragraph(Func<dynamic, IHtmlContent> paragraph);
```

The start and end HTML of the Message are generated via the `BeginMessage` and `EndMessage` methods in the template and the paragraph is generated via the `MessageParagraph` method in the template.

## Default usage

In order to get an instance of a `Message<TModel>` you can use the `BeginMessage` extension method on the [Form](the-form.md), e.g.:

```cshtml
@using (var m = form.BeginMessage(MessageType.Information, "Message title")) {
    @* Message content *@
}
```

The `BeginMessage` extension methods look like this:

```cs
        /// <summary>
        /// Creates a message.
        /// </summary>
        /// <example>
        /// @using (var m = f.BeginMessage(MessageType.Success, "Your submission was successful")) {
        ///     @m.Paragraph(string.Format("Your item was successfully created with id {0}", Model.Id))
        /// }
        /// </example>
        /// <typeparam name="TModel">The view model type for the current view</typeparam>        
        /// <param name="form">The form the message is being created in</param>
        /// <param name="messageType">The type of message to display</param>
        /// <param name="heading">The heading for the message</param>
        /// <returns>The message</returns>
        public static Message<TModel> BeginMessage<TModel>(this IForm<TModel> form, MessageType messageType, string heading = null)
        {
            return new Message<TModel>(form, messageType, heading);
        }

        /// <summary>
        /// Creates a message.
        /// </summary>
        /// <example>
        /// @using (var m = f.BeginMessage(MessageType.Success, new HtmlString("&lt;strong&gt;The submission was successful&lt;/strong&gt;"))) {
        ///     @m.Paragraph(string.Format("Your item was successfully created with id {0}", Model.Id))
        /// }
        /// </example>
        /// <typeparam name="TModel">The view model type for the current view</typeparam>        
        /// <param name="form">The form the message is being created in</param>
        /// <param name="messageType">The type of message to display</param>
        /// <param name="heading">The heading for the message</param>
        /// <returns>The message</returns>
        public static Message<TModel> BeginMessage<TModel>(this IForm<TModel> form, MessageType messageType, IHtmlContent heading)
        {
            return new Message<TModel>(form, messageType, heading);
        }

        /// <summary>
        /// Creates a message.
        /// </summary>
        /// <example>
        /// @using (var m = f.BeginMessage(MessageType.Success, new HtmlString(@&lt;strong&gt;The submission was successful&lt;/strong&gt;))) {
        ///     @m.Paragraph(string.Format("Your item was successfully created with id {0}", Model.Id))
        /// }
        /// </example>
        /// <typeparam name="TModel">The view model type for the current view</typeparam>        
        /// <param name="form">The form the message is being created in</param>
        /// <param name="messageType">The type of message to display</param>
        /// <param name="heading">The heading for the message as a templated razor delegate</param>
        /// <returns>The message</returns>
        public static Message<TModel> BeginMessage<TModel>(this IForm<TModel> form, MessageType messageType, Func<dynamic, IHtmlContent> heading)
        {
            return new Message<TModel>(form, messageType, heading(null));
        }
```

The `MessageType` enum is defined like this and appears in the `ChameleonForms.Enums` namespace:

```cs
    /// <summary>
    /// Types of messages that can be displayed to the user
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// User action required.
        /// </summary>
        Action,

        /// <summary>
        /// Action successful.
        /// </summary>
        Success,

        /// <summary>
        /// Action failed.
        /// </summary>
        Failure,

        /// <summary>
        /// Informational message.
        /// </summary>
        Information,

        /// <summary>
        /// Warning message.
        /// </summary>
        Warning
    }
```

## Message content

If you want to add paragraphs using the template you can do that by using one of the `Paragraph` methods as defined above and of course you can use normal HTML or any valid Razor code as well:

```cshtml
@using (var m = f.BeginMessage(MessageType.Information, "Message title")) {
    @m.Paragraph("Here is the first part of the message")
    <img src="/path/to/img" alt="alt text" />
    @m.Paragraph(new HtmlString("Here is a <strong>styled</strong> message"))
    @SomeRazorFunctionDefinedOnThisPage()
    <partial name="_WooWeCanGetReallyCrazyAndAddPartialsToo_OMG"></partial>
    @m.Paragraph(@<strong>Message content</strong>)
}
```

## Default HTML

### Begin HTML

```html
      <div class="%messagetype%_message">
          %if heading%<h3>%heading%</h3>%endif%
          <div class="message">
```

### End HTML

```html
          </div>
      </div>
```

### Paragraph HTML

```html
<p>%content%</p>
```

## Twitter Bootstrap 3 HTML

### Begin HTML
```html
      <div class="panel panel-%messageType.ToTwitterEmphasisStyle()%">
          %if heading%<div class="panel-heading"><h4 class="panel-title">%heading%</h4></div>%endif%
          <div class="panel-body">
```

The `ToTwitterEmphasisStyle` extension method performs the following conversions from the ChameleonForms message types to the [Twitter Emphasis Styles](http://getbootstrap.com/css/#buttons-options):

* `Action` becomes `Primary`
* `Success` becomes `Success`
* `Failure` becomes `Danger`
* `Information` becomes `Info`
* `Warning` becomes `Warning`
* Anything else becomes `Default`

### End HTML

```html
          </div>
      </div>
```

### Paragraph HTML

Same as default.
