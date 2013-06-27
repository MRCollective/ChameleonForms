(function(jQuery) {

    var isValidDate = function(value, format) {
        var getDateParser = function (regex, dayIndex, monthIndex, yearIndex) {
            return {
                regex: regex,
                dayIndex: dayIndex,
                monthIndex: monthIndex,
                yearIndex: yearIndex
            };
        };

        var dateParser;
        switch (format) {
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
            case "MM/dd/yy":
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

        var year = match[dateParser.yearIndex] * 1;
        var month = match[dateParser.monthIndex] * 1;
        var day = match[dateParser.dayIndex] * 1;
        if (year < 50)
            year += 2000;
        if (year < 100)
            year += 1900;

        var date = new Date(year, month - 1, day);

        // http://stackoverflow.com/questions/8098202/javascript-detecting-valid-dates
        if (date.getFullYear() != year || date.getMonth() + 1 != month || date.getDate() != day)
            return false;

        return true;
    };

    jQuery.validator.methods.date = function(value, element) {
        if (this.optional(element))
            return true;

        var format = jQuery(element).data("val-format");
        var formatSplitBySpaces = format.split(" ");
        if (format == "" || formatSplitBySpaces.length > 2)
            return !/Invalid|NaN/.test(new Date(value).toString());

        var valueSplitBySpaces = value.split(" ");
        if (formatSplitBySpaces.length != valueSplitBySpaces.length)
            return false;

        return isValidDate(valueSplitBySpaces[0], formatSplitBySpaces[0]);
    };

})(jQuery);
