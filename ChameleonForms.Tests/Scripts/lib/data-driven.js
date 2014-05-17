function all(values, test) {
    values.map(function(arguments) {
        test.apply(this, arguments);
    });
}
