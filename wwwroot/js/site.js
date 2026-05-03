$(document).ready(function () {
    $(".persianDatePicker").persianDatepicker();

    $(".select2").select2({
        theme: "bootstrap-5"
    });
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

