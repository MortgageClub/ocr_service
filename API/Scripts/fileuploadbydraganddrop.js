function dragEnter(evt) {
    evt.stopPropagation();
    evt.preventDefault();
    $(evt.target).addClass('over');
}

function dragLeave(evt) {
    evt.stopPropagation();
    evt.preventDefault();
    $(evt.target).removeClass('over');
}

function dragOver(evt) {
    evt.stopPropagation();
    evt.preventDefault();
}

function drop(evt) {
    evt.stopPropagation();
    evt.preventDefault();
    $(evt.target).removeClass('over');

    var files = evt.originalEvent.dataTransfer.files;

    if (files.length > 0) {
        if (window.FormData !== undefined) {
            var data = new FormData();
            for (i = 0; i < files.length; i++) {
                data.append("file" + i, files[i]);
            }

            $("#uploadNotification").show();

            $.ajax({
                type: "POST",
                url: root + "api/upload",
                contentType: false,
                processData: false,
                data: data,
                success: function (res) {
                    $.each(res, function (i, item) {
                        viewModel.uploads.push(item);
                    });
                    $("#uploadNotification").hide();
                }
            });
        } else {
            alert("Your browser needs to support HTML5.");
        }
    }
}

$(document).ready(function () {
    var $box = $("#uploadContainer");

    $box.bind("dragenter", dragEnter);
    $box.bind("dragleave", dragLeave);
    $box.bind("dragover", dragOver);
    $box.bind("drop", drop);
});

var viewModel = {
    uploads: ko.observableArray([])
}
ko.applyBindings(viewModel);