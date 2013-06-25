// Jasmine
/// <reference path="../lib/jasmine-1.3.1/jasmine.js"/>
/// <reference path="../lib/jasmine.using.js"/>
// Fixture data
/// <reference path="../fixtures/DateTimeValidationFixtures.js"/>
// Libs
/// <reference path="../src/jquery-1.4.4.js"/>
/// <reference path="../src/jquery.validate.js"/>
/// <reference path="../src/jquery.validate.unobtrusive.js"/>
// Src
/// <reference path="../src/jquery.validate.unobtrusive.chameleon.js"/>

describe("When validating a date field", function () {

    var element = {};
    var validatorContext = { optional: function () { return true; }};

    var invoke = function (value) {
        return jQuery.validator.methods.date.call(validatorContext, value, element);
    };

    describe("input with no value", function () {
        
        beforeEach(function() {
            spyOn(validatorContext, "optional").andReturn(true);
        });
        
        it("should return true", function () {
            expect(invoke()).toBe(true);
        });
    });

    describe("input with value", function () {

        var formatString;

        beforeEach(function () {
            spyOn(validatorContext, "optional").andReturn(false);
            spyOn($.fn, "data").andCallFake(function (key) {
                if (key == "val-format")
                    return formatString;
                return null;
            });
        });

        describe("input with no format", function () {

            beforeEach(function() {
                formatString = "";
            });

            it("should return true if the date is a JavaScript parsable date string", function() {
                expect(invoke("12/13/2000")).toBe(true);
            });
            
            it("should return false if the date is a JavaScript parsable date string for an invalid date", function () {
                expect(invoke("13/12/2000")).toBe(false);
            });
            
            it("should return false if the date is not a JavaScript parsable date string for an invalid date", function () {
                expect(invoke("notadate")).toBe(false);
            });
        });
        
        describe("input with date-only format", function () {
            using("valid format and value", ObjectMother.DateTimeFormats.Valid, function (format, value) {
                it("should return true", function () {
                    formatString = format;
                    expect(invoke(value)).toBe(true);
                });
            });
            
            using("invalid format and value", ObjectMother.DateTimeFormats.Invalid, function (format, value) {
                it("should return false", function () {
                    formatString = format;
                    expect(invoke(value)).toBe(false);
                });
            });
        });
    });
});