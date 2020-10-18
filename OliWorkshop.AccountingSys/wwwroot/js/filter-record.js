$(document).ready(function () {
    var toggle = true;
    var panel = $("#filter-panel");
    var filter = $("#filter_on");
    filter.click(function () {
        if (toggle) {
            panel.show();
        } else {
            panel.hide();
        }
        toggle = !toggle;
     });
});