describe("When validating a date field", function () {

    var element = {};
    var validatorContext = { optional: function () { return true; }};

    var invoke = function (value) {
        return jQuery.validator.methods.date.call(validatorContext, value, element);
    };

    describe("that has no value", function () {
        
        beforeEach(function() {
            spyOn(validatorContext, "optional").and.returnValue(true);
        });
        
        it("should return true", function () {
            expect(invoke()).toBe(true);
        });
    });

    describe("that has a value", function () {

        var formatString;

        beforeEach(function () {
            spyOn(validatorContext, "optional").and.returnValue(false);
            spyOn($.fn, "data").and.callFake(function (key) {
                if (key == "val-format")
                    return formatString;
                return null;
            });
        });

        describe("with no format", function () {

            beforeEach(function() {
                formatString = "";
            });

            it("should return true if the date is a JavaScript parsable date string", function() {
                expect(invoke("12/13/2000")).toBe(true);
            });
            
            it("should return false if the date is a JavaScript parsable date string for an invalid date", function () {
                expect(invoke("2000-13-12")).toBe(false);
            });
            
            it("should return false if the date is not a JavaScript parsable date string for an invalid date", function () {
                expect(invoke("notadate")).toBe(false);
            });
        });

        // Multiple spaces not currently supported so always return true to allow the submission
        describe("against a format with multiple spaces", function() {
            it("should return true", function () {
                formatString = "d/M/yyyy HH mm ss";
                expect(invoke("asdf")).toBe(true);
            });
        });
        
        describe("against a date-only format", function() {
            all(ObjectMother.DateFormats.Valid, function(format, value) {
                it("should return true for "+format+" and valid value ("+value+")", function() {
                    formatString = format;
                    expect(invoke(value)).toBe(true);
                });
            });

            all(ObjectMother.DateFormats.Invalid, function(format, value) {
                it("should return false for " + format + " and invalid value (" + value + ")", function() {
                    formatString = format;
                    expect(invoke(value)).toBe(false);
                });
            });

            it("should return false if the value has a space in it", function() {
                formatString = "d/M/yyyy";
                expect(invoke("12/12/2000 ")).toBe(false);
            });
            
            it("should return true if the format is unknown", function () {
                formatString = "asdf";
                expect(invoke("asdf")).toBe(true);
            });
        });

        describe("against a date and time format", function() {

            all(ObjectMother.DateTimeFormats.Valid, function(format, value) {
                it("should return true for " + format + " and valid value (" + value + ")", function() {
                    formatString = format;
                    expect(invoke(value)).toBe(true);
                });
            });

            all(ObjectMother.DateTimeFormats.InvalidDate, function(format, value) {
                it("should return false for " + format + " and invalid date (" + value + ")", function() {
                    formatString = format;
                    expect(invoke(value)).toBe(false);
                });
            });

            all(ObjectMother.DateTimeFormats.InvalidTime, function(format, value) {
                it("should return false for " + format + " and invalid time (" + value + ")", function() {
                    formatString = format;
                    expect(invoke(value)).toBe(false);
                });
            });

            all(ObjectMother.DateTimeFormats.InvalidDateAndTime, function(format, value) {
                it("should return false for " + format + " and invalid date and time (" + value + ")", function() {
                    formatString = format;
                    expect(invoke(value)).toBe(false);
                });
            });

            it("should return false if the value doesn't have a space in it", function () {
                formatString = "d/M/yyyy HH:mm:ss";
                expect(invoke("12/12/2000")).toBe(false);
            });
            
            it("should return true if the format is unknown", function () {
                formatString = "asdf asdf";
                expect(invoke("asdf asdf")).toBe(true);
            });
        });
    });
});