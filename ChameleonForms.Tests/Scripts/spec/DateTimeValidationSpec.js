// Jasmine
/// <reference path="../lib/jasmine-1.3.1/jasmine.js"/>
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
        });
    });
});