List Fields
===========

If you want the user to specify an item from an arbitrary list of objects you can use the `[ExistsIn]` attribute against a model property of the type of the value property, e.g.:

```csharp

public class MyObject
{
    public string Name { get; set; }
    public int Id { get; set; }
}

public class MyViewModel
{
    public MyViewModel() {
        ListValues = new List<MyObject>
        {
            new MyObject { Id = 1, Name = "First item" },
            new MyObject { Id = 2, Name = "Second item" },
        };
    }

    ...
    public List<MyObject> ListValues { get; set; }

    [ExistsIn("ListValues", "Id", "Name")]
    public int ListId { get; set; }

    [ExistsIn("ListValues", "Id", "Name")]
    public int? NullableListId { get; set; }
}
```

The `ExistsIn` attribute looks like this:

```csharp
    /// <summary>
    /// Indicates that the attributed property value should exist within the list property referenced by the attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ExistsInAttribute : ValidationAttribute, IMetadataAware
    {
        /// <summary>
        /// Application-wide configuration for whether or not to enable ExistsIn validation.
        /// </summary>
        public static bool EnableValidation = true;

        /// <summary>
        /// Instantiates an <see cref="ExistsInAttribute"/>.
        /// </summary>
        /// <param name="listProperty">The name of the property containing the list this property should reference.</param>
        /// <param name="valueProperty">The name of the property of the list items to use for the value</param>
        /// <param name="nameProperty">The name of the property of the list items to use for the name/label</param>
        public ExistsInAttribute(string listProperty, string valueProperty, string nameProperty) { ... }

        /// <summary>
        /// Instantiates an <see cref="ExistsInAttribute"/>.
        /// </summary>
        /// <param name="listProperty">The name of the property containing the list this property should reference.</param>
        /// <param name="valueProperty">The name of the property of the list items to use for the value</param>
        /// <param name="nameProperty">The name of the property of the list items to use for the name/label</param>
        /// <param name="enableValidation">Optional override for ExistsIn server-side validation configuration (if not specified, static configuration setting ExistsInAttribute.EnableValidation is used)</param>
        public ExistsInAttribute(string listProperty, string valueProperty, string nameProperty, bool enableValidation) { ... }
    }
```

Default HTML
------------

### Non-nullable list id (drop-down with no empty option)

When using the Default Field Generator then the default HTML of the [Field Element](field-element) for a non-nullable list id will be:

```html
<select %validationAttrs% %htmlAttributes% id="%propertyName%" name="%propertyName%">
%foreach item x in Model.{ListProperty}%
    <option value="%x.{ValueProperty}%">%x.{NameProperty}%</option>
%endforeach%
</select>
```

If the list id is non-nullable then the field will be Required regardless of whether you specified the `[Required]` attribute.

