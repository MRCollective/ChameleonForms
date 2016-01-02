Multiple-Select List Fields
===========================

If you want the user to specify multiple values from items in an arbitrary list of objects you can use the `[ExistsIn]` attribute against a model property that enumerates the type of the value property, e.g.:

```c#

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
    public IEnumerable<int> EnumerableListId { get; set; }

    [ExistsIn("ListValues", "Id", "Name")]
    public List<int> ListListId { get; set; }

    // Or, alternatively - we recommend using nullable types for multi-select items that aren't enums

    [ExistsIn("ListValues", "Id", "Name")]
    public IEnumerable<int?> NullableEnumerableListId { get; set; }

    [ExistsIn("ListValues", "Id", "Name")]
    public List<int?> NullableListListId { get; set; }
}
```

Note: as you will see below - there isn't much difference in specifying a nullable vs non-nullable type as the type being collected, except if you specify a non-nullable type and the list of items is not Required then the default MVC model binder doesn't work very well (if you are using a drop-down) and will conflict with the `[ExistsIn]` validation.

There is a definition for the `[ExistsIn]` attribute on the [List](list) page.

Default HTML
------------

### Required nullable or non-nullable list id (multi-select drop-down with no empty option)

When using the Default Field Generator then the default HTML of the [Field Element](field-element) for a Required nullable or non-nullable list id will be:

```html
<select %validationAttrs% %htmlAttributes% multiple="multiple" id="%propertyName%" name="%propertyName%">
%foreach item x in Model.{ListProperty}%
    <option value="%x.{ValueProperty}%">%x.{NameProperty}%</option>
%endforeach%
</select>
```

### Non-Required nullable or non-nullable list id (multi-select drop-down with empty option)

When using the Default Field Generator then the default HTML of the [Field Element](field-element) for a Non-Required nullable or non-nullable list id will be:

```html
<select %validationAttrs% %htmlAttributes% multiple="multiple" id="%propertyName%" name="%propertyName%">
    <option selected="selected" value="">%noneDescription%</option>
%foreach item x in Model.{ListProperty}%
    <option value="%x.{ValueProperty}%">%x.{NameProperty}%</option>
%endforeach%
</select>
```

Server-side validation
----------------------

If you want to provide server-side validation protection of the value the user submitted then the `[ExistsIn]` attribute will automatically take care of this for you by default.

The documentation for how to use and configure server-side validation can be found on the [List](list) page.

Configurability
---------------

### Display as list of checkboxes

You can force a list of list items field to display as a list of checkboxes (say that 10 times fast!) rather than a drop-down using the `AsCheckboxList` method on the Field Configuration, e.g.:

```c#
@s.FieldFor(m => m.EnumerableListId).AsCheckboxList()
@s.FieldFor(m => m.ListListId).AsCheckboxList()
@s.FieldFor(m => m.NullableEnumerableListId).AsCheckboxList()
@s.FieldFor(m => m.NullableListListId).AsCheckboxList()
```

This will change the default HTML for both Required and non-Required fields with nullable and non-nullable list ids as shown above to:

```html
<ul>
%foreach item x in Model.{ListProperty} with increment i%
    <li><input %htmlAttributes% id="%propertyName%_%i%" name="%propertyName%" type="checkbox" value="%x.{ValueProperty}%"> <label for="%propertyName%_%i%">%x.{NameProperty}%</label></li>
%endforeach%
</ul>
```

### Change the text description of none

When you display a non-Required list of list values field as a drop-down you can change the text that is used to display the `none` value to the user. By default the text used is `None`. To change the text simply use the `WithNoneAs` method, e.g.:

```c#
@s.FieldFor(m => m.EnumerableListId).WithNoneAs("No value")
```

This will change the default HTML for the enumerable list id field as shown above to:

```html
<select %validationAttrs% %htmlAttributes% multiple="multiple" id="%propertyName%" name="%propertyName%">
    <option selected="selected" value="">No value</option>
    @* List item values as <options>... *@
</select>
```

### Hide empty item
If you have a non-Required list of list values field as a drop-down then it will show the empty item and this item will be selected by default if there are no values selected. If for some reason you want one of these fields, but you would also like to hide the empty item you can do so with the `HideEmptyItem` method in the Field Configuration, e.g.:

```c#
@s.FieldFor(m => m.EnumerableListId).HideEmptyItem()
```

This will change the default HTML for the enumerable list id field as shown above to:

```html
<select %validationAttrs% %htmlAttributes% multiple="multiple" id="%propertyName%" name="%propertyName%">
    @* List item values as <options>... *@
</select>
```