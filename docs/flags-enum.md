Flags Enum Fields
=================

If you want the user to specify multiple values from an enum you can either use a [non-flags enum against any property with a type convertible to `IEnumerable<%enumType%>`](multiple-enum.md) or use a flags enum, e.g.:

```csharp
[Flags]
public enum MyFlagsEnum
{
    ValueOne = 1 << 0,
    ValueTwo = 1 << 1,
    ValueThree = 1 << 2,
    ...
}
...
[RequiredFlagsEnum]
public MyFlagsEnum RequiredFlagsEnumWithZeroAsUnselectedValue { get; set; }

[RequiredFlagsEnum]
public MyFlagsEnum? RequiredFlagsEnumWithNullAsUnselectedValue { get; set; }

public MyFlagsEnum? NonRequiredFlagsEnumAndNullAsUnselectedValue { get; set; }
```

Flags enums have a few rough edges on them if you aren't careful so it's a good idea to read the [guidance](https://msdn.microsoft.com/en-us/library/ms229062(v=vs.100).aspx) [for](https://msdn.microsoft.com/en-us/library/system.flagsattribute.aspx) how to use them. In particular, make sure that none of your values have a value of 0 and you explicitly assign integer values to all enum values in multiples of 2.

If you want the user to specify a single value from an enum then you can [use the enum type directly](enum.md).

Required validation
-------------------

ASP.NET MVC's default validation doesn't pick up `0` for a flags enum as the field not being specified, thus you need to alter the validation for requires flags enums. ChameleonForms provides the `[RequiredFlagsEnum]` attribute to overcome that problem. This might look like:

```csharp
[RequiredFlagsEnum]
public MyFlagsEnum FlagsEnumField { get; set; }
```

In order for this attribute to correctly apply client side validation you need to ensure the following call is made on your application start (this should be automatically added when installing ChameleonForms, but if you are upgrading from before version 3 it's possible it won't be added automatically):

```csharp
DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RequiredFlagsEnumAttribute), typeof(RequiredAttributeAdapter));
```

Model binding
-------------

The default MVC model binder does **not** correctly bind flags enum values. ChameleonForms provides the `FlagsEnumModelBinder` to assist with that.

When you install ChameleonForms it should automatically register this model binder for all of the flags enum types registered in your MVC project within the `RegisterChameleonFormsComponents.cs` file. If you are upgrading from a pre version 3.0 version of ChameleonForms then the registration may not automatically add itself. Also, if you have view models / flags enums defined outside of your MVC project then the default registration won't work. This is the registration code we add (you may need to alter the assembly being scanned or alternatively explicitly register the model binder for the flags enums in question):

```csharp
    GetType().Assembly.GetTypes().Where(t => t.IsEnum && t.GetCustomAttributes(typeof(FlagsAttribute), false).Any())
        .ToList().ForEach(t =>
        {
            System.Web.Mvc.ModelBinders.Binders.Add(t, new FlagsEnumModelBinder());
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(Nullable<>).MakeGenericType(t), new FlagsEnumModelBinder());
        });
```

If registering a specifc flags enum type you can simply use:

```csharp
	System.Web.Mvc.ModelBinders.Binders.Add(typeof(MyFlagsEnum), new FlagsEnumModelBinder());
	System.Web.Mvc.ModelBinders.Binders.Add(typeof(MyFlagsEnum?), new FlagsEnumModelBinder());
```

Default HTML
------------

### Required nullable or non-nullable enum (multi-select drop-down with no empty option)

```html
<select %validationAttrs% %htmlAttributes% multiple="multiple" id="%propertyName%" name="%propertyName%">
%foreach enum value x%
    <option value="%x.ToString()%">%x.Humanize()%</option>
%endforeach%
</select>
```

### Non-Required nullable enum (multi-select drop-down with empty option)

```html
<select %validationAttrs% %htmlAttributes% multiple="multiple" id="%propertyName%" name="%propertyName%">
    <option selected="selected" value="">%noneDescription%</option>
%foreach enum value x%
    <option value="%x.ToString()%">%x.Humanize()%</option>
%endforeach%
</select>
```

### Explanation and example

Please see the explanation an example on the [Enum Field](enum#explanation-and-example) page.

Configurability
---------------

### Display as list of checkboxes

You can force a list of enums field to display as a list of checkboxes rather than a multi-select drop-down using the `AsCheckboxList` method on the Field Configuration, e.g.:

```csharp
@s.FieldFor(m => m.FlagsEnum).AsCheckboxList()
```

This will change the default HTML for a both Required and non-Required list of enums (both nullable and non-nullable) fields as shown above to:

```html
<ul>
%foreach enum value x with increment i %
    <li><input %validationAttrs% %htmlAttributes% id="%propertyName%_%i%" name="%propertyName%" type="checkbox" value="%x.ToString()%" /> <label for="%propertyName%_%i%">%x.Humanize()%</label></li>
%endforeach%
</ul>
```

### Change the text description of none

When you display a non-Required list of enums field as a drop-down you can change the text that is used to display the `none` value to the user. By default the text used is `None`. To change the text simply use the `WithNoneAs` method, e.g.:

```csharp
@s.FieldFor(m => m.NonRequiredNullableFlagsEnum).WithNoneAs("No value")
```

This will change the default HTML for the non-Required drop-down list of enum field as shown above to:

```html
<select %validationAttrs% %htmlAttributes% multiple="multiple" id="%propertyName%" name="%propertyName%">
    <option selected="selected" value="">No value</option>
    @* Enum values as <options>... *@
</select>
```

### Hide empty item
If you have a non-Required list of enums field then it will show the empty item and this item will be selected by default if no values are selected. If for some reason you want a non-Required list of enums field, but you would also like to hide the empty item you can do so with the `HideEmptyItem` method in the Field Configuration, e.g.:

```csharp
@s.FieldFor(m => m.NullableEnumListField).HideEmptyItem()
```

This will change the default HTML for the non-Required drop-down list of enum field as shown above to:

```html
<select %validationAttrs% %htmlAttributes% multiple="multiple" id="%propertyName%" name="%propertyName%">
    @* Enum values as <options>... *@
</select>
```