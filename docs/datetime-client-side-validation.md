Client-side validation of DateTime fields in ASP.NET MVC using DisplayFormat
============================================================================

ChameleonForms provides a way for you to hook into the unobtrusive validation library that ASP.NET MVC uses to validate that the date format the user specifies is OK based on the format string you provided.

In order for this to work you need to:

1. Reference the [jquery.validate.unobtrusive.chameleon.js](https://github.com/MRCollective/ChameleonForms/blob/master/ChameleonForms.Example/Scripts/jquery.validate.unobtrusive.chameleon.js) file
    * By default, when you install ChameleonForms by NuGet this file will be placed into `Scripts\jquery.validate.unobtrusive.chameleon.js` for you.
2. Place the `[DisplayFormat(DataFormatString = "{0:%format%}", ApplyInEditMode = true)]` attribute on the model property
3. Output the field using ChameleonForms

If you aren't using ChameleonForms to output the field then replace step 2 and 3 with:

1. Include the `data-val="true"` attribute to turn on unobtrusive validation for that field
2. Include the `data-val-date="%errorMessageIfDateIsIncorrect%"` attribute to indicate the field is a date and what message should display if the user enters an invalid date
3. Include the `data-val-format="%formatString%"` attribute to indicate the format string that should be validated against

### Limitations

Only the following date format strings are supported:

* `d/M/yyyy`
* `d-M-yyyy`
* `d/M/yy`
* `d-M-yy`
* `dd/MM/yyyy`
* `dd-MM-yyyy`
* `dd/MM/yy`
* `dd-MM-yy`
* `M/d/yyyy`
* `M-d-yyyy`
* `M/d/yy`
* `M-d-yy`
* `MM/dd/yyyy`
* `MM-dd-yyyy`
* `MM/dd/yy`
* `MM-dd-yy`
* `yyyy/MM/dd`
* `yyyy-MM-dd`
* `yyyy/M/d`
* `yyyy-M-d`

Only the following time format strings are supported:

* `h:mmtt`
* `h:mm:sstt`
* `hh:mmtt`
* `hh:mm:sstt`
* `H:mm`
* `H:mm:ss`
* `HH:mm`
* `HH:mm:ss`

You can also combine one of the supported date formats with one of the supported time formats if the date format is first, followed by whitespace and then the time format.

To see what each of the format identifiers means, please consult the [relevant MSDN documentation](http://msdn.microsoft.com/en-us/library/8kb3ddd4.aspx).

### Want another format?

If you want support for another format string, please [lodge an issue](https://github.com/MRCollective/ChameleonForms/issues) or send us a pull request.