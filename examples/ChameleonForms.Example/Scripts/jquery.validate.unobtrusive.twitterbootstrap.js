$(function () {
    $(function() {
        $("form").each(function(index, item) {
            var settings = $.data(item, "validator").settings;
            var oldErrorFunction = settings.errorPlacement;
            settings.errorPlacement = function(error, inputElement) {
                oldErrorFunction.call(item, error, inputElement);
                if (error.text() == "")
                    inputElement.closest(".form-group").removeClass("has-error");
                else
                    inputElement.closest(".form-group").addClass("has-error");
            };
        });
    });
});
