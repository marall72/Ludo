$(document).ready(function () {
    $(".persianDatePicker").persianDatepicker();

    $(".select2").select2({
        theme: "bootstrap-5"
    });

    var myModal = new bootstrap.Modal(document.getElementById('exampleModal'));

    // باز کردن مودال
    myModal.show();
});
function GetStationGames(el) {
    var stationId = $(el).val();
    var url = "/home/GetStationGames?id=" + stationId
    var contentDiv = $("#gamePickerWrapper");
    fetch(url)
        .then(response => {
            return response.text();
        })
        .then(html => {
            $(contentDiv).html(html);
            $(".select2").select2({
                theme: "bootstrap-5"
            });
        });
}

let clock = document.getElementById('clock');
let dateElement = document.getElementById('date');

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
    let dateString = time.toLocaleDateString('en-US', options);

    clock.innerText = `${h}:${m}:${s} ${ampm}`;
    dateElement.innerText = dateString;

    setTimeout(digClock, 1000);
}

digClock();