So in the above example when outputting the Field Element HTML for the `ListId` property it would have (assuming you didn't specify any additional HTML attributes):

```html
<select data-val="true" data-val-number="The field List id must be a number." data-val-required="The List id field is required." id="ListId" name="ListId">
    <option value="1">First item</option>
    <option value="2">Second item</option>
</select>
```

### Nullable list id (drop-down with empty option)

When using the Default Field Generator then the default HTML of the [Field Element](field-element) for a nullable list id will be:

```html
<select %validationAttrs% %htmlAttributes% id="%propertyName%" name="%propertyName%">
    <option selected="selected" value="">%noneDescription%</option>
%foreach item x in Model.{ListProperty}%
    <option value="%x.{ValueProperty}%">%x.{NameProperty}%</option>
%endforeach%
</select>
```

So in the above example when outputting the Field Element HTML for the `ListId` property it would have (assuming you didn't specify any additional HTML attributes):

```html
<select data-val="true" data-val-number="The field List id must be a number." id="ListId" name="ListId">
    <option selected="selected" value=""></option>
    <option value="1">First item</option>
    <option value="2">Second item</option>
</select>
```

Server-side validation
----------------------

If you want to provide server-side validation protection of the value the user submitted then the `[ExistsIn]` attribute will automatically take care of this for you by default.

If you don't want to perform server-side validation then you can either:

* Turn off Exists In validation globally by setting the appropriate setting in your `Application_Start` function (or a method it calls) within `Global.asax.cs`:
```csharp
ExistsInAttribute.EnableValidation = false;
```
* Turn off validation on a per-usage basis by setting `false` to the `enableValidation` value when adding the attribute, e.g.:
```csharp
    [ExistsIn("ListValues", "Id", "Name", enableValidation: false)]
    public int ListId { get; set; }
```

If you turn off validation globally, but want to enable it for a specific usage then you can pass `true` to the `enableValidation` attribute - any value specified for it will override the global default.

If you want to take advantage of the server-side validation then the list needs to be populated when the `DefaultModelBinder` binds the property with the `[ExistsIn]` attribute specified. If the list is null at that point and validation is enabled then an exception will be thrown. If you want to specify the list at the right time then you have two options:

1. Define the list in the constructor (like the above example)
2. Create a custom model binder for your model type that creates the list first
    * This allows you to populate the list using the database by dependency injecting your database access component into the model binder (assuming you have model binders set up with your DI framework)
    * This also allows you to [easily unit test the model binder](http://blog.mdavies.net/2013/06/07/unit-testing-mvc3mvc4-model-binders/)

For example:

```csharp
    public class InvoiceSelectionViewModelBinder : DefaultModelBinder
    {
        private readonly IQueryExecutor _queryExecutor;

        public InvoiceSelectionViewModelBinder(IQueryExecutor queryExecutor)
        {
            _queryExecutor = queryExecutor;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = new InvoiceSelectionViewModel
            {
                Invoices = _queryExecutor.Execute(new GetInvoices(controllerContext.HttpContext.User.Identity.Name))
            };

            bindingContext.ModelMetadata.Model = model;

            return base.BindModel(controllerContext, bindingContext);
        }
    }
```

Remember to register the model binder with your type by using the following in `Global.asax.cs` (or, if you want dependency injection getting your DI framework to do it for you using attributes, e.g. [Autofac](http://alexmg.com/post/2010/12/08/Model-Binder-Injection-in-Autofac-ASPNET-MVC-3-Integration.aspx)):

```csharp
ModelBinders.Binders.Add(typeof(%yourModelType%), new %modelBinderType%());
```

Configurability
---------------

### Display as list of radio buttons

You can force a list field to display as a list of radio buttons rather than a drop-down using the `AsRadioList` method on the Field Configuration, e.g.:

```csharp
@s.FieldFor(m => m.ListId).AsRadioList()
@s.FieldFor(m => m.NullableListId).AsRadioList()
```

This will change the default HTML for the non-nullable list id field and the Required nullable list id field as shown above to:

```html
<ul>
%foreach item x in Model.{ListProperty} with increment i%
    <li><input %validationAttrs% %htmlAttributes% id="%propertyName%_%i%" name="%propertyName%" type="radio" value="%x.{ValueProperty}%"> <label for="%propertyName%_%i%">%x.{NameProperty}%</label></li>
%endforeach%
</ul>
```

And it will change the default HTML for the non-Required nullable list id field as shown above to:

```html
<ul>
    <li><input %validationAttrs% %htmlAttributes% id="%propertyName%_1" name="%propertyName%" type="radio" value=""> <label for="%propertyName%_1">%noneDescription%</label></li>
%foreach item x in Model.{ListProperty} with increment i%
    <li><input %htmlAttributes% id="%propertyName%_%i+1%" name="%propertyName%" type="radio" value="%x.{ValueProperty}%"> <label for="%propertyName%_%i+1%">%x.{NameProperty}%</label></li>
%endforeach%
</ul>
```

### Change the text description of none

When you display a nullable list id field as a drop-down or a non-Required nullable list id field as a list of radio buttons you can change the text that is used to display the `none` value to the user. By default the text used is an empty string for the drop-down and `None` for the radio button. To change the text simply use the `WithNoneAs` method, e.g.:

```csharp
@s.FieldFor(m => m.NullableListId).WithNoneAs("No value")
```

This will change the default HTML for the nullable list id field as shown above to:

```html
<select %validationAttrs% %htmlAttributes% id="%propertyName%" name="%propertyName%">
    <option selected="selected" value="">No value</option>
    @* List item values as <options>... *@
</select>
```

### Hide empty item
If you have a nullable list id field as a drop-down or a non-Required nullable list id field as a list of radio buttons then it will show the empty item and this item will be selected by default if the field value is null. If for some reason you want one of these fields, but you would also like to hide the empty item you can do so with the `HideEmptyItem` method in the Field Configuration, e.g.:

```csharp
@s.FieldFor(m => m.NullableListId).HideEmptyItem()
```

This will change the default HTML for the nullable list id field as shown above to:

```html
<select %validationAttrs% %htmlAttributes% id="%propertyName%" name="%propertyName%">
    @* List item values as <options>... *@
</select>
```