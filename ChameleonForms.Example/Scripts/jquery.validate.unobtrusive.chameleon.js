jQuery.validator.methods.date = function (value, element) {
    if (this.optional(element))
        return true;

    var format = jQuery(element).data("val-format");
    if (format == "")
        return !/Invalid|NaN/.test(new Date(value).toString());

    var dateFormat = format;
    // todo: support time too

    var getDateParser = function (regex, dayIndex, monthIndex, yearIndex) {
        return {
            regex: regex,
            dayIndex: dayIndex,
            monthIndex: monthIndex,
            yearIndex: yearIndex
        };
    };

    var dateParser;
    switch (dateFormat) {
        case "d/M/yyyy":
            dateParser = getDateParser(/^(\d{1,2})\/(\d{1,2})\/(\d{4})$/, 1, 2, 3);
            break;
        case "d-M-yyyy":
            dateParser = getDateParser(/^(\d{1,2})-(\d{1,2})-(\d{4})$/, 1, 2, 3);
            break;
        case "d/M/yy":
            dateParser = getDateParser(/^(\d{1,2})\/(\d{1,2})\/(\d{2})$/, 1, 2, 3);
            break;
        case "d-M-yy":
            dateParser = getDateParser(/^(\d{1,2})-(\d{1,2})-(\d{2})$/, 1, 2, 3);
            break;
        case "dd/MM/yyyy":
            dateParser = getDateParser(/^(\d{2})\/(\d{2})\/(\d{4})$/, 1, 2, 3);
            break;
        case "dd-MM-yyyy":
            dateParser = getDateParser(/^(\d{2})-(\d{2})-(\d{4})$/, 1, 2, 3);
            break;
        case "dd/MM/yy":
            dateParser = getDateParser(/^(\d{2})\/(\d{2})\/(\d{2})$/, 1, 2, 3);
            break;
        case "dd-MM-yy":
            dateParser = getDateParser(/^(\d{2})-(\d{2})-(\d{2})$/, 1, 2, 3);
            break;
        case "M/d/yyyy":
            dateParser = getDateParser(/^(\d{1,2})\/(\d{1,2})\/(\d{4})$/, 2, 1, 3);
            break;
        case "M-d-yyyy":
            dateParser = getDateParser(/^(\d{1,2})-(\d{1,2})-(\d{4})$/, 2, 1, 3);
            break;
        case "M/d/yy":
            dateParser = getDateParser(/^(\d{1,2})\/(\d{1,2})\/(\d{2})$/, 2, 1, 3);
            break;
        case "M-d-yy":
            dateParser = getDateParser(/^(\d{1,2})-(\d{1,2})-(\d{2})$/, 2, 1, 3);
            break;
        case "MM/dd/yyyy":
            dateParser = getDateParser(/^(\d{2})\/(\d{2})\/(\d{4})$/, 2, 1, 3);
            break;
        case "MM-dd-yyyy":
            dateParser = getDateParser(/^(\d{2})-(\d{2})-(\d{4})$/, 2, 1, 3);
            break;
        case "MM/ddyy":
            dateParser = getDateParser(/^(\d{2})\/(\d{2})\/(\d{2})$/, 2, 1, 3);
            break;
        case "MM-dd-yy":
            dateParser = getDateParser(/^(\d{2})-(\d{2})-(\d{2})$/, 2, 1, 3);
            break;
        case "yyyy/MM/dd":
            dateParser = getDateParser(/^(\d{4})\/(\d{2})\/(\d{2})$/, 3, 2, 1);
            break;
        case "yyyy-MM-dd":
            dateParser = getDateParser(/^(\d{4})-(\d{2})-(\d{2})$/, 3, 2, 1);
            break;
        case "yyyy/M/d":
            dateParser = getDateParser(/^(\d{4})\/(\d{1,2})\/(\d{1,2})$/, 3, 2, 1);
            break;
        case "yyyy-M-d":
            dateParser = getDateParser(/^(\d{4})-(\d{1,2})-(\d{1,2})$/, 3, 2, 1);
            break;
        default:
            return true;
    }

    var match = value.match(dateParser.regex);
    if (match == null)
        return false;

    var year = match[dateParser.yearIndex]*1;
    if (year < 50)
        year += 2000;
    if (year < 100)
        year += 1900;

    var date = new Date(year, match[dateParser.monthIndex]*1-1, match[dateParser.dayIndex]*1);

    if (!/Invalid|NaN/.test(date.toString()))
        return false;

    return true;
}