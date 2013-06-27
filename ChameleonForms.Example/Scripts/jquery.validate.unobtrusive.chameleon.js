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

    var isValidTime = function (value, format) {

        var getTimeParser = function (regex, hourIndex, minuteIndex, secondIndex, is12HourTime) {
            return {
                regex: regex,
                hourIndex: hourIndex,
                minuteIndex: minuteIndex,
                secondIndex: secondIndex,
                is12HourTime: is12HourTime
            };
        };
        
        var timeParser;
        switch (format) {
            case "h:mmtt":
                timeParser = getTimeParser(/(\d{1,2}):(\d{2})(am|pm)/i, 1, 2, -1, true);
                break;
            case "h:mm:sstt":
                timeParser = getTimeParser(/(\d{1,2}):(\d{2}):(\d{2})(am|pm)/i, 1, 2, 3, true);
                break;
            case "hh:mmtt":
                timeParser = getTimeParser(/(\d{2}):(\d{2})(am|pm)/i, 1, 2, -1, true);
                break;
            case "hh:mm:sstt":
                timeParser = getTimeParser(/(\d{2}):(\d{2}):(\d{2})(am|pm)/i, 1, 2, 3, true);
                break;
            case "H:mm":
                timeParser = getTimeParser(/(\d{1,2}):(\d{2})/, 1, 2, -1, false);
                break;
            case "H:mm:ss":
                timeParser = getTimeParser(/(\d{1,2}):(\d{2}):(\d{2})/, 1, 2, 3, false);
                break;
            case "HH:mm":
                timeParser = getTimeParser(/(\d{2}):(\d{2})/, 1, 2, -1, false);
                break;
            case "HH:mm:ss":
                timeParser = getTimeParser(/(\d{2}):(\d{2}):(\d{2})/, 1, 2, 3, false);
                break;
            default:
                return true;
        }

        var match = value.match(timeParser.regex);
        if (match == null)
            return false;
        
        var hour = match[timeParser.hourIndex] * 1;
        var minute = match[timeParser.minuteIndex] * 1;
        var is12Hour = timeParser.is12HourTime;

        if (hour < 1)
            return false;
        if (is12Hour && hour > 12)
            return false;
        if (!is12Hour && hour > 23)
            return false;
        if (minute < 0 || minute > 59)
            return false;
        if (timeParser.secondIndex > 0) {
            var second = match[timeParser.secondIndex] * 1;
            if (second < 0 || second > 59)
                return false;
        }

        return true;
    };

    jQuery.validator.methods.date = function(value, element) {
        if (this.optional(element))
            return true;

        var format = jQuery(element).data("val-format");
        if (format == "")
            return !/Invalid|NaN/.test(new Date(value).toString());
        
        var formatSplitBySpaces = format.split(" ");
        if (formatSplitBySpaces.length > 2)
            return true;

        var valueSplitBySpaces = value.split(" ");
        if (formatSplitBySpaces.length != valueSplitBySpaces.length)
            return false;

        return isValidDate(valueSplitBySpaces[0], formatSplitBySpaces[0])
            && (formatSplitBySpaces.length == 1 || isValidTime(valueSplitBySpaces[1], formatSplitBySpaces[1]));
    };

})(jQuery);
