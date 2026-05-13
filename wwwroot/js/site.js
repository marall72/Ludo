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
    ReservationToastUpdate();
    setInterval(function () {
        ReservationPartialUpdate();
        ReservationToastUpdate();
    }, 60000);

    SetClock();

    SetMapSelection();

    if (registerDraggable == true)
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

function ReservationPartialUpdate() {
    var url = "/home/ReservationUpdateInterval?q=" + $("#SearchText").val();
    var contentDiv = $("#reservationPartialUpdate");
    
    fetch(url)
        .then(response => {
            return response.text();
        })
        .then(html => {
            $(contentDiv).html("");
            $(contentDiv).html(html);
        });
}

function ReservationToastUpdate() {
    var url = "/home/GetUpcomingReservationsToasts";
    var contentDiv = $("#allToastsWrapper");
    fetch(url)
        .then(response => {
            return response.text();
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

$(document).ready(function () {
    SetStationsPosition();
});

