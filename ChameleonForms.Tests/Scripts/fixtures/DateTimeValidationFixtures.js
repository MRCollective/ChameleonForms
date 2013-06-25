﻿var ObjectMother = ObjectMother || {};

ObjectMother.DateTimeFormats = {
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
