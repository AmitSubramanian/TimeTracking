// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function getDayOnFirstMonday(year, month) {
    return 1 + (8 - new Date(month + '/01/' + year).getDay()) % 7;
}

function getNumberofWorkWeeksInMonth(year, month) {
    var dayOnFirstMondayOfMonth = getDayOnFirstMonday(year, month);
    var daysInMonth = new Date(year, month, 0).getDate();
    var numberOfMondaysInMonth = 1 + (daysInMonth - dayOnFirstMondayOfMonth) / 7;

    return numberOfMondaysInMonth;
}

function populateWorkWeekDropDownList(year, month)
{
    var workWeek = $("#WorkWeek");
    workWeek.empty();

    var numWorkWeeks = getNumberofWorkWeeksInMonth(year, month);

    var options = '<option value="">Select a Work Week</option>';
    for (var i = 0; i < numWorkWeeks; i++) {
        options += ' <option value="' + (i + 1).toString() + '">' + (i + 1).toString() + '</option>';
    }
    workWeek.append(options);
}

$('#Year').change(function () {
    $('#Month').prop('selectedIndex', 0);
    $('#WorkWeek').prop('selectedIndex', 0);
});

$('#Month').change(function () {
    $('#WorkWeek').prop('selectedIndex', 0);

    // if year and month are selected, repopulate WorkWeek drop-down list
    if ($('#Year').val().toString() != "" && $('#Month').val().toString() != "") {
        populateWorkWeekDropDownList(parseInt($('#Year').val()), $parseInt(('#Month').val()));
    }
});
