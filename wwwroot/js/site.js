$(document).ready(function () {
    $(".persianDatePicker").persianDatepicker();

    $(".select2").select2({
        theme: "bootstrap-5"
    });

    var exampleModal = document.getElementById('exampleModal');
    if (exampleModal != undefined) {
        var myModal = new bootstrap.Modal(exampleModal);
        myModal.show();
    }

    if (isInMainPage == true && logined) {

        ReservationToastUpdate();
        setInterval(function () {
            ReservationPartialUpdate();
            ReservationToastUpdate();
            //UpdateMap();
        }, 60000);
    }

    SetClock();

    if (logined && isInMainPage) {
        SetMapSelection();
        SetStationsPosition();
        SetMainPageReservationsTab();
        ResetMainPageReservationsTab();
    }

    if (isInMainPage && registerDraggable == true)
        interact('.resizable')
            .draggable({
                onmove: window.dragMoveListener,
                modifiers: [
                    interact.modifiers.restrict({
                        restriction: 'parent'
                    })
                ]
            });

});

var selectedTabCookieName = "SelectedReservationTab";
function SetMainPageReservationsTab() {
    var nav = $("#mainPageReservationsTab");
    if (nav != null) {
        var tabs = $(nav).find(".nav-item");
        $(tabs).on("click", function (el) {
            var tagretTabId = $(el.target).attr("data-bs-target");
            setCookie(selectedTabCookieName, tagretTabId, 1);
        });
    }
}

function ResetMainPageReservationsTab() {
    var content = $("#reservationTabContent");
    var targetTab = getCookie(selectedTabCookieName);
    if (content != null) {
        $($(content).children()).each(function (index, el) {
            if ("#" + $(el).attr("id") == targetTab) {
                $(el).addClass("show active");
            }
            else {
                $(el).removeClass("show active");
            }
        })
    }
    $($("#mainPageReservationsTab").find(".nav-link")).each(function (index, el) {
        $(el).removeClass("active");
    });

    $("#mainPageReservationsTab").find(".nav-link[data-bs-target='" + targetTab + "']").addClass("active");
}

function ReservationPartialUpdate() {
    var url = "/reservations/ReservationUpdateInterval?q=" + $("#SearchText").val();
    var contentDiv = $("#reservationPartialUpdate");

    fetch(url, {
        headers: {
            'X-Client-Request': 'fetch'
        }
    })
        .then(response => {
            if (response.status == 401) {
                window.location = "/home";
            }
            else {
                return response.text();
            }
        })
        .then(html => {
            $(contentDiv).html("");
            $(contentDiv).html(html);
        });
}

function ReservationToastUpdate() {
    var url = "/reservations/GetUpcomingReservationsToasts";
    var contentDiv = $("#allToastsWrapper");
    fetch(url, {
        headers: {
            'X-Client-Request': 'fetch'
        }
    })
        .then(response => {
            if (response.status == 401) {
                window.location = "/home";
            }
            else {
                return response.text();
            }
        })
        .then(html => {
            $(contentDiv).html(html);
            var toasts = $(".liveToast");
            if (toasts != undefined) {
                $(toasts).each(function (index, item) {
                    var toast = new bootstrap.Toast(item, {
                        autohide: false
                    });
                    toast.show();
                });
            }
        });
}
function SetClock() {

    let clock = document.getElementById('clock');

    function digClock() {
        let time = new Date();
        let h = time.getHours();
        let m = time.getMinutes();
        let s = time.getSeconds();
        let ampm = h >= 12 ? 'PM' : 'AM'; // Determine AM/PM

        h = h % 12; // Convert to 12-hour format
        h = h ? h : 12;
        h = h < 10 ? '0' + h : h;
        m = m < 10 ? '0' + m : m;
        s = s < 10 ? '0' + s : s;

        // Format date
        let options = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };

        if (clock != undefined) {
            clock.innerText = `${h}:${m}:${s} ${ampm}`;

            setTimeout(digClock, 1000);

        }
    }

    digClock();
}


function setCookie(name, value, days) {
    let date = new Date();
    date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
    let expires = "expires=" + date.toUTCString();
    document.cookie = `${name}=${value}; ${expires}; path=/`;
}

function deleteCookie(name) {
    document.cookie = name + "=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
}

function getCookie(name) {
    const cookieName = name + "=";
    const cookies = document.cookie.split(';');

    for (let i = 0; i < cookies.length; i++) {
        let cookie = cookies[i].trim();
        if (cookie.indexOf(cookieName) === 0) {
            return decodeURIComponent(cookie.substring(cookieName.length));
        }
    }
    return null;
}