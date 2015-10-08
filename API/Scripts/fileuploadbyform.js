$(document).ready(function () {
    var divProgress = $("#divProgress");
    var divBar = $("#divPercentBar");
    var divPercent = $("#divPercentText");

    $("#formUpload").ajaxForm({
        beforeSend: function () {
            divProgress.show();
            var percentVal = '0%';
            divBar.width(percentVal)
            divPercent.html(percentVal);
        },
        uploadProgress: function (event, position, total, percentComplete) {
            var percentVal = percentComplete + '%';
            divBar.width(percentVal)
            divPercent.html(percentVal);
        },
        success: function (res) {
            var percentVal = '100%';
            divBar.width(percentVal)
            divPercent.html(percentVal);
            $.each(res, function (i, item) {
                viewModel.uploads.push(item);
            });
        },
        complete: function (xhr) {
            //divProgress.hide();
        }
    });
});
var viewModel = {
    uploads: ko.observableArray([])
}
ko.applyBindings(viewModel);