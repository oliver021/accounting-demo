$(document).ready(function () {
    var panel = $("#filter-panel");
    var filter = $("#filter_on");
    var undo = $("#filter_undo");
    var more = $("#filter_advance");
    var extra = $("#extra-panel");

    filter.click(function () {
            panel.show();
            undo.show();
            more.show();
            filter.hide();
    });

    undo.click(function () {
        extra.hide();
        panel.hide();
        undo.hide();
        filter.show();
        more.hide();
    });

    more.click(function () {
        extra.show();
    });

});

