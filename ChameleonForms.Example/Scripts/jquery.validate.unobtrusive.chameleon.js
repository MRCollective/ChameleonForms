jQuery.validator.methods.date = function (value, element) {
    var $ = jQuery;

    if (this.optional(element))
        return true;

    var format = $(element).data("val-format");
    if (format == "")
        return !/Invalid|NaN/.test(new Date(value).toString());

    var dateFormat = format.split(" ")[0];
    var timeFormat = format.split(" ")[1];

    switch (dateFormat) {
        case "d/M/yyyy":
        case "d-M-yyyy":
            break;
        case "d/M/yy":
        case "d-M-yy":
            break;
        case "dd/MM/yyyy":
        case "dd-MM-yyyy":
            break;
        case "dd/MM/yy":
        case "dd-MM-yy":
            break;
        case "M/d/yyyy":
        case "M-d-yyyy":
            break;
        case "M/d/yy":
        case "M-d-yy":
            break;
        case "MM/dd/yyyy":
        case "MM-dd-yyyy":
            break;
        case "MM/dd/yy":
        case "MM-dd-yy":
            break;
        case "yyyy/MM/dd":
        case "yyyy-MM-dd":
            break;
        case "yyyy/M/d":
        case "yyyy-M-d":
            break;
        default:
            return true;
    }
}