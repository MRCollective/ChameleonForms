<div class="row equal-heights">
    <div class="col-md-3">
        <h1>Model-driven forms</h1>
        <p>Spend less time with tedious reptition by letting your view models do the hard work for you.</p>
    </div>
    <div class="col-md-3">
        <pre><code class="lang-c#">
public class SignupViewModel
{
    [Required]
    public MembershipClass? MembershipType { get; set; }
}
        </code></pre>
    </div>
    <div class="col-md-3">
        <pre><code class="lang-cshtml">
&lt;field for="MembershipType" />
        </code></pre>
    </div>
    <div class="col-md-3">
        <pre><code class="lang-html">
&lt;dt>&lt;label for="MembershipType">Membership type&lt;/label> &lt;em class="required">*&lt;/em>&lt;/dt>
&lt;dd>
    &lt;select data-val="true" data-val-required="The Membership type field is required." id="MembershipType" name="MembershipType" required="required">&lt;option selected="selected" value="Standard">Standard&lt;/option>
&lt;option value="Bonze">Bonze&lt;/option>
&lt;option value="Silver">Silver&lt;/option>
&lt;option value="Gold">Gold&lt;/option>
&lt;option value="Platinum">Platinum&lt;/option>
&lt;/select> &lt;span class="field-validation-valid" data-valmsg-for="MembershipType" data-valmsg-replace="true">&lt;/span>
&lt;/dd>
        </code></pre>
    </div>
</div>
