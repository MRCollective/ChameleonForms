var ObjectMother = ObjectMother || {};

ObjectMother.TimeFormats = {
    Valid: [
        ["h:mmtt", "1:59am"],
        ["h:mm:sstt", "1:59:59pm"],
        ["hh:mmtt", "01:59AM"],
        ["hh:mm:sstt", "01:59:59PM"],
        ["H:mm", "13:59"],
        ["H:mm:ss", "13:59:59"],
        ["HH:mm", "13:59"],
        ["HH:mm:ss", "13:59:59"]
    ],
    Invalid: [
        ["h:mmtt", "13:59pm"],
        ["h:mmtt", "0:59pm"],
        ["h:mmtt", "1:60pm"],
        ["h:mmtt", "1:00"],
        ["h:mmtt", "1:00xx"],
        ["h:mm:sstt", "13:59:59pm"],
        ["h:mm:sstt", "0:59:59pm"],
        ["h:mm:sstt", "1:60:59pm"],
        ["h:mm:sstt", "1:59:60pm"],
        ["h:mm:sstt", "1:59:59"],
        ["h:mm:sstt", "1:59:59xx"],
        ["hh:mmtt", "13:59pm"],
        ["hh:mmtt", "0:59pm"],
        ["hh:mmtt", "01:60pm"],
        ["hh:mmtt", "01:59"],
        ["hh:mmtt", "01:59xx"],
        ["hh:mm:sstt", "13:59:59pm"],
        ["hh:mm:sstt", "0:59:59pm"],
        ["hh:mm:sstt", "01:60:59pm"],
        ["hh:mm:sstt", "01:59:60pm"],
        ["hh:mm:sstt", "01:59:59"],
        ["hh:mm:sstt", "01:59:59xx"],
        ["H:mm", "24:59"],
        ["H:mm", "0:59"],
        ["H:mm", "13:60"],
        ["H:mm:ss", "24:59:59"],
        ["H:mm:ss", "0:59:59"],
        ["H:mm:ss", "13:60:59"],
        ["H:mm:ss", "13:59:60"],
        ["HH:mm", "24:59"],
        ["HH:mm", "0:59"],
        ["HH:mm", "13:60"],
        ["HH:mm:ss", "24:59:59"],
        ["HH:mm:ss", "0:59:59"],
        ["HH:mm:ss", "13:60:59"],
        ["HH:mm:ss", "13:59:60"]
    ]
};

ObjectMother.DateFormats = {
    Valid: [
        ["d/M/yyyy", "13/12/2000"],
        ["d-M-yyyy", "13-12-2000"],
        ["d/M/yy", "13/12/00"],
        ["d-M-yy", "13-12-00"],
        ["dd/MM/yyyy", "13/12/2000"],
        ["dd-MM-yyyy", "13-12-2000"],
        ["dd/MM/yy", "13/12/00"],
        ["dd-MM-yy", "13-12-00"],
        ["M/d/yyyy", "12/13/2000"],
        ["M-d-yyyy", "12-13-2000"],
        ["M/d/yy", "12/13/00"],
        ["M-d-yy", "12-13-00"],
        ["MM/dd/yyyy", "12/13/2000"],
        ["MM-dd-yyyy", "12-13-2000"],
        ["MM/dd/yy", "12/13/00"],
        ["MM-dd-yy", "12-13-00"],
        ["yyyy/MM/dd", "2000/12/13"],
        ["yyyy-MM-dd", "2000-12-13"],
        ["yyyy/M/d", "2000/12/13"],
        ["yyyy-M-d", "2000-12-13"]
    ],
    Invalid: [
        ["d/M/yyyy", "12/13/2000"],
        ["d-M-yyyy", "12-13-2000"],
        ["d/M/yy", "12/13/00"],
        ["d-M-yy", "12-13-00"],
        ["dd/MM/yyyy", "12/13/2000"],
        ["dd/MM/yyyy", "1/1/2000"],
        ["dd-MM-yyyy", "12-13-2000"],
        ["dd-MM-yyyy", "1-1-2000"],
        ["dd/MM/yy", "12/13/00"],
        ["dd/MM/yy", "1/1/00"],
        ["dd-MM-yy", "12-13-00"],
        ["dd-MM-yy", "1-1-00"],
        ["M/d/yyyy", "13/12/2000"],
        ["M-d-yyyy", "13-12-2000"],
        ["M/d/yy", "13/12/00"],
        ["M-d-yy", "13-12-00"],
        ["MM/dd/yyyy", "13/12/2000"],
        ["MM/dd/yyyy", "1/1/2000"],
        ["MM-dd-yyyy", "13-12-2000"],
        ["MM-dd-yyyy", "1-1-2000"],
        ["MM/dd/yy", "13/12/00"],
        ["MM/dd/yy", "1/1/00"],
        ["MM-dd-yy", "13-12-00"],
        ["MM-dd-yy", "1-1-00"],
        ["yyyy/MM/dd", "2000/13/12"],
        ["yyyy/MM/dd", "2000/1/1"],
        ["yyyy-MM-dd", "2000-13-12"],
        ["yyyy-MM-dd", "2000-1-1"],
        ["yyyy/M/d", "2000/13/12"],
        ["yyyy-M-d", "2000-13-12"]
    ],
};

(function (om) {
    var i, j;
    var valid = [];
    for (i in ObjectMother.DateFormats.Valid)
        for (j in ObjectMother.TimeFormats.Valid)
            valid.push([ObjectMother.DateFormats.Valid[i][0] + " " + ObjectMother.TimeFormats.Valid[j][0], ObjectMother.DateFormats.Valid[i][1] + " " + ObjectMother.TimeFormats.Valid[j][1]]);
    var invalidDate = [];
    for (i in ObjectMother.DateFormats.Invalid)
        for (j in ObjectMother.TimeFormats.Valid)
            invalidDate.push([ObjectMother.DateFormats.Invalid[i][0] + " " + ObjectMother.TimeFormats.Valid[j][0], ObjectMother.DateFormats.Invalid[i][1] + " " + ObjectMother.TimeFormats.Valid[j][1]]);
    var invalidTime = [];
    for (i in ObjectMother.DateFormats.Valid)
        for (j in ObjectMother.TimeFormats.Invalid)
            invalidDate.push([ObjectMother.DateFormats.Valid[i][0] + " " + ObjectMother.TimeFormats.Invalid[j][0], ObjectMother.DateFormats.Valid[i][1] + " " + ObjectMother.TimeFormats.Invalid[j][1]]);
    var invalidDateAndTime = [];
    for (i in ObjectMother.DateFormats.Invalid)
        for (j in ObjectMother.TimeFormats.Invalid)
            invalidDate.push([ObjectMother.DateFormats.Invalid[i][0] + " " + ObjectMother.TimeFormats.Invalid[j][0], ObjectMother.DateFormats.Invalid[i][1] + " " + ObjectMother.TimeFormats.Invalid[j][1]]);

    om.DateTimeFormats = {
        Valid: valid,
        InvalidDate: invalidDate,
        InvalidTime: invalidTime,
        InvalidDateAndTime: invalidDateAndTime
    };
})(ObjectMother);
