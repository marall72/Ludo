

function dragMoveListener(event) {
    var target = event.target,
        // keep the dragged position in the data-x/data-y attributes
        x = (parseFloat(target.getAttribute('data-x')) || 0) + event.dx,
        y = (parseFloat(target.getAttribute('data-y')) || 0) + event.dy;

    // translate the element
    target.style.webkitTransform =
        target.style.transform =
        'translate(' + x + 'px, ' + y + 'px)';

    // update the posiion attributes
    target.setAttribute('data-x', x);
    target.setAttribute('data-y', y);
}

function SetStationsPosition() {
    const draggableElements = document.querySelectorAll('.resizable');

    draggableElements.forEach(el => {
        const initialX = parseFloat(el.getAttribute('data-x')) || 0;
        const initialY = parseFloat(el.getAttribute('data-y')) || 0;

        el.style.transform = `translate(${initialX}px, ${initialY}px)`;

        el.setAttribute('data-x', initialX);
        el.setAttribute('data-y', initialY);
    });
}

function UpdateMap() {
    var url = "/stations/updatemap";
    var contentDiv = $("#mapAreaWrapper");

    var dateFrom = $("#mapDateFrom").val();
    var timeFrom = $("#mapTimeFrom").val();
    var dateTo = $("#mapDateTo").val();
    var timeTo = $("#mapTimeTo").val();

    var searchConditions = {
        DateFrom: dateFrom, TimeFrom: timeFrom, DateTo: dateTo, TimeTo: timeTo
    };

    fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'X-Client-Request': 'fetch'
        }, body: JSON.stringify(searchConditions)
    })
        .then(response => {
            if (response.status == 401) {
                window.location = "/home";
            }
            else if (response.ok) {
                return response.text();
            }
        })
        .then(html => {
            $(contentDiv).html("");
            $(contentDiv).html(html);
            SetStationsPosition();
            SetMapSelection();
        });
}

function SaveMapCoordinates() {
    var stations = $(".resizable");
    var stationCoordinates = [];
    $(stations).each(function (index, el) {
        var stationId = $(el).attr("stationId");
        stationCoordinates.push({ StationId: stationId, X: $(el).attr("data-x"), Y: $(el).attr("data-y") });
    });
    var url = "/stations/savemap";

    fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'X-Client-Request': 'fetch'
        },
        body: JSON.stringify(stationCoordinates)
    })
        .then(response => {
            if (response.status == 401) {
                window.location = "/home";
            }
            else {
                if (!response.ok) {
                    return response.text().then(text => {
                        throw new Error(`HTTP error! status: ${response.status}, message: ${text}`);
                    });

                }
                else {
                    var toast = new bootstrap.Toast($("#mapUpdateToast"));
                    toast.show();
                }
            }            
        });
}


function SetMapSelection() {
    var resizableSelectClass = "resizable-selected";
    var resizableFullClass = "resizable-full";
    var resizableComingClass = "resizable-coming";
    var resizableDeselectClass = "resizable-deselect";
    var resizableShadowClass = "resize-shadow";

    $(".resizable").each(function (index, el) {
        $(el).on("click", function () {
            var select = $($(el).children("." + resizableShadowClass)[0]);
            if ($(el).hasClass(resizableFullClass) || $(el).hasClass(resizableComingClass)) {

            }
            else {
                if ($(el).hasClass(resizableSelectClass)) {
                    $(el).removeClass(resizableSelectClass);
                    $(select).removeClass(resizableDeselectClass);
                    $(select).html("انتخاب");
                }
                else {
                    $(el).addClass(resizableSelectClass);
                    $(select).addClass(resizableDeselectClass);
                    $(select).html("حذف");
                }
            }
        });
    })
}


function SaveReservation() {
    var stations = $(".resizable");

    var dateFrom = $("#mapReservationDateFrom").val();
    var timeFrom = $("#mapReservationTimeFrom").val();
    var dateTo = $("#mapReservationDateTo").val();
    var timeTo = $("#mapReservationTimeTo").val();
    var selectedStations = $(".mapArea .resizable-selected");
    var gameIds = $("#MapReservation_Games_SelectedGamesIds").val();
    var clientId = $("#mapReservationClientId").val();
    var notValid = false;
    if (dateFrom == "") {
        $("#mapErrorToast .toast-body").html("تاریخ از را وارد کنید");
        notValid = true;
    }
    else if (dateTo == "") {
        $("#mapErrorToast .toast-body").html("تاریخ تا را وارد کنید");
        notValid = true;
    }
    //else if () {
    //    time validation check
    //}
    else if ($(selectedStations).length == 0) {
        $("#mapErrorToast .toast-body").html("سیستم را انتخاب کنید.");
        notValid = true;
    }
    else if (isNaN(parseInt(clientId))) {
        $("#mapErrorToast .toast-body").html("مشتری را انتخاب کنید.");
        notValid = true;
    }
    if (notValid) {
        var toast = new bootstrap.Toast($("#mapErrorToast"));
        toast.show();
        return;
    }

    var stationIds = [];
    $(selectedStations).each(function (index, el) {
        var stationId = $(el).attr("stationId");
        stationIds.push(stationId);
    });

    var reservation = { DateFrom: dateFrom, TimeFrom: timeFrom, DateTo: dateTo, TimeTo: timeTo, StationIds: stationIds, GameIds: gameIds, ClientId: clientId };

    var url = "/reservations/savemapreservation";

    fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'X-Client-Request': 'fetch'
        },
        body: JSON.stringify(reservation)
    })
        .then(response => {
            if (response.status == 401) {
                window.location = "/home";
            }
            else if (response.status == 409) {
                $("#mapErrorToast .toast-body").html("رزرو دیگری برای این سیستم ثبت شده است. لطفا صفحه را بروزرسانی کنید.");
                var toast = new bootstrap.Toast($("#mapErrorToast"));
                toast.show();

            }
            else if (!response.ok) {
                return response.text().then(text => {
                    throw new Error(`HTTP error! status: ${response.status}, message: ${text}`);
                });

            }
            else {
                $("#mapUpdateToast .toast-body").html("رزرو با موفقیت ثبت شد.");
                var toast = new bootstrap.Toast($("#mapUpdateToast"));
                toast.show();
                UpdateMap();
            }
        });
}

function ShowShadow(el) {
    var shadow = $(el).children(".resize-shadow");
    $(shadow).show();
}

function HideShadow(el) {
    var shadow = $(el).children(".resize-shadow");
    $(shadow).hide();
}