$(document).ready(function () {







    $(document).on("click", "li.cronPattern a", function () {
        //debugger;
        $("li.cronPattern a").removeClass("active");
        $(this).attr("class", "active")
        $('div[id$="TabDetail"]').attr("style", "display:none");
        generate();
    });

    $("#CronGenMainDiv select,input").change(function (e) {
        //debugger;
        generate();
    });
    $("#CronGenMainDiv input[type='checkbox']").change(function (e) {
        //debugger;
        generate();
    });



    $("#CronGenMainDiv input").keyup(function (e) {
        //debugger;
        generate();
    });

    var generate = function () {
        //debugger;
        var DailyMinutesCust = '0';
        var DailyHoursCust = '0';
        var MonthlyMinutesCust = '0';
        var MonthlyHoursCust = '0';
        var YearlyMinutes = '0';
        var YearlyHours = '0';
        var YearlyMinutesCust = '0';
        var YearlyHoursCust = '0';

        var activeTab = $("ul#CronGenTabs a.active").prop("id");//$("ul#CronGenTabs li.active a").prop("id");
        activeTab = activeTab + "Detail";
        $("#" + activeTab).attr("style", "display:block");

        var results = "";
        switch (activeTab) {
            case "MinutesTabDetail":
                results = "*/" + $("#MinutesInput").val() + " * * * *";
                break;
            case "HourlyTabDetail":
                switch ($("input:radio[name=HourlyRadio]:checked").val()) {
                    case "1":
                        results = "0 */" + $("#HoursInput").val() + " * * *";
                        break;
                    case "2":
                        results = "" + Number($("#AtMinutes").val()) + " " + Number($("#AtHours").val()) + " * * *";
                        break;
                }
                break;
            case "DailyTabDetail":
                switch ($("input:radio[name=DailyRadio]:checked").val()) {
                    case "1":
                        if (Number($("#DailyMinutes").val()) !== 0) DailyMinutesCust = Number($("#DailyMinutes").val());
                        if (Number($("#DailyHours").val()) !== 0) DailyHoursCust = Number($("#DailyHours").val());
                        results = "" + DailyMinutesCust + " " + DailyHoursCust + " */" + $("#DaysInput").val() + " * *";
                        break;
                    case "2":
                        if (Number($("#DailyMinutes").val()) !== 0) DailyMinutesCust = Number($("#DailyMinutes").val());
                        if (Number($("#DailyHours").val()) !== 0) DailyHoursCust = Number($("#DailyHours").val());
                        results = "" + DailyMinutesCust + " " + DailyHoursCust + " * * SAT,SUN,MON,TUE,WED,THU";
                        break;
                }
                break;
            case "WeeklyTabDetail":
                var selectedDays = "";
                $("#WeeklyTabDetail input:checkbox:checked").each(function () { selectedDays += $(this).val() + ","; });
                if (selectedDays.length > 0)
                    selectedDays = selectedDays.substr(0, selectedDays.length - 1);
                results = "" + Number($("#WeeklyMinutes").val()) + " " + Number($("#WeeklyHours").val()) + " * * " + selectedDays + "";
                break;
            case "MonthlyTabDetail":
                switch ($("input:radio[name=MonthlyRadio]:checked").val()) {
                    case "1":
                        if (Number($("#MonthlyMinutes").val()) !== 0) MonthlyMinutesCust = Number($("#MonthlyMinutes").val());
                        if (Number($("#MonthlyHours").val()) !== 0) MonthlyHoursCust = Number($("#MonthlyHours").val());
                        var monthlyArray = [];
                        $('input:checkbox.monthlyMonthInputs').each(function () {
                            if (this.checked) monthlyArray.push($(this).val());
                        });
                        results = "" + Number($("#MonthlyMinutes").val()) + " " + Number($("#MonthlyHours").val()) + " " + $("#DayOfMOnthInput").val() + " */" + monthlyArray.join() + " *";

                        break;
                    case "2":
                        if (Number($("#MonthlyMinutes").val()) !== 0) MonthlyMinutesCust = Number($("#MonthlyMinutes").val());
                        if (Number($("#MonthlyHours").val()) !== 0) MonthlyHoursCust = Number($("#MonthlyHours").val());
                        results = "" + Number($("#MonthlyMinutes").val()) + " " + Number($("#MonthlyHours").val()) + " " + $("#WeekDay").val() + " */" + Number($("#EveryMonthInput").val()) + " " + $("#DayInWeekOrder").val() + "";
                        break;
                }
                break;
            case "YearlyTabDetail":
                switch ($("input:radio[name=YearlyRadio]:checked").val()) {
                    case "1":
                        if (Number($("#YearlyMinutes").val()) !== 0) YearlyMinutesCust = Number($("#YearlyMinutes").val());
                        if (Number($("#YearlyHours").val()) !== 0) YearlyHoursCust = Number($("#YearlyHours").val());
                        results = "" + YearlyMinutesCust + " " + YearlyHoursCust + " " + $("#YearInput").val() + " " + $("#MonthsOfYear").val() + " *";
                        break;
                        // case "2":
                        // results = "" + YearlyMinutesCust + " " + YearlyHoursCust + " " + $("#DayOrderInYear").val() + " " + $("#MonthsOfYear2").val() + " " + Number($("#DayWeekForYear").val()) + "";
                        // break;
                }
                break;
        }

        // Update original control
        $("#PatternResult").val(results);
        //scope.$parent.onaftercronpatternset(results)
        //CronPatternSet(results);
        // Update display
        //displayElement.val(results);
    }

});