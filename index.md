<div class="row">
<div class="col-md-4">
    <h2>Model-driven forms</h2>
    <p>Spend less time with tedious reptition by letting your view models do the hard work for you.</p>
</div>
<div class="col-md-8 equal-heights">

# [View Model](#tab/model-1)
```c#
public class SignupViewModel
{
    [Required]
    public MembershipClass? MembershipType { get; set; }
}

public enum MembershipClass
{
    Standard,
    Bonze,
    Silver,
    Gold,
    Platinum
}
```

# [View](#tab/model-2)

**Tag helpers variant**

```html
<form-section heading="Form section">
    <field for="MembershipType" />
</form-section>
```

**HTML helpers variant**

```cshtml
@using (var s = f.BeginSection("Form section")) {
    @s.FieldFor(m => m.MembershipType)
}
```

# [HTML output](#tab/model-3)
```html
<fieldset>
    <legend>Form section</legend>
    <dl>
        <dt><label for="MembershipType">Membership type</label> <em class="required">*</em></dt>
        <dd>
            <select data-val="true" data-val-required="The Membership type field is required." id="MembershipType" name="MembershipType" required="required">
                <option selected="selected" value="Standard">Standard</option>
                <option value="Bonze">Bonze</option>
                <option value="Silver">Silver</option>
                <option value="Gold">Gold</option>
                <option value="Platinum">Platinum</option>
            </select>
            <span class="field-validation-valid" data-valmsg-for="MembershipType" data-valmsg-replace="true"></span>
        </dd>
    </dl>
</fieldset>
```

# [Notes](#tab/model-4)

Here are the things that ChameleonForms has done for us based on the model:

* The field label is based on the field name, but humanised (e.g. `MembershipType` -> `Membership type`)
* The `[Required]` attribute results in a required designator (`<em class="required">*</em>`), required attribute on the field (`required="required"`) required unobtrusive client-side validation attributes (`data-val` and `data-val-required`)
* The enum type automatically results in a `<select>` with the different enum values translated to `<option>`'s

# [Further reading](#tab/model-5)

Other model-driven form features you can explore:

* Inference from model type to output [Boolean fields](docs/boolean.md), [DateTime fields](docs/datetime.md), [Enum fields](docs/enum.md), [List fields](docs/list.md), [File upload fields](docs/file-upload.md) and [Number fields](docs/number.md)
* Multiple-select: [Flags enum fields](docs/flags-enum.md), [Multiple-select enum fields](docs/multiple-enum.md) and [Multiple-select list fields](docs/multiple-list.md)
* Inference from model property attributes to output [Textarea fields](docs/textarea.md), [Password fields](docs/password.md), [Email fields](docs/email.md) and [Uri fields](docs/uri.md) as well as supporting controlling the client-side and server-side validation of [DateTime fields](docs/datetime.md)

***

</div>
</div>
