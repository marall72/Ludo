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

interact('.resizable')
    .draggable({
        onmove: window.dragMoveListener,
        modifiers: [
            interact.modifiers.restrict({
                restriction: 'parent'
            })
        ]
    });

function SaveMapCoordinates() {
    var stations = $(".resizable");
    var stationCoordinates = [];
    $(stations).each(function (index, el) {
        var stationId = $(el).attr("stationId");
        var topLeftCoordinates = $(el).position();
        stationCoordinates.push({ StationId: stationId, Top: topLeftCoordinates.top, Left: topLeftCoordinates.left });
    });
    console.log(JSON.stringify(stationCoordinates));
    var url = "/stations/savemap";

    fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(stationCoordinates)
    })
        .then(response => {
            if (!response.ok) {
                return response.text().then(text => {
                    throw new Error(`HTTP error! status: ${response.status}, message: ${text}`);
                });
            }

        })
        .then(data => {
            console.log('Success:', data);
        })
        .catch(error => {
            console.error('Error saving map coordinates:', error);
        });
}